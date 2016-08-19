using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Platformer {

	public class Images {
        private static Dictionary<string, Texture2D> images;
        public static Texture2D DebugRect { get; set; }

        public static void Init(ContentManager content) {
            images = new Dictionary<string, Texture2D>();
            images.Add("Images/menu_back", content.Load<Texture2D>("Images/menu_back"));
            images.Add("Images/splash", content.Load<Texture2D>("Images/splash"));
            images.Add("Images/player", content.Load<Texture2D>("Images/player"));
            images.Add("Images/shelly_enemy", content.Load<Texture2D>("Images/shelly_enemy"));
            images.Add("Images/coin", content.Load<Texture2D>("Images/coin"));
            images.Add("Images/background1", content.Load<Texture2D>("Images/background1"));
            images.Add("Images/beach_tileset", content.Load<Texture2D>("Images/beach_tileset"));
            images.Add("Images/caves_tileset", content.Load<Texture2D>("Images/caves_tileset"));
            images.Add("Images/grass_tileset", content.Load<Texture2D>("Images/grass_tileset"));
            images.Add("Images/func_tileset", content.Load<Texture2D>("Images/func_tileset"));
            images.Add("Images/basic_sword", content.Load<Texture2D>("Images/basic_sword"));
            images.Add("Images/hp_bar_frame", content.Load<Texture2D>("Images/hp_bar_Frame"));
            images.Add("Images/hp_bar_fill", content.Load<Texture2D>("Images/hp_bar_fill"));
            images.Add("Images/dialogue_border", content.Load<Texture2D>("Images/dialogue_border"));
            images.Add("Images/particle1", content.Load<Texture2D>("Images/particle1"));
            images.Add("Images/particle2", content.Load<Texture2D>("Images/particle2"));
            images.Add("Images/dialogue_cursor", content.Load<Texture2D>("Images/dialogue_cursor"));
            images.Add("Images/dialogue_continue", content.Load<Texture2D>("Images/dialogue_continue"));
            images.Add("Images/beach_witch_npc", content.Load<Texture2D>("Images/beach_witch_npc"));
            images.Add("Images/beach_witch_profile", content.Load<Texture2D>("Images/beach_witch_profile"));
            images.Add("Images/beach_para1", content.Load<Texture2D>("Images/beach_para1"));
            images.Add("Images/beach_para2", content.Load<Texture2D>("Images/beach_para2"));
            images.Add("Images/beach_mist", content.Load<Texture2D>("Images/beach_mist"));
            images.Add("Images/herb_pouch", content.Load<Texture2D>("Images/herb_pouch"));
        }

        public static Texture2D GetImage(string name) {
            string full_name = "Images/" + name;
            return images[full_name];
        }
	}
}