using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer {

	class SplashState : GameState {
		private Color blendColor;
		private int fadeDelta;
		private int waitTicks;

		public SplashState(Main game) :
			base(game)
		{
			blendColor = Color.Black;
			fadeDelta = 2;
			waitTicks = 90;
		}

		private void HandleInput() {
			GamePadState controllerState = GamePad.GetState(PlayerIndex.One);

			if (KeyBind.WasCommandPressed(KeyBind.Command.MenuConfirm)) {
				KeyBind.Refresh();
				ChangeGameState(new MainMenuState(game));
			}
		}

		public override void Update(GameTime gameTime) {
			HandleInput();

			if (fadeDelta + (int)blendColor.R > 255) {
				blendColor.R = 255;
				blendColor.G = 255;
				blendColor.B = 255;
			} else if (fadeDelta + (int)blendColor.R < 0){
				blendColor.R = 0;
				blendColor.G = 0;
				blendColor.B = 0;
			} else {
				blendColor.R += (byte)fadeDelta;
				blendColor.G += (byte)fadeDelta;
				blendColor.B += (byte)fadeDelta;
			}

			if (blendColor.R == 255) {
				fadeDelta = -fadeDelta;
			} else if (blendColor.R == 0) {
				waitTicks--;

				if (waitTicks == 0) {
					ChangeGameState(new MainMenuState(game));
				}
			}
		}

        public override void Render(GameTime gameTime) {
            game.Renderer.RenderSplashScreen(game.SpriteBatch, blendColor);
        }

		protected override void Clean() {
		}
	}
}
