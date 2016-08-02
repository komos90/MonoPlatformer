using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor {

	public class Level {

		public Dictionary<Point, Tile> Tiles { get; set; }
		public List<Entity> Entities { get; set; }

		public Level() {

			Tiles = new Dictionary<Point, Tile>();
			Entities = new List<Entity>();
		}

		public void AddEntity(Point gridPos, Entity.EntityType type) {

			Point pos = new Point();
			pos.X = gridPos.X * Tile.Width + (3 * Tile.Width) / 8;
			pos.Y = gridPos.Y * Tile.Height + (3 * Tile.Width) / 8;

			var newEntity = new Entity(pos);
			newEntity.Type = type;
			Entities.Add(newEntity);
		}

		public void addTile(Point key, Tile newTile) {

			Tiles.Add(key, newTile);
		}

		public void removeTile(Point key) {

			Tiles.Remove(key);
		}

		public Tile getTile(Point key) {

			return Tiles[key];
		}

		public bool doesTileExist(Point key) {

			return Tiles.ContainsKey(key);
		}
	}
}
