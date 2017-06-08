using System;
using System.Linq;
using mazes.Core;

namespace mazes.Algorithms {
    using mazes.Core.Cells;
    using mazes.Core.Grids;
    using mazes.Core.Grids.Cartesian;

    public class AldousBroder : IMazeAlgorithm {

        private readonly Grid _grid;
        private readonly Random _rand;
        private Cell _currentCell;
        public Cell CurrentCell => _currentCell;
        private int _unvisited;


        public static Grid Maze(Grid grid, int seed = -1) {
            var rand = seed >= 0 ? new Random(seed) : new Random();

            var cell = grid.RandomCell(rand);
            var unvisited = grid.Size - 1;
            while (unvisited > 0) {
                var neighbor = cell.Neighbors.Sample(rand);
                if (!neighbor.Links.Any()) {
                    cell.Link(neighbor);
                    unvisited--;
                }
                cell = neighbor;
            }
            return grid;

        }

        public AldousBroder(Grid grid, int seed = -1) {
            _grid = grid;

            _rand = seed >= 0 ? new Random(seed) : new Random();
            _currentCell = _grid.RandomCell(_rand);
            _unvisited = _grid.Size - 1;
        }

        public bool Step() {
            if (_unvisited == 0) {
                return false;
            }
            var neighbor = _currentCell.Neighbors.Sample(_rand);
            if (!neighbor.Links.Any()) {
                _currentCell.Link(neighbor);
                _unvisited--;
            }
            _currentCell = neighbor;
            return true;
        }
    }
}