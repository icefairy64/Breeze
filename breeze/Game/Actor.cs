using System;

namespace Breeze.Game
{
    public class Saveable : Attribute
    {
    }

    public partial class Actor
    {
        public Graphics.Drawable Image;
        public string Name;
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

