using System;
using System.IO;
using Breeze;
using Breeze.Resources;

namespace BreezeTest
{
	class MainClass
	{		
		static void OnInit()
		{
            ResourceManager.RootDir = String.Format("..{0}..{0}..{0}Data{0}", Path.DirectorySeparatorChar);
            ResourceManager.SpritesDir = String.Format("sprites{0}", Path.DirectorySeparatorChar);

            Core.State = new TestState();
		}
		
		public static void Main(string[] args)
		{
			Core.OnInit = OnInit;
			
			Core.Init("Breeze", 800, 600);
			Core.Start();
			Core.Finish();
		}
	}
}
