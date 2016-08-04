using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Platformer {
    public class Sfx {
        public static SoundEffect Pickup { get; private set; }
        public static SoundEffect TextNoise { get; private set; }

        public static void Init(ContentManager content) {
            Pickup = content.Load<SoundEffect>("Sfx/pickup");
            TextNoise = content.Load<SoundEffect>("Sfx/text_noise");
        }
    }
}