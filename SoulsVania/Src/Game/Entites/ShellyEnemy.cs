﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Platformer {
    class ShellyEnemy : Entity {
        private Vector2[] patrolPts;
        private int currentPatrolPt = 0;
        private float patrolPtNearDelta = 10.0f;
        private float maxVel = 2;
        private StatBlock attackStats;

        public ShellyEnemy(World world, Vector2 patrolPt1, Vector2 patrolPt2):
            base(world, new Sprite(Images.GetImage("shelly_enemy"), 32, 32, Consts.PlayerAnimFrameLength))
        {
            Stats.Set("s_team", "evil");
            this.patrolPts = new Vector2[2];
            patrolPts[0] = patrolPt1;
            patrolPts[1] = patrolPt2;
            Dims = new Dimensions(32, 32);
            attackStats = new StatBlock(new Dictionary<string, string> {
                {"s_team", "good"},
                {"n_phy_dmg", "5.0"}
            });
        }

        public void Move(Direction dir) {
            switch (dir) {
                case Direction.Left:
                    SpriteSheet.ClipYIndex = 0;
                    Vel = new Vector2(-maxVel, Vel.Y);
                    break;
                case Direction.Right:
                    SpriteSheet.ClipYIndex = 1;
                    Vel = new Vector2(maxVel, Vel.Y);
                    break;
            }
        }

        override public void Update(GameTime gameTime) {
            float tmp = (Pos - patrolPts[currentPatrolPt]).Length();
            if ((Pos - patrolPts[currentPatrolPt]).Length() < patrolPtNearDelta) {
                currentPatrolPt = (currentPatrolPt + 1) % 2;
            }
            if ((Pos - patrolPts[currentPatrolPt]).X < 0) {
                Move(Direction.Right);
            } else {
                Move(Direction.Left);
            }
            //NOTE: Need to move player physics code up into entity and use that!!!
            Pos += Vel;
            base.Update(gameTime);
        }

        public override void CollisionWith(Entity other) {
            if (other is Player) {
                Vector2 smack = Vector2.Normalize(other.Pos - Pos) * 8.0f;
                other.Smack(smack);
                other.SpriteSheet.FlashSprite(1, 0.002f, Color.Red);
                other.DoDamage(attackStats);
            }
        }
    }
}
