using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Platformer {

    class Lerp {
        public delegate float LFunc(float startVal, float endVal, float fractionDone);
        public delegate void Callback();
        public static float Linear(float startVal, float endVal, float fractionDone) {
            return startVal * (1 - fractionDone) + endVal * fractionDone;
        }
        public static float Sine(float startVal, float endVal, float fractionDone) {
            float conFracDone = fractionDone * (float)Math.PI / 2;
            return startVal * (1 - (float)Math.Sin(conFracDone)) + endVal * (float)Math.Sin(conFracDone);
        }
        public static float Cos(float startVal, float endVal, float fractionDone) {
            float conFracDone = fractionDone * (float)Math.PI / 2;
            return startVal * (float)Math.Cos(conFracDone) + endVal * (1 - (float)Math.Cos(conFracDone));
        }

        public LerpNode Front { get; set; }
        public LerpNode Back { get; set; }
        public LerpNode Current { get; set; }
        public bool IsDone { get; private set; }

        public Lerp() {
            IsDone = false;
        }

        public float Update(GameTime gameTime) {
            if (Current.isConst) { IsDone = true; }
            return Current.Update(gameTime);
        }

        public void Add(float startVal, float endVal, float durationInSec, Lerp.LFunc lFunc, bool loop = false, Callback callback=null) {
            long duration = Convert.ToInt64(durationInSec * TimeSpan.TicksPerSecond);
            if (Front == null) {
                Back = new LerpNode(this, startVal, endVal, duration, lFunc, null, callback);
                Front = Back;
                Current = Front;
                Front.nextLerp = loop ? Front : null;
            } else {
                Back.nextLerp = new LerpNode(this, startVal, endVal, duration, lFunc, loop ? Front : null, callback);
                Back = Back.nextLerp;
            }
        }
    }
}
