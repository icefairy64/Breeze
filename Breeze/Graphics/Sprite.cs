using System;
using System.Xml;
using System.Collections.Generic;
using Breeze.Resources;
using SFML.Graphics;

namespace Breeze.Graphics
{
    [Serializable]
	public class Sprite : Drawable
	{
		public double AnimSpeed;
		public bool Animated;

        [NonSerialized]
        protected double Time;
        [NonSerialized]
        protected int FCurrentFrame;
        [NonSerialized]
        protected int FCurrentSheet;
        [NonSerialized]
        protected IntRect SrcRect;
        [NonSerialized]
        protected SFML.Graphics.Sprite Spr;

        protected string Base = "";
        protected List<SpriteSheet> Sheets;

		public int CurrentSheet
		{
			get { return FCurrentSheet;	}
			set
			{
				Time = 0;
				FCurrentSheet = value;
				CurrentFrame = 0;
			}
		}
		
		public int CurrentFrame
		{
            get { return FCurrentFrame; }
			set
			{
				FCurrentFrame = value;
                SrcRect.Left = (int)(W * (FCurrentFrame % Sheets[FCurrentSheet].Cols));
                SrcRect.Top = (int)(H * (FCurrentFrame / Sheets[FCurrentSheet].Cols));
                SrcRect.Width = W;
                SrcRect.Height = H;
			}
		}
		
        protected static SpriteSheet[] GetSheets(string[] sheets)
        {
            var tmp = new SpriteSheet[sheets.Length];
            for (int i = 0; i < sheets.Length; i++)
                tmp[i] = ResourceManager.Find<SpriteSheet>(sheets[i]);

            return tmp;
        }

		public void Start()
		{
			W = Sheets[0].W;
			H = Sheets[0].H;
			CurrentSheet = 0;
			
			if (Animated)
				Core.OnAnimate += Animate;
		}
		
		public Sprite(int zorder = 0) 
            : base(zorder)
		{
			Sheets = new List<SpriteSheet>();
            Spr = new SFML.Graphics.Sprite();
            SrcRect = new IntRect();
			FCurrentSheet = 0;
			AnimSpeed = 1;
			Animated = false;
		}
		
        public Sprite(string[] sheets, int zorder = 0)
            : this(GetSheets(sheets), zorder)
        {
        }

        public Sprite(SpriteSheet[] sheets, int zorder = 0) 
            : this(zorder)
		{
			if (sheets.Length == 0)
				return;

			for (int i = 0; i < sheets.Length; i++)
            {
				Sheets.Add(sheets[i]);
				Animated = Animated || Sheets[Sheets.Count - 1].Animated;
			}

			Start();
		}

        public Sprite(string baseName, int zorder = 0) 
            : this(ResourceManager.Find<SpriteBase>(baseName).Sheets, zorder)
        {
            Base = baseName;
        }

        public void AddSheet(string name)
        {
            Sheets.Add(ResourceManager.Find<SpriteSheet>(name));
        }

        protected override void InternalDraw(Transform tf)
        {
            States.Transform = tf;
            Spr.Texture = Sheets[FCurrentSheet].Texture;
            Spr.TextureRect = SrcRect;
            Spr.Color = Color;
            Screen.Target.Draw(Spr, States);
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
                Core.OnAnimate -= Animate;
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

