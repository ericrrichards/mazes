namespace mazes.Core {
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Drawing;
    using System.Linq;

    public class HexCell : Cell {

        public Cell NorthEast { get; set; }

        public Cell North { get; set; }
        public Cell NorthWest { get; set; }
        public Cell SouthEast { get; set; }
        public Cell South { get; set; }
        public Cell SouthWest { get; set; }
        public override List<Cell> Neighbors => new []{NorthWest, North, NorthEast, SouthWest, South, SouthEast}.Where(c=>c!=null).ToList();
        public HexCell(int row, int col) : base(row, col) { }

        public Point Center(int cellSize) {
            var size = cellSize / 2;
            var aSize = size / 2.0;
            var bSize = size * Math.Sqrt(3) / 2.0;
            var width = size * 2;
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