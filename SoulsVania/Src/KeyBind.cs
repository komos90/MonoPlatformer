using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Platformer {

	public class KeyBind {
		private static GamePadState oldGamePadState;
		private static KeyboardState oldKeyboardState;

		public enum Command {
			MenuConfirm,
			MenuBack,
			MenuUp,
			MenuDown,
			Start,
			
			PlayerLeft,
			PlayerRight,
			PlayerUp,
			PlayerDown,
			PlayerJump,
            PlayerAttack,
            PlayerInteract
		}

		public static void Refresh() {
			oldGamePadState = GamePad.GetState(PlayerIndex.One);
			oldKeyboardState = Keyboard.GetState();
		}

		public static bool WasCommandPressed(Command command) {
			GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
			KeyboardState keyboardState = Keyboard.GetState();

			bool result = false;

			switch (command) {
				case Command.MenuConfirm:
					if ((gamePadState.IsButtonDown(Buttons.A) && !oldGamePadState.IsButtonDown(Buttons.A)) ||
						(gamePadState.IsButtonDown(Buttons.Start) && !oldGamePadState.IsButtonDown(Buttons.Start)) ||
						(keyboardState.IsKeyDown(Keys.Enter) && !oldKeyboardState.IsKeyDown(Keys.Enter)))
					{
						result = true;
					}
					break;
				case Command.MenuBack:
					if ((gamePadState.IsButtonDown(Buttons.B) && !oldGamePadState.IsButtonDown(Buttons.B)) ||
						(keyboardState.IsKeyDown(Keys.Back) && !oldKeyboardState.IsKeyDown(Keys.Back)) ||
						(keyboardState.IsKeyDown(Keys.Escape) && !oldKeyboardState.IsKeyDown(Keys.Escape)))
					{
						result = true;
					}
					break;
				case Command.MenuUp:
					if ((gamePadState.IsButtonDown(Buttons.DPadUp) && !oldGamePadState.IsButtonDown(Buttons.DPadUp)) ||
						(gamePadState.ThumbSticks.Left.Y > 0.5 && !(oldGamePadState.ThumbSticks.Left.Y > 0.5)) ||
						(keyboardState.IsKeyDown(Keys.Up) && !oldKeyboardState.IsKeyDown(Keys.Up)) ||
						(keyboardState.IsKeyDown(Keys.W) && !oldKeyboardState.IsKeyDown(Keys.W)))
					{
						result = true;
					}
					break;
				case Command.MenuDown:
					if ((gamePadState.IsButtonDown(Buttons.DPadDown) && !oldGamePadState.IsButtonDown(Buttons.DPadDown)) ||
						(gamePadState.ThumbSticks.Left.Y < -0.5 && !(oldGamePadState.ThumbSticks.Left.Y < -0.5)) ||
						(keyboardState.IsKeyDown(Keys.Down) && !oldKeyboardState.IsKeyDown(Keys.Down)) ||
						(keyboardState.IsKeyDown(Keys.S) && !oldKeyboardState.IsKeyDown(Keys.S)))
					{
						result = true;
					}
					break;
				case Command.Start:
					if ((gamePadState.IsButtonDown(Buttons.Start) && !oldGamePadState.IsButtonDown(Buttons.Start)) ||
						(keyboardState.IsKeyDown(Keys.Escape) && !oldKeyboardState.IsKeyDown(Keys.Escape)))
					{
						result = true;
					}
					break;

				case Command.PlayerLeft:
					if ((gamePadState.IsButtonDown(Buttons.DPadLeft)) ||
						(gamePadState.ThumbSticks.Left.X < -0.5) ||
						(keyboardState.IsKeyDown(Keys.Left)) ||
						(keyboardState.IsKeyDown(Keys.A)))
					{
						result = true;
					}
					break;
				case Command.PlayerRight:
					if ((gamePadState.IsButtonDown(Buttons.DPadRight)) ||
						(gamePadState.ThumbSticks.Left.X > 0.5) ||
						(keyboardState.IsKeyDown(Keys.Right)) ||
						(keyboardState.IsKeyDown(Keys.D)))
					{
						result = true;
					}
					break;
				case Command.PlayerUp:
					if ((gamePadState.IsButtonDown(Buttons.DPadUp)) ||
						(gamePadState.ThumbSticks.Left.Y > 0.5) ||
						(keyboardState.IsKeyDown(Keys.Up)) ||
						(keyboardState.IsKeyDown(Keys.W)))
					{
						result = true;
					}
					break;
				case Command.PlayerDown:
					if ((gamePadState.IsButtonDown(Buttons.DPadDown)) ||
						(gamePadState.ThumbSticks.Left.Y < -0.5) ||
						(keyboardState.IsKeyDown(Keys.Down)) ||
						(keyboardState.IsKeyDown(Keys.S)))
					{
						result = true;
					}
					break;
				case Command.PlayerJump:
					if ((gamePadState.IsButtonDown(Buttons.A) && !oldGamePadState.IsButtonDown(Buttons.A)) ||
						(keyboardState.IsKeyDown(Keys.Space) && !oldKeyboardState.IsKeyDown(Keys.Space)))
					{
						result = true;
					}
					break;
                case Command.PlayerAttack:
                    if ((gamePadState.IsButtonDown(Buttons.X) && !oldGamePadState.IsButtonDown(Buttons.X)) ||
                        (keyboardState.IsKeyDown(Keys.F) && !oldKeyboardState.IsKeyDown(Keys.F))) {
                        result = true;
                    }
                    break;
                case Command.PlayerInteract:
                    if ((gamePadState.IsButtonDown(Buttons.B) && !oldGamePadState.IsButtonDown(Buttons.B)) ||
                        (keyboardState.IsKeyDown(Keys.G) && !oldKeyboardState.IsKeyDown(Keys.G))) {
                        result = true;
                    }
                    break;
            }
			return result;
		}

		public static bool IsCommandPressed(Command command) {
			GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
			KeyboardState keyboardState = Keyboard.GetState();

			bool result = false;

			switch (command) {
				case Command.PlayerJump:
					if (gamePadState.IsButtonDown(Buttons.A) ||
						keyboardState.IsKeyDown(Keys.Space))
					{
						result = true;
					}
					break;
			}
			return result;
		}
	}
}
