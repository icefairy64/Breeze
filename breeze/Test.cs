using System;
using SDL2;

namespace Breeze
{
	class TestClass
	{		
		static Sprite spr;
		
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
		
		static void Draw(IntPtr renderer)
		{
			SDL.SDL_SetRenderDrawColor(renderer, 255, 255, 255, 128);
			SDL.SDL_RenderDrawLine(renderer, 0, 0, 640, 480);
			spr.Draw();
			SDL.SDL_SetRenderDrawColor(renderer, 64, 96, 216, 255);
		}
		
		static uint Timer(uint interval, IntPtr param)
		{
			SDL.SDL_Event ev = new SDL.SDL_Event();
			ev.type = SDL.SDL_EventType.SDL_USEREVENT;
			SDL.SDL_PushEvent(ref ev);
			
			spr.Angle += 0.3;
			
			return 1;
		}		
		public static void Main(string[] args)
		{
			BreezeCore.OnEvent = ProcessEvents;
			BreezeCore.OnDraw = Draw;
			BreezeCore.Init("Breeze", 640, 480);
			
			spr = new Sprite("cirno.bspr");
			spr.Scale = 3;
			spr.X = 320 - spr.W / 2;
			spr.Y = 120;
			
			//SDL.SDL_AddTimer(20, Timer, IntPtr.Zero);
			
			BreezeCore.Start();
			BreezeCore.Finish();
		}
	}
}
