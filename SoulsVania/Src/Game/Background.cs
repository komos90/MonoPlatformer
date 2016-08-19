using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Platformer {
    public class Background {
        private Texture2D image;
        private float xrate;
        private float yrate;
        private float xInitOff;
        private float yInitOff;
        private bool xloops;
        private bool yloops;

        public Background(Texture2D image, float xrate, float yrate, float xoff, float yoff, bool xloops, bool yloops) {
            this.image = image;
            this.xrate = xrate;
            this.yrate = yrate;
            this.xrate = xrate;
            this.yrate = yrate;
            xInitOff = xoff;
            yInitOff = yoff;
            this.xloops = xloops;
            this.yloops = yloops;
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera) {
            int xoffset = (int)(camera.getPos().X * xrate + xInitOff * image.Width) % Consts.GameWidth;
            int yoffset = (int)(camera.getPos().Y * yrate + yInitOff * image.Height) % Consts.GameHeight;
            int xoff2 = xoffset > 0 ? xoffset - image.Width : image.Width + xoffset;
            int yoff2 = yoffset > 0 ? yoffset - image.Height : image.Height + yoffset;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, null);
            spriteBatch.Draw(image, new Rectangle(xoffset, yoffset, image.Width, image.Height), Color.White);
            if (xoff2 != 0 && xloops) {
                spriteBatch.Draw(image, new Rectangle(xoff2, yoffset, image.Width, image.Height), Color.White);
            }
            if (yoff2 != 0 && yloops) {
                spriteBatch.Draw(image, new Rectangle(xoffset, yoff2, image.Width, image.Height), Color.White);
                if (xoff2 != 0 && xloops) {
                    spriteBatch.Draw(image, new Rectangle(xoff2, yoff2, image.Width, image.Height), Color.White);
                }
            }
            spriteBatch.End();
        }
    }
}
