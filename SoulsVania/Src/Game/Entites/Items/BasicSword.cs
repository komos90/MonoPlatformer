using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Platformer {
    class BasicSword : Item {
        private Dictionary<string, string> attackStats;
        public BasicSword() {
            Name = "basic_sword";
            attackStats = new Dictionary<string,string> {
                {"s_team", "evil"},
                {"n_phy_dmg", "5.0"}
            };
        }
        public override void Activate(Player player, World world) {
            // Play attack animation
            // block player input for X ms
            // Spawn attack collider
            float x = player.Pos.X;
            float y = player.Pos.Y + 16;
            x += player.Facing == Direction.Right ? 32 : -32;
            world.AddEntity(new AttackCollider(world, attackStats, new Vector2(x, y), new Dimensions(32, 32), 100));
        }
    }
}
