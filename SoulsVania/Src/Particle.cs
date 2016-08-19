using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer {
    public class Particle {
        public Texture2D SpriteSheet { get; private set; }
        public Vector2 Pos { get; private set; }
        public Vector2 Vel { get; private set; }
        public float Rotation { get; private set; }
        public float AngularVel { get; private set; }
        public float Scale { get; private set; }
        public Color Colour { get; private set; }
        public long Duration { get; private set; }
        private long spawnTime;
        bool isFirstUpdate;
        public bool Alive;
        // For animation ?
        //public int SpriteWidth;
        //public int ClipXIndex;
        //public int ClipXWidth;

        public Particle(Texture2D spriteSheet, Vector2 pos, Vector2 vel, float rotation, float angVel, float scale, Color colour, float duration, int spriteWidth=0) {
            Alive = true;
            Pos = pos;
            Vel = vel;
            Rotation = rotation;
            AngularVel = angVel;
            Scale = scale;
            Colour = colour;
            Duration = (long)(TimeSpan.TicksPerSecond * duration);
            isFirstUpdate = true;
            SpriteSheet = spriteSheet;
            //SpriteWidth = spriteWidth == 0 ? SpriteSheet.Width : spriteWidth;
            //ClipXWidth = SpriteWidth / SpriteSheet.Width;
        }

        public void Update(GameTime gameTime) {
            if (isFirstUpdate) {
                spawnTime = gameTime.TotalGameTime.Ticks;
                isFirstUpdate = false;
            }
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Pos += Vel * dt;
            Rotation += AngularVel * dt;
            if (Duration >= 0 && gameTime.TotalGameTime.Ticks - spawnTime > Duration) {
                Alive = false;
            }
        }

        public void Draw(Camera camera, SpriteBatch spriteBatch) {
            if (SpriteSheet == null) { return; }

            int srcX = 0;
            int srcY = 0;
            int srcW = SpriteSheet.Width;
            int srcH = SpriteSheet.Height;

            int destX = (int)(Pos.X - camera.getPos().X);
            int destY = (int)(Pos.Y - camera.getPos().Y);
            var sprite = SpriteSheet;
            int destW = (int)(srcW * Scale);
            int destH = (int)(srcH * Scale);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, RasterizerState.CullCounterClockwise, null);
            spriteBatch.Draw(SpriteSheet, null, new Rectangle(destX, destY, destW, destH), new Rectangle(srcX, srcY, srcW, srcH), new Vector2(srcW / 2, srcH / 2), Rotation, null, Colour);
            spriteBatch.End();
        }
    }
}
