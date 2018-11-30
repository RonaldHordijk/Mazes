namespace Mazes
{
  using System;
  using System.Collections.Generic;

  public interface IReadOnlyCellCollection : IReadOnlyCollection<Cell>
  {
    bool IsEmpty { get; }

    Cell Sample();
  }

  public interface ICellCollection : ICollection<Cell>
  {
    bool IsEmpty { get; }

    Cell Sample();
  }

  public class CellCollection : List<Cell>, ICellCollection, IReadOnlyCellCollection
  {
    private static readonly Random random = new Random();

    public bool IsEmpty => Count == 0;

    public Cell Sample() => IsEmpty ? null : this[random.Next(Count)];
  }
}
