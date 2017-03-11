using System;
using System.Collections.Generic;
using System.Linq;

namespace mazes {
    public interface IMazeAlgorithm {
        bool Step();
    }

    public class Sidewinder : IMazeAlgorithm {
        private Grid _grid;
        private Random _rand;
        private IEnumerator<Cell> _currentCell;
        private List<Cell> _run = new List<Cell>();

        public Sidewinder(Grid grid) {
            _grid = grid;
            _currentCell = _grid.Cells.GetEnumerator();
            _rand = new Random();
        }

        public static Grid Maze(Grid grid) {
            var rand = new Random();
            foreach (var row in grid.Row) {
                var run = new List<Cell>();

                foreach (var cell in row) {
                    run.Add(cell);

                    var atEasternBoundary = cell.East == null;
                    var atNorthernBoundary = cell.North == null;

                    var shouldCloseOut = atEasternBoundary || (!atNorthernBoundary && rand.Next(2) == 0);

                    if (shouldCloseOut) {
                        var member = run[rand.Next(run.Count)];
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
                    var member = _run[_rand.Next(_run.Count)];
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