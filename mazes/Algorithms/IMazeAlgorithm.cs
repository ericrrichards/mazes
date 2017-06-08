namespace mazes.Algorithms {
    using mazes.Core;
    using mazes.Core.Cells;

    public interface IMazeAlgorithm {
        bool Step();
        Cell CurrentCell { get; }
    }
}