using System;
using System.Collections.Generic;
using SDL2;

namespace Breeze.Resources
{
	public static class ResourceManager
	{
		public static Dictionary<string, Resource> Resources;
		
		public static void Init()
		{
			Resources = new Dictionary<string, Resource>();
		}
		
		public static void Free()
		{
			UnloadAll();
		}
		
		public static Sprite LoadSprite(string filename)
		{
			Sprite spr = new Sprite(filename);
			Resources.Add(spr.Name, spr);
			return spr;
		}
		
		public static void Unload(string name)
		{
			Resources[name].Free();
			Resources.Remove(name);
		}
		
		public static void UnloadAll()
		{
			if (Resources.Count == 0)
				return;
			
			foreach (Resource res in Resources.Values)
			{
				res.Free();
			}
		}
		
		public static Resource Find(string name)
		{
			return Resources[name];
		}
		
		public static Sprite FindSprite(string name)
		{
			return (Sprite)Resources[name];
		}
	}
}

