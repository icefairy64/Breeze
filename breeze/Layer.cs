using System;
using System.Collections.Generic;
using SDL2;

namespace Breeze.Graphics
{
	public class Layer : Drawable
	{
		protected IntPtr Buffer;
		protected SDL.SDL_Rect SrcRect;
		public string Name { get; protected set; }
		
		public Layer(string name)
		{
			Buffer = SDL.SDL_CreateTexture(BreezeCore.Renderer, 
			                               SDL.SDL_GetWindowPixelFormat(BreezeCore.Window), 
			                               (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, 
			                               BreezeCore.ScrW, 
			                               BreezeCore.ScrH);
			SDL.SDL_SetTextureBlendMode(Buffer, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);
			
			SrcRect = new SDL.SDL_Rect() { x = 0, y = 0, w = BreezeCore.ScrW, h = BreezeCore.ScrH };
			Name = name;
		}
		
		protected override void SetAlpha(byte alpha)
		{
			SDL.SDL_SetTextureAlphaMod(Buffer, alpha);
		}
		
		protected override void InternalDraw(int x, int y, double angle)
		{
			// Do nothing
		}
		
		public override void Draw(int dx, int dy, double dangle)
		{
			SDL.SDL_SetRenderTarget(BreezeCore.Renderer, Buffer);
			SDL.SDL_SetRenderDrawColor(BreezeCore.Renderer, 255, 255, 255, 0);	// Move it!
			SDL.SDL_RenderClear(BreezeCore.Renderer);
			
			base.Draw(dx, dy, dangle);
			SDL.SDL_SetRenderTarget(BreezeCore.Renderer, IntPtr.Zero);
			
			SDL.SDL_RenderCopy(BreezeCore.Renderer, Buffer, ref SrcRect, ref SrcRect);
		}
		
		public void Insert(Drawable dr)
		{
			int index = Children.FindIndex(item => item.ZOrder > dr.ZOrder);
			index = (index >= 0) ? index : Children.Count;
			Children.Insert(index, dr);
		}
	}
}

