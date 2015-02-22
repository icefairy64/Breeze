using System;
using System.IO;
using Breeze;
using Breeze.Graphics;
using Breeze.Resources;

namespace BreezeTest
{
	class MainClass
	{		
		static void OnInit()
		{
            Breeze.Resources.ResourceManager.RootDir = String.Format("..{0}..{0}data{0}", Path.DirectorySeparatorChar);
            Breeze.Resources.ResourceManager.SpritesDir = String.Format("sprites{0}", Path.DirectorySeparatorChar);

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
