using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Platformer {
    class BeachWitchNpc : Entity {
        private Dictionary<int, Dialogue.ResponseFunc> responses;

        private static void chooseToSaveTheWorld(World world) {
            world.DialogueBox = new Dialogue("The world thanks you <3<b>", Images.BeachWitchProfile);
            var p = world.player;
            for(int i = 0; i < 10; i++) {
                world.AddParticle(new Particle(
                    Images.Particle1,
                    p.Pos + new Vector2(p.SpriteSheet.SpriteWidth / 2, p.SpriteSheet.SpriteHeight / 2) + new Vector2(((float)((world.rand.NextDouble() - 0.5) * 2.0)) * p.SpriteSheet.SpriteWidth / 2, ((float)((world.rand.NextDouble() - 0.5) * 2.0)) * p.SpriteSheet.SpriteHeight / 2),
                    new Vector2((float)((world.rand.NextDouble() - 0.5) * 10.0), (float)((world.rand.NextDouble() - 0.5) * 10.0)),
                    0.0f,
                    (float)((world.rand.NextDouble() - 0.5) * 4.0 * Math.PI),
                    (float)(world.rand.NextDouble() * 0.5),
                    new Color(Color.SandyBrown.ToVector3() + new Vector3((float)((world.rand.NextDouble() - 0.5) * 0.2), (float)((world.rand.NextDouble() - 0.5) * 0.2), (float)((world.rand.NextDouble() - 0.5) * 0.2))),
                    (float)(world.rand.NextDouble())));
            }
            Sfx.Pickup.Play();
        }

        private static void chooseToRunHome(World world) {
            world.DialogueBox = new Dialogue("*Sob* *Sniffle*<b>", Images.BeachWitchProfile);
        }

        public BeachWitchNpc(World world, Sprite sprite):
            base(world, sprite)
        {
            Dims = new Dimensions(64, 64);
            IsInteractable = true;

            //Dialogue functions
            responses = new Dictionary<int, Dialogue.ResponseFunc>{
                { 0, new Dialogue.ResponseFunc(chooseToSaveTheWorld) }, 
                { 1, new Dialogue.ResponseFunc(chooseToRunHome) }
            };
    }

        public override void Interact() {
            world.DialogueBox = new Dialogue("It's strange to see another in this place.<b>Washed ashore were you?<b>Might have been better to drown<b>Not that you'd listen to me<b>Hello World! :)<b>Another Line <b><0>Save the World? <b><1>Go home? <b>", Images.BeachWitchProfile, responses);
        }
    }
}
