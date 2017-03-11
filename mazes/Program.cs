using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mazes {
    class Program {
        static void Main(string[] args) {
            var grid = new Grid(10,10);
            BinaryTree.Maze(grid);
            Console.WriteLine("BinaryTree");
            Console.WriteLine(grid);
            var img = grid.ToPng();
            img.Save("binaryTree.png");
            //Process.Start("binaryTree.png");

            var grid2 = new Grid(10,10);
            Sidewinder.Maze(grid2);
            Console.WriteLine("Sidewinder");
            Console.WriteLine(grid2);
            img = grid2.ToPng();
            img.Save("sidewinder.png");
            //Process.Start("sidewinder.png");

            var grid3 = new Grid(10, 10);
            var btree = new BinaryTree(grid3);
            var i = 0;
            do {
                Console.WriteLine(i++);
                Console.WriteLine(grid3);
            } while (btree.Step());

            var grid4 = new Grid(10, 10);
            var sidewinder = new Sidewinder(grid4);
            i = 0;
            do {
                Console.WriteLine(i++);
                Console.WriteLine(grid4);
            } while (sidewinder.Step());

            Application.Run(new MazeForm());
        }
    }
}
