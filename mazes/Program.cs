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

            Application.Run(new MazeForm());
        }

        private static void PolarMaze() {
            var grid = new PolarGrid(10, 10);
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
