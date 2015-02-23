using System;
using SFML.Graphics;

namespace Breeze.Graphics
{
    public class MaskLayer : Layer
    {
        protected RectangleShape FillRect;
        protected RenderStates FillStates;

        public MaskLayer(string name, Layer src, int zorder)
            : base(name, zorder, false)
        {
            Sources = new Layer[1];
            Sources[0] = src;

            FillRect = new RectangleShape(new SFML.Window.Vector2f(FW, FH));
            FillRect.FillColor = new Color(0xff, 0xff, 0xff, 0x20);
            FillRect.Position = new SFML.Window.Vector2f(0, 0);

            FillStates = new RenderStates();
            FillStates.BlendMode = BlendMode.Add;
            FillStates.Transform = Transform.Identity;
        }

        public override void Draw()
        {
            if (!NeedsRendering)
                goto Drawing;

            if (Bypass)
            {
                Sources[0].Draw();
                return;
            }

            var prevTarget = Screen.CurrentTarget;
            Screen.CurrentTarget = Buffer;
            Buffer.Clear(Color.Transparent);
            Spr.Color = Color;

            Sources[0].Draw();
            Buffer.Draw(FillRect, FillStates);

            Buffer.Display();

            Screen.CurrentTarget = prevTarget;
            NeedsRendering = false;

            Drawing:
            Screen.CurrentTarget.Draw(Spr, States);
        }
    }
}

