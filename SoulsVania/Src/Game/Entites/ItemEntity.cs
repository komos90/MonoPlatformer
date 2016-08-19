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

            //TEMP Particle source
            if (world.rand.NextDouble() < 0.2) {
                world.AddParticle(new Particle(
                    Images.GetImage("particle1"),
                    Pos + new Vector2(SpriteSheet.SpriteWidth / 2, SpriteSheet.SpriteHeight / 2) + new Vector2( ((float)((world.rand.NextDouble() - 0.5) * 2.0)) * SpriteSheet.SpriteWidth / 2, ((float)((world.rand.NextDouble() - 0.5) * 2.0)) * SpriteSheet.SpriteHeight / 2), 
                    new Vector2((float)((world.rand.NextDouble() - 0.5) * 10.0), (float)((world.rand.NextDouble() - 0.5) * 10.0)),
                    0.0f,
                    (float)((world.rand.NextDouble() - 0.5) * 4.0 * Math.PI),
                    (float)(world.rand.NextDouble() * 0.5),
                    new Color(Color.SandyBrown.ToVector3() + new Vector3((float)((world.rand.NextDouble() - 0.5) * 0.2), (float)((world.rand.NextDouble() - 0.5) * 0.2), (float)((world.rand.NextDouble() - 0.5) * 0.2))),
                    (float)(world.rand.NextDouble())));
            }
        }

        public override void CollisionWith(Entity other) {
            if (other is Player) {
                Kill();
                (other as Player).PickupItem(containedItem);
                Sfx.Pickup.Play();
            }
        }
    }
}
