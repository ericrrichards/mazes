namespace mazes.Core {
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Linq;

    using JetBrains.Annotations;

    public class PolarCell : Cell {
        [CanBeNull]
        public PolarCell Clockwise { get; set; }
        [CanBeNull]
        public PolarCell CounterClockwise { get; set; }

        [CanBeNull]
        public PolarCell Inward { get; set; }
        [NotNull]
        public List<PolarCell> Outward { get; }

        public PolarCell(int row, int col) : base(row, col) {
            Outward = new List<PolarCell>();
        }

        public override List<Cell> Neighbors {
            get {
                var neighbors = new List<Cell> {
                    Clockwise,
                    CounterClockwise,
                    Inward
                };
                neighbors.AddRange(Outward);
                return neighbors.Where(c => c != null).ToList();
            }
        }

        public Point Center(int center, double theta, int cellSize) {
            var innerRadius = Row * cellSize;
            var outerRadius = (Row + 1) * cellSize;
            var thetaCCW = Column * theta;
            var thetaCW = (Column + 1) * theta;

            var ax = center + (float)(innerRadius * Math.Cos(thetaCCW));
            var ay = center + (float)(innerRadius * Math.Sin(thetaCCW));
            var bx = center + (float)(outerRadius * Math.Cos(thetaCCW));
            var by = center + (float)(outerRadius * Math.Sin(thetaCCW));
            var cx = center + (float)(innerRadius * Math.Cos(thetaCW));
            var cy = center + (float)(innerRadius * Math.Sin(thetaCW));
            var dx = center + (float)(outerRadius * Math.Cos(thetaCW));
            var dy = center + (float)(outerRadius * Math.Sin(thetaCW));

            var thetaCCWDegrees = (float)(thetaCCW * 180 / Math.PI);
            var thetaCWDegrees = (float)(thetaCW * 180 / Math.PI);
            var sweep = (float)(theta * 180 / Math.PI);

            using (var path = new GraphicsPath()) {
                if (Inward == null) {
                    path.AddEllipse(center - outerRadius, center - outerRadius, outerRadius * 2, outerRadius * 2);
                } else {
                    path.AddLine(cx, cy, dx, dy);
                    //path.AddLine(dx, dy, bx, by);
                    path.AddArc(center - outerRadius, center - outerRadius, outerRadius * 2, outerRadius * 2, thetaCWDegrees, -sweep);
                    path.AddLine(ax, ay, bx, by);
                    //path.AddLine(ax, ay, cx, cy);
                    path.AddArc(center - innerRadius, center - innerRadius, innerRadius * 2, innerRadius * 2, thetaCCWDegrees, sweep);

                    path.CloseFigure();
                }
                var bounds = path.GetBounds();

                return new Point((int)(bounds.Left + bounds.Width / 2), (int)(bounds.Top + bounds.Height / 2));
            }
        }
    }
}