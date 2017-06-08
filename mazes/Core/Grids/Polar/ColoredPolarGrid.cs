namespace mazes.Core.Grids.Polar {
    using System.Drawing;

    using mazes.Core.Cells;
    using mazes.Core.Grids.Interfaces;

    public class ColoredPolarGrid : PolarGrid, IColoredGrid {
        private Distances _distances;
        private Cell _farthest;
        private int _maximum;

        public Color BackColor { get; set; }

        public ColoredPolarGrid(int rows) : base(rows) {
            BackColor = Color.Green;
        }
        public Distances Distances {
            get => _distances;
            set {
                _distances = value;
                (_farthest, _maximum) = value.Max;
            }
        }

        protected override Color? BackgroundColorFor(Cell cell) {
            if (Distances == null || Distances[cell] < 0) {
                return null;
            }
            var distance = Distances[cell];
            var intensity = (_maximum - distance) / (float)_maximum;

            return BackColor.Scale(intensity);
        }
    }
}