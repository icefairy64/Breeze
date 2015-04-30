using System;
using Breeze.Resources;

namespace Breeze.Sound
{
    public class Sound : AbstractSource
    {
        protected SFML.Audio.Sound Snd;

        protected override float GetAttenuation()
        {
            return Snd.Attenuation;
        }

        protected override void SetAttenuation(float value)
        {
            Snd.Attenuation = value;
        }

        protected override float GetMinDistance()
        {
            return Snd.MinDistance;
        }

        protected override void SetMinDistance(float value)
        {
            Snd.MinDistance = value;
        }

        protected override float GetPitch()
        {
            return Snd.Pitch;
        }

        protected override void SetPitch(float value)
        {
            Snd.Pitch = value;
        }

        protected override float GetVolume()
        {
            return Snd.Volume;
        }

        protected override void SetVolume(float value)
        {
            Snd.Volume = value;
        }

        protected override float GetX()
        {
            return Snd.Position.X;
        }

        protected override void SetX(float value)
        {
            Snd.Position = FPosition;
        }

        protected override float GetY()
        {
            return Snd.Position.Z;
        }

        protected override void SetY(float value)
        {
            Snd.Position = FPosition;
        }

        protected override bool IsLooped()
        {
            return Snd.Loop;
        }

        protected override void SetLoop(bool value)
        {
            Snd.Loop = value;
        }

        protected override bool IsRelative()
        {
            return Snd.RelativeToListener;
        }

        protected override void SetRelative(bool value)
        {
            Snd.RelativeToListener = value;
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

