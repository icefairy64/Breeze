using System;
using System.Xml;
using System.Collections.Generic;
using SDL2;

namespace Breeze
{
	public class Sprite : Drawable
	{
		protected List<Resources.Sprite> Images;
		public double AnimSpeed;
		public bool Animated;
		SDL.SDL_Rect FrameRect;
		protected double Time;
		protected int CurrentFrame;
		protected int FCurrentImage;
		
		public int CurrentImage
		{
			get
			{
				return FCurrentImage;
			}
			set
			{
				Time = 0;
				CurrentFrame = 0;
				FCurrentImage = value;
			}
		}
		
		public void Start()
		{
			W = Images[0].W;
			H = Images[0].H;
			if (Animated)
				BreezeCore.OnAnimate += Animate;
		}
		
		public Sprite() : base()
		{
			Images = new List<Resources.Sprite>();
			Time = 0;
			CurrentFrame = 0;
			AnimSpeed = 1;
			Animated = false;
		}
		
		public Sprite(string filename) : this()
		{
			Images.Add(new Resources.Sprite(filename));
			Animated = Animated || Images[0].Animated;
			Start();
		}
		
		public Sprite(List<string> filenames) : this()
		{
			if (filenames.Count == 0)
				return;
			foreach (string fn in filenames)
			{
				Images.Add(new Resources.Sprite(fn));
				Animated = Animated || Images[Images.Count - 1].Animated;
			}
			Start();
		}
		
		protected override void InternalDraw(int x, int y, double angle)
		{
			SDL.SDL_Rect drect = new SDL.SDL_Rect();
			drect.x = x;
			drect.y = y;
			drect.w = W;
			drect.h = H;
			
			SDL.SDL_RenderCopyEx(BreezeCore.Renderer, Images[FCurrentImage].Texture, ref FrameRect, ref drect, angle, ref RotationCenter, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
		}
		
		protected void Animate(object sender, TimerEventArgs e)
		{
			Time += e.Interval;
		}
	}
}

