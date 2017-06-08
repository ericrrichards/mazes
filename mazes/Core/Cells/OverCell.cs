namespace mazes.Core.Cells {
    using System.Collections.Generic;
    using System.Linq;

    using mazes.Core.Grids;

    public sealed class UnderCell : OverCell {
        public UnderCell(OverCell overCell) : base(overCell.Row, overCell.Column, null) {
            if (overCell.HorizontalPassage) {
                North = overCell.North;
                overCell.North.South = this;
                South = overCell.South;
                overCell.South.North = this;

                Link(North);
                Link(South);
            } else {
                East = overCell.East;
                overCell.East.West = this;
                West = overCell.West;
                overCell.West.East = this;
                Link(East);
                Link(West);
            }


        }

        public override bool HorizontalPassage => East != null || West != null;
        public override bool VerticalPassage => North != null || South != null;
    }

    public class OverCell : CartesianCell {
        private readonly WeaveGrid _grid;

        public OverCell(int row, int col, WeaveGrid grid) : base(row, col) {
            _grid = grid;
        }
        
        public override void Link(Cell cell, bool bidirectional = true) {
            var overcell = (OverCell)cell; 
            OverCell neighbor = null;
            if (North != null && North == overcell.South) {
                neighbor = (OverCell)North;
            }
            if (South != null && South == overcell.North) {
                neighbor = (OverCell)South;
            }
            if (East != null && East == overcell.West) {
                neighbor = (OverCell)East;
            }
            if (West != null && West == overcell.East) {
                neighbor = (OverCell)West;
            }
            if (neighbor != null) {
                _grid.TunnelUnder(neighbor);
            } else {
                base.Link(cell, bidirectional);
            }
        }

        public override List<Cell> Neighbors {
            get {
                var list = new Cell[] { North, South, East, West }.Where(c => c != null).ToList();
                if (CanTunnelNorth) {
                    list.Add(North.North);
                }
                if (CanTunnelSouth) {
                    list.Add(South.South);
                }
                if (CanTunnelEast) {
                    list.Add(East.East);
                }
                if (CanTunnelWest) {
                    list.Add(West.West);
                }
                return list;
            }
        }
        public bool CanTunnelWest => West?.West != null && West.VerticalPassage;
        
        public bool CanTunnelEast => East?.East != null && East.VerticalPassage;
        public bool CanTunnelSouth => South?.South != null && South.HorizontalPassage;
        public bool CanTunnelNorth => North?.North != null && North.HorizontalPassage;

        public override bool HorizontalPassage => IsLinked(East) && IsLinked(West) && !IsLinked(North) && !IsLinked(South);
        public override bool VerticalPassage => IsLinked(North) && IsLinked(South) && !IsLinked(East) && !IsLinked(West);
    }
}