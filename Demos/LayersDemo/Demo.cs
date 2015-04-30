using System;
using System.IO;
using Breeze;
using Breeze.Resources;

namespace LayersDemo
{
    class MainClass
    {
        static void OnInit()
        {
            ResourceManager.RootDir = String.Format("..{0}..{0}..{0}Data{0}Layers{0}", Path.DirectorySeparatorChar);
            ResourceManager.SpritesDir = String.Format("Sprites{0}", Path.DirectorySeparatorChar);
            ResourceManager.FontsDir = "";

            BreezeCore.CurrentState = new DemoState();
        }

        public static void Main(string[] args)
        {
            BreezeCore.OnInit = OnInit;

            BreezeCore.Init("Breeze", 800, 600, false);
            BreezeCore.Start();
            BreezeCore.Finish();
        }
    }
}
