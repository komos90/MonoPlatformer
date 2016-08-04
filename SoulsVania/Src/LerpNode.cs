using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Platformer {

    class LerpNode {
        Lerp parent;
        public LerpNode nextLerp;
        float startVal;
        float endVal;
        long startTime;
        long duration;
        public bool isConst;
        bool isFirstUpdate;
        private Lerp.LFunc lFunc;
        private Lerp.Callback callback;

        public LerpNode(Lerp parent, float startVal, float endVal, long duration, Lerp.LFunc lFunc, LerpNode nextLerp = null, Lerp.Callback callback = null) {
            this.startVal = startVal;
            this.endVal = endVal;
            this.duration = duration;
            isFirstUpdate = true;
            this.lFunc = lFunc;
            this.nextLerp = nextLerp;
            this.parent = parent;
            this.callback = callback;
        }

        public float Update(GameTime gameTime) {
            if (isFirstUpdate) { 
                this.startTime = gameTime.TotalGameTime.Ticks;
                isFirstUpdate = false;
            } 
            if (isConst) { return endVal; }
            long delta = gameTime.TotalGameTime.Ticks - startTime;
            if (delta > duration) {
                if (callback != null) { callback(); }
                if (nextLerp != null) {
                    parent.Current = nextLerp;
                    isFirstUpdate = true;
                } else { 
                    isConst = true;
                }
                return endVal;
            }
            return lFunc(startVal, endVal, Convert.ToSingle(delta) / Convert.ToSingle(duration));
        }
    }
}
