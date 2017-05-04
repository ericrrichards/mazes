namespace mazes.Core {
    using System.Drawing;

    public interface IColoredGrid: IGrid {
        Color BackColor { get; set; }
        Distances Distances { get; set; }
    }
}