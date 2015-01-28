using System;
using System.Collections.Generic;
using SDL2;

namespace Breeze.Graphics
{
	public class Layer : Drawable
	{
		protected IntPtr Buffer;
		protected SDL.SDL_Rect SrcRect;
		public string Name { get; protected set; }
        public double ScrollSpeed = 1.0;
		
		public Layer(string name, int zorder = 0)
		{
			Buffer = SDL.SDL_CreateTexture(BreezeCore.Renderer, 
                                           SDL.SDL_GetWindowPixelFormat(BreezeCore.Window), 
			                               (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, 
			                               BreezeCore.ScrW, 
			                               BreezeCore.ScrH);
			SDL.SDL_SetTextureBlendMode(Buffer, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

			SrcRect = new SDL.SDL_Rect() { x = 0, y = 0, w = BreezeCore.ScrW, h = BreezeCore.ScrH };    // Use BreezeCore.ScrRect instead?
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
			
			base.Draw(dx, dy, dangle);

            SDL.SDL_SetRenderTarget(BreezeCore.Renderer, Screen.Buffer);
            SDL.SDL_SetTextureAlphaMod(Buffer, FAlpha);
            SDL.SDL_RenderCopy(BreezeCore.Renderer, Buffer, ref SrcRect, ref SrcRect);
		}
		
		public void Insert(Drawable dr)
		{
			int index = Children.FindIndex(item => item.ZOrder > dr.ZOrder);
			index = (index >= 0) ? index : Children.Count;
			Children.Insert(index, dr);
            dr.Layer = Name;
		}
	}
}

