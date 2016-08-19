using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer {

	public class Tile {

		public enum TileType {
			Ground,
            Ladder,
			Death,
            Air
		}

		public const int Width = 32;
		public const int Height = 32;

		public TileType Type { get; set; }
		public int SpriteXIndex { get; set; }
		public int SpriteYIndex { get; set; }
        private string tileSheetKey;

		public Tile(TileType tileType, int spriteXIndex, int spriteYIndex, string tileSheetKey) {
			Type = tileType;
			SpriteXIndex = spriteXIndex;
			SpriteYIndex = spriteYIndex;
            this.tileSheetKey = tileSheetKey;
		}

        public void Draw(Point pos, Camera camera, SpriteBatch spriteBatch, Renderer renderer) {
            int destX = (int)(pos.X * Tile.Width - camera.getPos().X);
            int destY = (int)(pos.Y * Tile.Height - camera.getPos().Y);
            int destW = Tile.Width;
            int destH = Tile.Height;

            int srcX = SpriteXIndex * TileSpriteSheet.SpriteWidth;
            int srcY = SpriteYIndex * TileSpriteSheet.SpriteHeight;
            int srcW = TileSpriteSheet.SpriteWidth;
            int srcH = TileSpriteSheet.SpriteHeight;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, renderer.spriteScale);
            spriteBatch.Draw(Images.GetImage(tileSheetKey), new Rectangle(destX, destY, destW, destH), new Rectangle(srcX, srcY, srcW, srcH), Color.White);
            spriteBatch.End();
        }
	}
}
