namespace mazes.Core {
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;

    using JetBrains.Annotations;

    public class Grid {
        public int Rows { get; }
        public int Columns { get; }
        public int Size => Rows * Columns;

        private List<List<Cell>> _grid;

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
                    row.Add(new Cell(r, c));
                }
                _grid.Add(row);
            }
        }

        private void ConfigureCells() {
            foreach (var cell in Cells) {
                var row = cell.Row;
                var col = cell.Column;

                cell.North = this[row - 1, col];
                cell.South = this[row + 1, col];
                cell.West = this[row, col - 1];
                cell.East = this[row, col + 1];
            }
        }

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
        public Cell RandomCell() {
            var rand = new Random();
            var row = rand.Next(Rows);
            var col = rand.Next(Columns);
            var randomCell = this[row, col];
            if (randomCell == null) {
                throw new InvalidOperationException("Random cell is null");
            }
            return randomCell;
        }

        public IEnumerable<List<Cell>> Row {
            get {
                foreach (var row in _grid) {
                    yield return row;
                }
            }
        }

        public IEnumerable<Cell> Cells {
            get {
                foreach (var row in Row) {
                    foreach (var cell in row) {
                        yield return cell;
                    }
                }
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
                foreach (var cell in row) {
                    const string body = "   ";
                    var east = cell.Linked(cell.East) ? " " : "|";

                    top += body + east;

                    var south = cell.Linked(cell.South) ? "   " : "---";
                    const string corner = "+";
                    bottom += south + corner;
                }
                output.AppendLine(top);
                output.AppendLine(bottom);
            }

            return output.ToString();
        }

        public Image ToPng(int cellSize = 50) {
            var width = cellSize * Columns;
            var height = cellSize * Rows;

            var img = new Bitmap(width, height);
            using (var g = Graphics.FromImage(img)) {
                g.Clear(Color.White);
                foreach (var cell in Cells) {
                    var x1 = cell.Column * cellSize;
                    var y1 = cell.Row * cellSize;
                    var x2 = (cell.Column + 1) * cellSize;
                    var y2 = (cell.Row + 1) * cellSize;


                    if (cell.North == null) {
                        g.DrawLine(Pens.Black, x1, y1, x2, y1);
                    }
                    if (cell.West == null) {
                        g.DrawLine(Pens.Black, x1, y1, x1, y2);
                    }

                    if (!cell.Linked(cell.East)) {
                        g.DrawLine(Pens.Black, x2, y1, x2, y2);
                    }
                    if (!cell.Linked(cell.South)) {
                        g.DrawLine(Pens.Black, x1, y2, x2, y2);
                    }
                }
            }


            return img;
        }
    }
}