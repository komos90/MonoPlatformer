using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer {
    class Particle {
        public Texture2D SpriteSheet { get; private set; }
        public Vector2 Pos { get; private set; }
        public Vector2 Vel { get; private set; }
        public float Rotation { get; private set; }
        public float AngularVel { get; private set; }
        public float Scale { get; private set; }
        public Color Colour { get; private set; }
        public long Duration { get; private set; }
        private long spawnTime;

        public Particle() {

        }

        public void Update(GameTime gameTime) {
            float dt = gameTime.ElapsedGameTime.Seconds;
            Pos += Vel * dt;
            Rotation += AngularVel * dt;


        }
    }
}
