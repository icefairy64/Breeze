using System;
using System.Xml;
using System.Collections.Generic;
using SDL2;

namespace Breeze.Resources
{
	public abstract class Resource
	{
		public string Name { get; protected set; }
		
		public Resource(string name)
		{
			Name = name;
		}
	}
	
	public class Sprite : Resource
	{
		public IntPtr Texture;
		public int[] FrameIntervals;
		public uint FrameCount;
		public uint Cols;
		public uint Rows;
		public int W;
		public int H;
		public bool Animated;
		
		public Sprite(string filename) : base(filename)
		{
			// TODO: implement image size retrieving
			Animated = false;
			
			if (!filename.EndsWith("bspr"))
			{
				// Loading plain 1-frame sprite
				Texture = SDL_image.IMG_LoadTexture(BreezeCore.Renderer, filename);
				FrameCount = 1;
				return;
			}
			
			// Loading from BSPR file (XML)
			// TODO: add BSPR format info
			XmlDocument xml = new XmlDocument();
			xml.Load(filename);
			FrameCount = 0;
			Cols = 1;
			Rows = 1;
			
			XmlElement root = xml.DocumentElement;
			if (root.HasAttribute("name"))
				Name = root.GetAttribute("name");
			
			foreach (XmlNode ch in root.ChildNodes)
			{
				XmlElement el = (XmlElement)ch;
				switch (el.Name)
				{
					case "image":
						if (el.HasAttribute("cols"))
							Cols = uint.Parse(el.GetAttribute("cols"));
						if (el.HasAttribute("rows"))
							Rows = uint.Parse(el.GetAttribute("rows"));
						if (el.HasAttribute("animated"))
							Animated = el.GetAttribute("animated") == "1" ? true : false;
						Texture = SDL_image.IMG_LoadTexture(BreezeCore.Renderer, el.Value);
						break;
						
					case "frames":
						List<int> frameIntervals = new List<int>();
						foreach (XmlNode fr in el.ChildNodes)
						{
							frameIntervals.Add(int.Parse(((XmlElement)fr).GetAttribute("interval")));
							FrameCount++;
						}
						FrameIntervals = frameIntervals.ToArray();
						break;
				}
			}
		}
	}
}

