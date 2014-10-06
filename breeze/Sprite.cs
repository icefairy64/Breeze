using System;
using System.Xml;
using System.Collections.Generic;
using SDL2;

namespace Breeze
{
	public class Sprite : Drawable
	{
		public List<Resources.Sprite> Images;
		public double AnimSpeed;
		public bool Animated;
		protected double Time;
		protected int FCurrentFrame;
		protected int FCurrentImage;
		protected SDL.SDL_Rect SrcRect;
		
		public int CurrentImage
		{
			get
			{
				return FCurrentImage;
			}
			set
			{
				Time = 0;
				FCurrentImage = value;
				CurrentFrame = 0;
			}
		}
		
		public int CurrentFrame
		{
			get
			{
				return FCurrentFrame;
			}
			set
			{
				FCurrentFrame = value;
				SrcRect = new SDL.SDL_Rect();
				SrcRect.x = (int)(W * (FCurrentFrame % Images[CurrentImage].Cols));
				SrcRect.y = (int)(H * (FCurrentFrame / Images[CurrentImage].Cols));
				SrcRect.w = W;
				SrcRect.h = H;
			}
		}
		
		public void Start()
		{
			W = Images[0].W;
			H = Images[0].H;
			CalculateRotationCenter(0.5, 0.5);
			CurrentImage = 0;
			
			if (Animated)
				BreezeCore.OnAnimate += Animate;
		}
		
		public Sprite() : base()
		{
			Images = new List<Resources.Sprite>();
			FCurrentImage = 0;
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
			drect.w = (int)(W * Scale);
			drect.h = (int)(H * Scale);
			
			SDL.SDL_RenderCopyEx(BreezeCore.Renderer, Images[FCurrentImage].Texture, ref SrcRect, ref drect, angle, ref RotationCenter, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
		}
		
		protected void Animate(object sender, TimerEventArgs e)
		{
			Time += e.Interval;
			
			if (Time >= Images[FCurrentImage].FrameIntervals[FCurrentFrame])
			{
				// Preparing for frame switch
				FCurrentFrame++;
				
				if (FCurrentFrame == Images[FCurrentImage].FrameCount)
				{
					Time -= Images[FCurrentImage].FrameIntervals[FCurrentFrame - 1];
					CurrentFrame = 0;
				}
				else  // Performing frame switch
					CurrentFrame = FCurrentFrame;
			}
		}
	}
}

