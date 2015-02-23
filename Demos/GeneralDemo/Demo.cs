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

            BreezeCore.CurrentState = new TestState();
		}
		
		public static void Main(string[] args)
		{
			BreezeCore.OnInit = OnInit;
			
			BreezeCore.Init("Breeze", 800, 600);
			BreezeCore.Start();
			BreezeCore.Finish();
		}
	}
}
