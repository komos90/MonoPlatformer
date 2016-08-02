using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Platformer {

	class MainMenu : Menu {
		public MainMenu() {
			Options = new MenuOptions(new string[] {"New Game", "Load Game", "Options", "Exit"});

			int width = 200;
			int height = 120;

			Dimensions = new Rectangle(
				(Consts.GameWidth - width) / 2,
				(Consts.GameHeight - height) / 2, 
				width, 
				height);

			this.Options.Offset = new Point(this.Dimensions.Width / 4, this.Dimensions.Height / 8);
		}

		public override void Update() {
			base.Update();
		}
	}
}
