using System;
using System.Xml;
using SDL2;

namespace Breeze
{
	public class Sprite : Drawable
	{
		IntPtr Texture;
		int[] FrameIntervals;
		uint CurrentFrame;
		uint FrameCount;
		uint Cols;
		uint Rows;
		public double AnimSpeed;
		public bool Animated;
		SDL.SDL_Rect FrameRect;
		
		public Sprite(string filename, IntPtr renderer) : base()
		{
			AnimSpeed = 1;
			if (!filename.EndsWith("bspr"))
			{
				// Loading plain 1-frame sprite
				Texture = SDL_image.IMG_LoadTexture(renderer, filename);
				FrameCount = 1;
				Animated = false;
				return;
			}
			
			// Loading from BSPR file (XML)
			// TODO: add BSPR format info
			XmlDocument xml = new XmlDocument();
			xml.Load(filename);
			
			XmlElement root = xml.DocumentElement;
			
			foreach (XmlNode ch in root.ChildNodes)
			{
				XmlElement el = (XmlElement)ch;
				switch (el.Name.ToLower())
				{
					case "image":
						if (el.HasAttribute("cols"))
							Cols = uint.Parse(el.GetAttribute("cols"));
						break;
				}
			}
		}
		
		protected override void InternalDraw(int x, int y, double angle)
		{
			SDL.SDL_Rect drect = new SDL.SDL_Rect();
			drect.x = x;
			drect.y = y;
			drect.w = W;
			drect.h = H;
			
			SDL.SDL_RenderCopyEx(BreezeCore.Renderer, Texture, ref FrameRect, ref drect, angle, ref RotationCenter, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
		}
	}
}

