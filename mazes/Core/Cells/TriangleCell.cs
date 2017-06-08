namespace mazes.Core.Cells {
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    public class TriangleCell : Cell {

        public bool Upright => (Row + Column) % 2 == 0;

        public TriangleCell West { get; set; }
        public TriangleCell East { get; set; }
        public TriangleCell North { get; set; }
        public TriangleCell South { get; set; }

        public TriangleCell(int row, int col) : base(row, col) { }
        public override List<Cell> Neighbors => new Cell[] { West, East, Upright ? South : North }.Where(c => c != null).ToList();

        public Point Center(int cellSize) {
            var halfWidth = cellSize / 2.0;
            var height = cellSize * Math.Sqrt(3) / 2;
            var halfHeight = height / 2.0;

            var cx = halfWidth + Column * halfWidth;
            var cy = halfHeight + Row * height;

            return new Point((int)cx, (int)cy);
        }
    }
}