using System;
using SFML.Graphics;

namespace Breeze.Resources
{
    public class Font : Resource
    {
        public SFML.Graphics.Font Handle { get; protected set; }

        public Font(string filename)
            : base(filename)
        {
            Handle = new SFML.Graphics.Font(filename);
        }

        public override void Free()
        {
            if (Handle == null)
                return; 
            Console.WriteLine("Freeing font: {0:x16}", Handle.CPointer.ToInt64());
            Handle.Dispose();
            Handle = null;
        }

        ~Font()
        {
            Free();
        }
    }
}

