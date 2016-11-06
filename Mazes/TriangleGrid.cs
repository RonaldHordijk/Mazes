namespace Mazes
{
  public class TriangleGrid : Grid
  {
    protected TriangleGrid(int rows, int columns) : base(rows, columns)
    {
    }

    public static TriangleGrid CreateTriangleGrid(int rows, int columns)
    {
      var grid = new TriangleGrid(rows, columns);

      grid.Init();

      return grid;
    }

    public static TriangleGrid CreateTriangleGrid(int size)
    {
      var width = (2 * size) - 1;
      var height = size;
      if (size % 2 == 0)
        height++;

      var grid = new TriangleGrid(height, width);

      grid.Init();

      // Disable cells
      for (int row = 0; row < height; row++)
      {
        var end = height - 1 - row;

        for (int col = 0; col < end; col++)
        {
          grid.SetCell(row, col, null);
          grid.SetCell(row, width - col - 1, null);
        }
      }

      grid.ConfigureCells();

      return grid;
    }

    protected override void PrepareGrid()
    {
      for (int row = 0; row < this.Rows; row++)
      {
        for (int column = 0; column < this.Columns; column++)
        {
          this.SetCell(row, column, new TriangleCell(row, column));
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

        var triangleCell = cell as TriangleCell;

        triangleCell.West = this.GetCell(row, column - 1);
        triangleCell.East = this.GetCell(row, column + 1);

        if (triangleCell.Upright())
        {
          triangleCell.South = this.GetCell(row + 1, column);
        }
        else
        {
          triangleCell.North = this.GetCell(row - 1, column);
        }
      }
    }
  }
}
