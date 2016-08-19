using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Platformer {
    public class LocationText {
        public Vector2 Offset { get; private set; }
        private float baseY;
        public string Text { get; private set; }
        public Color Colour { get; private set; }
        public bool Alive { get; private set; }
        private Lerp yLerp;
        private Lerp colourRLerp;
        private Lerp colourGLerp;
        private Lerp colourBLerp;
        private Lerp colourALerp;
        const float riseAmount = 32;

        public LocationText(string text) {
            Alive = true;
            Text = text;
            Offset = new Vector2(Consts.GameWidth / 2, Consts.GameHeight / 3);
            baseY = Offset.Y;
            yLerp = new Lerp();
            yLerp.Add(baseY, baseY + riseAmount, 2.0f, Lerp.Linear);
            colourRLerp = new Lerp();
            colourRLerp.Add(1.0f, 0.5f, 3.0f, Lerp.Linear);
            colourGLerp = new Lerp();
            colourGLerp.Add(1.0f, 0.5f, 3.0f, Lerp.Linear);
            colourBLerp = new Lerp();
            colourBLerp.Add(1.0f, 0.5f, 3.0f, Lerp.Linear);
        }

        public void Update(GameTime gameTime) {
            if (colourALerp != null && colourALerp.IsDone) { Alive = false; }
            if (colourALerp == null && colourRLerp.IsDone) {
                colourALerp = new Lerp();
                colourALerp.Add(1.0f, 0.0f, 0.5f, Lerp.Sine);
            }
            Offset = new Vector2(Offset.X, yLerp.Update(gameTime));
            var r = colourRLerp.Update(gameTime);
            var g = colourGLerp.Update(gameTime);
            var b = colourBLerp.Update(gameTime);
            var a = colourALerp == null ? 1.0f : colourALerp.Update(gameTime);
            Colour = new Color(r, g, b, a);
        }
    }
}
