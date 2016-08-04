using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer {

	public class Coin : Entity {
		public Coin(World world) :
			base(world, new Sprite(Images.Coin, 8, 8, Consts.CoinAnimFrameLength))
		{
			GravityAccn = 0.0f;
			SpriteOffset = new Point(0, 0);
			Dims = new Dimensions(8, 8);
		}

        public override void Update(GameTime gameTime) {
			base.Update(gameTime);
		}
	}
}