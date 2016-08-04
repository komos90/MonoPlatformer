using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platformer {

	public class MenuStack {
		private Stack<Menu> menus = new Stack<Menu>();
		public bool CanExitMenuStack { get; set; }

		public MenuStack() {
			CanExitMenuStack = true;
		}

		public void PushMenu(Menu newMenu) {
			newMenu.SetMenuStack(this);
			menus.Push(newMenu);
		}

		public bool IsStackEmpty() {
			return menus.Count == 0;
		}

		public Menu Peek() {
			return menus.Peek();
		}

		public void Pop() {
			if (menus.Count > 1 || CanExitMenuStack) menus.Pop();
		}

		public void Update() {
			if (menus.Count > 0) menus.Peek().Update();
		}
	}
}
