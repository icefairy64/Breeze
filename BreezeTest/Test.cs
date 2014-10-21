using System;
using Breeze;
using Breeze.Graphics;
using Breeze.Resources;
using SDL2;

namespace BreezeTest
{
	class MainClass
	{		
        static Breeze.Graphics.Sprite spr;
        static Breeze.Graphics.Layer layer;
        static Breeze.Graphics.Text txt;
        static int frames = 0;
        static bool[] keystate = new bool[4];
        static uint lastint = 0;
        static int dir = 2;
        static int fps = 0;
		
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
                Breeze.Graphics.Screen.CamY -= 1;
            if (keystate[1])
                Breeze.Graphics.Screen.CamX += 1;
            if (keystate[2])
                Breeze.Graphics.Screen.CamY += 1;
            if (keystate[3])
                Breeze.Graphics.Screen.CamX -= 1;
            
            lastint = interval;

			return 1;
		}	

        static uint FlushFrameCount(uint interval, IntPtr param)
        {
            fps = (int)(frames * 1000 / interval);
            frames = 0;
            return interval;
        }

        static void OnDraw(IntPtr renderer)
        {
            frames++;
            txt.Value = String.Format("FPS: {0}", fps);
        }
		
		static void OnInit()
		{
            Breeze.Resources.Manager.LoadSprite("cornet_0.bspr");
            Breeze.Resources.Manager.LoadSprite("cornet_1.bspr");
            Breeze.Resources.Manager.LoadSprite("cornet_2.bspr");
            Breeze.Resources.Manager.LoadSprite("cornet_3.bspr");
            Breeze.Resources.Manager.LoadFont("hammersmithone.ttf", 24);

            layer = Breeze.Graphics.Screen.CreateLayer("front", 1);
            layer.BlendMode = SDL.SDL_BlendMode.SDL_BLENDMODE_ADD;

            spr = new Breeze.Graphics.Sprite("cornet_walk0");
            spr.AddImage("cornet_walk1");
            spr.AddImage("cornet_walk2");
            spr.AddImage("cornet_walk3");
            spr.CurrentImage = 2;
			spr.Scale = 2;
            spr.X = (int)(BreezeCore.ScrW / 2 - spr.W * spr.Scale / 2);
            spr.Y = (int)(BreezeCore.ScrH / 2 - spr.H * spr.Scale / 2);
			spr.AnimSpeed = 1;
			layer.Insert(spr);

            layer = Breeze.Graphics.Screen.CreateLayer("back");
            layer.Alpha = 0xa0;

            txt = new Breeze.Graphics.Text(Breeze.Resources.Manager.FindFont("hammersmithone24"));
            txt.Value = "L-Kun uses BREEZE v0.1!";
            txt.X = 32;
            txt.Y = 32;
            layer.Insert(txt);
			
			SDL.SDL_AddTimer(1, Timer, IntPtr.Zero);
            SDL.SDL_AddTimer(1000, FlushFrameCount, IntPtr.Zero);
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
                case SDL.SDL_Keycode.SDLK_SPACE:
                    if (!newstate)
                        break;
                    dir = (dir + 1) % 4;
                    spr.CurrentImage = dir;
                    break;
            }
        }
		
		public static void Main(string[] args)
		{
			BreezeCore.OnEvent = ProcessEvents;
			BreezeCore.OnInit = OnInit;
            BreezeCore.OnDraw = OnDraw;
            BreezeCore.OnKeyInput = OnKey;
			
			BreezeCore.Init("Breeze", 800, 600);
			BreezeCore.Start();
			BreezeCore.Finish();
		}
	}
}
