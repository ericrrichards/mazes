namespace mazes.Core.Grids.Cartesian {
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;

    using JetBrains.Annotations;

    using mazes.Core.Cells;
    using mazes.Core.Grids.Interfaces;

    public class Grid : IGrid {
        // Dimensions of the grid
        public int Rows { get; }
        public int Columns { get; }
        public virtual int Size => Rows * Columns;

        // The actual grid
        protected List<List<Cell>> _grid;

        public Cell ActiveCell { get; set; }

        [CanBeNull]
        public virtual Cell this[int row, int column] {
            get {
                if (row < 0 || row >= Rows) {
                    return null;
                }
                if (column < 0 || column >= Columns) {
                    return null;
                }
                return _grid[row][column];
            }
        }
        [NotNull]
        public virtual Cell RandomCell(Random random = null) {

            var rand = random ?? new Random();
            var row = rand.Next(Rows);
            var col = rand.Next(Columns);
            var randomCell = this[row, col];
            if (randomCell == null) {
                throw new InvalidOperationException("Random cell is null");
            }
            return randomCell;
        }
        // Row iterator
        public IEnumerable<List<Cell>> Row {
            get {
                foreach (var row in _grid) {
                    yield return row;
                }
            }
        }
        // Cell iterator
        public virtual IEnumerable<Cell> Cells {
            get {
                foreach (var row in Row) {
                    foreach (var cell in row) {
                        if (cell != null) {
                            yield return cell;
                        }
                    }
                }
            }
        }

        public Grid(int rows, int cols) {
            Rows = rows;
            Columns = cols;

            PrepareGrid();
            ConfigureCells();
        }

        private void PrepareGrid() {
            _grid = new List<List<Cell>>();
            for (var r = 0; r < Rows; r++) {
                var row = new List<Cell>();
                for (var c = 0; c < Columns; c++) {
                    row.Add(new CartesianCell(r, c));
                }
                _grid.Add(row);
            }
        }

        private void ConfigureCells() {
            foreach (var cell in Cells.Cast<CartesianCell>()) {
                var row = cell.Row;
                var col = cell.Column;

                cell.North = (CartesianCell)this[row - 1, col];
                cell.South = (CartesianCell)this[row + 1, col];
                cell.West = (CartesianCell)this[row, col - 1];
                cell.East = (CartesianCell)this[row, col + 1];
            }
        }

        public override string ToString() {
            var output = new StringBuilder("+");
            for (var i = 0; i < Columns; i++) {
                output.Append("---+");
            }
            output.AppendLine();

            foreach (var row in Row) {
                var top = "|";
                var bottom = "+";
                foreach (var cell in row.Cast<CartesianCell>()) {
                    var body = $" {ContentsOf(cell)} ";
                    var east = cell.IsLinked(cell.East) ? " " : "|";

                    top += body + east;

                    var south = cell.IsLinked(cell.South) ? "   " : "---";
                    const string corner = "+";
                    bottom += south + corner;
                }
                output.AppendLine(top);
                output.AppendLine(bottom);
            }

            return output.ToString();
        }

        protected virtual string ContentsOf(Cell cell) {
            return " ";
        }

        public virtual Image ToImg(int cellSize = 50, float insetPrc = 0.0f) {
            var width = cellSize * Columns;
            var height = cellSize * Rows;
            var inset = (int)(cellSize * insetPrc);

            var img = new Bitmap(width, height);
            using (var g = Graphics.FromImage(img)) {
                g.Clear(Color.Transparent);
                foreach (var mode in new[] { DrawMode.Background, DrawMode.Walls, DrawMode.Path, }) {


                    foreach (var cell in Cells.Cast<CartesianCell>()) {
                        var x = cell.Column * cellSize;
                        var y = cell.Row * cellSize;

                        if (inset > 0) {
                            ToImgWithInset(g, cell, mode, cellSize, x, y, inset);
                        } else {
                            ToImgWithoutInset(g, cell, mode, cellSize, x, y);
                        }
                    }
                }
            }


            return img;
        }

        protected virtual void ToImgWithInset(Graphics g, CartesianCell cell, DrawMode mode, int cellSize, int x, int y, int inset) {
            var (x1, x2, x3, x4, y1, y2, y3, y4) = CellCoordinatesWithInset(x, y, cellSize, inset);

            if (mode == DrawMode.Background) {
                var color = BackgroundColorFor(cell);
                if (color != null) {
                    // fill center
                    var brush = new SolidBrush(color.GetValueOrDefault());
                    g.FillRectangle(brush, x2, y2, cellSize-inset*2, cellSize-inset*2);

                    if (cell.IsLinked(cell.North)) {
                        g.FillRectangle(brush, x2, y1, cellSize - 2*inset, inset);
                    }
                    if (cell.IsLinked(cell.South)) {
                        g.FillRectangle(brush, x2, y3, cellSize-2*inset, inset);
                    }
                    if (cell.IsLinked(cell.West)) {
                        g.FillRectangle(brush, x1, y2, inset, cellSize-2*inset);
                    }
                    if (cell.IsLinked(cell.East)) {
                        g.FillRectangle(brush, x3, y2, inset, cellSize - 2 * inset);
                    }
                }
            } else if (mode == DrawMode.Walls) {
                if (cell.IsLinked(cell.North)) {
                    g.DrawLine(Pens.Black, x2, y1, x2, y2);
                    g.DrawLine(Pens.Black, x3, y1, x3, y2);
                } else {
                    g.DrawLine(Pens.Black, x2, y2, x3, y2);
                }
                if (cell.IsLinked(cell.South)) {
                    g.DrawLine(Pens.Black, x2, y3, x2, y4);
                    g.DrawLine(Pens.Black, x3, y3, x3, y4);
                } else {
                    g.DrawLine(Pens.Black,x2, y3, x3, y3);
                }

                if (cell.IsLinked(cell.West)) {
                    g.DrawLine(Pens.Black, x1, y2, x2, y2);
                    g.DrawLine(Pens.Black, x1, y3, x2, y3);
                } else {
                    g.DrawLine(Pens.Black,x2, y2, x2, y3);
                }
                if (cell.IsLinked(cell.East)) {
                    g.DrawLine(Pens.Black, x3, y2, x4, y2);
                    g.DrawLine(Pens.Black, x3, y3, x4, y3);
                } else {
                    g.DrawLine(Pens.Black,x3, y2, x3, y3);
                }

                if (cell == ActiveCell) {
                    g.FillRectangle(Brushes.GreenYellow, x2 + 1, y2 + 1, cellSize - inset*2-2, cellSize-inset*2-2);
                }

            } else if (mode == DrawMode.Path) {
                DrawPath(cell, g, cellSize);
            }
        }

        protected (int x1, int x2, int x3, int x4, int y1, int y2, int y3, int y4) CellCoordinatesWithInset(int x, int y, int cellSize, int inset) {
            var x1 = x;
            var x4 = x + cellSize;
            var x2 = x1 + inset;
            var x3 = x4 - inset;

            var y1 = y;
            var y4 = y + cellSize;
            var y2 = y1 + inset;
            var y3 = y4 - inset;
            return (x1, x2, x3, x4, y1, y2, y3, y4);
        }

        private void ToImgWithoutInset(Graphics g, CartesianCell cell, DrawMode mode, int cellSize, int x, int y) {
            var x1 = x;
            var y1 = y;
            var x2 = x1 + cellSize;
            var y2 = y1 + cellSize;

            if (cell.Neighbors.Count == 0) {
                return;
            }
            if (mode == DrawMode.Background) {
                var color = BackgroundColorFor(cell);
                if (color != null) {
                    g.FillRectangle(new SolidBrush(color.GetValueOrDefault()), x1, y1, cellSize, cellSize);
                }
            } else if (mode == DrawMode.Walls) {
                if (cell.North == null) {
                    g.DrawLine(Pens.Black, x1, y1, x2, y1);
                }
                if (cell.West == null) {
                    g.DrawLine(Pens.Black, x1, y1, x1, y2);
                }

                if (!cell.IsLinked(cell.East)) {
                    g.DrawLine(Pens.Black, x2, y1, x2, y2);
                }
                if (!cell.IsLinked(cell.South)) {
                    g.DrawLine(Pens.Black, x1, y2, x2, y2);
                }

                if (cell == ActiveCell) {
                    g.FillRectangle(Brushes.GreenYellow, x1 + 2, y1 + 2, cellSize - 4, cellSize - 4);
                }

            } else if (mode == DrawMode.Path) {
                DrawPath(cell, g, cellSize);
            }
        }

        protected virtual void DrawPath(Cell cell, Graphics g, int cellSize) {

        }

        protected virtual Color? BackgroundColorFor(Cell cell) {
            return Color.White;
        }



        public List<Cell> Deadends() {
            return Cells.Where(c => c.Links.Count == 1).ToList();
        }

        public void Braid(double p = 1.0f) {
            var rand = new Random();
            foreach (var cell in Deadends().Shuffle()) {
                if (cell.Links.Count != 1 || rand.NextDouble() > p) {
                    continue;
                }
                var neighbors = cell.Neighbors.Where(n => !cell.IsLinked(n)).ToList();
                var best = neighbors.Where(n => n.Links.Count == 1).ToList();
                if (!best.Any()) {
                    best = neighbors;
                    if (!best.Any()) {
                        continue;
                    }
                }
                var neighbor = best.Sample(rand);
                cell.Link(neighbor);
            }
        }
    }
}