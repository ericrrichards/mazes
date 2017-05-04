namespace mazes.Algorithms {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using mazes.Core;

    public class BinaryTree : IMazeAlgorithm {
        private readonly Grid _grid;
        private readonly IEnumerator<Cell> _currentCell;
        public Cell CurrentCell => _currentCell.Current;

        private readonly Random _rand;

        public static Grid Maze(Grid grid, int seed = -1) {
            var rand = seed >= 0 ? new Random(seed) : new Random();
            foreach (var cell in grid.Cells) {
                var neighbors = GetNeighbors(cell);

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

        private static List<Cell> GetNeighbors(Cell cell) {
            switch (cell) {
                case CartesianCell cartesianCell:
                    return new Cell[] { cartesianCell.North, cartesianCell.East }.Where(c => c != null).ToList();
                case PolarCell polarCell:
                    return new Cell[] { polarCell.Inward, polarCell.Clockwise }.Where(c => c != null).ToList();
                case HexCell hexCell:
                    return new Cell[] { hexCell.North, hexCell.Column % 2 == 0 ? hexCell.SouthEast : hexCell.NorthEast }.Where(c => c != null).ToList();
                case TriangleCell triCell:
                    if (triCell.East == null && triCell.North == null) {
                        return new List<Cell>{triCell.West};
                    }
                    if (triCell.East != null && triCell.East.East == null && triCell.East.North == null && triCell.North != null) {
                        return new List<Cell>{triCell.North};
                    }
                    return new List<Cell> { triCell.North, triCell.East }.Where(c => c != null).ToList();


            }
            return new List<Cell>();
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
                var neighbors = GetNeighbors(cell);
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