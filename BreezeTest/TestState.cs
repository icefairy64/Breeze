using System;
using Breeze;
using Breeze.Game;
using Breeze.Graphics;
using SDL2;

namespace BreezeTest
{
    public class Cornet : Actor, IUpdatable
    {
        int FDirection;
        int FWalking;
        double Speed = 1 / 6f;

        public bool Walking
        {
            get { return FWalking == 1; }
            set
            {
                FWalking = value ? 1 : 0;
                ChangeImage();
            }
        }

        public int Direction
        {
            get { return FDirection; }
            set 
            { 
                FDirection = value;
                ChangeImage();
            }
        }

        void ChangeImage()
        {
            ((Breeze.Graphics.Sprite)Image).CurrentImage = FDirection + FWalking * 4;
        }

        public Cornet() : base("cornet")
        {
            Image = new Breeze.Graphics.Sprite(new string[]{ "cornet_stand0", "cornet_stand1", "cornet_stand2", "cornet_stand3", "cornet_walk0", "cornet_walk1", "cornet_walk2", "cornet_walk3" });
            Image.Scale = 2;
            Screen.FindLayer("front").Insert(Image);
            X = BreezeCore.ScrW / 2 - Image.W;
            Y = BreezeCore.ScrH / 2 - Image.H;
        }

        public void Update(uint interval)
        {
            if (Walking)
            {
                X += (2 - FDirection) * (FDirection % 2) * interval * Speed;
                Y += (FDirection - 1) * ((FDirection + 1) % 2) * interval * Speed;
            }
        }
    }

    public class TestState : GameState
    {
        bool[] KeyState = new bool[4];
        Cornet Player;
        Camera Cam;

        public TestState()
        {
            Breeze.Resources.Manager.LoadSprite("cornet_0.bspr");
            Breeze.Resources.Manager.LoadSprite("cornet_1.bspr");
            Breeze.Resources.Manager.LoadSprite("cornet_2.bspr");
            Breeze.Resources.Manager.LoadSprite("cornet_3.bspr");
            Breeze.Resources.Manager.LoadSprite("cornet_stand0.png");
            Breeze.Resources.Manager.LoadSprite("cornet_stand1.png");
            Breeze.Resources.Manager.LoadSprite("cornet_stand2.png");
            Breeze.Resources.Manager.LoadSprite("cornet_stand3.png");
            Breeze.Resources.Manager.LoadSprite("mothergreen_house0.png");
            Breeze.Resources.Manager.LoadFont("hammersmithone.ttf", 24);

            Screen.CreateLayer("front", 1);
            Screen.CreateLayer("back", 0);

            Breeze.Graphics.Sprite back = new Breeze.Graphics.Sprite("mothergreen_house0");
            back.Scale = 2;
            back.X = BreezeCore.ScrW / 2 - back.W;
            back.Y = BreezeCore.ScrH / 2 - back.H;
            Screen.FindLayer("back").Insert(back);

            Player = new Cornet();
            AddActor(Player);

            Cam = new Camera();
            Cam.Inertia = 0.1;
            Cam.Radius = BreezeCore.ScrH / 2 - 20;
            AddUpdatable(Cam);
        }

        public override void Enter()
        {
            Breeze.Graphics.AlphaAutomation auto = new Breeze.Graphics.AlphaAutomation(0);
            auto.AddPoint(1500, 0xff, InterpolationMethod.Linear);
            AddAutomation(auto);
            auto.AttachClient(Breeze.Graphics.Screen.FindLayer("front"));
            //auto.Active = true;
        }

        public override void Leave()
        {
            
        }

        void SetKeyState(int index, bool newstate)
        {
            KeyState[index] = newstate;
            int inv = (index + 2) % 4;
            if (newstate && KeyState[inv])
                KeyState[inv] = false;

            if (newstate)
                Player.Direction = index;
        }

        public override void KeyInput(SDL.SDL_KeyboardEvent ev)
        {
            if (ev.repeat == 1)
                return;

            bool newstate = ev.state == 1;
            switch (ev.keysym.sym)
            {
                case SDL.SDL_Keycode.SDLK_UP:
                    SetKeyState(0, newstate);
                    break;
                case SDL.SDL_Keycode.SDLK_RIGHT:
                    SetKeyState(1, newstate);
                    break;
                case SDL.SDL_Keycode.SDLK_DOWN:
                    SetKeyState(2, newstate);
                    break;
                case SDL.SDL_Keycode.SDLK_LEFT:
                    SetKeyState(3, newstate);
                    break;
                case SDL.SDL_Keycode.SDLK_ESCAPE:
                    BreezeCore.Exit = true;
                    break;
            }

            bool walking = false;
            int dir = Player.Direction;
            for (int i = 0; i < 4; i++)
            {
                walking |= KeyState[i];
                if (KeyState[i])
                    dir = i;
            }
            
            Player.Walking = walking;
            if (!newstate)
                Player.Direction = dir;
        }

        public override void Update(object sender, TimerEventArgs e)
        {
            base.Update(sender, e);
            Cam.X = Player.X + Player.Image.W / 2;
            Cam.Y = Player.Y + Player.Image.H / 2;
        }

        ~TestState()
        {
            
        }
    }
}

