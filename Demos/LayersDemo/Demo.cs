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
            ResourceManager.GraphicsDir = String.Format("Sprites{0}", Path.DirectorySeparatorChar);
            ResourceManager.FontsDir = "";

            Core.State = new DemoState();
        }

        public static void Main(string[] args)
        {
            Core.OnInit = OnInit;

            Core.Init("Breeze", 800, 600, false);
            Core.Start();
            Core.Finish();
        }
    }
}
