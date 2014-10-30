using System;
using System.Collections.Generic;
using SDL2;

namespace Breeze.Game
{
    public abstract class GameState
    {
        protected Dictionary<string, Actor> Actors;
        protected Dictionary<string, int> Counter;
        protected List<BaseAutomation> Automations;
        protected List<IUpdatable> Updatables;
        public GameState Prev;
        protected uint Time = 0;

        public GameState()
        {
            Actors = new Dictionary<string, Actor>();
            Counter = new Dictionary<string, int>();
            Automations = new List<BaseAutomation>();
            Updatables = new List<IUpdatable>();
        }

        public void AddActor(Actor actor)
        {
            if (!Counter.ContainsKey(actor.Name))
                Counter.Add(actor.Name, 0);
            
            Actors.Add(actor.Name + Counter[actor.Name].ToString(), actor);
            actor.InstanceID = Counter[actor.Name];
            Counter[actor.Name]++;

            if (actor is IUpdatable)
                AddUpdatable((IUpdatable)actor);
        }

        public void RemoveActor(string instance)
        {
            if (Actors[instance] is IUpdatable)
                RemoveUpdatable((IUpdatable)Actors[instance]);

            Actors.Remove(instance);
        }

        public void AddUpdatable(IUpdatable upd)
        {
            Updatables.Add(upd);
        }

        public void RemoveUpdatable(IUpdatable upd)
        {
            Updatables.Remove(upd);
        }

        protected virtual void OnAutomationFinish(BaseAutomation sender)
        {
            RemoveAutomation(sender);
        }

        public void AddAutomation(BaseAutomation automation)
        {
            Automations.Add(automation);
            ((BaseAutomation)automation).OnFinish = OnAutomationFinish;
        }

        public void RemoveAutomation(BaseAutomation automation)
        {
            Automations.Remove(automation);
        }

        public virtual void Update(object sender, TimerEventArgs e)
        {
            Time += e.Interval;

            foreach (IUpdatable upd in Updatables)
                upd.Update(e.Interval);
        }

        public abstract void Enter();
        public abstract void Leave();
        public abstract void KeyInput(SDL.SDL_KeyboardEvent ev);
    }
}

