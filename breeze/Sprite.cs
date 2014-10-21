using System;
using System.Xml;
using System.Collections.Generic;
using SDL2;

namespace Breeze.Graphics
{
	public class Sprite : Drawable
	{
		protected List<Resources.Sprite> Images;
		public double AnimSpeed;
		public bool Animated;
		protected double Time;
		protected int FCurrentFrame;
		protected int FCurrentImage;
		protected SDL.SDL_Rect SrcRect;
		
		public int CurrentImage
		{
			get { return FCurrentImage;	}
			set
			{
				Time = 0;
				FCurrentImage = value;
				SetAlpha(FAlpha);
				CurrentFrame = 0;
			}
		}
		
		public int CurrentFrame
		{
            get { return FCurrentFrame; }
			set
			{
				FCurrentFrame = value;
				SrcRect = new SDL.SDL_Rect();
				SrcRect.x = (int)(W * (FCurrentFrame % Images[FCurrentImage].Cols));
				SrcRect.y = (int)(H * (FCurrentFrame / Images[FCurrentImage].Cols));
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
		
		protected override void SetAlpha(byte alpha)
		{
			SDL.SDL_SetTextureAlphaMod(Images[FCurrentImage].Texture, alpha);
		}

        protected override void SetBlendMode(SDL.SDL_BlendMode mode)
        {
            SDL.SDL_SetTextureBlendMode(Images[FCurrentImage].Texture, mode);
        }
		
		public Sprite(int zorder = 0) : base(zorder)
		{
			Images = new List<Resources.Sprite>();
			FCurrentImage = 0;
			AnimSpeed = 1;
			Animated = false;
		}
		
		public Sprite(string name, int zorder = 0) : this(zorder)
		{
			Images.Add(Resources.Manager.FindSprite(name));
			Animated = Animated || Images[0].Animated;
			Start();
		}
		
		public Sprite(List<string> names, int zorder = 0) : this(zorder)
		{
			if (names.Count == 0)
				return;
			foreach (string fn in names)
			{
				Images.Add(Resources.Manager.FindSprite(fn));
				Animated = Animated || Images[Images.Count - 1].Animated;
			}
			Start();
		}

        public void AddImage(string name)
        {
            Images.Add(Resources.Manager.FindSprite(name));
        }
		
		protected override void InternalDraw(int x, int y, double angle)
		{
            SDL.SDL_RenderCopyEx(BreezeCore.Renderer, Images[FCurrentImage].Texture, ref SrcRect, ref DstRect, angle, ref RotationCenter, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
		}
		
		protected void Animate(object sender, TimerEventArgs e)
		{
			Time += e.Interval * AnimSpeed;
			
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

        ~Sprite()
        {
            if (Animated)
                BreezeCore.OnAnimate -= Animate;
        }
	}
}

