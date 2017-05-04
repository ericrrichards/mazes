namespace mazes.Core {
    public interface IPathGrid: IColoredGrid {
        int PathLength { get; }
        Distances Path { get; set; }
    }
}