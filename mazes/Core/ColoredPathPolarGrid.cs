namespace mazes.Core {
    using System;
    using System.Drawing;

    public class ColoredPathPolarGrid : ColoredPolarGrid, IPathGrid {
        public ColoredPathPolarGrid(int rows) : base(rows) { }

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
            DrawPathInternal((PolarCell)cell, g, cellSize);
        }

        private void DrawPathInternal(PolarCell cell, Graphics g, int cellSize) {
            var imgSize = 2 * Rows * cellSize;
            var center = imgSize / 2;
            var theta = 2 * Math.PI / _grid[cell.Row].Count;

            var thisDistance = Path[cell];
            if (thisDistance >= 0) {
                var myBounds = cell.GetBounds(center, theta, cellSize).GetBounds();
                var cx = myBounds.Left + myBounds.Width / 2;
                var cy = myBounds.Top + myBounds.Height / 2;
                using (var pen = new Pen(BackColor.Invert(), 2)) {
                    if (cell.Inward != null && (Path[cell.Inward] == thisDistance + 1 || Path[cell.Inward] == thisDistance - 1 && thisDistance != 0)) {
                        var theta2 = 2 * Math.PI / _grid[cell.Inward.Row].Count;
                        var bounds = cell.Inward.GetBounds(center, theta2, cellSize).GetBounds();
                        var x1 = bounds.Left + bounds.Width / 2;
                        var y1 = bounds.Top + bounds.Height / 2;
                        g.DrawLine(pen, cx, cy, x1, y1);
                    }
                    if (cell.CounterClockwise != null && (Path[cell.CounterClockwise] == thisDistance + 1 || Path[cell.CounterClockwise] == thisDistance - 1 && thisDistance != 0)) {
                        var theta2 = 2 * Math.PI / _grid[cell.CounterClockwise.Row].Count;
                        var bounds = cell.CounterClockwise.GetBounds(center, theta2, cellSize).GetBounds();
                        var x1 = bounds.Left + bounds.Width / 2;
                        var y1 = bounds.Top + bounds.Height / 2;
                        g.DrawLine(pen, cx, cy, x1, y1);
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