using System;
using System.IO;
using System.Collections.Generic;
using SDL2;

namespace Breeze.Resources
{
	public static class ResourceManager
	{
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

        public static string PathTo<T>() where T : Resource
        {
            switch (typeof(T).Name)
            {
                case "SpriteBase": 
                    goto case "SpriteSheet";
                case "SpriteSheet": 
                    return PathToSprites(); 
                case "Font":
                    return PathToFonts();
                default:
                    return RootDir;
            }
        }

        public static T Load<T>(string filename) where T : Resource
        {
            T tmp = (T)Activator.CreateInstance(typeof(T), PathTo<T>() + filename);
            Resources.Add(typeof(T).Name + ":" + tmp.Name, tmp);
            return tmp;
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

            Resources.Clear();
		}
		
		public static Resource Find(string name)
		{
			return Resources[name];
		}

        public static T Find<T>(string name) where T : Resource
        {
            return (T)Resources[typeof(T).Name + ":" + name];
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

