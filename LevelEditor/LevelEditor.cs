using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LevelEditor {
	public partial class LevelEditor : Form {

		Level level;
		Camera camera;

		String currentFilePath = "";
		PaintMode paintMode = PaintMode.Tile;
		int oldMouseX;
		int oldMouseY;

		public LevelEditor() {
			InitializeComponent();

			level = new Level();
			camera = new Camera(0, 0);
			panel1.Paint += panel1_Paint;
			panel1.MouseDown += panel1_MouseClick;
			panel1.MouseMove += panel1_MouseMove;

			oldMouseX = 0;
			oldMouseY = 0;
		}

		private void SaveLevel(StreamWriter sw) {

			foreach (KeyValuePair<Point, Tile> t in level.Tiles) {

				sw.WriteLine(Convert.ToString(t.Value.Type) + "," + Convert.ToString(t.Key.X) + "," + Convert.ToString(t.Key.Y) + "," + t.Value.SpriteXIndex + "," + t.Value.SpriteYIndex);
			}
			sw.WriteLine("#");
			foreach (Entity e in level.Entities) {

				sw.WriteLine(Convert.ToString(e.Type) + "," + Convert.ToString(e.Pos.X) + "," + Convert.ToString(e.Pos.Y));
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e) {

		}

		private void Form1_Load(object sender, EventArgs e) {
			ListBox tilesListBox = new ListBox();
			tilesListBox.Width = tabControl1.SelectedTab.Width;
			tilesListBox.Height = tabControl1.SelectedTab.Height;
			tilesListBox.Items.Add("Ground Tile");
			tilesListBox.Items.Add("Death Tile");
			tilesListBox.Text = "Test";
			tabControl1.SelectedTab.Controls.Add(tilesListBox);
		}

		private void panel1_Paint(Object sender, PaintEventArgs e) {

			using(Graphics g = e.Graphics) {

				foreach (KeyValuePair<Point, Tile> t in level.Tiles) {

					int x = t.Key.X * Tile.Width - camera.Pos.X;
					int y = t.Key.Y * Tile.Height - camera.Pos.Y;
					int w = Tile.Width;
					int h = Tile.Height;

					Color color = Color.Pink;
					switch (t.Value.Type) {

						case Tile.TileType.Ground:

							color = Color.Brown;
							break;
						case Tile.TileType.Death:

							color = Color.Red;
							break;
						default:
							break;
					}
					g.FillRectangle(new SolidBrush(color), new Rectangle(x, y, w, h));
				}

				foreach (Entity en in level.Entities) {

					int x = en.Pos.X - camera.Pos.X;
					int y = en.Pos.Y - camera.Pos.Y;
					int w = Tile.Width / 4;
					int h = Tile.Height / 4;

					switch (en.Type) {

						case Entity.EntityType.Coin:

							g.FillRectangle(new SolidBrush(Color.Yellow), new Rectangle(x, y, w, h));
							break;
						default:
							break;
					}

				}
			}
			base.OnPaint(e);
		}

		private void panel1_MouseClick(Object sender, MouseEventArgs e) {

			refreshPaintMode();

			int gridX = (int)((e.X + camera.Pos.X) / Tile.Width);
			int gridY = (int)((e.Y + camera.Pos.Y) / Tile.Height);
			Point gridPoint = new Point(gridX, gridY);

			if (e.Button == MouseButtons.Left) {

				if (paintMode == PaintMode.Entity) {

					addEntity(gridPoint, Entity.EntityType.Coin);
				}
				this.Refresh();
			}

			panel1_MouseMove(sender, e);
		}

		private void refreshPaintMode() {

			switch (tabControl1.SelectedIndex) {

				case 0:
					paintMode = PaintMode.Tile;
					
					break;
				case 2:
					paintMode = PaintMode.Entity;

					break;
				default:
					break;
			}
		}

		private void addTile(Point gridPoint, Tile.TileType type) {

			if (!level.doesTileExist(gridPoint)) {
				Tile tmpTile = new Tile(type);
				tmpTile.SpriteXIndex = 1;
				tmpTile.SpriteYIndex = 0;
				if(tmpTile.Type == Tile.TileType.Death){
					tmpTile.SpriteXIndex = 0;
					tmpTile.SpriteYIndex = 2;
				}
				level.addTile(gridPoint, tmpTile);
			}
		}

		private void removeTile(Point gridPoint) {

			if (level.doesTileExist(gridPoint)) {

				level.removeTile(gridPoint);
			}
		}

		private void addEntity(Point gridPoint, Entity.EntityType type) {

			level.AddEntity(gridPoint, type);
		}

		private void panel1_MouseMove(Object sender, MouseEventArgs e) {

			int gridX = (int)((e.X + camera.Pos.X) / Tile.Width);
			int gridY = (int)((e.Y + camera.Pos.Y) / Tile.Height);
			Point gridPoint = new Point(gridX, gridY);

			if (e.Button == MouseButtons.Left) {

				if (paintMode == PaintMode.Tile) {

					ListBox tileListBox = (ListBox)tabControl1.SelectedTab.Controls[0];
					Tile.TileType selectedTile = (Tile.TileType)tileListBox.SelectedIndex;
					addTile(gridPoint, selectedTile);
				}
				this.Refresh();
			}
			else if (e.Button == MouseButtons.Right) {

				if (paintMode == PaintMode.Tile) {

					removeTile(gridPoint);
				}
				this.Refresh();
			}
			if (e.Button == MouseButtons.Middle) {

				camera.Pos.X -= e.X - oldMouseX;
				camera.Pos.Y -= e.Y - oldMouseY;
				this.Refresh();
			}
			oldMouseX = e.X;
			oldMouseY = e.Y;
		}


		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {

			string savedFile = "";

			saveFileDialog1.InitialDirectory = "$HOME";
			saveFileDialog1.Title = "Save Level";
			saveFileDialog1.FileName = "";

			if(saveFileDialog1.ShowDialog() != DialogResult.Cancel) {

				savedFile = saveFileDialog1.FileName;

				using (StreamWriter sw = new StreamWriter(savedFile)) {

					SaveLevel(sw);
				}
			}
		}
	}
}
