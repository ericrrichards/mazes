using System;
using System.Collections.Generic;
using System.Linq;
using mazes.Core;

namespace mazes.Algorithms {
    public class Wilsons : IMazeAlgorithm {
        private readonly Grid _grid;
        private readonly Random _rand;
        private Cell _currentCell;
        public Cell CurrentCell => _currentCell;
        private List<Cell> _unvisited;
        private List<Cell> _path;

        public static Grid Maze(Grid grid, int seed = -1) {
            var rand = seed >= 0 ? new Random(seed) : new Random();

            var unvisited = grid.Cells.ToList();

            var first = unvisited.Sample(rand);
            unvisited.Remove(first);

            while (unvisited.Any()) {
                var cell = unvisited.Sample(rand);
                var path = new List<Cell> { cell};

                while (unvisited.Contains(cell)) {
                    cell = cell.Neighbors.Sample(rand);
                    var position = path.IndexOf(cell);
                    if (position != -1) {
                        path = path.Take(position+1).ToList();
                    } else {
                        path.Add(cell);
                    }
                }

                for (int i = 0; i < path.Count-1; i++) {
                    path[i].Link(path[i + 1]);
                    unvisited.Remove(path[i]);
                }
            }


            return grid;
        }

        public Wilsons(Grid grid, int seed = -1) {
            _grid = grid;

            _rand = seed >= 0 ? new Random(seed) : new Random();
            _unvisited = _grid.Cells.ToList();
            var firstCell = _unvisited.Sample(_rand);
            Console.WriteLine(firstCell.Location);
            _unvisited.Remove(firstCell);
            _currentCell = _unvisited.Sample(_rand);
            _path = null;
        }

        public bool Step() {    
            if (!_unvisited.Any()) {
                return false;
            }
            if (_path == null) {
                _path = new List<Cell> { _currentCell };
            }

            if (_unvisited.Contains(_currentCell)) {
                _currentCell = _currentCell.Neighbors.Sample(_rand);
                var position = _path.LastIndexOf(_currentCell);
                if (position != -1) {
                    _path = _path.Take(position+1).ToList();
                } else {
                    _path.Add(_currentCell);
                }
                Console.WriteLine(_path.Select(p => p.Location).Aggregate(string.Empty, (s, point) => s + point + ", "));
            } else {
                for (int i = 0; i < _path.Count - 1; i++) {
                    _path[i].Link(_path[i + 1]);
                    _unvisited.Remove(_path[i]);
                }
                _path = null;
                if (_unvisited.Any()) {
                    _currentCell = _unvisited.Sample(_rand);
                }
            }
            
            return true;
        }
    }
}