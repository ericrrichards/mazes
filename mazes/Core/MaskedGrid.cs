namespace mazes.Core {
    using System;

    public class MaskedGrid : Grid {
        public Mask Mask { get; set; }

        public MaskedGrid(Mask mask) : base(mask.Rows, mask.Columns) {
            Mask = mask;


            foreach (var cell in Cells) {
                var row = cell.Row;
                var col = cell.Column;

                if (!Mask[row, col]) {
                    // Unlink this cell from its neighbors
                    if (cell.North != null)
                        cell.North.South = null;
                    cell.North = null;
                    if (cell.South != null)
                        cell.South.North = null;
                    cell.South = null;
                    if (cell.West != null)
                        cell.West.East = null;
                    cell.West = null;
                    if (cell.East != null)
                        cell.East.West = null;
                    cell.East = null;
                }
            }
        }

        public override Cell RandomCell(Random random = null) {
            var location = Mask.RandomLocation(random);
            var randomCell = this[location.Y, location.X];
            if (randomCell == null) {
                throw new InvalidOperationException("Invalid random cell location: " + location);
            }
            return randomCell;
        }

        public override int Size => Mask.Count;
    }
}