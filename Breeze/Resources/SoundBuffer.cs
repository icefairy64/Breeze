using System;
using SFML.Audio;

namespace Breeze.Resources
{
    public class SoundBuffer : Resource
    {
        public SFML.Audio.SoundBuffer Buffer;

        public SoundBuffer(string filename)
            : base(filename)
        {
            Buffer = new SFML.Audio.SoundBuffer(filename);
        }

        public override void Free()
        {
            if (Buffer != null)
                Buffer.Dispose();
        }
    }
}

