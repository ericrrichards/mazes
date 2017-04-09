namespace mazes.Algorithms {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using mazes.Core;

    public class BinaryTree : IMazeAlgorithm {
        private readonly Grid _grid;
        private readonly IEnumerator<Cell> _currentCell;
        private readonly Random _rand;

        public static Grid Maze(Grid grid, int seed = -1) {
            var rand = seed >= 0 ? new Random(seed) : new Random();
            foreach (var cell in grid.Cells) {
                var neighbors = new[] { cell.North, cell.East }.Where(c => c != null).ToList();
                if (!neighbors.Any()) {
                    continue;
                }
                var neighbor = neighbors.Sample(rand);
                if (neighbor != null) {
                    cell.Link(neighbor);
                }
            }
            return grid;
        }

        public BinaryTree(Grid grid, int seed = -1) {
            _grid = grid;
            _currentCell = _grid.Cells.GetEnumerator();
            _rand = seed >= 0 ? new Random(seed) : new Random();
        }
        public bool Step() {
            var last = _currentCell.Current;
            _currentCell.MoveNext();
            var cell = _currentCell.Current;
            if (cell != null) {
                var neighbors = new[] { cell.North, cell.East }.Where(c => c != null).ToList();
                if (neighbors.Any()) {
                    var neighbor = neighbors.Sample(_rand);
                    if (neighbor != null) {
                        cell.Link(neighbor);
                    }
                }
            }
            return last != _currentCell.Current;
        }
    }
}