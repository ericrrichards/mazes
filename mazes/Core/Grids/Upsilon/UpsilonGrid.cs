namespace mazes.Core.Grids.Upsilon {
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using mazes.Core.Cells;
    using mazes.Core.Grids.Cartesian;

    public class UpsilonGrid : Grid {
        public UpsilonGrid(int rows, int cols) : base(rows, cols) {
            PrepareGrid();
            ConfigureCells();
        }

        private void PrepareGrid() {
            var rows = new List<List<Cell>>();
            for (var row = 0; row < Rows; row++) {
                var newRows = new List<Cell>();
                for (var column = 0; column < Columns; column++) {
                    if ((row + column) % 2 == 0) {
                        newRows.Add(new OctagonCell(row, column));
                    } else {
                        newRows.Add(new CartesianCell(row, column));
                    }
                }
                rows.Add(newRows);
            }
            _grid = rows;
        }

        private void ConfigureCells() {
            foreach (var cell in Cells.Cast<CartesianCell>()) {
                var row = cell.Row;
                var column = cell.Column;

                cell.North = (CartesianCell)this[row - 1, column];
                cell.South = (CartesianCell)this[row + 1, column];
                cell.West = (CartesianCell)this[row, column - 1];
                cell.East = (CartesianCell)this[row, column + 1];

                var octagonCell = cell as OctagonCell;
                if (octagonCell != null) {
                    octagonCell.NorthWest = (CartesianCell)this[row - 1, column - 1];
                    octagonCell.NorthEast = (CartesianCell)this[row - 1, column + 1];
                    octagonCell.SouthWest = (CartesianCell)this[row + 1, column - 1];
                    octagonCell.SouthEast = (CartesianCell)this[row + 1, column + 1];
                }
            }
        }

        public override Image ToImg(int cellSize = 50, float insetPrc = 0.0f) {
            cellSize = cellSize / 2;

            var aSize = cellSize / 2.0;
            var bSize = cellSize / Math.Sqrt(2);
            var octSize = cellSize + bSize * 2;

            var imgWidth = (int)(octSize + (cellSize + bSize) * (Columns - 1));
            var imgHeight = (int)(octSize + (cellSize + bSize) * (Rows - 1));

            var img = new Bitmap(imgWidth + 1, imgHeight + 1);
            using (var g = Graphics.FromImage(img)) {
                g.Clear(Color.Transparent);

                foreach (var mode in new[] { DrawMode.Background, DrawMode.Walls, DrawMode.Path, }) {
                    foreach (var cell in Cells.Cast<CartesianCell>()) {

                        var cx = bSize + aSize + (bSize + cellSize) * cell.Column;
                        var cy = bSize + aSize + (bSize + cellSize) * cell.Row;

                        if (mode == DrawMode.Background) {
                            var color = BackgroundColorFor(cell);
                            if (color != null) {
                                var octagonCell = cell as OctagonCell;
                                if (octagonCell != null) {
                                    FillOctagon(g, cx, cy, aSize, bSize, color);
                                } else {
                                    FillSquare(g, cx, cy, aSize, color);
                                }
                            }
                        } else if (mode == DrawMode.Walls) {
                            var octagonCell = cell as OctagonCell;
                            if (octagonCell != null) {
                                DrawOctagon(g, octagonCell, cx, cy, aSize, bSize);
                            } else {
                                DrawSquare(g, cell, cx, cy, aSize);
                            }
                        } else if (mode == DrawMode.Path) {
                            DrawPath(cell, g, cellSize);
                        }


                    }
                }
            }
            return img;
        }

        private void FillSquare(Graphics g, double cx, double cy, double aSize, Color? color) {
            var x1 = (int)(cx - aSize);
            var y1 = (int)(cy - aSize);

            g.FillRectangle(new SolidBrush(color.GetValueOrDefault()), x1, y1, (int)(aSize * 2), (int)(aSize * 2));
        }

        private void FillOctagon(Graphics g, double cx, double cy, double aSize, double bSize, Color? color) {
            var xFW = (int)(cx - aSize - bSize);
            var xNW = (int)(cx - aSize);
            var xNE = (int)(cx + aSize);
            var xFE = (int)(cx + aSize + bSize);

            var yFN = (int)(cy - aSize - bSize);
            var yNN = (int)(cy - aSize);
            var yNS = (int)(cy + aSize);
            var yFS = (int)(cy + aSize + bSize);

            g.FillPolygon(new SolidBrush(color.GetValueOrDefault()), new[] {
                new Point(xNW, yFN),
                new Point(xNE, yFN),
                new Point(xFE, yNN),
                new Point(xFE, yNS),
                new Point(xNE, yFS),
                new Point(xNW, yFS),
                new Point(xFW, yNS),
                new Point(xFW, yNN),
            });

        }

        private void DrawOctagon(Graphics g, OctagonCell cell, double cx, double cy, double aSize, double bSize) {
            var xFW = (int)(cx - aSize - bSize);
            var xNW = (int)(cx - aSize);
            var xNE = (int)(cx + aSize);
            var xFE = (int)(cx + aSize + bSize);

            var yFN = (int)(cy - aSize - bSize);
            var yNN = (int)(cy - aSize);
            var yNS = (int)(cy + aSize);
            var yFS = (int)(cy + aSize + bSize);

            if (cell == ActiveCell) {
                g.FillPolygon(Brushes.GreenYellow, new[] {
                    new Point(xNW, yFN),
                    new Point(xNE, yFN),
                    new Point(xFE, yNN),
                    new Point(xFE, yNS),
                    new Point(xNE, yFS),
                    new Point(xNW, yFS),
                    new Point(xFW, yNS),
                    new Point(xFW, yNN),
                });
            }

            if (cell.North == null) {
                g.DrawLine(Pens.Black, xNW, yFN, xNE, yFN);
            }
            if (cell.NorthWest == null) {
                g.DrawLine(Pens.Black, xNW, yFN, xFW, yNN);
            }
            if (cell.West == null) {
                g.DrawLine(Pens.Black, xFW, yNN, xFW, yNS);
            }
            if (cell.SouthWest == null) {
                g.DrawLine(Pens.Black, xFW, yNS, xNW, yFS);
            }

            if (!cell.IsLinked(cell.NorthEast)) {
                g.DrawLine(Pens.Black, xNE, yFN, xFE, yNN);
            }
            if (!cell.IsLinked(cell.East)) {
                g.DrawLine(Pens.Black, xFE, yNN, xFE, yNS);
            }
            if (!cell.IsLinked(cell.SouthEast)) {
                g.DrawLine(Pens.Black, xFE, yNS, xNE, yFS);
            }
            if (!cell.IsLinked(cell.South)) {
                g.DrawLine(Pens.Black, xNW, yFS, xNE, yFS);
            }


        }

        private void DrawSquare(Graphics g, CartesianCell cell, double cx, double cy, double aSize) {
            var x1 = (int)(cx - aSize);
            var y1 = (int)(cy - aSize);
            var x2 = (int)(cx + aSize);
            var y2 = (int)(cy + aSize);

            if (cell.North == null) {
                g.DrawLine(Pens.Black, x1, y1, x2, y1);
            }
            if (cell.West == null) {
                g.DrawLine(Pens.Black, x1, y1, x1, y2);
            }

            if (!cell.IsLinked(cell.East)) {
                g.DrawLine(Pens.Black, x2, y1, x2, y2);
            }
            if (!cell.IsLinked(cell.South)) {
                g.DrawLine(Pens.Black, x1, y2, x2, y2);
            }

            if (cell == ActiveCell) {
                g.FillRectangle(Brushes.GreenYellow, x1 + 2, y1 + 2, (int)(aSize * 2) - 4, (int)(aSize * 2) - 4);
            }
        }
    }
}