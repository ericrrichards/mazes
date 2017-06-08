namespace mazes.Core.Grids {
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using mazes.Core.Cells;
    using mazes.Core.Grids.Cartesian;

    public class WeaveGrid : Grid {
        private readonly List<UnderCell> _underCells = new List<UnderCell>();

        public WeaveGrid(int rows, int cols) : base(rows, cols) {
            PrepareGrid();
            ConfigureCells();
        }

        private void PrepareGrid() {
            var rows = new List<List<Cell>>();
            for (var row = 0; row < Rows; row++) {
                var newRow = new List<Cell>();
                for (var column = 0; column < Columns; column++) {
                    newRow.Add(new OverCell(row, column, this));
                }
                rows.Add(newRow);
            }
            _grid = rows;
        }

        private void ConfigureCells() {
            foreach (var cell in Cells.Cast<OverCell>()) {
                var row = cell.Row;
                var col = cell.Column;

                cell.North = (OverCell)this[row - 1, col];
                cell.South = (OverCell)this[row + 1, col];
                cell.West = (OverCell)this[row, col - 1];
                cell.East = (OverCell)this[row, col + 1];
            }
        }


        public void TunnelUnder(OverCell overCell) {
            var underCell = new UnderCell(overCell);
            _underCells.Add(underCell);
        }

        public override IEnumerable<Cell> Cells => base.Cells.Union(_underCells);

        public override Image ToImg(int cellSize = 50, float insetPrc = 0) {
            return base.ToImg(cellSize, insetPrc == 0 ? 0.1f : insetPrc);
        }

        protected override void ToImgWithInset(Graphics g, CartesianCell cell, DrawMode mode, int cellSize, int x, int y, int inset) {
            if (cell is UnderCell) {
                var (x1, x2, x3, x4, y1, y2, y3, y4) = CellCoordinatesWithInset(x, y, cellSize, inset);
                if (cell.VerticalPassage) {
                    g.DrawLine(Pens.Black, x2, y1, x2, y2);
                    g.DrawLine(Pens.Black, x3, y1, x3, y2);
                    g.DrawLine(Pens.Black, x2, y3, x2, y4);
                    g.DrawLine(Pens.Black, x3, y3, x3, y4);
                } else {
                    g.DrawLine(Pens.Black, x1, y2, x2, y2);
                    g.DrawLine(Pens.Black, x1, y3, x2, y3);
                    g.DrawLine(Pens.Black, x3, y2, x4, y2);
                    g.DrawLine(Pens.Black, x3, y3, x4, y3);
                }
            } else {
                base.ToImgWithInset(g, cell, mode, cellSize, x, y, inset);
            }
        }
    }
}