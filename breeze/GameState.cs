using System;
using System.Collections.Generic;
using SDL2;

namespace Breeze.Game
{
    public abstract class GameState
    {
        protected Dictionary<string, Actor> Actors;
        protected Dictionary<string, int> Counter;
        public GameState Prev;
        protected uint Time = 0;

        public GameState()
        {
            Actors = new Dictionary<string, Actor>();
            Counter = new Dictionary<string, int>();
        }

        public void AddActor(Actor actor)
        {
            if (!Counter.ContainsKey(actor.Name))
                Counter.Add(actor.Name, 0);
            
            Actors.Add(actor.Name + Counter[actor.Name].ToString(), actor);
            actor.InstanceID = Counter[actor.Name];
            Counter[actor.Name]++;
        }

        public void RemoveActor(string instance)
        {
            Actors.Remove(instance);
        }

        public virtual void Update(object sender, TimerEventArgs e)
        {
            Time += e.Interval;
        }

        public abstract void Enter();
        public abstract void Leave();
        public abstract void KeyInput(SDL.SDL_KeyboardEvent ev);
    }
}

