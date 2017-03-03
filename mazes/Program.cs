using System;
using System.Text;
using System.Threading.Tasks;

namespace mazes {
    class Program {
        static void Main(string[] args) {
            var grid = new Grid(4,4);
            BinaryTree.On(grid);
            Console.WriteLine(grid);

            var grid2 = new Grid(4,4);
            Sidewinder.On(grid2);
            Console.WriteLine(grid2);
        }
    }
}
