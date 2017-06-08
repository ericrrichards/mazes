namespace mazes.Core.Grids.Masked {
    using System;
    using System.Linq;

    using mazes.Core.Cells;
    using mazes.Core.Grids.Cartesian;

    public class MaskedColoredGrid : ColoredGrid {
        public Mask Mask { get; }
        public MaskedColoredGrid(Mask mask) : base(mask.Rows, mask.Columns) {
            Mask = mask;
            Mask.UnlinkMaskedCells(Mask, Cells.Cast<CartesianCell>());
        }
        public override Cell RandomCell(Random random = null) {
            return Mask.GetRandomCell(this, random);
        }

        public override int Size => Mask.Count;
    }
}