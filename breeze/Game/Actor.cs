using System;

namespace Breeze.Game
{
    public abstract class Actor : Entity, IUpdatable
    {
        protected Actor()
        {
        }

        public abstract void Update(uint interval);
    }
}

