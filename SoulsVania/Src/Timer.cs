using Microsoft.Xna.Framework;
using System;

namespace Platformer {
    class Timer {
        long startTime;
        long duration;

        public void Wait(GameTime gameTime, float duration) {
            startTime = gameTime.TotalGameTime.Ticks;
            this.duration = (long)(TimeSpan.TicksPerSecond * duration);
        }

        public bool IsDone(GameTime gameTime) {
            return gameTime.TotalGameTime.Ticks > startTime + duration;
        }
    }
}
