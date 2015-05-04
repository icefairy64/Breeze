using System;
using System.Linq;
using SFML.Graphics;
using Breeze.Resources;

namespace Breeze.Graphics
{
    public class Tile : Drawable
    {
        protected TileBase Base;
        protected uint Index;
        protected VertexArray VertArray;
        protected bool[] DrawPart;

        public override int X
        {
            get { return base.X; }
            set
            {
                base.X = value;
                RebuildArray();
            }
        }

        public override int Y
        {
            get { return base.Y; }
            set
            {
                base.Y = value;
                RebuildArray();
            }
        }

        public override int W
        {
            get { return base.W; }
            set
            {
                base.W = value;
                RebuildArray();
            }
        }

        public override int H
        {
            get { return base.H; }
            set
            {
                base.H = value;
                RebuildArray();
            }
        }

        public bool this[int i]
        {
            get { return DrawPart[i]; }
            set
            {
                if (DrawPart[i] == value)
                    return;

                DrawPart[i] = value;
                RebuildArray();
            }
        }

        public bool DrawAsNinePatch
        {
            get { return DrawPart.All(x => x); }
            set
            {
                if (DrawAsNinePatch == value)
                    return;

                if (Base.Tiles.Length <= Index + 8)
                    throw new BreezeException("Tried to use non-existing tile index");

                DrawPart[0] = true;
                for (int i = 1; i < 9; i++)
                    DrawPart[i] = value;
                RebuildArray();
            }
        }

        public bool DrawCenter
        {
            get { return DrawPart[0]; }
            set 
            { 
                DrawPart[0] = value; 
                RebuildArray();
            }
        }

        public bool DrawTop
        {
            get { return DrawPart[2]; }
            set
            {
                if (DrawTop == value)
                    return;

                if (Base.Tiles.Length <= Index + 3)
                    throw new BreezeException("Tried to use non-existing tile index");
                    
                DrawPart[1] = value && DrawLeft;
                DrawPart[2] = value;
                DrawPart[3] = value && DrawRight;
                RebuildArray();
            }
        }

        public bool DrawRight
        {
            get { return DrawPart[4]; }
            set
            {
                if (DrawRight == value)
                    return;

                if (Base.Tiles.Length <= Index + 5)
                    throw new BreezeException("Tried to use non-existing tile index");

                DrawPart[3] = value && DrawTop;
                DrawPart[4] = value;
                DrawPart[5] = value && DrawBottom;
                RebuildArray();
            }
        }

        public bool DrawBottom
        {
            get { return DrawPart[6]; }
            set
            {
                if (DrawBottom == value)
                    return;

                if (Base.Tiles.Length <= Index + 7)
                    throw new BreezeException("Tried to use non-existing tile index");

                DrawPart[5] = value && DrawRight;
                DrawPart[6] = value;
                DrawPart[7] = value && DrawLeft;
                RebuildArray();
            }
        }

        public bool DrawLeft
        {
            get { return DrawPart[8]; }
            set
            {
                if (DrawLeft == value)
                    return;

                if (Base.Tiles.Length <= Index + 8)
                    throw new BreezeException("Tried to use non-existing tile index");

                DrawPart[7] = value && DrawBottom;
                DrawPart[8] = value;
                DrawPart[1] = value && DrawTop;
                RebuildArray();
            }
        }

        protected void Fill(IntRect tile, int x, int y, int w, int h)
        {
            var tex = new SFML.System.Vector2f[4];
            int wm = w % tile.Width;
            int hm = h % tile.Height;
            tex[0] = new SFML.System.Vector2f(tile.Left, tile.Top);
            tex[1] = new SFML.System.Vector2f(tile.Left + tile.Width, tile.Top);
            tex[2] = new SFML.System.Vector2f(tile.Left + tile.Width, tile.Top + tile.Height);
            tex[3] = new SFML.System.Vector2f(tile.Left, tile.Top + tile.Height);

            for (int i = 0; i < Math.Ceiling((float)w / tile.Width); i++)
            {
                for (int j = 0; j < Math.Ceiling((float)h / tile.Height); j++)
                {
                    VertArray.Append(new Vertex(new SFML.System.Vector2f(x + i * tile.Width, y + j * tile.Height), tex[0]));
                    VertArray.Append(new Vertex(new SFML.System.Vector2f(x + (i + 1) * tile.Width, y + j * tile.Height), tex[1]));
                    VertArray.Append(new Vertex(new SFML.System.Vector2f(x + (i + 1) * tile.Width, y + (j + 1) * tile.Height), tex[2]));
                    VertArray.Append(new Vertex(new SFML.System.Vector2f(x + i * tile.Width, y + (j + 1) * tile.Height), tex[3]));
                }
                if (hm != 0)
                {
                    VertArray.Append(new Vertex(new SFML.System.Vector2f(x + i * tile.Width, y + h - hm), tex[0]));
                    VertArray.Append(new Vertex(new SFML.System.Vector2f(x + (i + 1) * tile.Width, y + h - hm), tex[1]));
                    VertArray.Append(new Vertex(new SFML.System.Vector2f(x + (i + 1) * tile.Width, y + h), new SFML.System.Vector2f(tile.Left + tile.Width, tile.Top + hm)));
                    VertArray.Append(new Vertex(new SFML.System.Vector2f(x + i * tile.Width, y + h), new SFML.System.Vector2f(tile.Left, tile.Top + hm)));
                }
            }

            if (wm != 0)
                for (int j = 0; j < Math.Ceiling((float)h / tile.Height); j++)
                {
                    VertArray.Append(new Vertex(new SFML.System.Vector2f(x + w - wm, y + j * tile.Height), tex[0]));
                    VertArray.Append(new Vertex(new SFML.System.Vector2f(x + w, y + j * tile.Height), new SFML.System.Vector2f(tile.Left + wm, tile.Top)));
                    VertArray.Append(new Vertex(new SFML.System.Vector2f(x + w, y + (j + 1) * tile.Height), new SFML.System.Vector2f(tile.Left + wm, tile.Top + tile.Height)));
                    VertArray.Append(new Vertex(new SFML.System.Vector2f(x + w - wm, y + (j + 1) * tile.Height), tex[3]));
                }

            if (wm != 0 && hm != 0)
            {
                VertArray.Append(new Vertex(new SFML.System.Vector2f(x + w - wm, y + h - hm), tex[0]));
                VertArray.Append(new Vertex(new SFML.System.Vector2f(x + w, y + h - hm), new SFML.System.Vector2f(tile.Left + wm, tile.Top)));
                VertArray.Append(new Vertex(new SFML.System.Vector2f(x + w, y + h), new SFML.System.Vector2f(tile.Left + wm, tile.Top + hm)));
                VertArray.Append(new Vertex(new SFML.System.Vector2f(x + w - wm, y + h), new SFML.System.Vector2f(tile.Left, tile.Top + hm)));
            }
        }

        protected void RebuildArray()
        {
            var x = new int[3];
            var y = new int[3];

            x[0] = FX;
            x[1] = DrawLeft || DrawRight ? x[0] + Base.Tiles[1].Width : x[0];
            x[2] = DrawLeft || DrawRight ? FX + FW - Base.Tiles[3].Width : FX + FW;
            y[0] = FY;
            y[1] = DrawTop || DrawBottom ? y[0] + Base.Tiles[1].Height : y[0];
            y[2] = DrawTop || DrawBottom ? FY + FH - Base.Tiles[5].Height : FY + FH;

            VertArray.Clear();
            if (DrawPart[0])
                Fill(Base.Tiles[0], x[1], y[1], x[2] - x[1], y[2] - y[1]);
            if (DrawPart[2])
                Fill(Base.Tiles[2], x[1], y[0], x[2] - x[1], Base.Tiles[2].Height);
            if (DrawPart[6])
                Fill(Base.Tiles[2], x[1], y[2], x[2] - x[1], Base.Tiles[6].Height);
            if (DrawPart[8])
                Fill(Base.Tiles[8], x[0], y[1], Base.Tiles[8].Width, y[2] - y[1]);
            if (DrawPart[4])
                Fill(Base.Tiles[4], x[2], y[1], Base.Tiles[4].Width, y[2] - y[1]);
            if (DrawPart[1])
                Fill(Base.Tiles[1], x[0], y[0], Base.Tiles[1].Width, Base.Tiles[1].Height);
            if (DrawPart[3])
                Fill(Base.Tiles[3], x[2], y[0], Base.Tiles[3].Width, Base.Tiles[3].Height);
            if (DrawPart[5])
                Fill(Base.Tiles[5], x[2], y[2], Base.Tiles[5].Width, Base.Tiles[5].Height);
            if (DrawPart[7])
                Fill(Base.Tiles[7], x[0], y[2], Base.Tiles[7].Width, Base.Tiles[7].Height);
        }

        public Tile(TileBase tile, uint index = 0, int zorder = 0)
            : base(zorder)
        {
            if (tile == null)
                throw new BreezeException("Tried to create tile with non-existing base");
            if (tile.Tiles.Length <= index)
                throw new BreezeException("Tried to create tile using non-existing tile index");

            Base = tile;
            Index = index;
            VertArray = new VertexArray(PrimitiveType.Quads);
            DrawPart = new bool[9];
            DrawPart[0] = true;
        }

        public Tile(string tile, uint index = 0, int zorder = 0)
            : this(ResourceManager.Find<TileBase>(tile), index, zorder)
        {
        }

        protected override void InternalDraw(Transform tf)
        {
            States.Transform = tf;
            Screen.Target.Draw(VertArray, States);
        }

        public override void Draw(Transform tf)
        {
            InternalDraw(tf);
        }
    }
}

