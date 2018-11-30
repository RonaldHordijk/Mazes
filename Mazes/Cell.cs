namespace Mazes
{
  public class Cell
  {
    private readonly CellCollection _links;

    public Cell(int row, int column)
    {
      Row = row;
      Column = column;

      _links = new CellCollection();
    }

    public IReadOnlyCellCollection Links => _links as IReadOnlyCellCollection;

    public int Row { get; }

    public int Column { get; }

    public Cell North { get; set; }

    public Cell East { get; set; }

    public Cell West { get; set; }

    public Cell South { get; set; }

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

    public IReadOnlyCellCollection GetUnusedNeighbors()
    {
      CellCollection unusedNeighbors = new CellCollection();

      foreach (Cell cell in this.Neighbors)
      {
        if (!cell.IsUsed)
          unusedNeighbors.Add(cell);
      }

      return unusedNeighbors as IReadOnlyCellCollection;
    }

    public IReadOnlyCellCollection GetUsedNeighbors()
    {
      CellCollection usedNeighbors = new CellCollection();

      foreach (Cell cell in Neighbors)
      {
        if (cell.IsUsed)
          usedNeighbors.Add(cell);
      }

      return usedNeighbors as IReadOnlyCellCollection;
    }

    public virtual void Link(Cell cell, bool bidi = true)
    {
      if (cell == null)
        return;

      _links.Add(cell);
      if (bidi)
        cell.Link(this, false);
    }

    public void Unlink(Cell cell, bool bidi = true)
    {
      if (cell == null)
        return;

      _links.Remove(cell);
      if (bidi)
        cell.Unlink(this, false);
    }

    public bool IsLinked(Cell cell) => _links.Contains(cell);

    public override string ToString() => Row.ToString() + " " + Column.ToString();

    private bool IsUsed => _links.Count != 0;
  }
}