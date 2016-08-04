using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
//using Microsoft.Xna.Framework.GamerServices;

namespace Platformer {

	public abstract class GameState {
		protected Main game;

		public GameState(Main game) {
			this.game = game;
		}

		protected void ChangeGameState(GameState newGameState) {
			game.CurrentGameState = newGameState;
			Clean();
		}

		abstract public void Update(GameTime gameTime);
		abstract public void Render(GameTime gameTime);
		abstract protected void Clean();
	}
}
