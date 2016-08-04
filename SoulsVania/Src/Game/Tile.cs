using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer {

	public class Tile {

		public enum TileType {
			Air,
			Ground,
			Death
		}

		public const int Width = 32;
		public const int Height = 32;

		public TileType Type { get; set; }
		public int SpriteXIndex { get; set; }
		public int SpriteYIndex { get; set; }

		public Tile(TileType tileType, int spriteXIndex, int spriteYIndex) {
			this.Type = tileType;
			this.SpriteXIndex = spriteXIndex;
			this.SpriteYIndex = spriteYIndex;
		}
	}
}
