using System;
using System.Windows.Forms;
using System.Drawing;

static class Program {
	static void Main() {
		Application.Run(new GameForm());
	}

	const int SIZE = 120;

	class GameForm: Form {
		Button[] buttons = new Button[9];
		bool o = false;
			
		public GameForm() {
			BackColor = SystemColors.Window;
			ClientSize = new Size(SIZE * 3, SIZE * 3);
			Text = "tic tac toe";

			for (int i = 0; i < buttons.Length; i++) {
				buttons[i] = new Button();
				buttons[i].Left = (i % 3) * SIZE;
				buttons[i].Top = (i / 3) * SIZE;
				buttons[i].Size = new Size(SIZE, SIZE);
				buttons[i].Font = new Font("comic sans ms", SIZE / 3);
				buttons[i].Click += new EventHandler(ButtonClick);
				Controls.Add(buttons[i]);
			}
		}

		private void ButtonClick(object sender, EventArgs e) {
			Button b = (Button)sender;
			if (b.Text == "") { // disabling the button using button.Enabled removes the fun colors :(
				b.Text = o ? "O" : "X";
				b.ForeColor = o ? Color.Black : Color.Red;
				o = !o;
				CheckWin();
			}
		}

		private void CheckWin() {
			int[][] patterns = {new int[]{0,1,2},new int[]{3,4,5},new int[]{6,7,8},new int[]{0,3,6},new int[]{1,4,7},new int[]{2,5,8},new int[]{0,4,8},new int[]{2,4,6}};
			for (int i = 0; i < patterns.Length; i++) {
				int[] pat = patterns[i];
				if (buttons[pat[0]].Text != "" && buttons[pat[0]].Text == buttons[pat[1]].Text && buttons[pat[1]].Text == buttons[pat[2]].Text) {
					new Dialog(buttons[pat[0]].Text + "'s win", buttons[pat[0]].ForeColor).ShowDialog();
					Reset();
					return;
				}
			}
			for (int i = 0; i < buttons.Length; i++) {
				if (buttons[i].Text == "") {
					return;
				}
			}
			new Dialog("draw", Color.Black).ShowDialog();
			Reset();
		}

		private void Reset() {
			for (int i = 0; i < buttons.Length; i++) {
				buttons[i].Text = "";
			}
		}
	}

	class Dialog: Form {
		public Dialog(string msg, Color color) {
			BackColor = SystemColors.Window;
			ClientSize = new Size(SIZE * 3, SIZE);

			Label label = new Label();
			label.Size = ClientSize;
			label.Text = msg;
			label.ForeColor = color;
			label.Font = new Font("comic sans ms", SIZE / 2);
			Controls.Add(label);
		}
	}
}
