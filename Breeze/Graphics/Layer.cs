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
        protected SFML.Graphics.Sprite Spr;
        protected bool NeedsRendering;

        [Editable("Scroll speed")]
        public float ScrollSpeed = 1.0f;
        [Editable]
        public Layer[] Sources;
        [Editable]
        public bool Bypass;
        [Editable]
        public bool Visible = true;

        public View View;
        public bool Chunked { get; protected set; }
        public string Name { get; protected set; }

        public float Zoom 
        {
            get { return FZoom; }
            set
            {
                View.Zoom(FZoom / value);
                FZoom = value;
            }
        }

        public Layer(string name, int zorder = 0, bool chunked = true)
            : base(0, 0, (int)Core.ScrW, (int)Core.ScrH, zorder)
		{
            Buffer = new RenderTexture((uint)FW, (uint)FH);
            View = new View(new FloatRect(0, 0, FW, FH));
            SrcRect = new FloatRect(0, 0, FW, FH);
            Spr = new SFML.Graphics.Sprite(Buffer.Texture);

            States.Transform = Transform.Identity;
            Spr.Color = Color;

            if (chunked)
                Chunks = new DrawableChunkCollection(256);

            Chunked = chunked;
			Name = name;
		}
		
        protected override void InternalDraw(Transform tf)
		{
            // Do nothing
		}
		
        public override void Draw()
		{
            if (!NeedsRendering)
                goto Drawing;

            var prevTarget = Screen.Target;
            Screen.Target = Buffer;
            Buffer.Clear(Color.Transparent);
            Buffer.SetView(View);
            Spr.Color = Color;
			
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
                //Console.WriteLine("{0} {1} {2}", ChunkValidateCounter, rx, ry);

                Chunks.ValidateChunkAt(
                    ((ChunkValidateCounter) / (ChunksInRow + OuterRenderRadius)) * Chunks.ChunkSize + rx, 
                    ((ChunkValidateCounter) % (ChunksInRow + OuterRenderRadius)) * Chunks.ChunkSize + ry);

                ChunkValidateCounter++;
                if (ChunkValidateCounter == (ChunksInRow + OuterRenderRadius) * (ChunksInColumn + OuterRenderRadius))
                    ChunkValidateCounter = -OuterRenderRadius * (ChunksInRow + OuterRenderRadius) - OuterRenderRadius;

                for (int x = -OuterRenderRadius; x <= ChunksInRow + OuterRenderRadius; x++)
                    for (int y = -OuterRenderRadius; y <= ChunksInColumn + OuterRenderRadius; y++)
                    {
                        var chunk = Chunks.GetChunkAt(x * Chunks.ChunkSize + rx, y * Chunks.ChunkSize + ry);

                        if (chunk != null)
                        {
                            foreach (Drawable dr in chunk)
                                dr.Draw();
                        }
                    }
            }
                
            Buffer.Display();
            Screen.Target = prevTarget;
            NeedsRendering = false;

            Drawing:
            Screen.Target.Draw(Spr, States);
		}
		
		public void Insert(Drawable dr)
		{
			int index = Children.FindIndex(item => item.ZOrder > dr.ZOrder);
			index = (index >= 0) ? index : Children.Count;
            dr.Layer = this;

			Children.Insert(index, dr);

            if (Chunked)
                Chunks.InsertAt(dr, dr.X, dr.Y);
		}

        public void Reset()
        {
            NeedsRendering = true;

            if (Sources != null)
            {
                foreach (Layer lr in Sources)
                    lr.Reset();
            }
        }

        ~Layer()
        {
            Buffer.Dispose();
        }
	}
}

