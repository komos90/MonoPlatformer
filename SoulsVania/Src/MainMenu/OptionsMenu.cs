using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Platformer {

	class OptionsMenu : Menu {
		public OptionsMenu() {
			Options = new MenuOptions(new string[] { "Music Volume", "Sound FX Volume", "Fullscreen", "Resolution" });

			int width = 400;
			int height = 120;

			this.Dimensions = new Rectangle(
				(Consts.GameWidth - width) / 2,
				(Consts.GameHeight - height) / 2,
				width,
				height);

			this.Options.Offset = new Point(this.Dimensions.Width / 8, this.Dimensions.Height / 8);
		}

		public override void Update() {
			base.Update();
		}
	}
}
