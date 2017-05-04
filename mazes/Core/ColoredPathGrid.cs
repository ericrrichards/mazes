namespace mazes.Core {
    using System.Drawing;

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
        public int PathLength => _maxDistance+1;

        protected override void DrawPath(Cell cell, Graphics g, int cellSize) {
            if (Path == null) {
                return;
            }
            DrawPathInternal((CartesianCell)cell, g, cellSize);
        }

        private void DrawPathInternal(CartesianCell cell, Graphics g, int cellSize) {
            var thisDistance = Path[cell];
            if (thisDistance >= 0) {
                var x1 = cell.Column * cellSize;
                var y1 = cell.Row * cellSize;
                var x2 = (cell.Column + 1) * cellSize;
                var y2 = (cell.Row + 1) * cellSize;
                var cx = cell.Column * cellSize + cellSize / 2;
                var cy = cell.Row * cellSize + cellSize / 2;
                using (var pen = new Pen(BackColor.Invert(), 2)) {
                    if (cell.North != null && (Path[cell.North] == thisDistance + 1 || Path[cell.North] == thisDistance - 1 && thisDistance != 0)) {
                        g.DrawLine(pen, cx, cy, cx, y1);
                    }
                    if (cell.South != null && (Path[cell.South] == thisDistance + 1 || Path[cell.South] == thisDistance - 1 && thisDistance != 0)) {
                        g.DrawLine(pen, cx, cy, cx, y2);
                    }
                    if (cell.East != null && (Path[cell.East] == thisDistance + 1 || Path[cell.East] == thisDistance - 1 && thisDistance != 0)) {
                        g.DrawLine(pen, cx, cy, x2, cy);
                    }
                    if (cell.West != null && (Path[cell.West] == thisDistance + 1 || Path[cell.West] == thisDistance - 1 && thisDistance != 0)) {
                        g.DrawLine(pen, cx, cy, x1, cy);
                    }

                    if (thisDistance == 0) {
                        g.DrawRectangle(pen, cx - 2, cy - 2, 4, 4);
                    }
                    if (thisDistance == _maxDistance) {
                        g.DrawLine(pen, cx - 4, cy - 4, cx + 4, cy + 4);
                        g.DrawLine(pen, cx + 4, cy - 4, cx - 4, cy + 4);
                    }
                }
            }
        }
    }
}