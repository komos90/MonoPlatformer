using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Platformer {

	public class MainMenuState : GameState {
		private MenuStack menus;
        private Renderer renderer;
		public MainMenuState(Main game) :
			base(game)
		{
			MainMenu mainMenu = new MainMenu();
			mainMenu.Options.SetOptionFunc(0, ToMainGameState);
			mainMenu.Options.SetOptionFunc(2, OpenOptionsMenu);
			mainMenu.Options.SetOptionFunc(3, Exit);

			menus = new MenuStack();
			menus.CanExitMenuStack = false;
			menus.PushMenu(mainMenu);
            renderer = game.Renderer;
		}

		public void OpenOptionsMenu() {
			var optionsMenu = new OptionsMenu();
			menus.PushMenu(optionsMenu);
		}

		public void Exit() {
			Clean();
			game.TryExit();
		}

		public void ToMainGameState() {
			Clean();
			game.CurrentGameState = new MainGameState(game);
		}

		public override void Update(GameTime gameTime) {
			menus.Update();
		}

		public override void Render(GameTime gameTime) {
            renderer.RenderMenuBack(game.SpriteBatch);
			renderer.RenderMenu(this.menus.Peek(), this.game.SpriteBatch);
		}

		protected override void Clean() {
		}
	}
}
