using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer {
    public class Player : Entity {
        // NOTE?: Should split into Player & PlayerData
        private List<Item> unequipedItems;
        private Item weapon;
        private Item armour;
        private Item[] itemSlots;

		private const float maxVel = 3;
		private const float walkAccn = 0.2f;
		private const float gravityAccn = 0.1f;
		private const float xMoveThreshold = 0.01f;
		private const float terminalVelocity = 6;
		private const float jumpVel = -6;
		private const int maxJumpFrames = 15;

		private int jumpFrameCount = 0;

		public Player(World world) :
			base(world, new Sprite(Images.Player, 64, 64, Consts.PlayerAnimFrameLength))
        {
            unequipedItems = new List<Item>();
            itemSlots = new Item[5];
			GravityAccn = 0.2F;
			SpriteOffset = new Point(-10, -8); 
			Dims = new Dimensions(32, 58);
            Stats.Set("n_total_health", "2.0");
		}

		override public void Kill() {

            base.Kill();
		}

		public void Input() {
            var Tiles = world.Tiles;
            SpriteSheet.AnimPaused = true;

            if (KeyBind.WasCommandPressed(KeyBind.Command.PlayerAttack)) {
                if (weapon != null) {
                    weapon.Activate(this, world);
                }
            }
            if (KeyBind.WasCommandPressed(KeyBind.Command.PlayerInteract)) {
                var offset = (Facing == Direction.Left ? -1 : 1) * new Vector2(32.0f, 0.0f);
                world.AddEntity(new InteractCollider(world, this, Pos + offset, new Dimensions(32, 32), 0.1f));
            }

			float xTargetVel = 0;
			float yTargetVel = terminalVelocity;
			if (KeyBind.WasCommandPressed(KeyBind.Command.PlayerLeft)) {

				xTargetVel = -maxVel;

                //NOTE: Should have direction variables
                //NOTE: Should have a better interface for playing/pausing etc animations
                SpriteSheet.ClipYIndex = 1;
                SpriteSheet.AnimPaused = false;
                Facing = Direction.Left;
			}
			else if (KeyBind.WasCommandPressed(KeyBind.Command.PlayerRight)) {

				xTargetVel = maxVel;

                SpriteSheet.ClipYIndex = 0;
                SpriteSheet.AnimPaused = false;
                Facing = Direction.Right;
			}
			else {

				xTargetVel = 0;
			}

			if (KeyBind.WasCommandPressed(KeyBind.Command.PlayerJump)) {

				if (isOnGround) {
					yTargetVel = jumpVel;
					jumpFrameCount = 0;
				}
			}
			if (KeyBind.IsCommandPressed(KeyBind.Command.PlayerJump)) {

				if (jumpFrameCount < maxJumpFrames) {
					yTargetVel = jumpVel;
					jumpFrameCount++;
				}

			}
			else {
				jumpFrameCount = maxJumpFrames;
			}
			KeyBind.Refresh();

			Vel = new Vector2(walkAccn * xTargetVel + Vel.X * (1 - walkAccn), gravityAccn * yTargetVel + Vel.Y * (1 - gravityAccn));
			if (Math.Abs(Vel.X) < xMoveThreshold) Vel = new Vector2(0, Vel.Y);

			if (Vel.X < 0) {

				Vector2 newPos = Pos + new Vector2(Vel.X, 0);
				Vector2 testPos = newPos + new Vector2(0, Dims.Height / 2);
				if (!DidCollideWithWall(Tiles, testPos)) {

					Pos = newPos;
				}
				else {
					int gridX = (int)((Pos.X - Player.maxVel) / Tile.Width) + 1;
					Pos = new Vector2(gridX * Tile.Width + 1, Pos.Y);
				}
			}
			else if (Vel.X > 0) {

				Vector2 newPos = Pos + new Vector2(Vel.X, 0);
				Vector2 testPos = newPos + new Vector2(Dims.Width, Dims.Height / 2);
				if (!DidCollideWithWall(Tiles, testPos)) {

					Pos = newPos;
				}
				else {
					int gridX = (int)((Pos.X + Player.maxVel) / Tile.Width);
					Pos = new Vector2(gridX * Tile.Width - 1, Pos.Y);
				}
			}

            if (SpriteSheet.AnimPaused) {
                SpriteSheet.ClipXIndex = 0;
			}
		}

        public override void Update(GameTime gameTime) {
            Vector2 newPos = Pos + new Vector2(0, Vel.Y);
            if (Vel.Y < 0) {
                Vector2 testPos = newPos + new Vector2(Dims.Width / 2, 0);
                if (!DidCollideWithWall(world.Tiles, testPos)) {
                    Pos = newPos;
                }
                else {
                    int gridY = (int)((Pos.Y + Vel.Y) / Tile.Height) + 1;
                    Pos = new Vector2(Pos.X, gridY * Tile.Width);
                    Vel = new Vector2(Vel.X, 0);
                }
            } else {
                Pos = newPos;
            }

            Tile tmpTile;
            if (world.Tiles.TryGetValue(GetBottomGridPos(), out tmpTile)) {
                if (tmpTile.Type == Tile.TileType.Ground) {
                    SetIsOnGround();
                }
                else if (tmpTile.Type == Tile.TileType.Death) {
                    Kill();
                }
            }
            else {
                SetIsOffGround();
            }

            if (world.Tiles.TryGetValue(new Point((int)((Pos.X + (Dims.Width) / 2) / Tile.Width), (int)((Pos.Y + Dims.Height / 2) / Tile.Height)), out tmpTile)) {
                if (tmpTile.Type == Tile.TileType.Death) {
                    Kill();
                }
            }
			base.Update(gameTime);
		}

		public void SetIsOnGround() {

			isOnGround = true;
			GravityAccn = 0.0F;
			Vel = new Vector2(Vel.X, 0);
			Pos = new Vector2(Pos.X, GetBottomGridPos().Y * Tile.Height - Dims.Height);
		}

		public void SetIsOffGround() {

			isOnGround = false;
			GravityAccn = 0.2F;
		}

        public void PickupItem(Item newItem) {
            unequipedItems.Add(newItem);
            // TEMP:
            weapon = newItem;
        }
	}
}
