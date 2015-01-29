using System;
using System.Runtime.InteropServices;
using SDL2;

namespace Breeze
{
    public class TimerEventArgs : EventArgs
    {
        public uint Interval;
    }

    public delegate void SDLEventHandler(SDL.SDL_Event ev);
    public delegate void DrawHandler(IntPtr renderer);
    public delegate void CoreEventHandler();
    public delegate void KeyInputHandler(SDL.SDL_KeyboardEvent ev);

    public interface IUpdatable
    {
        void Update(uint interval);
    }

    public static class BreezeCore
    {
        public static IntPtr Window;
        public static IntPtr Renderer;

        public static int ScrW { get; private set; }

        public static int ScrH { get; private set; }

        public static SDLEventHandler OnEvent;
        public static DrawHandler OnDraw;
        public static CoreEventHandler OnInit;
        public static CoreEventHandler OnMainLoopStart;
        public static CoreEventHandler OnMainLoop;
        public static CoreEventHandler OnMainLoopFinish;
        public static KeyInputHandler OnKeyInput;

        public static event EventHandler<TimerEventArgs> OnAnimate;
        public static event EventHandler<TimerEventArgs> OnUpdate;

        public static SDL.SDL_Rect ScrRect;
        public static bool Exit = false;
        static int AnimateTimer;
        static int UpdateTimer;
        static Game.GameState FCurrentState;

        public static Game.GameState CurrentState
        {
            get { return FCurrentState; }
            set
            {
                if (FCurrentState != null)
                {
                    OnUpdate -= FCurrentState.Update;
                    FCurrentState.Leave();
                }
                FCurrentState = value;

                FCurrentState.Enter();
                OnKeyInput = FCurrentState.KeyInput;
                OnEvent = FCurrentState.ProcessEvent;
                OnUpdate += FCurrentState.Update;
            }
        }

        public static void Init(string title, int scrw, int scrh)
        {
            ScrW = scrw;
            ScrH = scrh;
            ScrRect = new SDL.SDL_Rect() { x = 0, y = 0, w = ScrW, h = ScrH };
			
            // Initializing SDL subsystems
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO | SDL.SDL_INIT_TIMER | SDL.SDL_INIT_GAMECONTROLLER | SDL.SDL_INIT_JOYSTICK);
            SDL.SDL_Delay(500);
            
            // Hints
            //SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_SCALE_QUALITY, "1");  // Linear scaling
            SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_VSYNC, "1");

            
            SDL.SDL_CreateWindowAndRenderer(scrw, scrh, SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL, out Window, out Renderer);
            SDL.SDL_Delay(500); // Giving SDL some time to warm up
            SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_JPG | SDL_image.IMG_InitFlags.IMG_INIT_PNG);
            SDL_ttf.TTF_Init();
			
            Resources.ResourceManager.Init();
            Graphics.Screen.Init();
			
            if (OnInit != null)
                OnInit();
        }

        public static void Start()
        {
            if (OnMainLoopStart != null)
                OnMainLoopStart();
            AnimateTimer = SDL.SDL_AddTimer(5, AnimateCallback, IntPtr.Zero);
            UpdateTimer = SDL.SDL_AddTimer(5, UpdateCallback, IntPtr.Zero);
            SDL.SDL_SetRenderDrawBlendMode(Renderer, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
			
            SDL.SDL_Event ev;
            while (!Exit)
            {
                while (SDL.SDL_PollEvent(out ev) == 1)
                    ProcessEvents(ev);
                if (OnMainLoop != null)
                    OnMainLoop();
                Render();
            }
			
            SDL.SDL_RemoveTimer(AnimateTimer);
            SDL.SDL_RemoveTimer(UpdateTimer);
            if (OnMainLoopFinish != null)
                OnMainLoopFinish();
        }

        static uint AnimateCallback(uint interval, IntPtr param)
        {
            TimerEventArgs arg = new TimerEventArgs() { Interval = interval };
            EventHandler<TimerEventArgs> handler = OnAnimate;
            if (handler != null)
                handler(null, arg);
            return interval;
        }

        static uint UpdateCallback(uint interval, IntPtr param)
        {
            TimerEventArgs arg = new TimerEventArgs() { Interval = interval };
            EventHandler<TimerEventArgs> handler = OnUpdate;
            if (handler != null)
                handler(null, arg);
            return interval;
        }

        static void ProcessEvents(SDL.SDL_Event ev)
        {
            if ((ev.type == SDL.SDL_EventType.SDL_KEYDOWN) || (ev.type == SDL.SDL_EventType.SDL_KEYUP))
            {
                if (OnKeyInput != null)
                    OnKeyInput(ev.key);
                return;
            }
            OnEvent(ev);
        }

        static void Render()
        {
            SDL.SDL_RenderClear(Renderer);
            Graphics.Screen.Draw();
            if (OnDraw != null)
                OnDraw(Renderer);
            SDL.SDL_RenderPresent(Renderer);
        }

        public static void Finish()
        {
            Resources.ResourceManager.Free();		
            SDL_ttf.TTF_Quit();
            SDL_image.IMG_Quit();
            SDL.SDL_Quit();
        }
    }
}

