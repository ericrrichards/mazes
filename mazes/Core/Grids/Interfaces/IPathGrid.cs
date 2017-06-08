namespace mazes.Core.Grids.Interfaces {
    public interface IPathGrid: IColoredGrid {
        int PathLength { get; }
        Distances Path { get; set; }
    }
}