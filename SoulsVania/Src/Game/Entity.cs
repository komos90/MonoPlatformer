using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer {

	public abstract class Entity {
		public float GravityAccn { get; protected set; }
		public bool GravityPaused { get; protected set; }

		public string Type { get; set; }
		public Vector2 Pos { get; set; }
		protected Vector2 oldPos;
		public Vector2 Vel { get; set; }
		public Point SpriteOffset { get; set; }
		public Dimensions Dims { get; set; }
        public bool Alive { get; set; }
        protected World world;
        // NOTE: Should I have a Character subclass for things with a stat block
        public StatBlock Stats { get; protected set; }
        public Sprite SpriteSheet { get; set; }
        public Direction Facing { get; set; }
        protected bool isOnGround;
        public float Drag;
        public bool IsInteractable;

        public Entity(World world, Sprite sprite) {
            this.world = world;
            SpriteSheet = sprite;
			GravityAccn = 0.0F;
			GravityPaused = false;
            Drag = 0.9f;
            IsInteractable = false;

			Pos = new Vector2(0f, 0f);
			Vel = new Vector2(0f, 0f);
			Dims = new Dimensions(0, 0);

			SpriteOffset = new Point();
            Alive = true;

            Stats = new StatBlock(new Dictionary<string, string> {
                {"s_team", "neutral"},
                {"n_health", "1.0"},
                {"n_total_health", "1.0"},
                {"n_phy_def", "1.0"}
            });
		}

		public bool DidCollideWithWall(Dictionary<Point, Tile> tiles, Vector2 pos) {
			Tile tmpTile;
			tiles.TryGetValue(new Point((int)(pos.X / Tile.Width), (int)(pos.Y / Tile.Height)), out tmpTile);
			if(tmpTile != null && tmpTile.Type == Tile.TileType.Ground) {

				return true;
			}
			return false;
		}

		//Get tile collision points
		public Point GetTopGridPos() {
			return new Point((int)((Pos.X + Dims.Width) / Tile.Width), (int)(Pos.Y / Tile.Height));
		}

		public Point GetLeftGridPos() {
			return new Point((int)(Pos.X / Tile.Width), (int)((Pos.Y + Dims.Height/2 ) / Tile.Height));
		}

		public Point GetRightGridPos() {
			return new Point((int)((Pos.X + Dims.Width) / Tile.Width), (int)((Pos.Y + Dims.Height / 2) / Tile.Height));
		}

		public Point GetBLGridPos() {
			return new Point((int)(Pos.X / Tile.Width), (int)((Pos.Y + Dims.Height) / Tile.Height));
		}

		public Point GetBottomGridPos() {
			return new Point((int)((Pos.X + Dims.Width / 2) / Tile.Width), (int)((Pos.Y + Dims.Height) / Tile.Height));
		}

		public Point GetBRGridPos() {
			return new Point((int)((Pos.X + Dims.Width) / Tile.Width), (int)((Pos.Y + Dims.Height) / Tile.Height));
		}

        public void DoDamage(StatBlock damageStats) {
            Console.WriteLine(this.GetType().Name + Stats.Get("n_health") + Stats.Get("s_team") + damageStats.Get("s_team"));
            // NOTE: Make it so this is easier to do,with fewer Converts
            if (damageStats.Get("s_team").CompareTo(Stats.Get("s_team")) == 0) {
                Stats.Set("n_health", Convert.ToString(Convert.ToSingle(Stats.Get("n_health"))
                                                     - Convert.ToSingle(damageStats.Get("n_phy_dmg"))
                                                     * (1.0 / Convert.ToSingle(Stats.Get("n_phy_def")))));
            }
            if (Convert.ToSingle(Stats.Get("n_health")) < 0.0) {
                SpriteSheet.FlashSprite(5);
                Kill();
            }
        }

        public virtual void Interact() {

        }

        public virtual void Kill() {
            Alive = false;
            world.RemoveEntity(this);
        }

        public virtual void Update(GameTime gameTime) {
            //if (!isOnGround) { Vel *= Drag; }
            SpriteSheet.Update(gameTime);
        }

        public void Draw(Camera camera, SpriteBatch spriteBatch, Renderer renderer) {
            if (SpriteSheet == null) { return; }
            if (SpriteSheet.Invisible == true) { return; }
            int destX = (int)(Pos.X - camera.getPos().X);
            int destY = (int)(Pos.Y - camera.getPos().Y);
            var sprite = SpriteSheet;
            int destW = sprite.SpriteWidth;
            int destH = sprite.SpriteHeight;

            int srcX = sprite.ClipXIndex * sprite.SpriteWidth;
            int srcY = sprite.ClipYIndex * sprite.SpriteHeight;
            int srcW = sprite.SpriteWidth;
            int srcH = sprite.SpriteHeight;

            // Clear stencil buffer
            renderer.graphicsDevice.Clear(ClearOptions.Stencil, Color.Black, 0f, 0);

            // Draw your sprites using the structures above
            Color tintColour = SpriteSheet.IsFlashTint ? SpriteSheet.FlashColor : Color.White;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, renderer.beforeDepthStencilState, RasterizerState.CullCounterClockwise, renderer.alphaTestEffect, renderer.spriteScale);
            spriteBatch.Draw(sprite.SpriteSheet, new Rectangle(destX + SpriteOffset.X, destY + SpriteOffset.Y, destW, destH), new Rectangle(srcX, srcY, srcW, srcH), Color.Black);
            spriteBatch.End();

            // Draw a full screen white quad with the structure above
            if (sprite.IsFlashed && !sprite.IsFlashTint) {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, renderer.afterDepthStencilState, null);
                spriteBatch.Draw(renderer.pixel, renderer.graphicsDevice.Viewport.Bounds, sprite.FlashColor);
                spriteBatch.End();
            }

            if (Consts.DEBUG_MODE) {
                spriteBatch.Begin();
                spriteBatch.Draw(Images.DebugRect, new Rectangle(destX, destY, Dims.Width, Dims.Height), null, Color.Red);
                spriteBatch.End();
            }
        }

        public virtual void CollisionWith(Entity other) {

        }

        public void Smack(Vector2 force) {
            Vel += force;
            isOnGround = false;
        }
	}
}
