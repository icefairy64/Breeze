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
		
		public abstract void Free();
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
			Animated = false;
			Cols = 1;
			Rows = 1;
			uint format;
			int access;
			
			if (!filename.EndsWith("bspr"))
			{
				// Loading plain 1-frame sprite
				Texture = SDL_image.IMG_LoadTexture(BreezeCore.Renderer, filename);
				SDL.SDL_QueryTexture(Texture, out format, out access, out W, out H);
				FrameCount = 1;
			}
			else
			{
				// Loading from BSPR file (XML)
				// TODO: add BSPR format info
				XmlDocument xml = new XmlDocument();
				xml.Load(filename);
				FrameCount = 0;
				
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
							Texture = SDL_image.IMG_LoadTexture(BreezeCore.Renderer, el.InnerText);
							SDL.SDL_QueryTexture(Texture, out format, out access, out W, out H);
							W /= (int)Cols;
							H /= (int)Rows;
							break;
							
						case "frames":
							List<int> frameIntervals = new List<int>();
							int currentInterval = 0;
							foreach (XmlNode fr in el.ChildNodes)
							{
								int interval = int.Parse(((XmlElement)fr).GetAttribute("interval"));
								frameIntervals.Add(interval + currentInterval);
								currentInterval = frameIntervals[frameIntervals.Count - 1];
								FrameCount++;
							}
							FrameIntervals = frameIntervals.ToArray();
							break;
					}
				}
				
				if (FrameCount < 2)
					FrameCount = 1;
			}
		}
		
		public override void Free()
		{
			Console.WriteLine("Destroying texture: {0:x16}", Texture.ToInt64());
			SDL.SDL_DestroyTexture(Texture);
		}
	}
}

