using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Platformer {

	class Physics {

		public static void Gravity(Entity entity) {

			if (!entity.GravityPaused) {

				entity.Vel += new Vector2(0, entity.GravityAccn);
			}
		}
	}
}
