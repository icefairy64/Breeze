using System;
using SDL2;

namespace Breeze.Graphics
{
    public class Text : Drawable
    {
        string FValue;
        IntPtr Texture;
        public Resources.Font Font;
        public SDL.SDL_Color Color;
        SDL.SDL_Rect SrcRect;

        public string Value
        {
            get { return FValue; }
            set
            {
                FValue = value;
                int w, h, a;
                uint f;
                Texture = SDL.SDL_CreateTextureFromSurface(BreezeCore.Renderer, SDL_ttf.TTF_RenderText_Blended(Font.Handle, FValue, Color));
                SDL.SDL_SetTextureBlendMode(Texture, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
                SDL.SDL_SetTextureAlphaMod(Texture, FAlpha);
                SDL.SDL_QueryTexture(Texture, out f, out a, out w, out h);
                W = w;
                H = h;
                SrcRect = new SDL.SDL_Rect() { w = w, h = h };
            }
        }

        protected override void SetAlpha(byte alpha)
        {
            SDL.SDL_SetTextureAlphaMod(Texture, alpha);
        }

        protected override void SetBlendMode(SDL.SDL_BlendMode mode)
        {
            SDL.SDL_SetTextureBlendMode(Texture, mode);
        }

        protected override void InternalDraw(int x, int y, double angle)
        {
            SDL.SDL_RenderCopyEx(BreezeCore.Renderer, Texture, ref SrcRect, ref DstRect, angle, ref RotationCenter, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
        }

        public Text(Resources.Font font, int zorder = 0) : base(zorder)
        {
            Font = font;
            Color = new SDL.SDL_Color() { r = 0xff, g = 0xff, b = 0xff, a = 0xff };
        }
    }
}

