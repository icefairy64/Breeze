using System;

namespace Breeze.Graphics
{
    public class GroupLayer : Layer
    {
        public GroupLayer(string name, Layer[] sources, int zorder = 0, bool chunked = true)
            : base(name, zorder, chunked)
        {
            Sources = sources;
        }

        public override void Draw()
        {
            if (Bypass)
            {
                foreach (var src in Sources)
                    src.Draw();
                return;
            }

            if (!NeedsRendering)
                goto Drawing;

            var prevTarget = Screen.Target;
            Screen.Target = Buffer;

            foreach (var src in Sources)
                src.Draw();

            Buffer.Display();

            Screen.Target = prevTarget;
            NeedsRendering = false;

            Drawing:
            Screen.Target.Draw(Spr, States);
        }
    }
}

