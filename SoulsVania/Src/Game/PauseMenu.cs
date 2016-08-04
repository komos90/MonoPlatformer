using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Platformer {

	class PauseMenu : Menu {

		public PauseMenu() {

			this.Options = new MenuOptions(new string[] { "Save", "Exit to Menu", "Exit to Desktop"});

			int width = 400;
			int height = 80;
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
