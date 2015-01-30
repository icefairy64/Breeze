using System;
using System.Collections.Generic;
using System.Xml;

namespace Breeze.Resources
{
    public class SpriteBase : Resource
    {
        public string[] Sheets;

        public SpriteBase(string filename) 
            : base(filename)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(filename);
            List<string> tmp = new List<string>();

            XmlElement root = xml.DocumentElement;
            if (root.HasAttribute("name"))
                Name = root.GetAttribute("name");

            foreach (XmlNode child in root.ChildNodes)
            {
                Resource ld = ResourceManager.Load<SpriteSheet>(((XmlElement)child).InnerText);
                tmp.Add(ld.Name);
            }

            Sheets = tmp.ToArray();
        }

        public override void Free()
        {
            // Do nothing; this is managed resource type
        }
    }
}

