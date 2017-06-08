using System;
using System.Linq;
using mazes.Core;

namespace mazes.Algorithms {
    using mazes.Core.Cells;
    using mazes.Core.Grids;
    using mazes.Core.Grids.Cartesian;

    public class HuntAndKill : IMazeAlgorithm {
        public Cell CurrentCell { get; private set; }
        private readonly Grid _grid;
        private readonly Random _rand;

        public static Grid Maze(Grid grid, int seed = -1) {
            var rand = seed >= 0 ? new Random(seed) : new Random();

            var current = grid.RandomCell(rand);

            while (current != null) {
                var unvisitedNeighbors = current.Neighbors.Where(n => !n.Links.Any()).ToList();

                if (unvisitedNeighbors.Any()) {
                    var neighbor = unvisitedNeighbors.Sample(rand);
                    current.Link(neighbor);
                    current = neighbor;
                } else {
                    current = null;

                    foreach (var cell in grid.Cells) {
                        var visitedNeighbors = cell.Neighbors.Where(n => n.Links.Any()).ToList();
                        if (!cell.Links.Any() && visitedNeighbors.Any()) {
                            current = cell;
                            var neighbor = visitedNeighbors.Sample(rand);
                            current.Link(neighbor);
                            break;
                        }
                    }
                }
            }


            return grid;
        }

        public bool Step() {
            if (CurrentCell == null) {
                return false;
            }
            var unvisitedNeighbors = CurrentCell.Neighbors.Where(n => !n.Links.Any()).ToList();
            if (unvisitedNeighbors.Any()) {
                var neighbor = unvisitedNeighbors.Sample(_rand);
                CurrentCell.Link(neighbor);
                CurrentCell = neighbor;
            } else {
                CurrentCell = null;

                foreach (var cell in _grid.Cells) {
                    var visitedNeighbors = cell.Neighbors.Where(n => n.Links.Any()).ToList();
                    if (!cell.Links.Any() && visitedNeighbors.Any()) {
                        CurrentCell = cell;
                        var neighbor = visitedNeighbors.Sample(_rand);
                        CurrentCell.Link(neighbor);
                        break;
                    }
                }
            }

            return true;
        }

        public HuntAndKill(Grid grid, int seed = -1) {
            _grid = grid;

            _rand = seed >= 0 ? new Random(seed) : new Random();
            CurrentCell = _grid.RandomCell(_rand);
        }



    }
}