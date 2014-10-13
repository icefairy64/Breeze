using System;

namespace Breeze.Game
{
    public class Actor
    {
        public Graphics.Drawable Image;
        public string Name { get; protected set; }
        public int InstanceID;
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

        public Actor(string name)
        {
            Name = name;
        }

        public Actor(string name, Graphics.Drawable image) : this(name)
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

