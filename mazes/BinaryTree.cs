using System;
using System.Linq;

namespace mazes {
    public class BinaryTree {
        public static Grid On(Grid grid) {
            var rand = new Random();
            foreach (var cell in grid.Cells) {
                var neighbors = new[] {cell.North, cell.East}.Where(c => c != null).ToList();
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
    }
}