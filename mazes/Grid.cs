using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace mazes {
    public class Grid {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int Size { get { return Rows * Columns; } }

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
            for (int i = 0; i < Columns; i++) {
                output.Append("---+");
            }
            output.AppendLine();

            foreach (var row in Row) {
                var top = "|";
                var bottom = "+";
                foreach (var cell in row) {
                    var body = "   ";
                    var east = cell.Linked(cell.East) ? " " : "|";

                    top += body + east;

                    var south = cell.Linked(cell.South) ? "   " : "---";
                    var corner = "+";
                    bottom += south + corner;
                }
                output.AppendLine(top);
                output.AppendLine(bottom);
            }

            return output.ToString();
        }
    }
}