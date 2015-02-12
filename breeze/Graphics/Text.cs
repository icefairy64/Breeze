using System;
using SFML.Graphics;

namespace Breeze.Graphics
{
    public class Text : Drawable
    {
        string FValue;
        public Resources.Font Font;

        protected SFML.Graphics.Text Txt;

        public string Value
        {
            get { return FValue; }
            set
            {
                FValue = value;
                Txt.DisplayedString = value;
            }
        }

        public uint Size
        {
            get { return Txt.CharacterSize; }
            set { Txt.CharacterSize = value; }
        }

        protected override void InternalDraw(Transform tf)
        {
            States.Transform = tf;
            Txt.Color = Color;
            Screen.CurrentTarget.Draw(Txt, States);
        }

        public Text(Resources.Font font, int zorder = 0) 
            : base(zorder)
        {
            Font = font;
            Txt = new SFML.Graphics.Text();
        }

        ~Text()
        {
            Txt.Dispose();
        }
    }
}

