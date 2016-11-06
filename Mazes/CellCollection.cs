namespace Mazes
{
  using System;
  using System.Collections.Generic;

  public interface IReadOnlyCellCollection : IReadOnlyCollection<Cell>
  {
    bool IsEmpty();

    Cell Sample();
  }

  public interface ICellCollection : ICollection<Cell>
  {
    bool IsEmpty();

    Cell Sample();
  }

  public class CellCollection : List<Cell>, ICellCollection, IReadOnlyCellCollection
  {
    private static Random random = new Random();

    public bool IsEmpty()
    {
      return this.Count == 0;
    }

    public Cell Sample()
    {
      if (this.IsEmpty())
        return null;

      return this[random.Next(this.Count)];
    }
  }
}
