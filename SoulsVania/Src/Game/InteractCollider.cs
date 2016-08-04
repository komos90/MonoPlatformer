using Microsoft.Xna.Framework;
using System;

namespace Platformer {
    public class InteractCollider : Entity {
        long lifetime;
        long startTime;
        Entity source;

        public InteractCollider(World world, Entity source, Vector2 pos, Dimensions dims, float lifetime) :
            base(world, Sprite.None) {
            startTime = world.gameTime.TotalGameTime.Ticks;
            this.lifetime = (long)(TimeSpan.TicksPerSecond * lifetime);
            Pos = pos;
            Dims = dims;
            this.source = source;
        }

        public override void Update(GameTime gameTime) {
            if (gameTime.TotalGameTime.Ticks - startTime > lifetime) {
                Kill();
            }
        }

        public override void CollisionWith(Entity other) {
            if (!(other == source) && other.IsInteractable) {
                other.Interact();
                Kill();
            }
        }
    }
}
