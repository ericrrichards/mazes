using System;
using System.Collections.Generic;
using System.Linq;

namespace mazes {
    public class BinaryTree {
        private Grid _grid;
        private IEnumerator<Cell> _currentCell;
        private Random _rand;

        public static Grid Maze(Grid grid) {
            var rand = new Random();
            foreach (var cell in grid.Cells) {
                var neighbors = new[] { cell.North, cell.East }.Where(c => c != null).ToList();
                if (!neighbors.Any()) {
                    continue;
                }
                var index = rand.Next(neighbors.Count);
                var neighbor = neighbors[index];
                if (neighbor != null) {
                    cell.Link(neighbor);
                }
            }
            return grid;
        }

        public BinaryTree(Grid grid) {
            _grid = grid;
            _currentCell = _grid.Cells.GetEnumerator();
            _rand = new Random();
        }
        public bool Step() {
            var last = _currentCell.Current;
            _currentCell.MoveNext();
            var cell = _currentCell.Current;
            if (cell != null) {
                var neighbors = new[] { cell.North, cell.East }.Where(c => c != null).ToList();
                if (neighbors.Any()) {
                    var index = _rand.Next(neighbors.Count);
                    var neighbor = neighbors[index];
                    if (neighbor != null) {
                        cell.Link(neighbor);
                    }
                }
            }
            return last != _currentCell.Current;
        }
    }
}