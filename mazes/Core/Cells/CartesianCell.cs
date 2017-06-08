namespace mazes.Core.Cells {
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using JetBrains.Annotations;

    public class CartesianCell : Cell {


        // Neighboring cells
        [CanBeNull]
        public CartesianCell North { get; set; }
        [CanBeNull]
        public CartesianCell South { get; set; }
        [CanBeNull]
        public CartesianCell East { get; set; }
        [CanBeNull]
        public CartesianCell West { get; set; }

        public override List<Cell> Neighbors => new Cell[] { North, South, East, West }.Where(c => c != null).ToList();

        public CartesianCell(int row, int col) : base(row, col) {
        }

        public Point Center(int cellSize) {
            var cx = Column * cellSize + cellSize / 2;
            var cy = Row * cellSize + cellSize / 2;

            return new Point(cx, cy);
        }
        public virtual bool HorizontalPassage => false;
        public virtual bool VerticalPassage => false;
    }
}