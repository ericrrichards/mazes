namespace mazes.UI {
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    using mazes.Algorithms;
    using mazes.Core;

    public partial class MazeForm : Form {
        private const int GridSize = 50;
        private const int MazeSize = 11;
        private Grid _grid;
        private IMazeAlgorithm _algorithm;
        private Point? _tempPoint;
        private Point? _startPoint;
        private Point? _endPoint;

        public MazeForm() {
            InitializeComponent();
            _grid = new Grid(MazeSize, MazeSize);
            pbMaze.Image = _grid.ToImg();
            cbAlgorithm.SelectedIndex = 0;
            _algorithm = new BinaryTree(_grid);

            tsmiPickStart.Click += TsmiPickStartOnClick;
            tsmiPickEnd.Click += TsmiPickEndOnClick;
        }

        private void TsmiPickEndOnClick(object sender, EventArgs eventArgs) {
            _endPoint = _tempPoint;
            tsslEndPoint.Text = "End: " + _endPoint;
        }

        private void TsmiPickStartOnClick(object sender, EventArgs eventArgs) {
            _startPoint = _tempPoint;
            tsslStartPoint.Text = "Start: " +_startPoint;
        }

        private void pbMaze_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                _tempPoint = new Point(e.X / GridSize, e.Y / GridSize);
            }
        }

        private void DrawMaze(object sender, EventArgs e) {
            if (cbAlgorithm.SelectedItem != null) {
                Image img = null;
                var grid = new Grid(MazeSize, MazeSize);
                if ((string)cbAlgorithm.SelectedItem == "BinaryTree") {
                    img = BinaryTree.Maze(grid, (int)numericUpDown1.Value).ToImg();
                } else if ((string)cbAlgorithm.SelectedItem == "Sidewinder") {
                    img = Sidewinder.Maze(grid, (int)numericUpDown1.Value).ToImg();
                }
                pbMaze.Image = img;
            }
        }

        private void ResetMaze(object sender, EventArgs e) {
            _grid = new Grid(MazeSize, MazeSize);
            pbMaze.Image = _grid.ToImg();
            if (cbAlgorithm.SelectedItem != null) {
                if ((string)cbAlgorithm.SelectedItem == "BinaryTree") {
                    _algorithm = new BinaryTree(_grid, (int)numericUpDown1.Value);
                } else if ((string)cbAlgorithm.SelectedItem == "Sidewinder") {
                    _algorithm = new Sidewinder(_grid, (int)numericUpDown1.Value);
                }
            }
            btnStep.Enabled = true;
            _startPoint = null;
            _endPoint = null;
        }

        private void StepMaze(object sender, EventArgs e) {
            if (!_algorithm.Step()) {
                btnStep.Enabled = false;
            }
            pbMaze.Image = _grid.ToImg();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                pbMaze.Image.Save(saveFileDialog1.FileName);
            }
        }

        private void btnPickColor_Click(object sender, EventArgs e) {
            if (colorDialog1.ShowDialog() == DialogResult.OK) {
                pbColor.BackColor = colorDialog1.Color;
            }
        }

        private void btnColorize_Click(object sender, EventArgs e) {
            if (cbAlgorithm.SelectedItem != null) {
                var colorGrid = new ColoredGrid(MazeSize, MazeSize);
                if ((string)cbAlgorithm.SelectedItem == "BinaryTree") {
                    BinaryTree.Maze(colorGrid, (int)numericUpDown1.Value);
                } else if ((string)cbAlgorithm.SelectedItem == "Sidewinder") {
                    Sidewinder.Maze(colorGrid, (int)numericUpDown1.Value);
                }
                var start = _startPoint.HasValue 
                    ? colorGrid[_startPoint.Value.Y, _startPoint.Value.X]
                    : colorGrid[colorGrid.Rows / 2, colorGrid.Columns / 2];
                colorGrid.Distances = start.Distances;

                colorGrid.BackColor = pbColor.BackColor;

                pbMaze.Image = colorGrid.ToImg();
            }
        }

        private void btnDrawPath_Click(object sender, EventArgs e) {
            if (_startPoint == null || _endPoint == null) {
                return;
            }
            if (cbAlgorithm.SelectedItem != null) {
                var colorGrid = new ColoredPathGrid(MazeSize, MazeSize);
                if ((string)cbAlgorithm.SelectedItem == "BinaryTree") {
                    BinaryTree.Maze(colorGrid, (int)numericUpDown1.Value);
                } else if ((string)cbAlgorithm.SelectedItem == "Sidewinder") {
                    Sidewinder.Maze(colorGrid, (int)numericUpDown1.Value);
                }
                var start = colorGrid[_startPoint.Value.Y, _startPoint.Value.X];
                var end = colorGrid[_endPoint.Value.Y, _endPoint.Value.X];

                colorGrid.Distances = start.Distances;
                colorGrid.Path = start.Distances.PathTo(end);

                tsslPathLength.Text = "Path Length: " + colorGrid.PathLength;

                colorGrid.BackColor = pbColor.BackColor;

                pbMaze.Image = colorGrid.ToImg();
            }

        }

        private void btnLongestPath_Click(object sender, EventArgs e) {
            if (cbAlgorithm.SelectedItem != null) {
                var colorGrid = new ColoredPathGrid(MazeSize, MazeSize);
                if ((string)cbAlgorithm.SelectedItem == "BinaryTree") {
                    BinaryTree.Maze(colorGrid, (int)numericUpDown1.Value);
                } else if ((string)cbAlgorithm.SelectedItem == "Sidewinder") {
                    Sidewinder.Maze(colorGrid, (int)numericUpDown1.Value);
                }
                var start = colorGrid[0, 0];
                var distances = start.Distances;
                var (newStart, distance) = distances.Max;
                start = newStart;
                distances = start.Distances;
                var (end, distance2) = distances.Max;

                _startPoint = start.Location;
                tsslStartPoint.Text = "Start: " + _startPoint;

                _endPoint = end.Location;
                tsslEndPoint.Text = "End: " + _endPoint;

                colorGrid.Distances = start.Distances;
                colorGrid.Path = start.Distances.PathTo(end);

                tsslPathLength.Text = "Path Length: " + colorGrid.PathLength;

                colorGrid.BackColor = pbColor.BackColor;

                pbMaze.Image = colorGrid.ToImg();
            }
        }
    }
}
