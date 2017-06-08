namespace mazes.Core.Grids.Polar {
    using System;
    using System.Drawing;

    using mazes.Core.Cells;
    using mazes.Core.Grids.Interfaces;

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


            var thisDistance = Path[cell];
            if (thisDistance < 0)
                return;

            var imgSize = 2 * Rows * cellSize;
            var center = imgSize / 2;
            var theta = 2 * Math.PI / _grid[cell.Row].Count;

            var centerCell = cell.Center(center, theta, cellSize);

            using (var pen = new Pen(BackColor.Invert(), 2)) {
                if (cell.Inward != null && (Path[cell.Inward] == thisDistance + 1 || Path[cell.Inward] == thisDistance - 1 && thisDistance != 0)) {
                    var theta2 = 2 * Math.PI / _grid[cell.Inward.Row].Count;
                    var bounds = cell.Inward.Center(center, theta2, cellSize);
                    g.DrawLine(pen, centerCell, bounds);
                }
                if (cell.CounterClockwise != null && (Path[cell.CounterClockwise] == thisDistance + 1 || Path[cell.CounterClockwise] == thisDistance - 1 && thisDistance != 0)) {
                    var theta2 = 2 * Math.PI / _grid[cell.CounterClockwise.Row].Count;
                    var bounds = cell.CounterClockwise.Center(center, theta2, cellSize);
                    g.DrawLine(pen, centerCell, bounds);
                }
                if (thisDistance == 0) {
                    g.DrawRectangle(pen, centerCell.X - 2, centerCell.Y - 2, 4, 4);
                }
                if (thisDistance == _maxDistance) {
                    g.DrawLine(pen, centerCell.X - 4, centerCell.Y - 4, centerCell.X + 4, centerCell.Y + 4);
                    g.DrawLine(pen, centerCell.X + 4, centerCell.Y - 4, centerCell.X - 4, centerCell.Y + 4);
                }
            }
        }
    }
}