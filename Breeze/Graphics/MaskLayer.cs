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

            FillRect = new RectangleShape(new SFML.System.Vector2f(FW, FH));
            FillRect.FillColor = new Color(0xff, 0xff, 0xff, 0x20);
            FillRect.Position = new SFML.System.Vector2f(0, 0);

            FillStates = new RenderStates();
            FillStates.BlendMode = BlendMode.Add;
            FillStates.Transform = Transform.Identity;
        }

        public override void Draw()
        {
            if (Bypass)
            {
                Sources[0].Draw();
                return;
            }

            if (!NeedsRendering)
                goto Drawing;

            var prevTarget = Screen.Target;
            Screen.Target = Buffer;
            Buffer.Clear(Color.Transparent);
            Spr.Color = Color;

            Sources[0].Draw();
            Buffer.Draw(FillRect, FillStates);

            Buffer.Display();

            Screen.Target = prevTarget;
            NeedsRendering = false;

            Drawing:
            Screen.Target.Draw(Spr, States);
        }
    }
}

