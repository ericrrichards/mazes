namespace mazes.Core {
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    public class PolarCell : Cell {
        [CanBeNull]
        public Cell Clockwise { get; set; }
        [CanBeNull]
        public Cell CounterClockwise { get; set; }

        [CanBeNull]
        public Cell Inward { get; set; }
        [NotNull]
        public List<Cell> Outward { get; }

        public PolarCell(int row, int col) : base(row, col) {
            Outward = new List<Cell>();
        }

        public override List<Cell> Neighbors {
            get {
                var neighbors =  new List<Cell> {
                    Clockwise,
                    CounterClockwise,
                    Inward
                };
                neighbors.AddRange(Outward);
                return neighbors.Where(c => c != null).ToList();
            }
        }
    }
}