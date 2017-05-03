namespace mazes.Core {
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    public abstract class Cell {
        // Position in the maze
        public int Row { get; }
        public int Column { get; }
        public Point Location => new Point(Column, Row);
        // Cells that are linked to this cell
        private readonly Dictionary<Cell, bool> _links;
        public List<Cell> Links => _links.Keys.ToList();
        public abstract List<Cell> Neighbors { get; }

        public Cell(int row, int col) {
            Row = row;
            Column = col;
            _links = new Dictionary<Cell, bool>();
        }

        public void Link(Cell cell, bool bidirectional = true) {
            _links[cell] = true;
            if (bidirectional) {
                cell.Link(this, false);
            }
        }
        public void Unlink(Cell cell, bool bidirectional = true) {
            _links.Remove(cell);
            if (bidirectional) {
                cell.Unlink(this, false);
            }
        }
        public bool IsLinked(Cell cell) {
            if (cell == null) {
                return false;
            }
            return _links.ContainsKey(cell);
        }

        public Distances Distances {
            get {
                var distances = new Distances(this);
                var frontier = new HashSet<Cell> {
                    this
                };

                while (frontier.Any()) {
                    var newFrontier = new HashSet<Cell>();

                    foreach (var cell in frontier) {
                        foreach (var linked in cell.Links) {
                            if (distances[linked] >= 0) {
                                continue;
                            }
                            distances[linked] = distances[cell] + 1;
                            newFrontier.Add(linked);
                        }
                    }
                    frontier = newFrontier;
                }
                return distances;
            }
        }
    }
}