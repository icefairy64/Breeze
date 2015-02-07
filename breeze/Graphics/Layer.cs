using System;
using System.Collections.Generic;
using SDL2;

namespace Breeze.Graphics
{
	public class Layer : Drawable
	{
		protected IntPtr Buffer;
		protected SDL.SDL_Rect SrcRect;
        protected DrawableChunkCollection Chunks;
        protected int ChunksInRow;
        protected int ChunksInColumn;
        protected int ChunkValidateCounter = 0;
        protected int OuterRenderRadius = 2;

		public string Name { get; protected set; }
        public bool Chunked { get; protected set; }
        public double ScrollSpeed = 1.0;
		
        public Layer(string name, int zorder = 0, bool chunked = true)
		{
			Buffer = SDL.SDL_CreateTexture(BreezeCore.Renderer, 
                                           SDL.SDL_GetWindowPixelFormat(BreezeCore.Window), 
			                               (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, 
			                               BreezeCore.ScrW, 
			                               BreezeCore.ScrH);
			SDL.SDL_SetTextureBlendMode(Buffer, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

			SrcRect = new SDL.SDL_Rect() { x = 0, y = 0, w = BreezeCore.ScrW, h = BreezeCore.ScrH };    // Use BreezeCore.ScrRect instead?

            if (chunked)
            {
                Chunks = new DrawableChunkCollection(256);
                ChunksInRow = SrcRect.w / 256;
                ChunksInColumn = SrcRect.h / 256;
            }

            Chunked = chunked;
			Name = name;
            ZOrder = zorder;
		}
		
		protected override void SetAlpha(byte alpha)
		{
            //SDL.SDL_SetTextureBlendMode(Buffer, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
            SDL.SDL_SetTextureAlphaMod(Buffer, alpha);
		}

        protected override void SetBlendMode(SDL.SDL_BlendMode mode)
        {
            SDL.SDL_SetTextureBlendMode(Buffer, mode);
        }
		
		protected override void InternalDraw(int x, int y, double angle)
		{
            // Do nothing
		}
		
		public override void Draw(int dx, int dy, double dangle)
		{
			SDL.SDL_SetRenderTarget(BreezeCore.Renderer, Buffer);
			SDL.SDL_RenderClear(BreezeCore.Renderer);
            SDL.SDL_SetTextureAlphaMod(Buffer, 0xff);
			
            if (!Chunked)
                base.Draw(dx, dy, dangle);
            else
            {
                int rx = X + dx;
                int ry = Y + dy;
                double rangle = Angle + dangle;

                DstRect.x = rx;
                DstRect.y = ry;
                //InternalDraw(rx, ry, rangle);

                //Console.WriteLine(ChunkValidateCounter);
                Chunks.ValidateChunkAt(
                    ((ChunkValidateCounter) / (ChunksInRow + OuterRenderRadius)) * Chunks.ChunkSize - rx, 
                    ((ChunkValidateCounter) % (ChunksInRow + OuterRenderRadius)) * Chunks.ChunkSize - ry);

                ChunkValidateCounter++;
                if (ChunkValidateCounter == (ChunksInRow + OuterRenderRadius) * (ChunksInColumn + OuterRenderRadius))
                    ChunkValidateCounter = -OuterRenderRadius * (ChunksInRow + OuterRenderRadius) - OuterRenderRadius;

                for (int x = -OuterRenderRadius; x <= ChunksInRow + OuterRenderRadius; x++)
                    for (int y = -OuterRenderRadius; y <= ChunksInColumn + OuterRenderRadius; y++)
                    {
                        var chunk = Chunks.GetChunkAt(x * Chunks.ChunkSize - rx, y * Chunks.ChunkSize - ry);

                        if (chunk != null)
                        {
                            foreach (Drawable dr in chunk)
                                dr.Draw(rx, ry, rangle);
                        }
                    }
            }

            //foreach (Drawable dr in Children)
            //    dr.Draw(rx, ry, rangle);

            SDL.SDL_SetRenderTarget(BreezeCore.Renderer, Screen.Buffer);
            SDL.SDL_SetTextureAlphaMod(Buffer, FAlpha);
            SDL.SDL_RenderCopy(BreezeCore.Renderer, Buffer, ref SrcRect, ref SrcRect);
		}
		
		public void Insert(Drawable dr)
		{
			int index = Children.FindIndex(item => item.ZOrder > dr.ZOrder);
			index = (index >= 0) ? index : Children.Count;
            dr.Layer = Name;

			Children.Insert(index, dr);
            Chunks.InsertAt(dr, dr.X, dr.Y);
		}
	}
}

