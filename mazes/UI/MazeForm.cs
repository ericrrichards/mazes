using System.Linq;
using System.Reflection;
using System.Threading;

namespace mazes.UI {
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    using Algorithms;
    using Core;

    public partial class MazeForm : Form {
        private const int GridSize = 25;
        private const int MazeSize = 11;
        private Grid _grid;
        private IMazeAlgorithm _algorithm;
        private Point? _tempPoint;
        private Point? _startPoint;
        private Point? _endPoint;

        private bool IsAnimating;

        public MazeForm() {
            InitializeComponent();
            _grid = new Grid(MazeSize, MazeSize);
            pbMaze.Image = _grid.ToImg(GridSize);
            var it = typeof(IMazeAlgorithm);
            var algoNames = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                .Where(t => it.IsAssignableFrom(t) && t.IsClass).Select(t => t.Name).ToArray();
            cbAlgorithm.Items.AddRange(algoNames);

            cbAlgorithm.SelectedIndex = 0;
            SetAlgorithm();

            tsmiPickStart.Click += TsmiPickStartOnClick;
            tsmiPickEnd.Click += TsmiPickEndOnClick;
        }

        private void TsmiPickEndOnClick(object sender, EventArgs eventArgs) {
            if (pbMask.Image != null) {
                return;
            }
            _endPoint = _tempPoint;
            tsslEndPoint.Text = "End: " + _endPoint;
        }

        private void TsmiPickStartOnClick(object sender, EventArgs eventArgs) {
            if (pbMask.Image != null) {
                return;
            }
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
                if (pbMask.Image != null) {
                    var mask = Mask.FromBitmap((Bitmap)pbMask.Image);
                    grid = new MaskedGrid(mask);
                }
                if (!CreateSelectedMaze(grid)) {
                    return;
                }
                img = grid.ToImg(GridSize);
                pbMaze.Image = img;
            }
        }

        private bool CreateSelectedMaze(Grid grid) {
            var algo = (string) cbAlgorithm.SelectedItem;

            var type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == algo);
            if (type == null) {
                MessageBox.Show("No algorithm type for " + algo);
                return false;
            }
            if (pbMask.Image != null && type == typeof(Sidewinder) || type == typeof(BinaryTree)) {
                MessageBox.Show("Cannot use masks with Sidewinder and BinaryTree algorithms");
            }

            type.GetMethod("Maze", new[]{typeof(Grid), typeof(int)}).Invoke(null, new object[] {grid, (int) nudRNGSeed.Value});
            return true;
        }

        private void ResetMaze(object sender, EventArgs e) {
            _grid = new Grid(MazeSize, MazeSize);
            if (pbMask.Image != null) {
                var mask = Mask.FromBitmap((Bitmap)pbMask.Image);
                _grid = new MaskedGrid(mask);
            }
            pbMaze.Image = _grid.ToImg(GridSize);
            if (cbAlgorithm.SelectedItem != null) {
                SetAlgorithm();
            }
            btnStep.Enabled = true;
            _startPoint = null;
            tsslStartPoint.Text = "Start: " + _startPoint;
            _endPoint = null;
            tsslEndPoint.Text = "End: " + _endPoint;
            tsslPathLength.Text = "Path Length: ";
            IsAnimating = false;
        }

        private void SetAlgorithm() {
            var algo = (string) cbAlgorithm.SelectedItem;

            var type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == algo);
            if (type == null) {
                MessageBox.Show("No algorithm type for " + algo);
            }
            if (pbMask.Image != null && type == typeof(Sidewinder) || type == typeof(BinaryTree)) {
                MessageBox.Show("Cannot use masks with Sidewinder and BinaryTree algorithms");
            }
            _algorithm = (IMazeAlgorithm) Activator.CreateInstance(type, _grid, (int) nudRNGSeed.Value);
        }

        private void StepMaze(object sender, EventArgs e) {
            if (!_algorithm.Step()) {
                btnStep.Enabled = false;
            }
            _grid.ActiveCell = _algorithm.CurrentCell;
            pbMaze.Image = _grid.ToImg(GridSize);
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
                if (pbMask.Image != null) {
                    var mask = Mask.FromBitmap((Bitmap)pbMask.Image);
                    colorGrid = new MaskedColoredGrid(mask);
                }

                if (!CreateSelectedMaze(colorGrid)) {
                    return;
                }
                Cell start;
                if (_startPoint.HasValue)
                    start = colorGrid[_startPoint.Value.Y, _startPoint.Value.X];
                else if (colorGrid is MaskedColoredGrid) {
                    start = colorGrid.RandomCell();
                }
                else
                    start = colorGrid[colorGrid.Rows / 2, colorGrid.Columns / 2];
                colorGrid.Distances = start.Distances;

                colorGrid.BackColor = pbColor.BackColor;

                pbMaze.Image = colorGrid.ToImg(GridSize);
            }
        }

        private void btnDrawPath_Click(object sender, EventArgs e) {
            if (_startPoint == null || _endPoint == null || pbMask.Image != null) {
                return;
            }
            if (cbAlgorithm.SelectedItem != null) {
                var colorGrid = new ColoredPathGrid(MazeSize, MazeSize);

                if (!CreateSelectedMaze(colorGrid)) {
                    return;
                }
                var start = colorGrid[_startPoint.Value.Y, _startPoint.Value.X];
                var end = colorGrid[_endPoint.Value.Y, _endPoint.Value.X];

                colorGrid.Distances = start.Distances;
                colorGrid.Path = start.Distances.PathTo(end);

                tsslPathLength.Text = "Path Length: " + colorGrid.PathLength;

                colorGrid.BackColor = pbColor.BackColor;

                pbMaze.Image = colorGrid.ToImg(GridSize);
            }

        }

        private void btnLongestPath_Click(object sender, EventArgs e) {
            if (cbAlgorithm.SelectedItem != null) {
                var colorGrid = new ColoredPathGrid(MazeSize, MazeSize);
                if (pbMask.Image != null) {
                    var mask = Mask.FromBitmap((Bitmap)pbMask.Image);
                    colorGrid = new MaskedColoredPathGrid(mask);
                }
                if (!CreateSelectedMaze(colorGrid)) {
                    return;
                }
                var start = colorGrid.RandomCell();
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

                pbMaze.Image = colorGrid.ToImg(GridSize);
            }
        }

        private void btnAnimate_Click(object sender, EventArgs e) {
            Cursor = Cursors.WaitCursor;
            IsAnimating = true;
            while (_algorithm.Step() && IsAnimating) {
                _grid.ActiveCell = _algorithm.CurrentCell;
                pbMaze.Image = _grid.ToImg(GridSize);

                Application.DoEvents();
                Thread.Sleep(100);
            }
            btnStep.Enabled = false;
            Cursor = Cursors.Default;
            IsAnimating = false;
        }

        private void btnLoadMask_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                pbMask.Image = Image.FromFile(openFileDialog1.FileName);
                pbMaze.SizeMode = PictureBoxSizeMode.Zoom;
                pbMaze.Dock = DockStyle.Fill;
            } else {
                pbMask.Image.Dispose();
                pbMask.Image = null;
                pbMaze.SizeMode = PictureBoxSizeMode.AutoSize;
                pbMaze.Dock = DockStyle.None;
            }
            ResetMaze(sender, e);
        }
    }
}
