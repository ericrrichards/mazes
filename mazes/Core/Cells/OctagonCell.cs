namespace mazes.Core.Cells {
    using System.Collections.Generic;
    using System.Linq;

    class OctagonCell : CartesianCell {

        public CartesianCell NorthWest { get; set; }
        public CartesianCell NorthEast { get; set; }
        public CartesianCell SouthWest { get; set; }
        public CartesianCell SouthEast { get; set; }

        public override List<Cell> Neighbors => new Cell[] { North, NorthWest, NorthEast, East, West, South, SouthEast, SouthWest }.Where(c => c != null).ToList();


        public OctagonCell(int row, int col) : base(row, col) { }
        
    }
}