using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer {

    public abstract class Item {
        public string Name { get; protected set; }
        StatBlock stats;

        // NOTE: Generalise to Entity? (Probably not)
        public virtual void Activate(Player player, World world) {

        }
        public StatBlock Passive() {
            return stats;
        }
    }
}
