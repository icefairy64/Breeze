using System;
using System.Collections.Generic;
using System.Xml;

namespace Breeze.Resources
{
    [Serializable]
    public class SpriteBase : Resource
    {
        public SpriteSheet[] Sheets;

        public SpriteBase(string filename) 
            : base(filename)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filename);
            List<SpriteSheet> tmp = new List<SpriteSheet>();

            XmlElement root = xml.DocumentElement;
            if (root.HasAttribute("name"))
                Name = root.GetAttribute("name");

            foreach (XmlNode child in root.ChildNodes)
            {
                var ld = ResourceManager.Load<SpriteSheet>(((XmlElement)child).InnerText);
                tmp.Add(ld);
            }

            Sheets = tmp.ToArray();
        }

        public override void Free()
        {
            // Do nothing; this is managed resource type
        }
    }
}

