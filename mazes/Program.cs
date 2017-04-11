using System;
using System.Windows.Forms;

namespace mazes {
    using mazes.Core;
    using mazes.UI;

    internal static class Program {
        [STAThread]
        private static void Main(string[] args) {
            
            var grid = new Grid(10,10);
            Console.WriteLine(grid);
            var img = grid.ToImg();
            img.Save("grid_img.png");

            Application.Run(new MazeForm());
        }
    }
}
