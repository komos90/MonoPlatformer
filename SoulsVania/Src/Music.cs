using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Platformer {
    public class Music {
        public static Song Back { get; private set; }

        public static void Init(ContentManager content) {
            Back = content.Load<Song>("Music/back.ogg");
        }
    }
}