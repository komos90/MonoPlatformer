using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer {
	public class TileSpriteSheet {

		public Texture2D SpriteSheet {get; set;}
		public const int SpriteWidth = 32;
		public const int SpriteHeight = 32;

		public TileSpriteSheet(Texture2D spriteSheet) {
			this.SpriteSheet = spriteSheet;
		}

        public int GetWidthInTiles() {
            return SpriteSheet.Width / SpriteWidth;
        }

        public int GetHeightInTiles() {
            return SpriteSheet.Height / SpriteHeight;
        }
	}
}
