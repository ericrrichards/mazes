namespace mazes.Core.Cells {
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

        public int Weight { get; set; }

        public Cell(int row, int col) {
            Row = row;
            Column = col;
            Weight = 1;
            _links = new Dictionary<Cell, bool>();
        }

        public virtual void Link(Cell cell, bool bidirectional = true) {
            _links[cell] = true;
            if (bidirectional) {
                cell.Link(this, false);
            }
        }
        public virtual void Unlink(Cell cell, bool bidirectional = true) {
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

        public Distances WeightedDistances {
            get {
                var weights = new Distances(this);
                var pending = new HashSet<Cell>{this};

                while (pending.Any()) {
                    var cell = pending.OrderBy(c => weights[c]).First();
                    pending.Remove(cell);

                    foreach (var neighbor in cell.Links) {
                        var totalWeight = weights[cell] + neighbor.Weight;
                        if (weights[neighbor] >= 0 || totalWeight < weights[neighbor]) {
                            pending.Add(neighbor);
                            weights[neighbor] = totalWeight;
                        }
                    }
                }

                return weights;
            }
        }
    }
}