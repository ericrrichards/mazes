using System;
using System.Windows.Forms;

namespace mazes {
    using mazes.UI;

    internal static class Program {
        [STAThread]
        private static void Main(string[] args) {
            
            Application.Run(new MazeForm());
        }
    }
}
