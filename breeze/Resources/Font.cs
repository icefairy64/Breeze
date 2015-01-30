using System;
using SDL2;

namespace Breeze.Resources
{
    public class Font : Resource
    {
        public IntPtr Handle { get; protected set; }

        public Font(string filename, int pt)
            : base(filename)
        {
            Handle = SDL_ttf.TTF_OpenFont(filename, pt);
            Name += pt.ToString();
        }

        public Font(string filename)
            : base(filename)
        {
            string[] parts = filename.Split(new char[] { ':' }, 2);
            Handle = SDL_ttf.TTF_OpenFont(parts[0], Convert.ToInt32(parts[1]));
            Name += parts[1];
        }

        public override void Free()
        {
            if (Handle == IntPtr.Zero)
                return; 
            Console.WriteLine("Freeing font: {0:x16}", Handle.ToInt64());
            SDL_ttf.TTF_CloseFont(Handle);
            Handle = IntPtr.Zero;
        }

        ~Font()
        {
            Free();
        }
    }
}

