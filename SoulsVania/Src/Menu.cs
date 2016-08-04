using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Platformer {

	public class Menu {

		public Rectangle Dimensions { get; set; }
		public MenuOptions Options { get; set; }

		private MenuStack menuStack;
		private GamePadState oldControllerState;

		public Menu() {

			oldControllerState = GamePad.GetState(PlayerIndex.One);
		}

		public virtual void Update() {

			if (KeyBind.WasCommandPressed(KeyBind.Command.MenuUp)) {

				Options.PrevOption();
			}
			else if (KeyBind.WasCommandPressed(KeyBind.Command.MenuDown)) {

				Options.NextOption();
			}

			if (KeyBind.WasCommandPressed(KeyBind.Command.MenuConfirm)) {

				Options.Confirm();
			}
			else if (KeyBind.WasCommandPressed(KeyBind.Command.MenuBack)) {

				menuStack.Pop();
			}
			KeyBind.Refresh();
		}

		public void SetMenuStack(MenuStack menuStack) {

			this.menuStack = menuStack;
		}
	}
}
