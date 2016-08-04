using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer {
    class AnimData {
        public int YClip;
        public int length = 0;

        public AnimData(int yClip, int length=0) {
            YClip = yClip;
            this.length = length;
        }
    }
}
