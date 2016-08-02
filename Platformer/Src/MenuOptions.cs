using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Platformer {
	public class MenuOptions {
		public Point Offset { get; set; }
		public string[] OptionsText { get; private set; }
		public int SelectedOption { get; private set; }

		public delegate void OptionFunc();
		private OptionFunc[] optionFuncs;

		public MenuOptions(string[] OptionsText) {
			this.OptionsText = OptionsText;
			optionFuncs = new OptionFunc[OptionsText.Length];
			SelectedOption = 0;
		}

		public void SetOptionFunc(int i, OptionFunc func) {
			optionFuncs[i] = func;
		}

		public void NextOption() {
			SelectedOption = (SelectedOption + 1) % OptionsText.Length;
		}

		public void PrevOption() {
			--SelectedOption;
			if (SelectedOption < 0) SelectedOption = OptionsText.Length - 1;
		}

		public void Confirm() {
			if (optionFuncs[SelectedOption] != null) {
				optionFuncs[SelectedOption]();
			}
		}
	}
}
