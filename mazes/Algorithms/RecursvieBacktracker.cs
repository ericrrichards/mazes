using System;
using System.Collections.Generic;
using System.Linq;
using mazes.Core;

namespace mazes.Algorithms {
    public class RecursvieBacktracker : IMazeAlgorithm {
        public Cell CurrentCell { get; private set; }
        private readonly Grid _grid;
        private readonly Random _rand;
        private Stack<Cell> _stack;


        public RecursvieBacktracker(Grid grid, int seed = -1) : this(grid, seed, null) { }
        public RecursvieBacktracker(Grid grid, int seed = -1, Cell startAt = null) {
            _grid = grid;

            _rand = seed >= 0 ? new Random(seed) : new Random();
            CurrentCell = startAt ?? _grid.RandomCell(_rand);
            _stack = new Stack<Cell>();
            _stack.Push(CurrentCell);
        }
        public static Grid Maze(Grid grid, int seed = -1) { return Maze(grid, seed, null); }
        public static Grid Maze(Grid grid, int seed = -1, Cell startAt=null) {
            var rand = seed >= 0 ? new Random(seed) : new Random();

            if (startAt == null) {
                startAt = grid.RandomCell(rand);
            }
            var stack = new Stack<Cell>();
            stack.Push(startAt);
            
            while (stack.Any()) {
                var current = stack.Peek();
                var neighbors = current.Neighbors.Where(n => !n.Links.Any()).ToList();
                if (neighbors.Any()) {
                    var neighbor = neighbors.Sample(rand);
                    current.Link(neighbor);
                    stack.Push(neighbor);
                } else {
                    stack.Pop();
                }
            }

            return grid;
        }

        public bool Step() {
            if (!_stack.Any()) {
                return false;
            }
            CurrentCell = _stack.Peek();
            var neighbors = CurrentCell.Neighbors.Where(n => !n.Links.Any()).ToList();
            if (neighbors.Any()) {
                var neighbor = neighbors.Sample(_rand);
                CurrentCell.Link(neighbor);
                _stack.Push(neighbor);
            } else {
                _stack.Pop();
            }


            return true;
        }

        
    }
}