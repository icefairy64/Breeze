using System;
using System.Collections.Generic;
using Breeze;
using SFML.Graphics;

namespace Breeze.Graphics
{
    public static class Screen
    {
        static List<Layer> Layers;
        static Dictionary<string, Layer> LayersDict;
        static RectangleShape ClearRect;
        static SFML.Graphics.Sprite Spr;

        public static RenderTexture Buffer { get; private set; }
        public static RenderTarget CurrentTarget;
        public static float CamX;
        public static float CamY;

        public static void Init()
        {
            Layers = new List<Layer>();
            LayersDict = new Dictionary<string, Layer>();

            Buffer = new RenderTexture(BreezeCore.ScrW, BreezeCore.ScrH);
            Spr = new SFML.Graphics.Sprite(Buffer.Texture);
            ClearRect = new RectangleShape(new SFML.System.Vector2f(BreezeCore.ScrW, BreezeCore.ScrH)) { FillColor = Color.Black };
        }

        public static void InsertLayer(Layer layer)
        {
            int index = Layers.FindIndex(lr => lr.ZOrder > layer.ZOrder);
            index = (index >= 0) ? index : Layers.Count;
            Layers.Insert(index, layer);
            LayersDict.Add(layer.Name, layer);
        }

        public static void RemoveLayer(string name)
        {
            Layer lr = LayersDict[name];
            Layers.Remove(lr);
            LayersDict.Remove(name);
        }

        public static Layer CreateLayer(string name, int zorder = 0, bool chunked = true)
        {
            Layer lr = new Layer(name, zorder, chunked);
            InsertLayer(lr);
            return lr;
        }

        public static Layer FindLayer(string name)
        {
            return LayersDict[name];
        }

        public static void Draw()
        {
            // Clearing buffer
            Buffer.Draw(ClearRect);

            // Rendering
            foreach (Layer lr in Layers)
            {
                lr.View.Center = new SFML.System.Vector2f(CamX * lr.ScrollSpeed, CamY * lr.ScrollSpeed);
                lr.Draw();
            }
            
            // Drawing
            Buffer.Display();
            CurrentTarget = BreezeCore.Window;
            CurrentTarget.Draw(Spr);
        }
    }
}

