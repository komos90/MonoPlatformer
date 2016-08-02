using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Platformer {
    public class Sprite {

        //Animation properties
        public bool AnimPaused { get; set; }
        public int ClipXIndex { get; set; }
        public int ClipYIndex { get; set; }
        public int SpriteWidth { get; private set; }
        public int SpriteHeight { get; private set; }
        public int AnimFrameLength { get; private set; }
        private int frameCounter;
        public int[] ClipXWidths { get; private set; }
        public Texture2D SpriteSheet { get; set; }
        public bool Invisible { get; set; }
        private Dictionary<string, AnimData> animationData;
        private AnimData currentAnimData;
        public bool IsFlashed { get; private set; }
        public Color FlashColor { get; private set; }
        private int flashesLeft;
        private long flashDuration;
        private long lastFlashTime;

        public static Sprite None = new Sprite(null, 0, 0, 0, true, true);

        public Sprite(Texture2D spriteSheet, int spriteWidth, int spriteHeight, int animFrameLength, bool invisible=false, bool animPaused=false) {
            this.SpriteSheet = spriteSheet;
            this.SpriteWidth = spriteWidth;
            this.SpriteHeight = spriteWidth;
            this.Invisible = invisible;

            AnimFrameLength = animFrameLength;
            if (spriteSheet != null) { ClipXWidths = new int[spriteSheet.Height / spriteHeight]; }
            AnimPaused = animPaused;
            currentAnimData = new AnimData(0);
            animationData = new Dictionary<string,AnimData>();
            animationData.Add("default", currentAnimData);
            IsFlashed = false;
            FlashColor = Color.White;
        }

        public void Update(GameTime gameTime) {
            if (AnimPaused) return;

            int clipXwidth = SpriteSheet.Width / SpriteWidth;
            if (ClipXWidths[ClipYIndex] != 0) {
                clipXwidth = ClipXWidths[ClipXIndex];
            }

            frameCounter++;
            if (frameCounter >= AnimFrameLength) {
                frameCounter = 0;
                ClipXIndex++;
                if (ClipXIndex >= clipXwidth) {
                    ClipXIndex = 0;
                }
            }

            if (flashesLeft > 0 && gameTime.TotalGameTime.Ticks - lastFlashTime > flashDuration) {
                IsFlashed = !IsFlashed;
                lastFlashTime = gameTime.TotalGameTime.Ticks;
                flashesLeft--;
            }
            if (flashesLeft <= 0) {
                IsFlashed = false;
            }
        }

        public void AddAnimation(string name, int yClip, int length = 0) {
            animationData.Add(name, new AnimData(yClip, length));
        }

        public void PlayAnimation(string name) {
            var animData = animationData[name];
            ClipYIndex = animData.YClip;
            ClipXIndex = 0;
        }

        public void FlashSprite(int flashCount, float flashDuration=0.05f, Color? flashColor=null) {
            FlashColor = flashColor ?? Color.White;
            this.flashesLeft = 2 * flashCount;
            this.lastFlashTime = 0;
            this.flashDuration = Convert.ToInt64(flashDuration * TimeSpan.TicksPerSecond);
        }
    }
}
