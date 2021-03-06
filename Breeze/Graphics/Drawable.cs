using System;
using System.Collections.Generic;
using SFML.Graphics;

namespace Breeze.Graphics
{
	public abstract class Drawable
	{
		protected int FX, FY, FW, FH;
        
        // Dimension properties

        public virtual int X
        {
            get { return FX; }
            set 
            {
                Transform.Translate((value - FX) / FScale, 0);
                FX = value;
            }
        }
        public virtual int Y
        {
            get { return FY; }
            set 
            {
                Transform.Translate(0, (value - FY) / FScale);
                FY = value;
            }
        }

        public virtual int W
        {
            get { return FW; }
            set { FW = value; }
        }

        public virtual int H
        {
            get { return FH; }
            set { FH = value; }
        }

        public float Angle
        {
            get { return FAngle; }
            set
            {
                Transform.Rotate(value - FAngle, RotationCenterX, RotationCenterY);
                FAngle = value;
            }
        }

        public float Scale
        {
            get { return FScale; }
            set
            {
                Transform.Scale(value / FScale, value / FScale);
                FScale = value;
            }
        }

		public byte Alpha
		{
			get	{ return FAlpha; }
			set 
            { 
                FAlpha = value; 
                Color.A = value; 
            }
		}

        public BlendMode BlendMode
        {
            get { return States.BlendMode; }
            set { States.BlendMode = value; }
        }

        public Shader Shader
        {
            get { return States.Shader; }
            set { States.Shader = value; }
        }

		public int ZOrder { get; protected set; }
        public float RotationCenterX = 0.5f;
        public float RotationCenterY = 0.5f;
        public Color Color;

		protected List<Drawable> Children;
        protected Transform Transform;
        protected RenderStates States;
        protected float FAngle;
        protected byte FAlpha;
        protected float FScale;

        public Layer Layer;

        // Methods

		protected Drawable(int zorder = 0)
		{
			Children = new List<Drawable>();
            Transform = Transform.Identity;
            States = new RenderStates();
            States.BlendMode = BlendMode.Alpha;
            Color = new Color(0xff, 0xff, 0xff, 0xff);
			FScale = 1;
			FAlpha = 0xff;
			ZOrder = zorder;
		}
		
		protected Drawable(int w, int h, int zorder = 0)
            : this(zorder)
		{
			W = w;
			H = h;
		}
		
		protected Drawable(int x, int y, int w, int h, int zorder = 0)
            : this(w, h, zorder)
		{
			X = x;
			Y = y;
		}
		
        protected abstract void InternalDraw(Transform tf);

        public virtual void Draw(Transform tf)
        {
            InternalDraw(Transform * tf);

            foreach (Drawable dr in Children)
                dr.Draw(Transform * tf);
        }
		
        public virtual void Draw()
		{
            Draw(SFML.Graphics.Transform.Identity);
		}
		
		public void AddChild(Drawable dr)
		{
			Children.Add(dr);
		}
		
		public void RemoveChild(Drawable dr)
		{
			Children.Remove(dr);
		}
	}
}

