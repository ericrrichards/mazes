namespace mazes.Core {
    using System;

    public class MaskedColoredGrid : ColoredGrid {
        public Mask Mask { get; }
        public MaskedColoredGrid(Mask mask) : base(mask.Rows, mask.Columns) {
            Mask = mask;
            Mask.UnlinkMaskedCells(Mask, Cells);
        }
        public override Cell RandomCell(Random random = null) {
            return Mask.GetRandomCell(this, random);
        }

        public override int Size => Mask.Count;
    }
}