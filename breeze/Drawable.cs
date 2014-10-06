using System;
using System.Collections.Generic;
using SDL2;

namespace Breeze
{
	public abstract class Drawable
	{
		public int X;
		public int Y;
		public int W { get; protected set; }
		public int H { get; protected set; }
		public double Angle;
		public byte Alpha;
		public double Scale;
		public SDL.SDL_Point RotationCenter;
		List<Drawable> Children;
		
		public Drawable()
		{
			Children = new List<Drawable>();
		}
		
		public Drawable(int w, int h) : this()
		{
			W = w;
			H = h;
		}
		
		public Drawable(int x, int y, int w, int h) : this(w, h)
		{
			X = x;
			Y = y;
		}
		
		protected abstract void InternalDraw(int x, int y, double angle);
		
		public void Draw(int dx, int dy, double dangle)
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
	}
}

