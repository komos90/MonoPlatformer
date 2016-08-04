using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer {
    public class StatBlock {
        Dictionary<string, string> stats;

        public StatBlock(Dictionary<string, string> stats) {
            this.stats = stats;
        }

        void AddTo(StatBlock other) {
            foreach (var stat in other.stats) {
                if (stat.Key[0] == 'n') {
                    stats[stat.Key] += Convert.ToSingle(stat.Value);
                } else if (stat.Key[0] == 'b')  {
                    stats[stat.Key] = Convert.ToString(Convert.ToBoolean(stat.Value) || Convert.ToBoolean(stats[stat.Key]));
                }
            }
        }

        public string Get(string key) { return stats[key]; }
        public void Set(string key, string value) { stats[key] = value;  }
    }
}
