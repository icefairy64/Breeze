using System;
using System.Xml;
using System.Collections.Generic;
using SDL2;
using Breeze.Resources;

namespace Breeze.Graphics
{
	public class Sprite : Drawable
	{
		protected List<Resources.SpriteSheet> Sheets;
		public double AnimSpeed;
		public bool Animated;
		protected double Time;
		protected int FCurrentFrame;
		protected int FCurrentSheet;
		protected SDL.SDL_Rect SrcRect;
        protected string Base = "";
		
		public int CurrentSheet
		{
			get { return FCurrentSheet;	}
			set
			{
				Time = 0;
				FCurrentSheet = value;
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
				SrcRect.x = (int)(W * (FCurrentFrame % Sheets[FCurrentSheet].Cols));
				SrcRect.y = (int)(H * (FCurrentFrame / Sheets[FCurrentSheet].Cols));
				SrcRect.w = W;
				SrcRect.h = H;
			}
		}
		
		public void Start()
		{
			W = Sheets[0].W;
			H = Sheets[0].H;
			CalculateRotationCenter(0.5, 0.5);
			CurrentSheet = 0;
			
			if (Animated)
				BreezeCore.OnAnimate += Animate;
		}
		
		protected override void SetAlpha(byte alpha)
		{
			SDL.SDL_SetTextureAlphaMod(Sheets[FCurrentSheet].Texture, alpha);
		}

        protected override void SetBlendMode(SDL.SDL_BlendMode mode)
        {
            SDL.SDL_SetTextureBlendMode(Sheets[FCurrentSheet].Texture, mode);
        }
		
		public Sprite(int zorder = 0) : base(zorder)
		{
			Sheets = new List<Resources.SpriteSheet>();
			FCurrentSheet = 0;
			AnimSpeed = 1;
			Animated = false;
		}
		
		public Sprite(string name, int zorder = 0) : this(ResourceManager.Find<SpriteBase>(name).Sheets, zorder)
		{
            Base = name;
		}
		
		public Sprite(string[] sheets, int zorder = 0) : this(zorder)
		{
			if (sheets.Length == 0)
				return;
			for (int i = 0; i < sheets.Length; i++)
            {
				Sheets.Add(ResourceManager.Find<SpriteSheet>(sheets[i]));
				Animated = Animated || Sheets[Sheets.Count - 1].Animated;
			}
			Start();
		}

        public void AddSheet(string name)
        {
            Sheets.Add(ResourceManager.Find<SpriteSheet>(name));
        }
		
		protected override void InternalDraw(int x, int y, double angle)
		{
            SDL.SDL_RenderCopyEx(BreezeCore.Renderer, Sheets[FCurrentSheet].Texture, ref SrcRect, ref DstRect, angle, ref RotationCenter, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
		}
		
		protected void Animate(object sender, TimerEventArgs e)
		{
			Time += e.Interval * AnimSpeed;

            if (Sheets[FCurrentSheet].Animated == false)
                return;

			if (Time >= Sheets[FCurrentSheet].FrameIntervals[FCurrentFrame])
			{
				// Preparing for frame switch
				FCurrentFrame++;
				
				if (FCurrentFrame == Sheets[FCurrentSheet].FrameCount)
				{
					Time -= Sheets[FCurrentSheet].FrameIntervals[FCurrentFrame - 1];
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

        public override string ToString()
        {
            if (Base != "")
                return Base;
            else
            {
                string tmp = "!";
                foreach (SpriteSheet spr in Sheets)
                    tmp += spr.Name + ";";
                return tmp;
            }
        }
	}
}

