using System;
using System.Runtime.InteropServices;
using Breeze.Game;
using Breeze.Graphics;
using Breeze.Resources;
using SFML;
using SFML.Window;
using SFML.Graphics;

namespace Breeze
{
    public class TimerEventArgs : EventArgs
    {
        public uint Interval;
    }
        
    public delegate void DrawHandler(IntPtr renderer);
    public delegate void CoreEventHandler();

    public interface IUpdatable
    {
        void Update(uint interval);
    }

    public static class BreezeCore
    {
        public static RenderWindow Window;

        public static uint ScrW { get; private set; }
        public static uint ScrH { get; private set; }

        public static DrawHandler OnDraw;
        public static CoreEventHandler OnInit;
        public static CoreEventHandler OnMainLoopStart;
        public static CoreEventHandler OnMainLoop;
        public static CoreEventHandler OnMainLoopFinish;

        public static event EventHandler<TimerEventArgs> OnAnimate;
        public static event EventHandler<TimerEventArgs> OnUpdate;

        public static bool Exit = false;
        static GameState FCurrentState;

        private static void HandleException(Exception e)
        {
            Console.WriteLine(e.StackTrace);
        }

        public static GameState CurrentState
        {
            get { return FCurrentState; }
            set
            {
                if (FCurrentState != null)
                {
                    Window.KeyPressed -= FCurrentState.HandleKeyPress;
                    Window.KeyReleased -= FCurrentState.HandleKeyRelease;
                    OnUpdate -= FCurrentState.Update;
                    FCurrentState.Leave();
                }
                FCurrentState = value;

                FCurrentState.Enter();
                Window.KeyPressed += FCurrentState.HandleKeyPress;
                Window.KeyReleased += FCurrentState.HandleKeyRelease;
                OnUpdate += FCurrentState.Update;
            }
        }

        public static void Init(string title, uint scrw, uint scrh)
        {
            ScrW = scrw;
            ScrH = scrh;
		
            Window = new RenderWindow(new VideoMode(scrw, scrh), title);
            Window.SetVerticalSyncEnabled(false);
            Window.SetFramerateLimit(60);
            Window.SetKeyRepeatEnabled(false);
			
            ResourceManager.Init();
            Screen.Init();
			
            if (OnInit != null)
                OnInit();
        }

        public static void Start()
        {
            if (OnMainLoopStart != null)
                OnMainLoopStart();

            DateTime before;
            DateTime after = DateTime.Now;
            uint dt = 0;

            try
            {
                while (!Exit)
                {
                    before = after;

                    Window.DispatchEvents();

                    if (OnMainLoop != null)
                        OnMainLoop(); 

                    UpdateCallback(dt, IntPtr.Zero);
                    AnimateCallback(dt, IntPtr.Zero);

                    Render();

                    after = DateTime.Now;
                    //dt = (uint)after.Subtract(before).Milliseconds;
                    dt = 16;
                }
            }
            catch (Exception e)
            {
                HandleException(e);
            }

            if (OnMainLoopFinish != null)
                OnMainLoopFinish();
        }

        static uint AnimateCallback(uint interval, IntPtr param)
        {
            try
            {
                TimerEventArgs arg = new TimerEventArgs() { Interval = interval };
                EventHandler<TimerEventArgs> handler = OnAnimate;
                if (handler != null)
                    handler(null, arg);
            }
            catch (Exception e)
            {
                HandleException(e);
            }

            return interval;
        }

        static uint UpdateCallback(uint interval, IntPtr param)
        {
            try
            {
                TimerEventArgs arg = new TimerEventArgs() { Interval = interval };
                EventHandler<TimerEventArgs> handler = OnUpdate;
                if (handler != null)
                    handler(null, arg);
            }
            catch (Exception e)
            {
                HandleException(e);
            }

            return interval;
        }       

        static void Render()
        {
            Window.Clear();
            Screen.Draw();
            Window.Display();

            //if (OnDraw != null)
            //    OnDraw(Renderer);
        }

        public static void Finish()
        {
            ResourceManager.Free();
        }
    }
}

