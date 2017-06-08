namespace mazes.Core.Grids.Cartesian {
    using System.Drawing;

    using mazes.Core.Cells;
    using mazes.Core.Grids.Interfaces;

    public class ColoredPathGrid : ColoredGrid, IPathGrid {
        public ColoredPathGrid(int rows, int cols) : base(rows, cols) { }

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
            DrawPathInternal((CartesianCell)cell, g, cellSize);
        }

        private void DrawPathInternal(CartesianCell cell, Graphics g, int cellSize) {
            var thisDistance = Path[cell];
            if (thisDistance < 0)
                return;
            var center = cell.Center(cellSize);
            using (var pen = new Pen(BackColor.Invert(), 2)) {

                if (cell.North != null && (Path[cell.North] == thisDistance + 1 || Path[cell.North] == thisDistance - 1 && thisDistance != 0)) {
                    g.DrawLine(pen, center, cell.North.Center(cellSize));
                }

                if (cell.East != null && (Path[cell.East] == thisDistance + 1 || Path[cell.East] == thisDistance - 1 && thisDistance != 0)) {
                    g.DrawLine(pen, center, cell.East.Center(cellSize));
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
    }
}