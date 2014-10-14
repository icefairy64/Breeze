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
        static uint lastint = 0;
		
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
            
            lastint = interval;

			return 1;
		}	

        static void OnDraw(IntPtr renderer)
        {
            frames++;
            txt.Value = String.Format("Frame: {0}", frames);
            //txt.Value = Resources.Manager.GetResourceList();
        }
		
		static void OnInit()
		{
			Resources.Manager.LoadSprite("lkun.bspr");
            Resources.Manager.LoadFont("hammersmithone.ttf", 24);

            layer = Graphics.Screen.CreateLayer("front", 1);
            layer.Alpha = 0xa0;
            layer.BlendMode = SDL.SDL_BlendMode.SDL_BLENDMODE_ADD;

            spr = new Graphics.Sprite("lkun");
			spr.Scale = 3;
            spr.X = (int)(BreezeCore.ScrW / 2 - spr.W * spr.Scale / 2);
            spr.Y = (int)(BreezeCore.ScrH / 2 - spr.H * spr.Scale / 2);
			spr.AnimSpeed = 1;
			layer.Insert(spr);

            layer = Graphics.Screen.CreateLayer("back");
            layer.ScrollSpeed = 0.7;
            layer.Alpha = 0xa0;

            txt = new Graphics.Text(Resources.Manager.FindFont("hammersmithone24"));
            txt.Value = "L-Kun uses BREEZE v0.1!";
            txt.X = BreezeCore.ScrW / 2 - txt.W / 2;
            txt.Y = BreezeCore.ScrH / 2 - txt.H / 2;
            layer.Insert(txt);
			
			SDL.SDL_AddTimer(1, Timer, IntPtr.Zero);
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
			
			BreezeCore.Init("Breeze", 800, 600);
			BreezeCore.Start();
			BreezeCore.Finish();
		}
	}
}
