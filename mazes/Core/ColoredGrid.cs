namespace mazes.Core {
    using System.Drawing;

    public class ColoredGrid : Grid {
        private Distances _distances;
        private Cell _farthest;
        private int _maximum;
        public ColoredGrid(int rows, int cols) : base(rows, cols) { }

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
            var dark = (int)(255 * intensity);
            var bright = (int)(128 + 127 * intensity);
            return Color.FromArgb(dark, bright, dark);
        }
    }
}