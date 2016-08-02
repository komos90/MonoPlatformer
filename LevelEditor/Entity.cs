using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor {

	public class Entity {

		public Point Pos {get; set;}
		public EntityType Type { get; set; }

		public enum EntityType {

			Coin
		}

		public Entity(Point pos) {

			this.Pos = pos;
		}
	}
}
