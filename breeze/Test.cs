using System;
using SDL2;

namespace Breeze
{
	class TestClass
	{		
		static int x1 = 640;
		static int x2 = 0;
		
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
			SDL.SDL_RenderDrawLine(renderer, x2, 0, x1, 480);
			SDL.SDL_SetRenderDrawColor(renderer, 64, 96, 216, 255);
		}
		
		static uint Timer(uint interval, IntPtr param)
		{
			SDL.SDL_Event ev = new SDL.SDL_Event();
			ev.type = SDL.SDL_EventType.SDL_USEREVENT;
			//SDL.SDL_PushEvent(ref ev);
			
			x1 += 10;
			if (x1 > BreezeCore.ScrW)
			{
				x1 = 0;
				x2 += 10;
			}
			if (x2 > BreezeCore.ScrW)
				x2 = 0;
			
			return 1;
		}		
		public static void Main(string[] args)
		{
			BreezeCore.OnEvent = ProcessEvents;
			BreezeCore.OnDraw = Draw;
			BreezeCore.Init("Breeze", 640, 480);
			//SDL.SDL_AddTimer(20, Timer, IntPtr.Zero);
			BreezeCore.Start();
			BreezeCore.Finish();
		}
	}
}
