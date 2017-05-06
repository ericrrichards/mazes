namespace mazes.Core {
    using System;
    using System.Drawing;

    public class ColoredPathUpsilonGrid : ColoredUpsilonGrid, IPathGrid {
        public ColoredPathUpsilonGrid(int rows, int cols) : base(rows, cols) { }
        private Distances _path;
        private Cell _end;
        private int _maxDistance;

        public Distances Path {
            get => _path;
            set {
                _path = value;
                (_end, _maxDistance) = value.Max;
            }
        }
        public int PathLength => _maxDistance + 1;

        protected override void DrawPath(Cell cell, Graphics g, int cellSize) {
            if (Path == null) {
                return;
            }
            //cellSize = cellSize / 2;
            DrawPathInternal((CartesianCell)cell, g, cellSize);
        }

        private void DrawPathInternal(CartesianCell cell, Graphics g, int cellSize) {
            var thisDistance = Path[cell];
            if (thisDistance < 0)
                return;

            var aSize = cellSize / 2.0;
            var bSize = cellSize / Math.Sqrt(2);

            var cx = bSize + aSize + (bSize + cellSize) * cell.Column;
            var cy = bSize + aSize + (bSize + cellSize) * cell.Row;

            var center = new Point((int)cx, (int)cy);

            using (var pen = new Pen(BackColor.Invert(), 2)) {

                if (cell.North != null && (Path[cell.North] == thisDistance + 1 || Path[cell.North] == thisDistance - 1 && thisDistance != 0)) {
                    g.DrawLine(pen, center, Center(cell.North, cellSize, bSize, aSize));
                }
                if (cell.East != null && (Path[cell.East] == thisDistance + 1 || Path[cell.East] == thisDistance - 1 && thisDistance != 0)) {
                    g.DrawLine(pen, center, Center(cell.East, cellSize, bSize, aSize));
                }
                var oCell = cell as OctagonCell;
                if (oCell != null) {
                    if (oCell.SouthEast != null && (Path[oCell.SouthEast] == thisDistance + 1 || Path[oCell.SouthEast] == thisDistance - 1 && thisDistance != 0)) {
                        g.DrawLine(pen, center, Center(oCell.SouthEast, cellSize, bSize, aSize));
                    }
                    if (oCell.NorthEast != null && (Path[oCell.NorthEast] == thisDistance + 1 || Path[oCell.NorthEast] == thisDistance - 1 && thisDistance != 0)) {
                        g.DrawLine(pen, center, Center(oCell.NorthEast, cellSize, bSize, aSize));
                    }
                }

                if (thisDistance == 0) {
                    g.DrawRectangle(pen, center.X - 2, center.Y - 2, 4, 4);
                }
                if (thisDistance == _maxDistance) {
                    g.DrawLine(pen, center.X - 4, center.Y - 4, center.X + 4, center.Y + 4);
                    g.DrawLine(pen, center.X + 4, center.Y - 4, center.X - 4, center.Y + 4);
                }
            }
        }

        private Point Center(CartesianCell cell, int cellSize, double bSize, double aSize) {
            var cx = bSize + aSize + (bSize + cellSize) * cell.Column;
            var cy = bSize + aSize + (bSize + cellSize) * cell.Row;

            return new Point((int)cx, (int)cy);
        }
    }
}