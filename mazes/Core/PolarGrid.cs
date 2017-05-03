namespace mazes.Core {
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using JetBrains.Annotations;

    public class PolarGrid : Grid {
        public PolarGrid(int rows, int cols) : base(rows, 1) {

            PrepareGrid();
            ConfigureCells();
        }

        private void PrepareGrid() {
            var rows = new List<List<Cell>>();
            var rowHeight = 1.0 / Rows;
            rows.Add(new List<Cell>{new PolarCell(0,0)});

            for (var row = 1; row < Rows; row++) {
                var newRow = new List<Cell>();
                var radius = (double)row / Rows;
                var circumference = 2 * Math.PI * radius;

                var previousCount = rows[row - 1].Count;
                var estimatedCellWidth = circumference / previousCount;
                var ratio = (int)Math.Round(estimatedCellWidth / rowHeight);
                var cells = previousCount * ratio;
                for (var col = 0; col < cells; col++) {
                    newRow.Add(new PolarCell(row, col));
                }
                rows.Add(newRow);
            }
            _grid = rows;
        }
        private void ConfigureCells() {
            foreach (var cell in Cells) {
                var pCell = (PolarCell)cell;
                var row = cell.Row;
                var col = cell.Column;
                if (row > 0) {
                    pCell.Clockwise = this[row, col + 1];
                    pCell.CounterClockwise = this[row, col - 1];
                    var ratio = _grid[row].Count / _grid[row - 1].Count;
                    var parent = (PolarCell)_grid[row - 1][col / ratio];
                    parent.Outward.Add(cell);
                    pCell.Inward = parent;
                }
            }
        }

        public override Cell RandomCell(Random random = null) {
            var rand = random ?? new Random();
            var row = rand.Next(Rows);
            var col = rand.Next(_grid[row].Count);
            var randomCell = _grid[row][col];
            if (randomCell == null) {
                throw new InvalidOperationException("Random cell is null");
            }
            return randomCell;
        }
        [CanBeNull]
        public virtual Cell this[int row, int column] {
            get {
                if (row < 0 || row >= Rows) {
                    return null;
                }
                if (column < 0) {
                    column = _grid[row].Count - 1;
                }
                
                return _grid[row][column % _grid[row].Count];
            }
        }

        public override Image ToImg(int cellSize = 50) {
            var imgSize = 2 * Rows * cellSize;
            var center = imgSize / 2;

            var img = new Bitmap(imgSize+1, imgSize+1);
            using (var g = Graphics.FromImage(img)) {
                g.Clear(Color.White);
                
                foreach (var cell in Cells.Cast<PolarCell>()) {
                    if (cell.Row == 0) {
                        continue;
                    }
                    var theta = 2 * Math.PI / _grid[cell.Row].Count;
                    var innerRadius = cell.Row * cellSize;
                    var outerRadius = (cell.Row + 1) * cellSize;
                    var thetaCCW = cell.Column * theta;
                    var thetaCW = (cell.Column + 1) * theta;
                    
                    var cx = center + (int)(innerRadius * Math.Cos(thetaCW));
                    var cy = center + (int)(innerRadius * Math.Sin(thetaCW));
                    var dx = center + (int)(outerRadius * Math.Cos(thetaCW));
                    var dy = center + (int)(outerRadius * Math.Sin(thetaCW));
                    
                    var thetaCCWDegrees = (float)(thetaCCW * 180 / Math.PI);
                    var sweep = (float)(theta * 180 / Math.PI);

                    if (!cell.IsLinked(cell.Inward)) {
                        g.DrawArc(Pens.Black, center - innerRadius, center - innerRadius, innerRadius * 2, innerRadius * 2, thetaCCWDegrees, sweep);
                        
                    }
                    if (!cell.IsLinked(cell.Clockwise)) {
                        g.DrawLine(Pens.Black, cx, cy, dx, dy);
                    }
                }
                g.DrawEllipse(Pens.Black, center - Rows * cellSize, center - Rows*cellSize, Rows*cellSize*2, Rows* cellSize*2);
            }
            return img;
        }
    }
}