using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Platformer {

	public class Utils {

		public static bool CollidesWith(Rectangle r1, Rectangle r2) {

			return (r1.X + r1.Width >= r2.X &&
				r1.Y + r1.Height >= r2.Y &&
				r2.X + r2.Width >= r1.X &&
				r2.Y + r2.Width >= r1.Y);
		}
	}
}
