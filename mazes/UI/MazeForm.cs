namespace mazes.UI {
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    using mazes.Algorithms;
    using mazes.Core;

    public partial class MazeForm : Form {
        private Grid _grid;
        private IMazeAlgorithm _algorithm;

        public MazeForm() {
            InitializeComponent();
            _grid = new Grid(10, 10);
            pbMaze.Image = _grid.ToPng();
            cbAlgorithm.SelectedIndex = 0;
            _algorithm = new BinaryTree(_grid);
        }

        private void DrawMaze(object sender, EventArgs e) {
            if (cbAlgorithm.SelectedItem != null) {
                Image img = null;
                var grid = new Grid(10, 10);
                if ((string)cbAlgorithm.SelectedItem == "BinaryTree") {
                    img = BinaryTree.Maze(grid, (int)numericUpDown1.Value).ToPng();
                } else if ((string)cbAlgorithm.SelectedItem == "Sidewinder") {
                    img = Sidewinder.Maze(grid, (int)numericUpDown1.Value).ToPng();
                }
                pbMaze.Image = img;
            }
        }

        private void ResetMaze(object sender, EventArgs e) {
            _grid = new Grid(10, 10);
            pbMaze.Image = _grid.ToPng();
            if (cbAlgorithm.SelectedItem != null) {
                if ((string)cbAlgorithm.SelectedItem == "BinaryTree") {
                    _algorithm = new BinaryTree(_grid, (int)numericUpDown1.Value);
                } else if ((string)cbAlgorithm.SelectedItem == "Sidewinder") {
                    _algorithm = new Sidewinder(_grid, (int)numericUpDown1.Value);
                }
            }
            btnStep.Enabled = true;
        }

        private void StepMaze(object sender, EventArgs e) {
            if (!_algorithm.Step()) {
                btnStep.Enabled = false;
            }
            pbMaze.Image = _grid.ToPng();
        }
    }
}
