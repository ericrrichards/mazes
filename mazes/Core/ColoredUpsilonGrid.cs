namespace mazes.Core {
    using System.Drawing;

    public class ColoredUpsilonGrid : UpsilonGrid, IColoredGrid {
        private Distances _distances;
        private Cell _farthest;
        private int _maximum;
        public ColoredUpsilonGrid(int rows, int cols) : base(rows, cols) {
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
}