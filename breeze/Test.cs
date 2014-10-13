using System;
using Breeze.Graphics;
using Breeze.Resources;
using SDL2;

namespace Breeze
{
	class TestClass
	{		
		static Graphics.Sprite spr;
		static Graphics.Layer layer;
        static Graphics.Text txt;
        static int frames = 0;
		
		static int ProcessEvents(SDL.SDL_Event ev)
		{
			switch (ev.type)
			{
				case SDL.SDL_EventType.SDL_KEYDOWN:
					if (ev.key.keysym.sym == SDL.SDL_Keycode.SDLK_ESCAPE)
						return 1;
					break;
				
			}
			return 0;
		}
		
		static uint Timer(uint interval, IntPtr param)
		{
			SDL.SDL_Event ev = new SDL.SDL_Event();
			ev.type = SDL.SDL_EventType.SDL_USEREVENT;
			SDL.SDL_PushEvent(ref ev);
			
			spr.Angle += 0.3;
			
			return 1;
		}	

        static void OnDraw(IntPtr renderer)
        {
            frames++;
            txt.Value = String.Format("Frames: {0}", frames);
        }
		
		static void OnInit()
		{
            layer = Graphics.Screen.CreateLayer("front");
			layer.Alpha = 0xff;
			
			Resources.Manager.LoadSprite("lkun.bspr");
            Resources.Manager.LoadFont("hammersmithone.ttf", 24);

			spr = new Graphics.Sprite("lkun");
			spr.Scale = 3;
			spr.X = 320 - spr.W * 3 / 2;
			spr.Y = 120;
			spr.AnimSpeed = 1;
			layer.Insert(spr);

            txt = new Graphics.Text(Resources.Manager.FindFont("hammersmithone24"));
            txt.Value = "L-Kun uses BREEZE v0.1!";
            txt.X = 100;
            txt.Y = 60;
            layer.Insert(txt);
			
			//SDL.SDL_AddTimer(20, Timer, IntPtr.Zero);
		}
		
		public static void Main(string[] args)
		{
			BreezeCore.OnEvent = ProcessEvents;
			BreezeCore.OnInit = OnInit;
            BreezeCore.OnDraw = OnDraw;
			
			BreezeCore.Init("Breeze", 640, 480);
			BreezeCore.Start();
			BreezeCore.Finish();
		}
	}
}
