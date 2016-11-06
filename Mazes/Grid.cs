namespace Mazes
{
  using System;
  using System.Collections.Generic;

  public class Grid
  {
    private Cell[] cells;

    protected Grid(int rows, int columns)
    {
      this.Rows = rows;
      this.Columns = columns;
    }

    public virtual int Columns
    {
      get;
      private set;
    }

    public int Rows
    {
      get;
      private set;
    }

    public static Grid CreateGrid(int rows, int columns)
    {
      var grid = new Grid(rows, columns);

      grid.Init();

      return grid;
    }

    public virtual int Size()
    {
      return this.Rows * this.Columns;
    }

    public virtual int UsedCells()
    {
      int count = 0;
      foreach (var cell in this.GetCells())
      {
        if (cell != null)
          count++;
      }

      return count;
    }

    public virtual Cell GetCell(int row, int column)
    {
      if ((row < 0) || (row >= this.Rows))
        return null;

      if ((column < 0) || (column >= this.ColumnSize(row)))
        return null;

      return this.cells[this.Index(row, column)];
    }

    public virtual int ColumnSize(int row)
    {
      return this.Columns;
    }

    public virtual Cell RandomCell()
    {
      Random rnd = new Random();

      while (true)
      {
        int row = rnd.Next(this.Rows);
        int column = rnd.Next(this.Columns);

        var cell = this.GetCell(row, column);
        if (cell != null)
          return cell;
      }
    }

    public virtual Cell[] GetCells()
    {
      return this.cells;
    }

    protected void SetCell(int row, int column, Cell cell)
    {
      this.cells[this.Index(row, column)] = cell;
    }

    protected void Init()
    {
      this.CreateCellBuffer();
      this.PrepareGrid();
      this.ConfigureCells();
    }

    protected virtual void CreateCellBuffer()
    {
      this.cells = new Cell[this.Size()];
    }

    protected virtual void PrepareGrid()
    {
      for (int row = 0; row < this.Rows; row++)
      {
        for (int column = 0; column < this.Columns; column++)
        {
          this.cells[this.Index(row, column)] = new Cell(row, column);
        }
      }
    }

    protected virtual void ConfigureCells()
    {
      foreach (Cell cell in this.cells)
      {
        if (cell == null)
          continue;

        int row = cell.Row;
        int column = cell.Column;

        cell.North = this.GetCell(row - 1, column);
        cell.South = this.GetCell(row + 1, column);
        cell.West = this.GetCell(row, column - 1);
        cell.East = this.GetCell(row, column + 1);
      }
    }

    protected virtual int Index(int row, int column)
    {
      return (row * this.Columns) + column;
    }
  }
}
