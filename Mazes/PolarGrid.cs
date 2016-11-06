namespace Mazes
{
  using System;

  public class PolarGrid : Grid
  {
    private int[] rowSize;
    private int[] rowOffset;

    protected PolarGrid(int rows) : base(rows, 0)
    {
    }

    public static PolarGrid CreatePolarGrid(int rows)
    {
      var grid = new PolarGrid(rows);

      grid.Init();

      return grid;
    }

    public override Cell RandomCell()
    {
      Random rnd = new Random();

      int row = rnd.Next(this.Rows);
      int column = rnd.Next(this.ColumnSize(row));
      return this.GetCell(row, column);
    }

    public override Cell GetCell(int row, int column)
    {
      return base.GetCell(row, (column + this.ColumnSize(row)) % this.ColumnSize(row));
    }

    public override int ColumnSize(int row)
    {
      return this.rowSize[row];
    }

    public override int Size()
    {
      if (this.rowOffset == null)
        return 0;

      return this.rowOffset[this.Rows];
    }

    protected override void PrepareGrid()
    {
      this.rowSize = new int[Rows];
      this.rowOffset = new int[Rows + 1];

      var rowHeight = 1.0 / Rows;

      this.rowSize[0] = 1;
      this.rowOffset[0] = 0;
      this.rowOffset[1] = 1;

      // just counting
      for (int row = 1; row < this.Rows; row++)
      {
        var radius = (1.0 * row) / Rows;
        var circum = 2.0 * Math.PI * radius;

        var prevCount = this.rowSize[row - 1];
        var cellWidth = circum / prevCount;
        var ratio = Convert.ToInt32(Math.Round(cellWidth / rowHeight));
        this.rowSize[row] = prevCount * ratio;
        this.rowOffset[row + 1] = this.rowOffset[row] + this.rowSize[row];
      }

      // create the cells
      this.CreateCellBuffer();
      for (int row = 0; row < this.Rows; row++)
      {
        for (int col = 0; col < this.rowSize[row]; col++)
        {
          this.SetCell(row, col, new PolarCell(row, col));
        }
      }
    }

    protected override void ConfigureCells()
    {
      foreach (Cell cell in this.GetCells())
      {
        if (cell == null)
          continue;

        int row = cell.Row;
        int column = cell.Column;

        if (row == 0)
          continue;

        var pc = cell as PolarCell;
        pc.Clockwise = this.GetCell(row, column + 1);
        pc.Counterclockwise = this.GetCell(row, column - 1);

        var ratio = (1.0 * this.rowSize[row]) / this.rowSize[row - 1];
        var parent = this.GetCell(row - 1, Convert.ToInt32(Math.Truncate(column / ratio)));
        pc.Inward = parent;
        (parent as PolarCell).Outward.Add(pc);
      }
    }

    protected override int Index(int row, int column)
    {
      return this.rowOffset[row] + column;
    }
  }
}
