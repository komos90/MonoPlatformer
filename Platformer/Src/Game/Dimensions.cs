using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer {

	public class Dimensions {
		public int Width { get; set; }
		public int Height { get; set; }

		public Dimensions(int Width = 0, int Height = 0) {
			this.Width = Width;
			this.Height = Height;
		}
	}
}
