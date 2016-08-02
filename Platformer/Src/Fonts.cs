using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Platformer {

	public class Fonts {

		public static SpriteFont OpenSans { get; private set; }
        public static SpriteFont Aaargh { get; private set; }

		public static void Init(ContentManager content) {

			OpenSans = content.Load<SpriteFont>("Fonts/PressStart2P");
            Aaargh = content.Load<SpriteFont>("Fonts/Aaargh");
		}
	}
}
