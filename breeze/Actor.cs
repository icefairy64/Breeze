using System;

namespace Breeze.Game
{
    public class Actor
    {
        public Graphics.Drawable Image;
        public string Name { get; protected set; }
        public int InstanceID;
        protected double FX, FY;

        public string InstanceStr
        {
            get { return Name + InstanceID.ToString(); }
        }

        public double X
        {
            get { return FX; }
            set
            {
                FX = value;
                Image.X = (int)value;
            }
        }

        public double Y
        {
            get { return FY; }
            set
            {
                FY = value;
                Image.Y = (int)value;
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

