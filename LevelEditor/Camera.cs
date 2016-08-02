using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor {

	public class Camera {

		public Point Pos { get; set; }

		public Camera(int x, int y) {


			Pos = new Point(0, 0);
		}
	}
}
