using System;
using Breeze;
using Breeze.Graphics;

namespace Breeze.Graphics
{
    public class Camera : IUpdatable
    {
        public float X, Y;
        public float Inertia = 0;
        public int Radius = (int)Core.ScrH / 2;
        protected float RX, RY;

        public Camera()
        {
            RX = Core.ScrW / 2;
            RY = Core.ScrH / 2;
            X = RX;
            Y = RY;
        }

        public void Update(uint interval)
        {
            int dx = (int)Math.Max(0, Math.Abs(RX - X) - (Core.ScrW / 2 - Radius));
            int dy = (int)Math.Max(0, Math.Abs(RY - Y) - (Core.ScrH / 2 - Radius));
            int R = (int)Math.Sqrt(dx * dx + dy * dy);

            RX = RX + (X - RX) * (1 - Inertia) * R / Radius;
            RY = RY + (Y - RY) * (1 - Inertia) * R / Radius;
            Screen.CamX = RX;
            Screen.CamY = RY;
        }
    }
}

