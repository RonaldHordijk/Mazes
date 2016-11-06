namespace Mazes
{
  using System;
  using System.Linq;

  public class Mask
  {
    private bool[] bits;

    public Mask(int rows, int columns)
    {
      this.Rows = rows;
      this.Columns = columns;
      this.bits = Enumerable.Repeat(true, rows * columns).ToArray(); // initialze array with true
    }

    public int Columns
    {
      get;
      private set;
    }

    public int Rows
    {
      get;
      private set;
    }

    public bool IsMasked(int row, int column)
    {
      if ((row < 0) || (row >= this.Rows))
        return true;

      if ((column < 0) || (column >= this.Columns))
        return true;

      return !this.bits[(row * this.Columns) + column];
    }

    public void SetMask(int row, int column, bool value)
    {
      if ((row < 0) || (row >= this.Rows))
        return;

      if ((column < 0) || (column >= this.Columns))
        return;

      this.bits[(row * this.Columns) + column] = value;
    }

    public int Count()
    {
      int cnt = 0;
      foreach (bool value in this.bits)
      {
        if (value)
          cnt++;
      }

      return cnt;
    }

    public void RandomLocation(out int row, out int column)
    {
      while (true)
      {
        Random rnd = new Random();

        row = rnd.Next(this.Rows);
        column = rnd.Next(this.Columns);

        if (!this.IsMasked(row, column))
          return;
      }
    }
  }
}
