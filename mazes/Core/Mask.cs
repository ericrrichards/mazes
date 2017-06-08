namespace mazes.Core {
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;

    using mazes.Core.Cells;
    using mazes.Core.Grids;
    using mazes.Core.Grids.Cartesian;

    public class Mask {
        public int Rows { get; }
        public int Columns { get; }

        private List<List<bool>> Bits { get; }

        public Mask(int rows, int columns) {
            Rows = rows;
            Columns = columns;
            Bits = new List<List<bool>>();
            for (var r = 0; r < Rows; r++) {
                Bits.Add(new List<bool>());
                for (var c = 0; c < Columns; c++) {
                    Bits[r].Add(true);
                }
            }
        }

        public static Mask FromString(List<string> maskStrings) {
            var rows = maskStrings.Count;
            var columns = maskStrings[0].Length;

            var mask = new Mask(rows, columns);

            for (var r = 0; r < rows; r++) {
                for (var c = 0; c < columns; c++) {
                    if (maskStrings[r][c] == 'X') {
                        mask[r, c] = false;
                    }
                }
            }

            return mask;
        }
        public static Mask FromTextFile(string filename) {
            var lines = File.ReadAllLines(filename).Select(l => l.Trim()).ToList();
            return FromString(lines);
        }

        public static Mask FromImageFile(string filename) {
            var img = new Bitmap(filename);
            return FromBitmap(img);
        }

        public static Mask FromBitmap(Bitmap img) {
            var rows = img.Height;
            var columns = img.Width;

            var mask = new Mask(rows, columns);
            var black = Color.Black.ToArgb();
            for (var r = 0; r < rows; r++) {
                for (var c = 0; c < columns; c++) {
                    if (img.GetPixel(c, r).ToArgb() == black) {
                        mask[r, c] = false;
                    }
                }
            }
            return mask;
        }

        public bool this[int row, int column] {
            get {
                if (row >= 0 && row < Rows && column >= 0 && column < Columns)
                    return Bits[row][column];
                return false;
            }
            set {
                if (row >= 0 && row < Rows && column >= 0 && column < Columns) {
                    Bits[row][column] = value;
                }

            }
        }

        public int Count {
            get { return Bits.Aggregate(0, (i, list) => i + list.Count(cell => cell)); }
        }

        public Point RandomLocation(Random rand = null) {
            if (rand == null) {
                rand = new Random();
            }
            if (Count == 0) {
                throw new InvalidOperationException("No enabled cells in mask");
            }
            do {
                var row = rand.Next(Rows);
                var col = rand.Next(Columns);
                if (Bits[row][col]) {
                    return new Point(col, row);
                }
            } while (true);
        }

        public static void UnlinkMaskedCells(Mask mask, IEnumerable<CartesianCell> cells) {
            foreach (var cell in cells) {
                var row = cell.Row;
                var col = cell.Column;

                if (!mask[row, col]) {
                    // Unlink this cell from its neighbors
                    if (cell.North != null)
                        ((CartesianCell)cell.North).South = null;
                    cell.North = null;
                    if (cell.South != null)
                        ((CartesianCell)cell.South).North = null;
                    cell.South = null;
                    if (cell.West != null)
                        ((CartesianCell)cell.West).East = null;
                    cell.West = null;
                    if (cell.East != null)
                        ((CartesianCell)cell.East).West = null;
                    cell.East = null;
                }
            }
        }

        public Cell GetRandomCell(Grid grid, Random random ) {
            var location = RandomLocation(random);
            var randomCell = grid[location.Y, location.X];
            if (randomCell == null) {
                throw new InvalidOperationException("Invalid random cell location: " + location);
            }
            return randomCell;
        }
    }
}