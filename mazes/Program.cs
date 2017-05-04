using System;
using System.Windows.Forms;

namespace mazes {
    using System.Collections.Generic;
    using System.Diagnostics;

    using mazes.Algorithms;
    using mazes.Core;
    using mazes.UI;

    internal static class Program {
        [STAThread]
        private static void Main(string[] args) {
            //MaskedMazes();

            //PolarMaze();

            //ColorizeThetaMazes();

            //MakeTriangleDeltaMaze();

            Application.Run(new MazeForm());
        }

        private static void MakeTriangleDeltaMaze() {
            var grid = new TriangleGrid(11);
            RecursiveBacktracker.Maze(grid, startAt: null);
            var img = grid.ToImg();
            img.Save("triGrid.png");
            Process.Start("triGrid.png");
        }

        private static void ColorizeThetaMazes() {
            var grid = new ColoredPolarGrid(100);
            BinaryTree.Maze(grid);
            var start = grid[0, 0];
            grid.Distances = start.Distances;

            var img = grid.ToImg(25);
            img.Save("binaryTheta.png");

            grid = new ColoredPolarGrid(100);
            Sidewinder.Maze(grid);
            start = grid[0, 0];
            grid.Distances = start.Distances;
            img = grid.ToImg(25);
            img.Save("sidewinderTheta.png");

            grid = new ColoredPolarGrid(100);
            AldousBroder.Maze(grid);
            start = grid[0, 0];
            grid.Distances = start.Distances;
            img = grid.ToImg(25);
            img.Save("aldousBroderTheta.png");

            grid = new ColoredPolarGrid(100);
            HuntAndKill.Maze(grid);
            start = grid[0, 0];
            grid.Distances = start.Distances;
            img = grid.ToImg(25);
            img.Save("huntAndKillTheta.png");

            grid = new ColoredPolarGrid(100);
            RecursiveBacktracker.Maze(grid, startAt: null);
            start = grid[0, 0];
            grid.Distances = start.Distances;
            img = grid.ToImg(25);
            img.Save("recursiveBacktrackerTheta.png");

            grid = new ColoredPolarGrid(100);
            Wilsons.Maze(grid);
            start = grid[0, 0];
            grid.Distances = start.Distances;
            img = grid.ToImg(25);
            img.Save("wilsonsTheta.png");

            Process.Start("binaryTheta.png");
            Process.Start("sidewinderTheta.png");
            Process.Start("aldousBroderTheta.png");
            Process.Start("huntAndKillTheta.png");
            Process.Start("recursiveBacktrackerTheta.png");
            Process.Start("wilsonsTheta.png");
        }

        private static void PolarMaze() {
            var grid = new PolarGrid(10);
            RecursiveBacktracker.Maze(grid, 0);
            var img = grid.ToImg();
            img.Save("polarGrid.png");
            Process.Start("polarGrid.png");
        }

        private static void MaskedMazes() {
            var mask = new Mask(5, 5);
            mask[0, 0] = false;
            mask[2, 2] = false;
            mask[4, 4] = false;

            var grid = new MaskedGrid(mask);
            RecursiveBacktracker.Maze(grid, 0);
            Console.WriteLine(grid);

            var maskStr = new List<string> {
                "X........X",
                "....XX....",
                "...XXXX...",
                "....XX....",
                "X........X",
                "X........X",
                "....XX....",
                "...XXXX...",
                "....XX....",
                "X........X"
            };
            mask = Mask.FromString(maskStr);
            grid = new MaskedGrid(mask);
            RecursiveBacktracker.Maze(grid, 0);
            Console.WriteLine(grid);
            var img = grid.ToImg();
            img.Save("masked.png");
            Process.Start("masked.png");

            mask = Mask.FromImageFile("maze_text.png");
            grid = new MaskedGrid(mask);
            RecursiveBacktracker.Maze(grid, 0);
            img = grid.ToImg();
            img.Save("masked2.png");
            Process.Start("masked2.png");
        }
    }
}
