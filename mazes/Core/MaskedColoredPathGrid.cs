namespace mazes.Core {
    using System;

    public class MaskedColoredPathGrid : ColoredPathGrid {
        public Mask Mask { get; }
        public MaskedColoredPathGrid(Mask mask) : base(mask.Rows, mask.Columns) {
            Mask = mask;
            Mask.UnlinkMaskedCells(Mask, Cells);
        }
        public override Cell RandomCell(Random random = null) {
            return Mask.GetRandomCell(this, random);
        }

        public override int Size => Mask.Count;
    }
}