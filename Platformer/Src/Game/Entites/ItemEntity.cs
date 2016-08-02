using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Platformer {
    class ItemEntity : Entity {
        //public Dictionary<string, string> ItemStats {get; set; }
        private Item containedItem;
        Lerp yLerp;
        float baseY;

        public ItemEntity(Item containedItem, World world, Texture2D spriteSheet, int spriteWidth, int spriteHeight, int animFrameLength):
            base(world, new Sprite(spriteSheet, spriteWidth, spriteHeight, animFrameLength))
        {
            this.containedItem = containedItem;
        }

        public override void Update(GameTime gameTime) {
            if (yLerp == null) {
                baseY = Pos.Y;
                yLerp = new Lerp();
                yLerp.Add(baseY, baseY + 4, 0.5f, Lerp.Sine);
                yLerp.Add(baseY + 4, baseY, 0.5f, Lerp.Cos);
                yLerp.Add(baseY, baseY - 4, 0.5f, Lerp.Sine);
                yLerp.Add(baseY - 4, baseY, 0.5f, Lerp.Cos, true);
            }
            Pos = new Vector2(Pos.X, yLerp.Update(gameTime));
        }

        public override void CollisionWith(Entity other) {
            if (other is Player) {
                Kill();
                (other as Player).PickupItem(containedItem);
            }
        }
    }
}
