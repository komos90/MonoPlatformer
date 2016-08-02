using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelEditor {

	public class Tile {

		public enum TileType {
			Ground,
			Death
		}

		public const int Width = 16;
		public const int Height = 16;

		public TileType Type { get; set; }
		public int SpriteXIndex { get; set; }
		public int SpriteYIndex { get; set; }

		public Tile(TileType tileType) {
			this.Type = tileType;
		}
	}
}
