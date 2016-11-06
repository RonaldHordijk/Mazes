namespace Mazes
{
  public class HexCell : Cell
  {
    public HexCell(int row, int column) : base(row, column)
    {
    }

    public Cell Northeast
    {
      get;
      set;
    }

    public Cell Northwest
    {
      get;
      set;
    }

    public Cell Southeast
    {
      get;
      set;
    }

    public Cell Southwest
    {
      get;
      set;
    }

    public override IReadOnlyCellCollection Neighbors
    {
      get
      {
        CellCollection neighbors = new CellCollection();
        if (this.Northeast != null)
          neighbors.Add(this.Northeast);

        if (this.Northwest != null)
          neighbors.Add(this.Northwest);

        if (this.Southeast != null)
          neighbors.Add(this.Southeast);

        if (this.Southwest != null)
          neighbors.Add(this.Southwest);

        if (this.North != null)
          neighbors.Add(this.North);

        if (this.South != null)
          neighbors.Add(this.South);

        if (this.East != null)
          neighbors.Add(this.East);

        if (this.West != null)
          neighbors.Add(this.West);

        return neighbors as IReadOnlyCellCollection;
      }
    }
  }
}
