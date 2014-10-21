using System;
using System.IO;
using Breeze;
using Breeze.Graphics;
using Breeze.Resources;
using SDL2;

namespace BreezeTest
{
	class MainClass
	{		
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
		
		static void OnInit()
		{
            Breeze.Resources.Manager.RootDir = String.Format("..{0}..{0}data{0}", Path.DirectorySeparatorChar);
            Breeze.Resources.Manager.SpritesDir = String.Format("sprites{0}", Path.DirectorySeparatorChar);

            BreezeCore.CurrentState = new TestState();
		}
		
		public static void Main(string[] args)
		{
			BreezeCore.OnEvent = ProcessEvents;
			BreezeCore.OnInit = OnInit;
			
			BreezeCore.Init("Breeze", 800, 600);
			BreezeCore.Start();
			BreezeCore.Finish();
		}
	}
}
