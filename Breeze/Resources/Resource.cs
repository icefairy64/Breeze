using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace Breeze.Resources
{
    [Serializable]
    public abstract class Resource
	{
		public string Name { get; protected set; }
        public string Source { get; protected set; }
		
        protected Resource(string name)
		{
            Source = name;
            Name = Path.GetFileNameWithoutExtension(name);
		}
		
		public abstract void Free();
	}
}

