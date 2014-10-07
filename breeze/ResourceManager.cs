using System;
using System.Collections.Generic;
using SDL2;

namespace Breeze.Resources
{
	public static class Manager
	{
		static readonly string SpritePref = "sprite.";
		
		static Dictionary<string, Resource> Resources;
		
		public static void Init()
		{
			Resources = new Dictionary<string, Resource>();
		}
		
		public static void Free()
		{
			UnloadAll();
		}
		
		public static void LoadSprite(string filename)
		{
			Sprite spr = new Sprite(filename);
			Resources.Add(SpritePref + spr.Name, spr);
		}
		
		public static void Unload(string name)
		{
			Resources[name].Free();
			Resources.Remove(name);
		}
		
		public static void UnloadAll()
		{
			foreach (Resource res in Resources.Values)
				res.Free();
		}
		
		public static Resource Find(string name)
		{
			return Resources[name];
		}
		
		public static Sprite FindSprite(string name)
		{
			return (Sprite)Resources[SpritePref + name];
		}
	}
}

