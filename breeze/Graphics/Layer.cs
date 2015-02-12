using System;
using System.Collections.Generic;
using SFML.Graphics;

namespace Breeze.Graphics
{
	public class Layer : Drawable
	{
        protected RenderTexture Buffer;
        protected FloatRect SrcRect;
        protected DrawableChunkCollection Chunks;
        protected int ChunksInRow;
        protected int ChunksInColumn;
        protected int ChunkValidateCounter = 0;
        protected int OuterRenderRadius = 2;
        protected float FZoom = 1f;

        public View View;
		public string Name { get; protected set; }
        public bool Chunked { get; protected set; }
        public float ScrollSpeed = 1.0f;
		
        public float Zoom 
        {
            get { return FZoom; }
            set
            {
                View.Zoom(FZoom/value);
                FZoom = value;
            }
        }

        public Layer(string name, int zorder = 0, bool chunked = true)
		{
            Buffer = new RenderTexture(BreezeCore.ScrW, BreezeCore.ScrH);
            View = new View(new FloatRect(0, 0, BreezeCore.ScrW, BreezeCore.ScrH));
            SrcRect = new FloatRect(0, 0, BreezeCore.ScrW, BreezeCore.ScrH);

            if (chunked)
                Chunks = new DrawableChunkCollection(256);

            Chunked = chunked;
			Name = name;
            ZOrder = zorder;
		}
		
        protected override void InternalDraw(Transform tf)
		{
            // Do nothing
		}
		
        public override void Draw()
		{
            Screen.CurrentTarget = Buffer;
            Buffer.Clear(Color.Transparent);
            Buffer.SetView(View);
			
            if (!Chunked)
                base.Draw();
            else
            {
                // TODO Add rotation
                int rx = (int)(View.Center.X - (View.Size.X / 2));
                int ry = (int)(View.Center.Y - (View.Size.Y / 2));
                ChunksInRow = (int)View.Size.X / 256;
                ChunksInColumn = (int)View.Size.Y / 256;

                //double rangle = Angle + dangle;
                //DstRect.x = rx;
                //DstRect.y = ry;
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
                                dr.Draw();
                        }
                    }
            }

            //foreach (Drawable dr in Children)
            //    dr.Draw(rx, ry, rangle);

            Buffer.Display();
            Screen.CurrentTarget = Screen.Buffer;
            // TODO Optimize!
            Screen.Buffer.Draw(new SFML.Graphics.Sprite(Buffer.Texture));
		}
		
		public void Insert(Drawable dr)
		{
			int index = Children.FindIndex(item => item.ZOrder > dr.ZOrder);
			index = (index >= 0) ? index : Children.Count;
            dr.Layer = Name;

			Children.Insert(index, dr);

            if (Chunked)
                Chunks.InsertAt(dr, dr.X, dr.Y);
		}

        ~Layer()
        {
            Buffer.Dispose();
        }
	}
}

