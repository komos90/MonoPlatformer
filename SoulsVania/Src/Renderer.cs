using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer {

	public class Renderer {

        private Texture2D pixel;
        private GraphicsDevice graphicsDevice;
        private AlphaTestEffect alphaTestEffect;
        private DepthStencilState beforeDepthStencilState;
        private DepthStencilState afterDepthStencilState;
        private Matrix spriteScale;

        public Renderer(GraphicsDevice graphicsDevice) {
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            this.graphicsDevice = graphicsDevice;
            float screenscale = (float)graphicsDevice.Viewport.Width / Consts.GameWidth;
            spriteScale = Matrix.CreateScale(screenscale, screenscale, 1);
            // Prepare the alpha test effect object (create it only once on initilization)
            alphaTestEffect = new AlphaTestEffect(graphicsDevice) {
                DiffuseColor = Color.White.ToVector3(),
                AlphaFunction = CompareFunction.Greater,
                ReferenceAlpha = 0,
                World = Matrix.Identity,
                View = Matrix.Identity,
                Projection = Matrix.CreateTranslation(-0.5f, -0.5f, 0) *
                Matrix.CreateOrthographicOffCenter(0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, 0, 0, 1)
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
			spriteBatch.Draw(Images.MenuBack, new Rectangle(dims.X, dims.Y, dims.Width, dims.Height), Color.Black);
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

		public void RenderEntity(Entity entity, Camera camera, SpriteBatch spriteBatch) {
            if (entity.SpriteSheet == null) { return; }
            if (entity.SpriteSheet.Invisible == true) { return; }
			int destX = (int)(entity.Pos.X - camera.getPos().X);
			int destY = (int)(entity.Pos.Y - camera.getPos().Y);
            var sprite = entity.SpriteSheet;
            int destW = sprite.SpriteWidth;
            int destH = sprite.SpriteHeight;

            int srcX = sprite.ClipXIndex * sprite.SpriteWidth;
            int srcY = sprite.ClipYIndex * sprite.SpriteHeight;
            int srcW = sprite.SpriteWidth;
            int srcH = sprite.SpriteHeight;

            // Clear stencil buffer
            graphicsDevice.Clear(ClearOptions.Stencil, Color.Black, 0f, 0);

            // Draw your sprites using the structures above
            Color tintColour = entity.SpriteSheet.IsFlashTint ? entity.SpriteSheet.FlashColor : Color.White;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, beforeDepthStencilState, RasterizerState.CullCounterClockwise, alphaTestEffect);
            spriteBatch.Draw(sprite.SpriteSheet, new Rectangle(destX + entity.SpriteOffset.X, destY + entity.SpriteOffset.Y, destW, destH), new Rectangle(srcX, srcY, srcW, srcH), Color.Black);
            spriteBatch.End();
            
            // Draw a full screen white quad with the structure above
            if (sprite.IsFlashed && !sprite.IsFlashTint) {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, afterDepthStencilState, null);
                spriteBatch.Draw(pixel, graphicsDevice.Viewport.Bounds, sprite.FlashColor);
                spriteBatch.End();
            }
            
			if (Consts.DEBUG_MODE) {
                spriteBatch.Begin();
				spriteBatch.Draw(Images.DebugRect, new Rectangle(destX, destY, entity.Dims.Width, entity.Dims.Height), null, Color.Red);
                spriteBatch.End();
            }
		}

		public void RenderTile(Point pos, Tile tile, Camera camera, TileSpriteSheet tileSpriteSheet, SpriteBatch spriteBatch) {
			int destX = (int)(pos.X * Tile.Width - camera.getPos().X);
			int destY = (int)(pos.Y * Tile.Height - camera.getPos().Y);
			int destW = Tile.Width;
			int destH = Tile.Height;

			int srcX = tile.SpriteXIndex * TileSpriteSheet.SpriteWidth;
			int srcY = tile.SpriteYIndex * TileSpriteSheet.SpriteHeight;
			int srcW = TileSpriteSheet.SpriteWidth;
			int srcH = TileSpriteSheet.SpriteHeight;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, spriteScale);
			spriteBatch.Draw(tileSpriteSheet.SpriteSheet, new Rectangle(destX, destY, destW, destH), new Rectangle(srcX, srcY, srcW, srcH), Color.White);
            spriteBatch.End();
		}

        public void RenderSplashScreen(SpriteBatch spriteBatch, Color blendColor) {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, spriteScale);
            spriteBatch.Draw(Images.SplashScreen, new Rectangle(0, 0, Consts.GameWidth, Consts.GameHeight), blendColor);
            spriteBatch.End();
        }

        public void RenderMenuBack(SpriteBatch spriteBatch) {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, spriteScale);
            spriteBatch.Draw(Images.MenuBack, new Rectangle(0, 0, Consts.GameWidth, Consts.GameHeight), Color.White);
            spriteBatch.End();
        }

        public void RenderHpBar(SpriteBatch spriteBatch, Player player) {
            float hpFraction = Convert.ToSingle(player.Stats.Get("n_health")) / Convert.ToSingle(player.Stats.Get("n_total_health"));
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, spriteScale);
            spriteBatch.Draw(Images.HpBarFill, new Vector2(Consts.HpBarXPos, Consts.HpBarYPos), new Rectangle(0, 0,Convert.ToInt32(hpFraction * Images.HpBarFill.Width), Images.HpBarFill.Height), Color.White);
            spriteBatch.Draw(Images.HpBarFrame, new Vector2(Consts.HpBarXPos, Consts.HpBarYPos), Color.White);
            spriteBatch.End();
        }
	}
}
