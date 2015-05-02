using System;
using Breeze;
using Breeze.Game;
using Breeze.Graphics;
using Breeze.Resources;
using SFML.Window;

namespace LayersDemo
{
    public class DemoState : GameState
    {
        protected double Phase;

        public DemoState()
            : base()
        {
            ResourceManager.Load<SpriteSheet>("Buildings1.png");
            ResourceManager.Load<SpriteSheet>("Buildings2.png");
            ResourceManager.Load<SpriteSheet>("Stars.png");
            var font = ResourceManager.Load<Font>("hammersmithone.ttf");

            var starsl = Screen.CreateLayer("stars", 0, false);
            var back1l = Screen.CreateLayer("back1", 1, false);
            var back2l = Screen.CreateLayer("back2", 2, false);
            var osdl = Screen.CreateLayer("osd", 3, false);

            var stars = new Sprite(new string[] { "Stars" });
            var back1 = new Sprite(new string[] { "Buildings2" });
            var back2 = new Sprite(new string[] { "Buildings1" });

            stars.X = -stars.W / 2;
            stars.Y = -stars.H + (int)Core.ScrH / 2;
            back1.Y = stars.H - back1.H + stars.Y;
            back2.Y = stars.H - back2.H + stars.Y;
            back1.X = -back1.W / 2;
            back2.X = -back2.W / 2;

            starsl.ScrollSpeed = 0.5f;
            back1l.ScrollSpeed = 0.8f;
            back2l.ScrollSpeed = 1f;
            osdl.ScrollSpeed = 0f;

            starsl.Insert(stars);
            back1l.Insert(back1);
            back2l.Insert(back2);

            var text = new Text(font);
            text.Size = 14;
            text.X = -(int)Core.ScrW / 2 + 32;
            text.Y = -(int)Core.ScrH / 2 + 32;
            text.Value = "Z: toggle front layer visibility";
            text.Color = SFML.Graphics.Color.White;

            osdl.Insert(text);

            // Masking test

            var maskl = new MaskLayer("mask", back2l, 5);
            Screen.InsertLayer(maskl);

            maskl.X = back2l.X;
            maskl.Y = back2l.Y;
            maskl.ScrollSpeed = back2l.ScrollSpeed;
            maskl.Bypass = false;
            maskl.Alpha = 0;
        }

        public override void Enter()
        {

        }

        public override void Leave()
        {

        }

        protected void HandleKeyInput(KeyEventArgs e, bool state)
        {
            if (!state)
                return;

            switch (e.Code)
            {
                case Keyboard.Key.Escape:
                    Core.Exit = true;
                    break;
                case Keyboard.Key.Z:
                    Screen.FindLayer("back2").Visible ^= true;
                    break;
                case Keyboard.Key.X:
                    Screen.FindLayer("mask").Alpha ^= 0xff;
                    break;
            }
        }

        public override void HandleKeyPress(object sender, KeyEventArgs e)
        {
            HandleKeyInput(e, true);
        }

        public override void HandleKeyRelease(object sender, KeyEventArgs e)
        {
            HandleKeyInput(e, false);
        }

        public override void Update(object sender, TimerEventArgs e)
        {
            base.Update(sender, e);
            Phase += e.Interval / 5000d;
            Screen.CamX = (float)Math.Sin(Phase) * 1000f;
        }
    }
}

