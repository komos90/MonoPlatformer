using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Platformer {

	public class MainGameState : GameState {

		public ScoreManager Score;
		private Player player;
		private Camera camera;
		private int currentLevel;
        private Coin hudCoin;

		private MenuStack menus;
		private PauseMenu pauseMenu;
        private World world;
        private Renderer renderer;
        private LocationText locationText;
        
		public MainGameState(Main game) :
		base(game)
		{
			menus = new MenuStack();
			pauseMenu = new PauseMenu();
			pauseMenu.Options.SetOptionFunc(1, ExitToMainMenu);
			pauseMenu.Options.SetOptionFunc(2, ExitToDesktop);

			Score = new ScoreManager();

			currentLevel = 1;

			world = new World(new TileSpriteSheet(Images.TileSpriteSheet));
            player = world.LoadLevel("../../../Content/the_level.oel");
            camera = new Camera(player);
            renderer = game.Renderer;

            hudCoin = new Coin(world);
            hudCoin.Pos = new Vector2(-9999, -9999);
            locationText = new LocationText("Welcome to Nightvale");
		}

		public void ExitToMainMenu() {
			ChangeGameState(new MainMenuState(game));
		}

		public void ExitToDesktop() {
			Clean();
			game.TryExit();
		}

		private void HandleInput() {
			if (KeyBind.WasCommandPressed(KeyBind.Command.Start)) {
				menus.PushMenu(pauseMenu);
                locationText = new LocationText("Welcome to Nightvale");
			}
			player.Input();
			KeyBind.Refresh();
		}

		public override void Update(GameTime gameTime) {
			this.menus.Update();
            if (this.menus.IsStackEmpty()) {
                camera.Update();
                HandleInput();
                world.Update(gameTime);
            }
            //Update HUD
            hudCoin.Update(gameTime);
            locationText.Update(gameTime);
		}

		public override void Render(GameTime gameTime) { 			
			//Render backgrounds
			//ToDo get backgrounds from level file
			renderer.RenderBackground(Images.Background1, game.SpriteBatch);

			{ // DrawEachTile()
				var i = world.Tiles.GetEnumerator();
				while (i.MoveNext()) {
					KeyValuePair<Point, Tile> curTile = i.Current;
					renderer.RenderTile(curTile.Key, curTile.Value, camera, world.TileSpriteSheet, game.SpriteBatch);
				}
			}
			{ // DrawEachEntity()
				foreach (Entity entity in world.Entities) {
					renderer.RenderEntity(entity, camera, game.SpriteBatch);
				}		
			}
            {// Draw locationText
                renderer.RenderLocationText(locationText, game.SpriteBatch);
            }

			renderer.RenderScore(Score, game.SpriteBatch, hudCoin);
            renderer.RenderHpBar(game.SpriteBatch, player);

			if (!menus.IsStackEmpty()) {
				renderer.RenderMenu(this.menus.Peek(), this.game.SpriteBatch);
			}
		}

		protected override void Clean() {
			System.Console.WriteLine("Cleaned");
		}
	}
}
