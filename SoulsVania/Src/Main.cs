using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        //TMP
        private Texture2D test;

		public Main () {
            Graphics = new GraphicsDeviceManager(this) { PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8 };
			shouldExit = false;
			Content.RootDirectory = "Content";
		}

		public void TryExit() {
			shouldExit = true;
		}

		protected override void Initialize() {
			renderTarget = new RenderTarget2D(GraphicsDevice, Consts.GameWidth, Consts.GameHeight, false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24Stencil8);
			CurrentGameState = new SplashState(this);

			Graphics.PreferredBackBufferWidth = Consts.WindowWidth;
			Graphics.PreferredBackBufferHeight = Consts.WindowHeight;
            //NEED THIS IF I WANT SCALE!
            Graphics.GraphicsDevice.SetRenderTarget(renderTarget);
            Graphics.ToggleFullScreen();
            Graphics.IsFullScreen = true;
            Renderer = new Renderer(Graphics.GraphicsDevice);
            Window.Title = "--- The Legend of Al ---";

			Window.AllowUserResizing = true;
			// Create the scale transform for Draw.
			// Do not scale the sprite depth (Z=1).

			base.Initialize();
		}

		protected override void LoadContent() {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Images.Init(Content);
            Music.Init(Content);
            Sfx.Init(Content);
			Images.DebugRect = new Texture2D(GraphicsDevice, 1, 1);
            Images.DebugRect.SetData(new[] { new Color(255, 255, 255, 255) });
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
            GraphicsDevice.Clear(Color.BlueViolet);
            CurrentGameState.Render(gameTime);

            Graphics.GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                        SamplerState.LinearClamp, DepthStencilState.Default,
                        RasterizerState.CullNone);
            SpriteBatch.Draw(renderTarget, new Rectangle(0, 0, Consts.WindowWidth, Consts.WindowHeight), Color.White);
            SpriteBatch.End();
            Graphics.GraphicsDevice.SetRenderTarget(renderTarget);

            base.Draw(gameTime);
		}
	}
}