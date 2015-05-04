using System;
using System.Xml;
using System.Collections.Generic;
using SFML.Graphics;

namespace Breeze.Resources
{
    public class TileBase : Resource
    {
        public IntRect[] Tiles { get; protected set; }
        public SpriteSheet Sheet { get; protected set; }

        public TileBase(string filename)
            : base(filename)
        {
            var xml = new XmlDocument();
            xml.Load(filename);

            var root = xml.DocumentElement;
            if (root.HasAttribute("name"))
                Name = root.GetAttribute("name");
            Sheet = ResourceManager.Load<SpriteSheet>(root.GetAttribute("sheet"));

            var tmp = new List<IntRect>();
            foreach (XmlNode child in root.ChildNodes)
            {
                int x = int.Parse(child.Attributes.GetNamedItem("x").InnerText);
                int y = int.Parse(child.Attributes.GetNamedItem("y").InnerText);
                int w = int.Parse(child.Attributes.GetNamedItem("w").InnerText);
                int h = int.Parse(child.Attributes.GetNamedItem("h").InnerText);
                var ld = new IntRect(x, y, w, h);
                tmp.Add(ld);
            }

            Tiles = tmp.ToArray();
        }

        public override void Free()
        {
            // Do nothing
        }
    }
}

