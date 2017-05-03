namespace mazes.Core {
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    public class CartesianCell : Cell {
        

        // Neighboring cells
        [CanBeNull]
        public Cell North { get; set; }
        [CanBeNull]
        public Cell South { get; set; }
        [CanBeNull]
        public Cell East { get; set; }
        [CanBeNull]
        public Cell West { get; set; }

        public override List<Cell> Neighbors {
            get { return new[] { North, South, East, West }.Where(c => c != null).ToList(); }
        }

        

        public CartesianCell(int row, int col) :base(row, col){
        }
        
        

        
        
    }
}