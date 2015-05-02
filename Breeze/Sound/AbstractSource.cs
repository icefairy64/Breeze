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
            get { return FVolume; }
            set 
            { 
                FVolume = value;
                SetVolume(value * (FChannel != null ? FChannel.Volume : 1.0f)); 
            }
        }

        public float Attenuation
        {
            get { return FAttenuation; }
            set 
            { 
                FAttenuation = value;
                SetAttenuation(value * (FChannel != null ? FChannel.AttenuationMultiplier : 1.0f)); 
            }
        }

        public float MinDistance
        {
            get { return FMinDistance; }
            set 
            { 
                FMinDistance = value;
                SetMinDistance(value * (FChannel != null ? FChannel.MinDistanceMultiplier : 1.0f)); 
            }
        }

        public float X
        {
            get { return FPosition.X; }
            set 
            { 
                FPosition.X = value;
                SetX(value);
            }
        }

        public float Y
        {
            get { return FPosition.Z; }
            set 
            { 
                FPosition.Z = value;
                SetY(value);
            }
        }

        public abstract float Pitch { get; set; }
        public abstract bool Loop { get; set; }
        public abstract bool RelativeToListener { get; set; }
            
        protected abstract void SetVolume(float value);
        protected abstract void SetAttenuation(float value);
        protected abstract void SetMinDistance(float value);
        protected abstract void SetX(float value);
        protected abstract void SetY(float value);

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

