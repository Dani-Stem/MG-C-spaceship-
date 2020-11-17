using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MonoGame
{
	class Bullet
	{

		public Texture2D Texture;
		public Vector2 Position;
		public bool Active;
		Viewport viewport;
		public float movespeed;

		public int Width
		{
			get { return Texture.Width; }
		}

		public int Height
		{
			get { return Texture.Height; }
		}

		public void Initialize(Viewport viewport, Texture2D texture, Vector2 position, float Movespeed )
		{
			Texture = texture;
			Position = position;
			this.viewport = viewport;
			Active = true;
			movespeed = Movespeed;
		}

		public void Update()
		{
			Position.Y -= movespeed;

			if(Position.Y + Texture.Width / 2 > viewport.Width){
				Active = false;

			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Texture, Position, null, Color.White, 0f,
            new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);

		}



	}

}