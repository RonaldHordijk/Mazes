namespace Mazes
{
  using System.Collections.Generic;

  public class PolarCell : Cell
  {
    public PolarCell(int row, int column) : base(row, column)
    {
    }

    public Cell Clockwise
    {
      get;
      set;
    }

    public Cell Counterclockwise
    {
      get;
      set;
    }

    public Cell Inward
    {
      get;
      set;
    }

    public ICollection<PolarCell> Outward
    {
      get;
    } = new List<PolarCell>();

    public override IReadOnlyCellCollection Neighbors
    {
      get
      {
        CellCollection neighbors = new CellCollection();
        if (this.Clockwise != null)
          neighbors.Add(this.Clockwise);

        if (this.Counterclockwise != null)
          neighbors.Add(this.Counterclockwise);

        if (this.Inward != null)
          neighbors.Add(this.Inward);

        neighbors.AddRange(this.Outward);

        return neighbors as IReadOnlyCellCollection;
      }
    }
  }
}
