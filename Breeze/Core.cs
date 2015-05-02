using System;
using System.Runtime.InteropServices;
using Breeze.Game;
using Breeze.Graphics;
using Breeze.Resources;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using Neo.IronLua;

namespace Breeze
{
    public class TimerEventArgs : EventArgs
    {
        public uint Interval;
    }
        
    public delegate void DrawHandler(IntPtr renderer);
    public delegate void CoreEventHandler();
    public delegate void ExceptionHandler(Exception e);

    public interface IUpdatable
    {
        void Update(uint interval);
    }

    public static class Core
    {
        public static RenderWindow Window;
        public static bool FixedDT;

        public static uint ScrW { get; private set; }
        public static uint ScrH { get; private set; }

        public static bool Exit;
        public static World World;
        public static Lua LuaEngine { get; private set; }
        public static LuaGlobal LuaEnvironment { get; private set; }
        public static LuaCompileOptions LuaCompileOptions { get; private set; }

        public static DrawHandler OnDraw;
        public static CoreEventHandler OnInit;
        public static CoreEventHandler OnMainLoopStart;
        public static CoreEventHandler OnMainLoop;
        public static CoreEventHandler OnMainLoopFinish;
        public static ExceptionHandler OnException;

        public static event EventHandler<TimerEventArgs> OnAnimate;
        public static event EventHandler<TimerEventArgs> OnUpdate;

        static GameState FState;
        static Clock Clock;

        static void HandleException(Exception e)
        {
            if (ExceptionHandler != null)
            {
                ExceptionHandler(e);
                return;
            }

            Console.WriteLine(e.StackTrace);
            throw e;
        }

        public static GameState State
        {
            get { return FState; }
            set
            {
                if (FState != null)
                {
                    Window.KeyPressed -= FState.HandleKeyPress;
                    Window.KeyReleased -= FState.HandleKeyRelease;
                    OnUpdate -= FState.Update;
                    FState.Leave();
                }
                FState = value;

                FState.Enter();
                Window.KeyPressed += FState.HandleKeyPress;
                Window.KeyReleased += FState.HandleKeyRelease;
                OnUpdate += FState.Update;
            }
        }

        public static void Init(string title, uint scrw, uint scrh, bool fixedDT = false)
        {
            ScrW = scrw;
            ScrH = scrh;
            FixedDT = fixedDT;
		
            Window = new RenderWindow(new VideoMode(scrw, scrh), title);
            Window.SetVerticalSyncEnabled(false);
            Window.SetFramerateLimit(60);
            Window.SetKeyRepeatEnabled(false);

            Clock = new Clock();

            LuaEngine = new Lua();
            LuaEnvironment = LuaEngine.CreateEnvironment();
            LuaCompileOptions = new LuaCompileOptions();
			
            ResourceManager.Init();
            Screen.Init();

            World = new World();
			
            if (OnInit != null)
                OnInit();
        }

        public static void Start()
        {
            if (OnMainLoopStart != null)
                OnMainLoopStart();
                
            uint dt = 0;
            var arg = new TimerEventArgs();

            try
            {
                while (!Exit)
                {
                    Window.DispatchEvents();

                    if (OnMainLoop != null)
                        OnMainLoop(); 

                    Update(arg, IntPtr.Zero);
                    Animate(arg, IntPtr.Zero);

                    Render();

                    dt = FixedDT ? 16 : (uint)Clock.Restart().AsMilliseconds();
                    arg.Interval = dt;
                }
            }
            catch (Exception e)
            {
                HandleException(e);
            }

            if (OnMainLoopFinish != null)
                OnMainLoopFinish();
        }

        static uint Animate(TimerEventArgs e, IntPtr param)
        {
            try
            {
                var handler = OnAnimate;
                if (handler != null)
                    handler(null, e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return e.Interval;
        }

        static uint Update(TimerEventArgs e, IntPtr param)
        {
            try
            {
                var handler = OnUpdate;
                if (handler != null)
                    handler(null, e);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return e.Interval;
        }       

        static void Render()
        {
            Window.Clear();
            Screen.Draw();
            Window.Display();
        }

        public static void Finish()
        {
            ResourceManager.Free();
        }
    }
}

