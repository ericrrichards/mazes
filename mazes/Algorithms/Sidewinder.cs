namespace mazes.Algorithms {
    using System;
    using System.Collections.Generic;

    using mazes.Core;

    public class Sidewinder : IMazeAlgorithm {
        private readonly Grid _grid;
        private readonly Random _rand;
        private readonly IEnumerator<Cell> _currentCell;
        private List<Cell> _run = new List<Cell>();

        public static Grid Maze(Grid grid, int seed = -1) {
            var rand = seed >= 0 ? new Random(seed) : new Random();
            foreach (var row in grid.Row) {
                var run = new List<Cell>();

                foreach (var cell in row) {
                    run.Add(cell);

                    var atEasternBoundary = cell.East == null;
                    var atNorthernBoundary = cell.North == null;

                    var shouldCloseOut = atEasternBoundary || (!atNorthernBoundary && rand.Next(2) == 0);

                    if (shouldCloseOut) {
                        var member = run.Sample(rand);
                        if (member.North != null) {
                            member.Link(member.North);
                        }
                        run.Clear();
                    } else {
                        cell.Link(cell.East);
                    }
                }
            }
            return grid;
        }

        public Sidewinder(Grid grid, int seed=-1) {
            _grid = grid;
            _currentCell = _grid.Cells.GetEnumerator();
            _rand = seed >= 0 ? new Random(seed) : new Random();
        }

        public bool Step() {
            var last = _currentCell.Current;
            _currentCell.MoveNext();
            var cell = _currentCell.Current;
            if (cell != null) {
                if (cell.Column == 0) {
                    _run = new List<Cell>();
                }
                _run.Add(cell);

                var atEasternBoundary = cell.East == null;
                var atNorthernBoundary = cell.North == null;

                var shouldCloseOut = atEasternBoundary || (!atNorthernBoundary && _rand.Next(2) == 0);

                if (shouldCloseOut) {
                    var member = _run.Sample(_rand);
                    if (member.North != null) {
                        member.Link(member.North);
                    }
                    _run.Clear();
                } else {
                    cell.Link(cell.East);
                }
            }
            return last != _currentCell.Current;
        }
    }

    
}