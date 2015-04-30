using System;
using System.IO;
using System.Collections.Generic;

namespace Breeze.Resources
{
	public static class ResourceManager
	{
        /// <summary>
        /// Specifies root directory for resources; trailing slash is required
        /// </summary>
        public static string RootDir = "";

        /// <summary>
        /// Specifies relative path for sprites; trailing slash is required
        /// </summary>
        public static string SpritesDir = "";

        /// <summary>
        /// Specifies relative path for fonts; trailing slash is required
        /// </summary>
        public static string FontsDir = "";

        /// <summary>
        /// Specifies relative path for sounds; trailing slash is required
        /// </summary>
        public static string SoundsDir = "";
		
		private static Dictionary<string, Resource> Resources;
		
        public static bool IsLoaded(string filename)
        {
            bool result = false;

            foreach (Resource res in Resources.Values)
            {
                result |= res.Source.Equals(filename);
                if (result)
                    break;
            }

            return result;
        }

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

        public static string PathToSounds()
        {
            return RootDir + SoundsDir;
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
                case "SoundBuffer":
                    return PathToSounds();
                default:
                    return RootDir;
            }
        }

        public static T Load<T>(string filename) where T : Resource
        {
            if (!IsLoaded(PathTo<T>() + filename))
            {
                T tmp = (T)Activator.CreateInstance(typeof(T), PathTo<T>() + filename);
                Resources.Add(typeof(T).Name + ":" + tmp.Name, tmp);

                return tmp;
            }
            else
            {
                var tmp = new List<Resource>(Resources.Values);
                return (T)tmp[tmp.FindIndex( (res) => res.Source.Equals(PathTo<T>() + filename) )];
            }
        }
		
		public static void Unload(string name)
		{
            if (Resources.ContainsKey(name))
            {
                Resources[name].Free();
                Resources.Remove(name);
            }
		}

        public static void Unload<T>(string name) where T : Resource
        {
            Unload(typeof(T).Name + ":" + name);
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

