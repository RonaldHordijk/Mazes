namespace Mazes
{
  public class OverCell : Cell
  {
    private CreateUnderCellFunction createUnderCell;

    public OverCell(int row, int column, CreateUnderCellFunction function) : base(row, column)
    {
      this.createUnderCell = function;
    }

    public delegate void CreateUnderCellFunction(OverCell cell);

    public override IReadOnlyCellCollection Neighbors
    {
      get
      {
        var neighbors = base.Neighbors as CellCollection;

        if (this.CanTunnelNorth())
          neighbors.Add(this.North.North);

        if (this.CanTunnelSouth())
          neighbors.Add(this.South.South);

        if (this.CanTunnelEast())
          neighbors.Add(this.East.East);

        if (this.CanTunnelWest())
          neighbors.Add(this.West.West);

        return neighbors as IReadOnlyCellCollection;
      }
    }

    public override void Link(Cell cell, bool bidi = true)
    {
      if (cell == null)
        return;
      Cell neighbor = null;

      if ((this.North != null) && (this.North == cell.South))
        neighbor = this.North;
      if ((this.South != null) && (this.South == cell.North))
        neighbor = this.South;
      if ((this.East != null) && (this.East == cell.West))
        neighbor = this.East;
      if ((this.West != null) && (this.West == cell.East))
        neighbor = this.West;

      if (neighbor != null)
      {
        this.createUnderCell(neighbor as OverCell);
      }
      else
      {
        base.Link(cell, bidi);
      }
    }

    public bool IsHorizontalPassage()
    {
      return
        this.IsLinked(this.East) &&
        this.IsLinked(this.West) &&
        !this.IsLinked(this.North) &&
        !this.IsLinked(this.South);
    }

    public bool IsVerticalPassage()
    {
      return
        !this.IsLinked(this.East) &&
        !this.IsLinked(this.West) &&
        this.IsLinked(this.North) &&
        this.IsLinked(this.South);
    }

    private bool CanTunnelNorth()
    {
      if ((this.North == null) ||
         (this.North.North == null))
        return false;

      if (this.North is OverCell)
        return (this.North as OverCell).IsHorizontalPassage();
      else
        return (this.North as UnderCell).IsHorizontalPassage();
    }

    private bool CanTunnelSouth()
    {
      if ((this.South == null) ||
         (this.South.South == null))
        return false;

      if (this.South is OverCell)
        return (this.South as OverCell).IsHorizontalPassage();
      else
        return (this.South as UnderCell).IsHorizontalPassage();
    }

    private bool CanTunnelEast()
    {
      if ((this.East == null) ||
         (this.East.East == null))
        return false;

      if (this.East is OverCell)
        return (this.East as OverCell).IsVerticalPassage();
      else
        return (this.East as UnderCell).IsVerticalPassage();
    }

    private bool CanTunnelWest()
    {
      if ((this.West == null) ||
         (this.West.West == null))
        return false;

      if (this.West is OverCell)
        return (this.West as OverCell).IsVerticalPassage();
      else
        return (this.West as UnderCell).IsVerticalPassage();
    }
  }
}
