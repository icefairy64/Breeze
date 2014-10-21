using System;
using Breeze;
using Breeze.Game;
using SDL2;

namespace BreezeTest
{
    public class Cornet : Actor
    {
        int FDirection;
        int FWalking;

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
            Breeze.Graphics.Screen.FindLayer("front").Insert(Image);
            X = (int)(BreezeCore.ScrW / 2 - Image.W);
            Y = (int)(BreezeCore.ScrH / 2 - Image.H);
        }
    }

    public class TestState : GameState
    {
        bool[] KeyState = new bool[4];
        Cornet Player;

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
            Breeze.Resources.Manager.LoadFont("hammersmithone.ttf", 24);

            Breeze.Graphics.Screen.CreateLayer("front", 1).Alpha = 0;

            Player = new Cornet();
            AddActor(Player);
        }

        public override void Enter()
        {
            Breeze.Graphics.AlphaAutomation auto = new Breeze.Graphics.AlphaAutomation(0);
            auto.AddPoint(1500, 0xff, InterpolationMethod.Linear);
            AddAutomation(auto);
            auto.AttachClient(Breeze.Graphics.Screen.FindLayer("front"));
            auto.Active = true;
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

        ~TestState()
        {
            
        }
    }
}

