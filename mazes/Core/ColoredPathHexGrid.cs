namespace mazes.Core {
    using System;
    using System.Drawing;

    public class ColoredPathHexGrid : ColoredHexGrid, IPathGrid {
        public ColoredPathHexGrid(int rows, int cols) : base(rows, cols) { }

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
            DrawPathInternal((HexCell)cell, g, cellSize);
        }

        private void DrawPathInternal(HexCell cell, Graphics g, int cellSize) {
            var thisDistance = Path[cell];
            if (thisDistance >= 0) {
                var size = cellSize / 2;
                var aSize = size / 2.0;
                var bSize = size * Math.Sqrt(3) / 2.0;
                var height = bSize * 2;

                var cx = size + 3 * cell.Column * aSize;
                var cy = bSize + cell.Row * height;
                if (cell.Column % 2 == 1) {
                    cy += bSize;
                }
                var cyi = (int)cy;
                var cxi = (int)cx;
                
                using (var pen = new Pen(BackColor.Invert(), 2)) {
                    
                    if (cell.North != null && (Path[cell.North] == thisDistance + 1 || Path[cell.North] == thisDistance - 1 && thisDistance != 0)) {
                        g.DrawLine(pen, cell.Center(cellSize), ((HexCell)cell.North).Center(cellSize));
                    }
                    if (cell.NorthEast != null && (Path[cell.NorthEast] == thisDistance + 1 || Path[cell.NorthEast] == thisDistance - 1 && thisDistance != 0)) {
                        g.DrawLine(pen, cell.Center(cellSize), ((HexCell)cell.NorthEast).Center(cellSize));
                    }
                    if (cell.SouthEast != null && (Path[cell.SouthEast] == thisDistance + 1 || Path[cell.SouthEast] == thisDistance - 1 && thisDistance != 0)) {
                        g.DrawLine(pen, cell.Center(cellSize), ((HexCell)cell.SouthEast).Center(cellSize));
                    }


                    if (thisDistance == 0) {
                        g.DrawRectangle(pen, cxi - 2, cyi - 2, 4, 4);
                    }
                    if (thisDistance == _maxDistance) {
                        g.DrawLine(pen, cxi - 4, cyi - 4, cxi + 4, cyi + 4);
                        g.DrawLine(pen, cxi + 4, cyi - 4, cxi - 4, cyi + 4);
                    }
                }
            }
        }
    }
}