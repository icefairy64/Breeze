using System;
using System.Collections.Generic;
using SDL2;

namespace Breeze.Graphics
{
	public class Layer : Drawable
	{
		protected IntPtr Buffer;
		
		public Layer()
		{
			Buffer = SDL.SDL_CreateTexture(BreezeCore.Renderer, 
			                               SDL.SDL_PIXELFORMAT_RGBA8888, 
			                               (int)SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, 
			                               BreezeCore.ScrW, 
			                               BreezeCore.ScrH);
		}
		
		protected override void InternalDraw(int x, int y, double angle)
		{
			// Do nothing
		}
		
		public override void Draw(int dx, int dy, double dangle)
		{
			SDL.SDL_SetRenderTarget(BreezeCore.Renderer, Buffer);
			base.Draw(dx, dy, dangle);
			SDL.SDL_SetRenderTarget(BreezeCore.Renderer, IntPtr.Zero);
		}
		
		public void Insert(Drawable dr)
		{
			int index = Children.FindIndex(item => item.ZOrder > dr.ZOrder);
			index = (index > 0) ? index : Children.Count;
			Children.Insert(index, dr);
		}
	}
}

