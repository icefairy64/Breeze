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
            var xml = new XmlDocument();
            xml.Load(filename);
            var tmp = new List<SpriteSheet>();

            var root = xml.DocumentElement;
            if (root.HasAttribute("name"))
                Name = root.GetAttribute("name");

            foreach (XmlNode child in root.ChildNodes)
            {
                var ld = ResourceManager.Load<SpriteSheet>(child.InnerText);
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

