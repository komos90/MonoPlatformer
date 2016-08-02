using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Platformer {

	public class Images {
		public static Texture2D MenuBack		{ get; private set; }
		public static Texture2D SplashScreen	{ get; private set; }
		public static Texture2D Player			{ get; private set; }
        public static Texture2D ShellyEnemy     { get; private set; }
        public static Texture2D BasicSword      { get; private set; }
        public static Texture2D Coin			{ get; private set; }
		public static Texture2D Background1		{ get; private set; }
		public static Texture2D TileSpriteSheet { get; private set; }
		public static Texture2D LevelEnd		{ get; private set; }
        public static Texture2D HpBarFrame      { get; private set; }
        public static Texture2D HpBarFill       { get; private set; }
		public static Texture2D DebugRect		{ get; set; }

		public static void Init(ContentManager content) {
			MenuBack =			content.Load<Texture2D>("Images/back");
			SplashScreen =		content.Load<Texture2D>("Images/splash");
			Player =			content.Load<Texture2D>("Images/player_base");
            ShellyEnemy =       content.Load<Texture2D>("Images/shelly_enemy");
			Coin =				content.Load<Texture2D>("Images/coin");
			Background1 =		content.Load<Texture2D>("Images/background1");
			TileSpriteSheet =	content.Load<Texture2D>("Images/beach_tileset");
            BasicSword =        content.Load<Texture2D>("Images/basic_sword");
            HpBarFrame =        content.Load<Texture2D>("Images/hp_bar_Frame");
            HpBarFill  =        content.Load<Texture2D>("Images/hp_bar_fill");
		}
	}
}