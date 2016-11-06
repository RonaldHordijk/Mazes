namespace Mazes
{
  public class TriangleCell : Cell
  {
    public TriangleCell(int row, int column) : base(row, column)
    {
    }

    public override IReadOnlyCellCollection Neighbors
    {
      get
      {
        CellCollection neighbors = new CellCollection();

        if (this.East != null)
          neighbors.Add(this.East);

        if (this.West != null)
          neighbors.Add(this.West);

        if (!this.Upright() && (this.North != null))
          neighbors.Add(this.North);

        if (this.Upright() && (this.South != null))
          neighbors.Add(this.South);

        return neighbors as IReadOnlyCellCollection;
      }
    }

    public bool Upright()
    {
      return (this.Row + this.Column) % 2 == 0;
    }
  }
}
