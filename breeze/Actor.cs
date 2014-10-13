using System;

namespace Breeze.Game
{
    public class Actor
    {
        public Graphics.Drawable Image;
        protected int FX, FY;

        public int X
        {
            get { return FX; }
            set
            {
                FX = value;
                Image.X = FX;
            }
        }

        public int Y
        {
            get { return FY; }
            set
            {
                FY = value;
                Image.Y = FY;
            }
        }

        public Actor()
        {
        }

        public Actor(Graphics.Drawable image) : this()
        {
            Image = image;
        }

        ~Actor()
        {
            if (Image != null)
                Graphics.Screen.FindLayer(Image.Layer).RemoveChild(Image);
        }
    }
}

