using System;
using Breeze.Resources;

namespace Breeze.Sound
{
    public class Sound : AbstractSource
    {
        protected SFML.Audio.Sound Snd;

        public override bool Loop
        {
            get { return Snd.Loop; }
            set { Snd.Loop = value; }
        }

        public override bool RelativeToListener
        {
            get { return Snd.RelativeToListener; }
            set { Snd.RelativeToListener = value; }
        }

        public override float Pitch 
        {
            get { return Snd.Pitch; }
            set { Snd.Pitch = value; }
        }

        protected override void SetAttenuation(float value)
        {
            Snd.Attenuation = value;
        }

        protected override void SetMinDistance(float value)
        {
            Snd.MinDistance = value;
        }

        protected override void SetVolume(float value)
        {
            Snd.Volume = value;
        }

        protected override void SetX(float value)
        {
            Snd.Position = FPosition;
        }

        protected override void SetY(float value)
        {
            Snd.Position = FPosition;
        }

        public Sound(SoundBuffer buffer, Channel channel = null)
            : base(channel)
        {
            Snd = new SFML.Audio.Sound(buffer.Buffer);
        }

        public Sound(string buffer)
            : this(ResourceManager.Find<SoundBuffer>(buffer))
        {
        }

        public override void Play()
        {
            Snd.Play();
        }

        public override void Pause()
        {
            Snd.Pause();
        }

        public override void Stop()
        {
            Snd.Stop();
        }
    }
}

