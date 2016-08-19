using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Platformer {

	public class Camera {
		private const float lerpAmount = 0.1F;

		private Entity entity;
		private Vector2 pos;

		public Camera(Entity entity) {
			this.entity = entity;

            pos = new Vector2(entity.Pos.X, entity.Pos.Y);
		}

		public void Update() {
			pos = Vector2.Lerp(pos, entity.Pos, lerpAmount); // Vector2(entity.Pos.X, 200); //

            //Correct out of screen
			Vector2 testVec = entity.Pos - pos;
			if (testVec.Y < -Consts.GameHeight / 3) {
				pos.Y = entity.Pos.Y + Consts.GameHeight / 3;
			} else if (testVec.Y > Consts.GameHeight / 3) {
				pos.Y = entity.Pos.Y  - Consts.GameHeight / 3;
			}
			if (testVec.X < -Consts.GameWidth / 3) {
				pos.X = entity.Pos.X + Consts.GameWidth / 3;
			} else if (testVec.X > Consts.GameWidth / 3) {
				pos.X = entity.Pos.X - Consts.GameWidth / 3;
			}
        }

		public Vector2 getPos() {
			return new Vector2(pos.X - Consts.GameWidth/2, pos.Y - Consts.GameHeight/2);
		}
	}
}
