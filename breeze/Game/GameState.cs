using System;
using System.Collections.Generic;
using System.Xml;
using SFML.Window;

namespace Breeze.Game
{
    public abstract class GameState
    {
        protected List<BaseAutomation> Automations;
        protected List<IUpdatable> Updatables;
        public GameState Prev;
        protected uint Time = 0;

        protected GameState()
        {
            Automations = new List<BaseAutomation>();
            Updatables = new List<IUpdatable>();
        }

        public void AddEntity(Entity entity)
        {
            BreezeCore.CurrentWorld.AddEntity(entity);

            if (entity is IUpdatable)
                AddUpdatable((IUpdatable)entity);
        }

        public void RemoveEntity(string instance)
        {
            var entity = BreezeCore.CurrentWorld.GetEntity(instance);

            if (entity is IUpdatable)
                RemoveUpdatable((IUpdatable)entity);

            BreezeCore.CurrentWorld.RemoveEntity(instance);
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
            automation.OnFinish = OnAutomationFinish;
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
        public abstract void HandleKeyPress(object sender, KeyEventArgs e);
        public abstract void HandleKeyRelease(object sender, KeyEventArgs e);
    }
}

