using System;
using System.IO;
using System.Collections.Generic;
using SDL2;

namespace Breeze.Resources
{
	public static class Manager
	{
		static readonly string SpritePref = "sprite.";
        static readonly string FontPref = "font.";

        public static string RootDir = "";
        public static string SpritesDir = "";
        public static string FontsDir = "";
		
		static Dictionary<string, Resource> Resources;
		
		public static void Init()
		{
			Resources = new Dictionary<string, Resource>();
		}
		
		public static void Free()
		{
			UnloadAll();
		}
		
        public static string PathToSprites()
        {
            return RootDir + SpritesDir;
        }

        public static string PathToFonts()
        {
            return RootDir + FontsDir;
        }
        
        public static void LoadSprite(string filename)
		{
            Sprite spr = new Sprite(PathToSprites() + filename);
			Resources.Add(SpritePref + spr.Name, spr);
		}

        public static void LoadFont(string filename, int pt)
        {
            Font fnt = new Font(PathToFonts() + filename, pt);
            Resources.Add(FontPref + fnt.Name, fnt);
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

        public static Font FindFont(string name)
        {
            return (Font)Resources[FontPref + name];
        }

        public static string GetResourceList()
        {
            string tmp = "";
            foreach (Resource res in Resources.Values)
                tmp += res.Name + "\n";
            return tmp;
        }
	}
}

