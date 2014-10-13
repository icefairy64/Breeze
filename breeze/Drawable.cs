using System;
using System.Collections.Generic;
using SDL2;

namespace Breeze.Graphics
{
	public abstract class Drawable
	{
		protected int FX, FY, FW, FH;
        
        // Dimension properties
        public int X
        {
            get { return FX; }
            set {
                FX = value;
                CalculateDestRect();
            }
        }
        public int Y
        {
            get { return FY; }
            set {
                FY = value;
                CalculateDestRect();
            }
        }
        public int W
        {
            get { return FW; }
            protected set {
                FW = value;
                CalculateDestRect();
            }
        }
        public int H
        {
            get { return FH; }
            protected set {
                FH = value;
                CalculateDestRect();
            }
        }

		public double Angle;
		protected byte FAlpha;

		public byte Alpha
		{
			get	{ return FAlpha; }
			set { FAlpha = value; SetAlpha(value); }
		}

		public double FScale;
		public double RotationCenterX;
		public double RotationCenterY;
		public int ZOrder { get; protected set; }
		protected SDL.SDL_Point RotationCenter;
		protected List<Drawable> Children;
        protected SDL.SDL_Rect DstRect;
        public string Layer;
        public SDL.SDL_BlendMode BlendMode
        {
            set
            {
                SetBlendMode(value);
            }
        }
		
		public double Scale
		{
			get { return FScale; }
			set { FScale = value; CalculateRotationCenter(); }
		}

        // Methods

        void CalculateDestRect()
        {
            DstRect.x = FX;
            DstRect.y = FY;
            DstRect.w = (int)(FW * Scale);
            DstRect.h = (int)(FH * Scale);
        }

		public Drawable(int zorder = 0)
		{
			Children = new List<Drawable>();
            DstRect = new SDL.SDL_Rect();
			RotationCenter = new SDL.SDL_Point();
			Scale = 1;
			FAlpha = 0xff;
			ZOrder = zorder;
		}
		
		public Drawable(int w, int h, int zorder = 0) : this(zorder)
		{
			W = w;
			H = h;
		}
		
		public Drawable(int x, int y, int w, int h, int zorder = 0) : this(w, h, zorder)
		{
			X = x;
			Y = y;
		}
		
		protected abstract void SetAlpha(byte alpha);
        protected abstract void SetBlendMode(SDL.SDL_BlendMode mode);
		
		protected abstract void InternalDraw(int x, int y, double angle);
		
		public virtual void Draw(int dx, int dy, double dangle)
		{
			int rx = X + dx;
			int ry = Y + dy;
			double rangle = Angle + dangle;

            DstRect.x = rx;
            DstRect.y = ry;
			InternalDraw(rx, ry, rangle);

			foreach (Drawable dr in Children)
				dr.Draw(rx, ry, rangle);
		}
		
		public void Draw()
		{
			Draw(0, 0, 0);
		}
		
		public void AddChild(Drawable dr)
		{
			Children.Add(dr);
		}
		
		public void RemoveChild(Drawable dr)
		{
			Children.Remove(dr);
		}
		
		protected void CalculateRotationCenter()
		{
			RotationCenter.x = (int)(W * RotationCenterX * FScale);
			RotationCenter.y = (int)(H * RotationCenterX * FScale);
		}
		
		protected void CalculateRotationCenter(double x, double y)
		{
			RotationCenterX = x;
			RotationCenterY = y;
			CalculateRotationCenter();
		}
	}
}

