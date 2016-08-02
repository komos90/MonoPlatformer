using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
//using Microsoft.Xna.Framework.GamerServices;

namespace Platformer {
	
	public class Main : Game {

		public GraphicsDeviceManager Graphics { get; private set; }
		public SpriteBatch SpriteBatch { get; private set; }
		private RenderTarget2D renderTarget;
		private Matrix SpriteScale;
		public GameState CurrentGameState { get; set; }
        public Renderer Renderer { get; private set; }

		private bool shouldExit;


		public Main () {
            Graphics = new GraphicsDeviceManager(this) { PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8 };
			shouldExit = false;
			Content.RootDirectory = "Content";
		}

		public void TryExit() {
			shouldExit = true;
		}

		protected override void Initialize() {
			renderTarget = new RenderTarget2D(Graphics.GraphicsDevice, Consts.GameWidth, Consts.GameHeight);
			SpriteBatch = new SpriteBatch(GraphicsDevice);
			CurrentGameState = new SplashState(this);

			Graphics.PreferredBackBufferWidth = Consts.WindowWidth;
			Graphics.PreferredBackBufferHeight = Consts.WindowHeight;
			Graphics.GraphicsDevice.SetRenderTarget(renderTarget);
			Graphics.ApplyChanges();
            Renderer = new Renderer(Graphics.GraphicsDevice);

			Window.AllowUserResizing = true;
			// Create the scale transform for Draw.
			// Do not scale the sprite depth (Z=1).

			base.Initialize();
		}

		protected override void LoadContent() {
			Images.Init(Content);
			Images.DebugRect = new Texture2D(GraphicsDevice, 1, 1);
			Fonts.Init(Content);
		}

		protected override void UnloadContent() {
		}

		protected override void Update(GameTime gameTime) {
			CurrentGameState.Update(gameTime);
			base.Update(gameTime);

			if (this.shouldExit) {
				Exit();
			}
		}

		protected override void Draw(GameTime gameTime) {
			CurrentGameState.Render(gameTime);

		    base.Draw(gameTime);
		}
	}
}