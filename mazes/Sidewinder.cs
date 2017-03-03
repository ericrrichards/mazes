using System;
using System.Collections.Generic;

namespace mazes {
    public class Sidewinder {
        public static Grid On(Grid grid) {
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
    }
}