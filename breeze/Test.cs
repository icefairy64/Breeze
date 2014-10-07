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
			layer.Draw();
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
		
		static void OnInit()
		{
			layer = new Graphics.Layer("front");
			layer.Alpha = 0xff;
			
			Resources.Manager.LoadSprite("cirno.bspr");
			
			for (int i = 0; i < 9; i++)
			{
				spr = new Graphics.Sprite("cirno", 9 - i);
				spr.Scale = 3;
				spr.X = 120 + i * 32;
				spr.Y = 120 - i * 4;
				spr.AnimSpeed = 1 + (i / 10.0);
				layer.Insert(spr);
			}
			
			//SDL.SDL_AddTimer(20, Timer, IntPtr.Zero);
		}
		
		public static void Main(string[] args)
		{
			BreezeCore.OnEvent = ProcessEvents;
			BreezeCore.OnDraw = Draw;
			BreezeCore.OnInit = OnInit;
			
			BreezeCore.Init("Breeze", 640, 480);
			BreezeCore.Start();
			BreezeCore.Finish();
		}
	}
}
