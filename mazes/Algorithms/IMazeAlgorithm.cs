namespace mazes.Algorithms {
    using mazes.Core;

    public interface IMazeAlgorithm {
        bool Step();
        Cell CurrentCell { get; }
    }
}