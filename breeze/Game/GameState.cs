using System;
using System.Collections.Generic;
using System.Xml;
using SFML.Window;

namespace Breeze.Game
{
    public abstract class GameState
    {
        protected Dictionary<string, Entity> Entities;
        protected Dictionary<string, int> Counter;
        protected List<BaseAutomation> Automations;
        protected List<IUpdatable> Updatables;
        public GameState Prev;
        protected uint Time = 0;

        protected GameState()
        {
            Entities = new Dictionary<string, Entity>();
            Counter = new Dictionary<string, int>();
            Automations = new List<BaseAutomation>();
            Updatables = new List<IUpdatable>();
        }

        public void AddEntity(Entity entity)
        {
            if (!Counter.ContainsKey(entity.Name))
                Counter.Add(entity.Name, 0);
            
            Entities.Add(entity.Name + Counter[entity.Name].ToString(), entity);
            entity.InstanceID = Counter[entity.Name];
            Counter[entity.Name]++;

            if (entity is IUpdatable)
                AddUpdatable((IUpdatable)entity);
        }

        public void RemoveEntity(string instance)
        {
            if (Entities[instance] is IUpdatable)
                RemoveUpdatable((IUpdatable)Entities[instance]);

            Entities.Remove(instance);
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

        public void ImportMap(string filename)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filename);

            XmlElement root = xml.DocumentElement;
            foreach (XmlNode ch in root.ChildNodes)
            {
                XmlElement el = (XmlElement)ch;
                switch (el.Name)
                {
                    case "entity":
                        // Spawning entities
                        foreach (XmlNode anode in el.ChildNodes)
                        {
                            BuildParams par = new BuildParams();
                            string type = "";
                            foreach (XmlAttribute attr in ((XmlElement)anode).Attributes)
                            {
                                if (attr.Name != "type")
                                    par.Add(attr.Name, attr.Value);
                                else
                                    type = attr.Value;
                            }
                            AddEntity(Entity.Build(Type.GetType(type), par));
                        }
                        break;
                }
            }
        }

        public abstract void Enter();
        public abstract void Leave();
        public abstract void HandleKeyPress(object sender, KeyEventArgs e);
        public abstract void HandleKeyRelease(object sender, KeyEventArgs e);
        public abstract void ProcessEvent(object sender, EventArgs e);
    }
}

