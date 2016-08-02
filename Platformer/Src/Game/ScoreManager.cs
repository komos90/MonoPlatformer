using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer {

	public class ScoreManager {

		public int CoinCount { get; set; }
		public int Lives { get; set; }

		public ScoreManager() {

			CoinCount = 0;
			Lives = 3;
		}

		public void AddCoins(int coins) {

			this.CoinCount += coins;
		}
	}
}
