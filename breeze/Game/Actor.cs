using System;
using System.Runtime.Serialization;

namespace Breeze.Game
{
    [Serializable]
    public abstract class Actor : Entity, IUpdatable
    {
        protected Actor()
        {
        }

        public abstract void Update(uint interval);
    }
}

