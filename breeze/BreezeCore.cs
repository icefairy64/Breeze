using System;
using System.Runtime.InteropServices;
using SDL2;

namespace Breeze
{
	public class TimerEventArgs : EventArgs
	{
		public uint Interval;
	}
	
	public delegate int EventHandler(SDL.SDL_Event ev);
	public delegate void DrawHandler(IntPtr renderer);
	
	public static class BreezeCore
	{
		public static IntPtr Window;
		public static IntPtr Renderer;
		public static int ScrW { get; private set; }
		public static int ScrH { get; private set; }
		public static EventHandler OnEvent;
		public static DrawHandler OnDraw;
		public static event EventHandler<TimerEventArgs> OnAnimate;
		public static uint TargetFPS;
		static SDL.SDL_Rect ScrRect;
		static uint DrawTimerInterval;
		static bool Exit = false;
		
		public static void Init(string title, int scrw, int scrh)
		{
			ScrW = scrw;
			ScrH = scrh;
			ScrRect = new SDL.SDL_Rect() { x = 0, y = 0, w = ScrW, h = ScrH };
			TargetFPS = 60;
			DrawTimerInterval = 1000 / TargetFPS;
			
			SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_TIMER | SDL.SDL_INIT_GAMECONTROLLER | SDL.SDL_INIT_JOYSTICK);
			SDL.SDL_Delay(500);
			SDL.SDL_CreateWindowAndRenderer(scrw, scrh, SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL, out Window, out Renderer);
			SDL.SDL_Delay(500);
			SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_JPG | SDL_image.IMG_InitFlags.IMG_INIT_PNG);
			SDL.SDL_AddEventWatch(WatchEvents, IntPtr.Zero);
		}
		
		public static void Start()
		{
			SDL.SDL_AddTimer(5, AnimateCallback, IntPtr.Zero);
			SDL.SDL_SetRenderDrawBlendMode(Renderer, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
			
			SDL.SDL_Event ev;
			while (!Exit)
			{
				while (SDL.SDL_PollEvent(out ev) == 1)
					Exit = ProcessEvents(ev);
				SDL.SDL_Delay(DrawTimerInterval);
				Render();
			}
		}
		
		static uint AnimateCallback(uint interval, IntPtr param)
		{
			TimerEventArgs arg = new TimerEventArgs() { Interval = interval };
			EventHandler<TimerEventArgs> handler = OnAnimate;
			if (handler != null)
				handler(null, arg);
			return interval;
		}
		
		static bool ProcessEvents(SDL.SDL_Event ev)
		{
			return OnEvent(ev) == 1;
		}
		
		static int WatchEvents(IntPtr userdata, IntPtr ev)
		{
			SDL.SDL_Event evt = new SDL.SDL_Event();
			evt = (SDL.SDL_Event)Marshal.PtrToStructure(ev, typeof(SDL.SDL_Event));
			if (OnEvent(evt) == 1)
				Exit = true;
			return 0;
		}
		
		static void Render()
		{
			SDL.SDL_RenderFillRect(Renderer, ref ScrRect);
			//SDL.SDL_RenderClear(Renderer);
			OnDraw(Renderer);
			SDL.SDL_RenderPresent(Renderer);
		}
		
		public static void Finish()
		{
			SDL_image.IMG_Quit();
			SDL.SDL_Quit();
		}
	}
}

