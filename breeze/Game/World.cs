using System;
using System.Collections.Generic;
using System.Xml;

namespace Breeze.Game
{
    public class World
    {
        public bool Chunked { get; protected set; }

        protected Dictionary<string, Entity> Entities;
        protected Dictionary<string, int> Counter;

        public World(bool chunked = true)
        {
            Entities = new Dictionary<string, Entity>();
            Counter = new Dictionary<string, int>();

            Chunked = chunked;
        }

        public void AddEntity(Entity entity)
        {
            if (!Counter.ContainsKey(entity.CommonName))
                Counter.Add(entity.CommonName, 0);

            Entities.Add(entity.CommonName + Counter[entity.CommonName], entity);
            entity.InstanceID = Counter[entity.CommonName];
            Counter[entity.CommonName]++;
        }

        public Entity GetEntity(string instance)
        {
            return Entities.ContainsKey(instance) ? Entities[instance] : null;
        }

        public void RemoveEntity(string instance)
        {
            Entities.Remove(instance);
        }

        public void RenameEntity(string currentName, string newName)
        {
            if (!Entities.ContainsKey(currentName))
                return;

            if (Entities.ContainsKey(newName))
                throw new WorldException(String.Format("Entity with name \"{0}\" already exists in current world", newName));

            Entities.Add(newName, Entities[currentName]);
            Entities.Remove(currentName);
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
    }

    public class WorldException : Exception
    {
        public WorldException(string message)
            : base(message)
        {
        }
    }
}

