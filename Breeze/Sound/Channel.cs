using System;

namespace Breeze.Sound
{
    public class Channel
    {
        public string Name { get; protected set; }

        protected float FVolume = 1.0f;
        protected float FAttenuationMultiplier = 1.0f;
        protected float FMinDistanceMultiplier = 1.0f;

        public float Volume
        {
            get { return FVolume; }
            set { FVolume = value; }
        }

        public float AttenuationMultiplier
        {
            get { return FAttenuationMultiplier; }
            set { FAttenuationMultiplier = value; }
        }

        public float MinDistanceMultiplier
        {
            get { return FMinDistanceMultiplier; }
            set { FMinDistanceMultiplier = value; }
        }

        public Channel(string name)
        {
            Name = name;
        }
    }
}

