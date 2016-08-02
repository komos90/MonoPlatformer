using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Platformer {

	class CollisionDetect {
		public static bool detectCollision(Vector2 point, Rectangle rect) {
			return (point.X >= rect.X && point.X <= rect.X + rect.Width &&
				point.Y >= rect.Y && point.Y <= rect.Y + rect.Height);
		}
	}
}
