namespace mazes.Core.Grids.Hex {
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using mazes.Core.Cells;
    using mazes.Core.Grids.Cartesian;

    public class HexGrid : Grid {
        public HexGrid(int rows, int cols) : base(rows, cols) {

            PrepareGrid();
            ConfigureCells();
        }

        private void PrepareGrid() {
            var rows = new List<List<Cell>>();
            for (var row = 0; row < Rows; row++) {
                var newRows = new List<Cell>();
                for (var column = 0; column < Columns; column++) {
                    newRows.Add(new HexCell(row, column));
                }
                rows.Add(newRows);
            }
            _grid = rows;
        }

        private void ConfigureCells() {
            foreach (var cell in Cells) {
                var hCell = (HexCell)cell;
                var row = cell.Row;
                var column = cell.Column;

                int northDiagonal;
                int southDiagonal;

                if (column % 2 == 0) {
                    northDiagonal = row - 1;
                    southDiagonal = row;
                } else {
                    northDiagonal = row;
                    southDiagonal = row + 1;
                }
                hCell.NorthWest = (HexCell)this[northDiagonal, column - 1];
                hCell.North = (HexCell)this[row - 1, column];
                hCell.NorthEast = (HexCell)this[northDiagonal, column + 1];
                hCell.SouthWest = (HexCell)this[southDiagonal, column - 1];
                hCell.South = (HexCell)this[row + 1, column];
                hCell.SouthEast = (HexCell)this[southDiagonal, column + 1];
            }
        }

        public override Image ToImg(int cellSize = 50, float insetPrc = 0.0f) {
            var size = cellSize / 2;
            var aSize = size / 2.0;
            var bSize = size * Math.Sqrt(3) / 2.0;
            var width = size * 2;
            var height = bSize * 2;

            var imgWidth = (int)(3 * aSize * Columns + aSize + 0.5);
            var imgHeight = (int)(height * Rows + bSize + 0.5);

            var img = new Bitmap(imgWidth + 1, imgHeight + 1);
            using (var g = Graphics.FromImage(img)) {
                g.Clear(Color.Transparent);

                foreach (var mode in new[] { DrawMode.Background, DrawMode.Walls, DrawMode.Path, }) {
                    foreach (var cell in Cells.Cast<HexCell>()) {

                        var cx = size + 3 * cell.Column * aSize;
                        var cy = bSize + cell.Row * height;
                        if (cell.Column % 2 == 1) {
                            cy += bSize;
                        }

                        var xFW = (int)(cx - size);
                        var xNW = (int)(cx - aSize);
                        var xNE = (int)(cx + aSize);
                        var xFE = (int)(cx + size);

                        var yN = (int)(cy - bSize);
                        var yM = (int)cy;
                        var yS = (int)(cy + bSize);

                        if (mode == DrawMode.Background) {
                            var color = BackgroundColorFor(cell);
                            if (color != null) {
                                g.FillPolygon(new SolidBrush(color.GetValueOrDefault()), new[] {
                                    new Point(xFW, yM),
                                    new Point(xNW, yN),
                                    new Point(xNE, yN),
                                    new Point(xFE, yM),
                                    new Point(xNE, yS),
                                    new Point(xNW, yS),
                                });
                            }
                        } else if (mode == DrawMode.Walls) {
                            if (cell == ActiveCell) {
                                g.FillPolygon(
                                              new SolidBrush(Color.GreenYellow),
                                              new[] {
                                                  new Point(xFW, yM),
                                                  new Point(xNW, yN),
                                                  new Point(xNE, yN),
                                                  new Point(xFE, yM),
                                                  new Point(xNE, yS),
                                                  new Point(xNW, yS),
                                              });
                            }
                            if (cell.SouthWest == null) {
                                g.DrawLine(Pens.Black, xFW, yM, xNW, yS);
                            }
                            if (cell.NorthWest == null) {
                                g.DrawLine(Pens.Black, xFW, yM, xNW, yN);
                            }
                            if (cell.North == null) {
                                g.DrawLine(Pens.Black, xNW, yN, xNE, yN);
                            }
                            if (!cell.IsLinked(cell.NorthEast)) {
                                g.DrawLine(Pens.Black, xNE, yN, xFE, yM);
                            }
                            if (!cell.IsLinked(cell.SouthEast)) {
                                g.DrawLine(Pens.Black, xFE, yM, xNE, yS);
                            }
                            if (!cell.IsLinked(cell.South)) {
                                g.DrawLine(Pens.Black, xNE, yS, xNW, yS);
                            }
                        } else if (mode == DrawMode.Path) {
                            DrawPath(cell, g, cellSize);
                        }
                    }
                }
            }
            return img;

        }
    }
}