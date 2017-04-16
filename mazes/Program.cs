using System;
using System.Windows.Forms;

namespace mazes {
    using System.Diagnostics;

    using mazes.Algorithms;
    using mazes.Core;
    using mazes.UI;

    internal static class Program {
        [STAThread]
        private static void Main(string[] args) {
            
            var grid = new Grid(10,10);
            Console.WriteLine(grid);
            var img = grid.ToImg();
            img.Save("grid_img.png");


            var distGrid = new DistanceGrid(10,10);
            BinaryTree.Maze(distGrid);
            var start = distGrid[0, 0];
            var distances = start.Distances;
            distGrid.Distances = distances;
            Console.WriteLine(distGrid);

            distGrid.Distances = distances.PathTo(distGrid[distGrid.Rows-1, 0]);
            Console.WriteLine(distGrid);


            var longestGrid = new DistanceGrid(10,10);
            BinaryTree.Maze(longestGrid);
            start = longestGrid[0, 0];
            distances = start.Distances;
            var (newStart, distance) = distances.Max;

            var newDistances = newStart.Distances;

            var (goal, distance2) = newDistances.Max;

            longestGrid.Distances = newDistances.PathTo(goal);
            Console.WriteLine(longestGrid);


            var colorGrid = new ColoredGrid(25,25);
            BinaryTree.Maze(colorGrid);
            start = colorGrid[colorGrid.Rows / 2, colorGrid.Columns / 2];

            colorGrid.Distances = start.Distances;

            colorGrid.ToImg().Save("colorized.png");
            //Process.Start("colorized.png");
            
            Application.Run(new MazeForm());
        }
    }
}
