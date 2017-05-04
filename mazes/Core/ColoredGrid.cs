namespace mazes.Core {
    using System.Drawing;

    public class ColoredGrid : Grid, IColoredGrid {
        private Distances _distances;
        private Cell _farthest;
        private int _maximum;

        public ColoredGrid(int rows, int cols) : base(rows, cols) {
            BackColor = Color.Green;
        }

        public Color BackColor { get; set; }

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

    public class ColoredPolarGrid : PolarGrid, IColoredGrid {
        private Distances _distances;
        private Cell _farthest;
        private int _maximum;

        public Color BackColor { get; set; }

        public ColoredPolarGrid(int rows, int cols) : base(rows, cols) {
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