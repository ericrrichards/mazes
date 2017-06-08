namespace mazes.Algorithms {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using mazes.Core;
    using mazes.Core.Cells;
    using mazes.Core.Grids;
    using mazes.Core.Grids.Cartesian;

    public class Sidewinder : IMazeAlgorithm {
        private readonly Grid _grid;
        private readonly Random _rand;
        private readonly IEnumerator<Cell> _currentCell;
        public Cell CurrentCell => _currentCell.Current;
        private List<Cell> _run = new List<Cell>();

        public static Grid Maze(Grid grid, int seed = -1) {
            var rand = seed >= 0 ? new Random(seed) : new Random();
            foreach (var row in grid.Row) {
                var run = new List<Cell>();

                foreach (var cell in row) {
                    run.Add(cell);

                    var atEasternBoundary = NextCell(cell)==null;
                    var atNorthernBoundary = CloseOutCell(cell)==null;

                    var shouldCloseOut = atEasternBoundary || (!atNorthernBoundary && rand.Next(2) == 0);

                    if (shouldCloseOut) {
                        var member = SampleRun(run, rand);
                        if (CloseOutCell(member) != null) {
                            member.Link(CloseOutCell(member));
                        }
                        run.Clear();
                    } else {
                        cell.Link(NextCell(cell));
                    }
                }
            }
            return grid;
        }

        private static Cell SampleRun(List<Cell> run, Random rand) {
            if (run.First() is TriangleCell ) {
                var filtered = run.Where(c => ((TriangleCell)c).North != null).ToList();
                if (filtered.Count > 0) {
                    return filtered.Sample(rand);
                }
            }
            return run.Sample(rand);
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

                var atEasternBoundary = NextCell(cell)==null;
                var atNorthernBoundary = CloseOutCell(cell)== null;

                var shouldCloseOut = atEasternBoundary || (!atNorthernBoundary && _rand.Next(2) == 0);

                if (shouldCloseOut) {
                    var member = SampleRun(_run, _rand);
                    if (CloseOutCell(member) != null) {
                        member.Link(CloseOutCell(member));
                    }
                    _run.Clear();
                } else {
                    cell.Link(NextCell(cell));
                }
            }
            return last != _currentCell.Current;
        }

        private static Cell NextCell(Cell cell) {
            switch (cell) {
                case CartesianCell cartesianCell:
                    return cartesianCell.East;
                case PolarCell polarCell:
                    return polarCell.Clockwise;
                case HexCell hexCell:
                    return hexCell.Column % 2 == 0 ? hexCell.SouthEast : hexCell.NorthEast;
                case TriangleCell triCell:
                    return triCell.East;
            }
            return null;
        }

        private static Cell CloseOutCell(Cell member) {
            switch (member) {
                case CartesianCell cartesianCell:
                    return cartesianCell.North;
                case PolarCell polarCell:
                    return polarCell.Inward;
                case HexCell hexCell:
                    return hexCell.North;
                case TriangleCell triCell:
                    return triCell.Row == 0 ? triCell.North : triCell.North??triCell.West;
            }
            return null;
        }

        public Sidewinder(Grid grid, int seed=-1) {
            _grid = grid;
            _currentCell = _grid.Cells.GetEnumerator();
            _rand = seed >= 0 ? new Random(seed) : new Random();
        }
    }
}