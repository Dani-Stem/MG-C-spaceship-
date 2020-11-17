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
	class Score : IComparable<Score>
	{
		public string PlayerName { get; set; }

        public int PlayerScore { get; set; }

		public override string ToString()
		{
			return "Name: " + PlayerName + "Score: " + PlayerScore;
		}

		//loads

		//save

		public int CompareTo(Score comparescore){
			if(comparescore == null){
				return 1;
			}
			else
			{
				return this.PlayerScore.CompareTo(comparescore.PlayerScore);
			}
		}

	}
}