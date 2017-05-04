namespace mazes.Core {
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    public class HexCell : Cell {

        public HexCell NorthEast { get; set; }

        public HexCell North { get; set; }
        public HexCell NorthWest { get; set; }
        public HexCell SouthEast { get; set; }
        public HexCell South { get; set; }
        public HexCell SouthWest { get; set; }
        public override List<Cell> Neighbors => new Cell[] { NorthWest, North, NorthEast, SouthWest, South, SouthEast }.Where(c => c != null).ToList();
        public HexCell(int row, int col) : base(row, col) { }

        public Point Center(int cellSize) {
            var size = cellSize / 2;
            var aSize = size / 2.0;
            var bSize = size * Math.Sqrt(3) / 2.0;
            var height = bSize * 2;

            var cx = size + 3 * Column * aSize;
            var cy = bSize + Row * height;
            if (Column % 2 == 1) {
                cy += bSize;
            }
            var cyi = (int)cy;
            var cxi = (int)cx;

            return new Point(cxi, cyi);
        }
    }
}