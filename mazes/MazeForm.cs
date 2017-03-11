using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mazes {
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

        private void btnDraw_Click(object sender, EventArgs e) {
            if (cbAlgorithm.SelectedItem != null) {
                Image img = null;
                var grid = new Grid(10, 10);
                if (cbAlgorithm.SelectedItem == "BinaryTree") {
                    img = BinaryTree.Maze(grid).ToPng();
                } else if (cbAlgorithm.SelectedItem == "Sidewinder") {
                    img = Sidewinder.Maze(grid).ToPng();
                }
                pbMaze.Image = img;
            }
        }

        private void btnReset_Click(object sender, EventArgs e) {
            _grid = new Grid(10, 10);
            pbMaze.Image = _grid.ToPng();
            if (cbAlgorithm.SelectedItem != null) {
                Image img = null;
                var grid = new Grid(10, 10);
                if (cbAlgorithm.SelectedItem == "BinaryTree") {
                    _algorithm = new BinaryTree(_grid);
                } else if (cbAlgorithm.SelectedItem == "Sidewinder") {
                    _algorithm = new Sidewinder(_grid);
                }
            }
            btnStep.Enabled = true;
        }

        private void btnStep_Click(object sender, EventArgs e) {
            if (!_algorithm.Step()) {
                btnStep.Enabled = false;
            }
            pbMaze.Image = _grid.ToPng();
        }
    }
}
