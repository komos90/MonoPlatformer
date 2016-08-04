using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Platformer {
    public class AttackCollider : Entity {
        StatBlock attackStats;
        long lifetime;
        long startTime;

        public AttackCollider(World world, Dictionary<string, string> stats, Vector2 pos, Dimensions dims, long lifetime):
            base(world, Sprite.None)
        {
            this.startTime = world.gameTime.TotalGameTime.Ticks;
            this.lifetime = lifetime;
            attackStats = new StatBlock(stats);
            Pos = pos;
            Dims = dims;
        }

        public override void Update(GameTime gameTime) {
            if (gameTime.TotalGameTime.Ticks - startTime > lifetime) {
                Kill();
            }
        }

        public override void CollisionWith(Entity other) {
            other.DoDamage(attackStats);
        }
    }
}
