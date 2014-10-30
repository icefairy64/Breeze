﻿using System;
using Breeze;
using Breeze.Graphics;
using SDL2;

namespace Breeze.Graphics
{
    public class Camera : IUpdatable
    {
        public double X, Y;
        public double Inertia = 0;
        public int Radius = BreezeCore.ScrH / 2;
        protected double RX, RY;

        public Camera()
        {
            RX = BreezeCore.ScrW / 2;
            RY = BreezeCore.ScrH / 2;
            X = RX;
            Y = RY;
        }

        public void Update(uint interval)
        {
            int dx = (int)Math.Max(0, Math.Abs(RX - X) - (BreezeCore.ScrW / 2 - Radius));
            int dy = (int)Math.Max(0, Math.Abs(RY - Y) - (BreezeCore.ScrH / 2 - Radius));
            int R = (int)Math.Sqrt(dx * dx + dy * dy);

            RX = RX + (X - RX) * (1 - Inertia) * R / Radius;
            RY = RY + (Y - RY) * (1 - Inertia) * R / Radius;
            Screen.CamX = RX - BreezeCore.ScrW / 2;
            Screen.CamY = RY - BreezeCore.ScrH / 2;
        }
    }
}

