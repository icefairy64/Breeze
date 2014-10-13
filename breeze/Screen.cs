using System;
using System.Collections.Generic;
using SDL2;

namespace Breeze.Graphics
{
    public static class Screen
    {
        static List<Layer> Layers;
        static Dictionary<string, Layer> LayersDict;
        public static IntPtr Buffer { get; private set; }
        public static int CamX;
        public static int CamY;

        public static void Init()
        {
            Layers = new List<Layer>();
            LayersDict = new Dictionary<string, Layer>();

            Buffer = SDL.SDL_CreateTexture(BreezeCore.Renderer, 
                SDL.SDL_GetWindowPixelFormat(BreezeCore.Window),
                (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET,
                BreezeCore.ScrW,
                BreezeCore.ScrH);

            SDL.SDL_SetTextureBlendMode(Buffer, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
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

        public static Layer CreateLayer(string name, int zorder = 0)
        {
            Layer lr = new Layer(name, zorder);
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
            SDL.SDL_SetRenderTarget(BreezeCore.Renderer, Buffer);
            SDL.SDL_SetRenderDrawColor(BreezeCore.Renderer, 0, 0, 0, 0xff);
            SDL.SDL_RenderClear(BreezeCore.Renderer);
            SDL.SDL_SetRenderDrawColor(BreezeCore.Renderer, 0, 0, 0, 0);

            // Rendering
            foreach (Layer lr in Layers)
                lr.Draw(-CamX, -CamY, 0);
            
            // Drawing
            SDL.SDL_SetRenderTarget(BreezeCore.Renderer, IntPtr.Zero);
            SDL.SDL_RenderCopy(BreezeCore.Renderer, Buffer, ref BreezeCore.ScrRect, ref BreezeCore.ScrRect);
        }
    }
}

