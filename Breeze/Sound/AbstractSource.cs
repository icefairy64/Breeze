using System;
using SFML.Audio;
using SFML.System;

namespace Breeze.Sound
{
    public abstract class AbstractSource
    {
        protected float FVolume;
        protected float FAttenuation;
        protected float FMinDistance;
        protected Channel FChannel;
        protected Vector3f FPosition = new Vector3f();

        public Channel Channel
        {
            get { return FChannel; }
            set
            {
                FChannel = value;
               
                if (value != null)
                    Recalculate();
            }
        }

        public float Volume
        {
            get { return GetVolume(); }
            set { SetVolume(value * (FChannel != null ? FChannel.Volume : 1.0f)); }
        }

        public float Attenuation
        {
            get { return GetAttenuation(); }
            set { SetAttenuation(value * (FChannel != null ? FChannel.AttenuationMultiplier : 1.0f)); }
        }

        public float MinDistance
        {
            get { return GetMinDistance(); }
            set { SetMinDistance(value * (FChannel != null ? FChannel.MinDistanceMultiplier : 1.0f)); }
        }

        public float X
        {
            get { return GetX(); }
            set 
            { 
                FPosition.X = value;
                SetX(value);
            }
        }

        public float Y
        {
            get { return GetY(); }
            set 
            { 
                FPosition.Y = value;
                SetY(value);
            }
        }

        public float Pitch
        {
            get { return GetPitch(); }
            set { SetPitch(value); }
        }

        public bool Loop
        {
            get { return IsLooped(); }
            set { SetLoop(value); }
        }

        public bool RelativeToListener
        {
            get { return IsRelative(); }
            set { SetRelative(value); }
        }

        protected abstract float GetVolume();
        protected abstract void SetVolume(float value);
        protected abstract float GetAttenuation();
        protected abstract void SetAttenuation(float value);
        protected abstract float GetMinDistance();
        protected abstract void SetMinDistance(float value);
        protected abstract float GetX();
        protected abstract void SetX(float value);
        protected abstract float GetY();
        protected abstract void SetY(float value);
        protected abstract float GetPitch();
        protected abstract void SetPitch(float value);
        protected abstract bool IsLooped();
        protected abstract void SetLoop(bool value);
        protected abstract bool IsRelative();
        protected abstract void SetRelative(bool value);

        public abstract void Pause();
        public abstract void Play();
        public abstract void Stop();

        protected AbstractSource(Channel channel)
        {
            Channel = channel;
        }

        public void RecalculateVolume()
        {
            Volume = FVolume;
        }

        public void RecalculateAttenuation()
        {
            Attenuation = FAttenuation;
        }

        public void RecalculateMinDistance()
        {
            MinDistance = FMinDistance;
        }

        public void Recalculate()
        {
            RecalculateAttenuation();
            RecalculateMinDistance();
            RecalculateVolume();
        }

    }
}

