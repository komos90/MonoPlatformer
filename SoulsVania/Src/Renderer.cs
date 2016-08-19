using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer {

	public class Renderer {

        public Texture2D pixel;
        public GraphicsDevice graphicsDevice;
        public AlphaTestEffect alphaTestEffect;
        public DepthStencilState beforeDepthStencilState;
        public DepthStencilState afterDepthStencilState;
        public Matrix spriteScale;

        public Renderer(GraphicsDevice graphicsDevice) {
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            this.graphicsDevice = graphicsDevice;
            float screenscale = 1;//(float)graphicsDevice.Viewport.Width / Consts.GameWidth;
            spriteScale = Matrix.CreateScale(screenscale, screenscale, 1);
            // Prepare the alpha test effect object (create it only once on initilization)
            alphaTestEffect = new AlphaTestEffect(graphicsDevice) {
                DiffuseColor = Color.White.ToVector3(),
                AlphaFunction = CompareFunction.Greater,
                ReferenceAlpha = 0,
                World = Matrix.Identity,
                View = Matrix.Identity,
                Projection = Matrix.CreateTranslation(-0.5f, -0.5f, 0) *
                Matrix.CreateOrthographicOffCenter(0, Consts.GameWidth, Consts.GameHeight, 0, 0, 1)
            };

            // Prepare the first DepthStencilState (create only once, or put it in a static class)
            beforeDepthStencilState = new DepthStencilState {
                StencilEnable = true,
                StencilFunction = CompareFunction.Always,
                StencilPass = StencilOperation.Replace,
                ReferenceStencil = 1,
                DepthBufferEnable = false
            };
            afterDepthStencilState = new DepthStencilState {
                StencilEnable = true,
                StencilFunction = CompareFunction.Equal,
                ReferenceStencil = 1
            };
        }

		public void RenderBackground(Texture2D background, SpriteBatch spriteBatch) {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, spriteScale);
			spriteBatch.Draw(background, new Rectangle(0, 0, Consts.GameWidth, Consts.GameHeight), Color.White);
            spriteBatch.End();
		}

		public void RenderMenu(Menu menu, SpriteBatch spriteBatch) {
			Rectangle dims = menu.Dimensions;

			int xOffset = dims.X + menu.Options.Offset.X;
			int yOffset = dims.Y + menu.Options.Offset.Y;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, spriteScale);
			spriteBatch.Draw(Images.GetImage("menu_back"), new Rectangle(dims.X, dims.Y, dims.Width, dims.Height), Color.Black);
            spriteBatch.End();
			string[] optionsText = menu.Options.OptionsText;

			for (int i = 0; i < optionsText.Length; i++) {
				var color = Color.AliceBlue;
				if (menu.Options.SelectedOption == i) color = Color.Plum;

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, spriteScale);
				spriteBatch.DrawString(Fonts.OpenSans, optionsText[i], new Vector2(xOffset, yOffset + 20 * i), color);
                spriteBatch.End();
			}
		}

		public void RenderScore(ScoreManager score, SpriteBatch spriteBatch, Coin hudCoin) {
            var sprite = hudCoin.SpriteSheet;
            int destW = sprite.SpriteWidth;
            int destH = sprite.SpriteHeight;

            int srcX = sprite.ClipXIndex * sprite.SpriteWidth;
            int srcY = sprite.ClipYIndex * sprite.SpriteHeight;
            int srcW = sprite.SpriteWidth;
            int srcH = sprite.SpriteHeight;
            
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, spriteScale);
            spriteBatch.DrawString(Fonts.OpenSans, Convert.ToString(score.CoinCount), new Vector2(Consts.HudCoinsXPos + 12, Consts.HudCoinsYPos-4), Color.Black);
            spriteBatch.Draw(sprite.SpriteSheet, new Rectangle(Consts.HudCoinsXPos, Consts.HudCoinsYPos, destW, destH), new Rectangle(srcX, srcY, srcW, srcH), Color.White);
            spriteBatch.End();
		}

        public void RenderLocationText(LocationText locationText, SpriteBatch spriteBatch) {
            if (!locationText.Alive) { return; }
            var shadowCol = new Color(0, 0, 0, locationText.Colour.A);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, spriteScale);
            spriteBatch.DrawString(Fonts.Aaargh, locationText.Text, locationText.Offset + new Vector2( 1, 0), shadowCol);
            spriteBatch.DrawString(Fonts.Aaargh, locationText.Text, locationText.Offset + new Vector2(-1, 0), shadowCol);
            spriteBatch.DrawString(Fonts.Aaargh, locationText.Text, locationText.Offset + new Vector2(0, -1), shadowCol);
            spriteBatch.DrawString(Fonts.Aaargh, locationText.Text, locationText.Offset + new Vector2(0, 1), shadowCol);
            spriteBatch.DrawString(Fonts.Aaargh, locationText.Text, locationText.Offset, locationText.Colour);
            spriteBatch.End();
        }

		public void RenderEntity(Entity entity, Camera camera, SpriteBatch spriteBatch, Renderer renderer) {
            
		}

        public void RenderSplashScreen(SpriteBatch spriteBatch, Color blendColor) {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, spriteScale);
            spriteBatch.Draw(Images.GetImage("splash"), new Rectangle(0, 0, Consts.GameWidth, Consts.GameHeight), blendColor);
            spriteBatch.End();
        }

        public void RenderMenuBack(SpriteBatch spriteBatch) {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, spriteScale);
            spriteBatch.Draw(Images.GetImage("menu_back"), new Rectangle(0, 0, Consts.GameWidth, Consts.GameHeight), Color.White);
            spriteBatch.End();
        }

        public void RenderHpBar(SpriteBatch spriteBatch, Player player) {
            float hpFraction = Convert.ToSingle(player.Stats.Get("n_health")) / Convert.ToSingle(player.Stats.Get("n_total_health"));
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, spriteScale);
            spriteBatch.Draw(Images.GetImage("hp_bar_fill"), new Vector2(Consts.HpBarXPos, Consts.HpBarYPos), new Rectangle(0, 0,Convert.ToInt32(hpFraction * Images.GetImage("hp_bar_fill").Width), Images.GetImage("hp_bar_fill").Height), Color.White);
            spriteBatch.Draw(Images.GetImage("hp_bar_frame"), new Vector2(Consts.HpBarXPos, Consts.HpBarYPos), Color.White);
            spriteBatch.End();
        }
	}
}
