using System;
using System.Collections.Generic;
using SDL2;

namespace Breeze.Graphics
{
	public abstract class Drawable
	{
		public int X;
		public int Y;
		public int W { get; protected set; }
		public int H { get; protected set; }
		public double Angle;
		public byte Alpha;
		public double FScale;
		public double RotationCenterX;
		public double RotationCenterY;
		public int ZOrder { get; protected set; }
		protected SDL.SDL_Point RotationCenter;
		protected List<Drawable> Children;
		
		public double Scale
		{
			get { return FScale; }
			set { FScale = value; CalculateRotationCenter(); }
		}
		
		public Drawable(int zorder = 0)
		{
			Children = new List<Drawable>();
			RotationCenter = new SDL.SDL_Point();
			FScale = 1;
			Alpha = 0xff;
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
		
		protected abstract void InternalDraw(int x, int y, double angle);
		
		public virtual void Draw(int dx, int dy, double dangle)
		{
			int rx = X + dx;
			int ry = Y + dy;
			double rangle = Angle + dangle;
			
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

