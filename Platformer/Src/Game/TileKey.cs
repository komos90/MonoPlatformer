using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Platformer {

	public class TileKey {

		public int X { get; set; }
		public int Y { get; set; }

		public TileKey() {

		}

		public TileKey(int x, int y) {

			this.X = x;
			this.Y = y;
		}

		public override bool Equals(object obj) {

			if (obj == null) {
				return false;
			}
			else if (this.GetType() != obj.GetType()) {
				return false;
			}
			else if (this.X == ((TileKey)obj).X && this.Y == ((TileKey)obj).Y) {
				return true;
			}
			else {
				return false;
			}
		}

		public override int GetHashCode() {

			var sha = new SHA1Managed();

			byte[] xBytes = BitConverter.GetBytes(this.X);
			byte[] yBytes = BitConverter.GetBytes(this.Y);
			byte[] total = new byte[8];

			xBytes.CopyTo(total, 0);
			yBytes.CopyTo(total, xBytes.Length);

			byte[] hashValue = sha.ComputeHash(total);

			return BitConverter.ToInt32(hashValue, 0);
		}
	}
}
