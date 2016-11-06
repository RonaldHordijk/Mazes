namespace Mazes
{
  public class Cell
  {
    private CellCollection links;

    public Cell(int row, int column)
    {
      this.Row = row;
      this.Column = column;

      this.links = new CellCollection();
    }

    public IReadOnlyCellCollection Links
    {
      get
      {
        return this.links as IReadOnlyCellCollection;
      }
    }

    public int Row
    {
      get;
      private set;
    }

    public int Column
    {
      get;
      private set;
    }

    public Cell North
    {
      get;
      set;
    }

    public Cell East
    {
      get;
      set;
    }

    public Cell West
    {
      get;
      set;
    }

    public Cell South
    {
      get;
      set;
    }

    public virtual IReadOnlyCellCollection Neighbors
    {
      get
      {
        CellCollection neighbors = new CellCollection();
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

    public IReadOnlyCellCollection UnusedNeighbors
    {
      get
      {
        CellCollection unusedNeighbors = new CellCollection();

        foreach (Cell cell in this.Neighbors)
        {
          if (!cell.IsUsed())
            unusedNeighbors.Add(cell);
        }

        return unusedNeighbors as IReadOnlyCellCollection;
      }
    }

    public IReadOnlyCellCollection UsedNeighbors
    {
      get
      {
        CellCollection usedNeighbors = new CellCollection();

        foreach (Cell cell in this.Neighbors)
        {
          if (cell.IsUsed())
            usedNeighbors.Add(cell);
        }

        return usedNeighbors as IReadOnlyCellCollection;
      }
    }

    public virtual void Link(Cell cell, bool bidi = true)
    {
      if (cell == null)
        return;

      this.links.Add(cell);
      if (bidi)
        cell.Link(this, false);
    }

    public void Unlink(Cell cell, bool bidi = true)
    {
      if (cell == null)
        return;

      this.links.Remove(cell);
      if (bidi)
        cell.Unlink(this, false);
    }

    public bool IsLinked(Cell cell)
    {
      return this.links.Contains(cell);
    }

    public override string ToString()
    {
      return this.Row.ToString() + " " + this.Column.ToString();
    }

    private bool IsUsed()
    {
      return this.links.Count != 0;
    }
  }
}
