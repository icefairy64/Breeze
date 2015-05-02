using System;
using Breeze.Graphics;

namespace Breeze.Game
{
    [Serializable]
    public partial class Entity
    {
        [Editable]
        public Drawable Image;
       
        public string CommonName { get; protected set; }

        protected int FInstanceID;
        protected double FX, FY;
        protected string FName;

        [Editable]
        public string Name
        {
            get { return FName; }
            set 
            {
                if (!String.IsNullOrEmpty(FName))
                    Core.World.RenameEntity(FName, value);
                FName = value;
            }
        }

        public int InstanceID
        {
            get { return FInstanceID; }
            set
            {
                FInstanceID = value;
                Name = InstanceStr;
            }
        }

        public string InstanceStr
        {
            get { return CommonName + FInstanceID; }
        }

        [Editable]
        public double X
        {
            get { return FX; }
            set
            {
                FX = value;
                Image.X = (int)value;
            }
        }

        [Editable]
        public double Y
        {
            get { return FY; }
            set
            {
                FY = value;
                Image.Y = (int)value;
            }
        }

        public Entity()
        {
            
        }

        public Entity(Drawable image)
            : this()
        {
            Image = image;
        }

        ~Entity()
        {
            if (Image != null)
                Image.Layer.RemoveChild(Image);
        }
    }
}

