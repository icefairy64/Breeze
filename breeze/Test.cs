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
        static bool[] keystate = new bool[4];
		
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

            if (keystate[0])
                Graphics.Screen.CamY -= 1;
            if (keystate[1])
                Graphics.Screen.CamX += 1;
            if (keystate[2])
                Graphics.Screen.CamY += 1;
            if (keystate[3])
                Graphics.Screen.CamX -= 1;

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
			
			SDL.SDL_AddTimer(20, Timer, IntPtr.Zero);
		}

        static void OnKey(SDL.SDL_KeyboardEvent ev)
        {
            bool newstate = ev.state == 1;
            switch (ev.keysym.sym)
            {
                case SDL.SDL_Keycode.SDLK_UP:
                    keystate[0] = newstate;
                    break;
                case SDL.SDL_Keycode.SDLK_RIGHT:
                    keystate[1] = newstate;
                    break;
                case SDL.SDL_Keycode.SDLK_DOWN:
                    keystate[2] = newstate;
                    break;
                case SDL.SDL_Keycode.SDLK_LEFT:
                    keystate[3] = newstate;
                    break;
                case SDL.SDL_Keycode.SDLK_ESCAPE:
                    BreezeCore.Exit = true;
                    break;
            }
        }
		
		public static void Main(string[] args)
		{
			BreezeCore.OnEvent = ProcessEvents;
			BreezeCore.OnInit = OnInit;
            BreezeCore.OnDraw = OnDraw;
            BreezeCore.OnKey = OnKey;
			
			BreezeCore.Init("Breeze", 640, 480);
			BreezeCore.Start();
			BreezeCore.Finish();
		}
	}
}
