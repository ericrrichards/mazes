namespace mazes.Core {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MaskedGrid : Grid {
        public Mask Mask { get; }

        public MaskedGrid(Mask mask) : base(mask.Rows, mask.Columns) {
            Mask = mask;
            Mask.UnlinkMaskedCells(Mask, Cells.Cast<CartesianCell>());
        }

        public override Cell RandomCell(Random random = null) {
            return Mask.GetRandomCell(this, random);
        }

        public override int Size => Mask.Count;
    }
}