namespace Mazes
{
  public class UpsilonGrid : Grid
  {
    protected UpsilonGrid(int rows, int columns) : base(rows, columns)
    {
    }

    public static UpsilonGrid CreateUpsilonGrid(int rows, int columns)
    {
      var grid = new UpsilonGrid(rows, columns);

      grid.Init();

      return grid;
    }

    protected override void PrepareGrid()
    {
      for (int row = 0; row < this.Rows; row++)
      {
        for (int column = 0; column < this.Columns; column++)
        {
          this.SetCell(row, column, new UpsilonCell(row, column));
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

        var upsilonCell = cell as UpsilonCell;

        upsilonCell.North = this.GetCell(row - 1, column);
        upsilonCell.South = this.GetCell(row + 1, column);
        upsilonCell.East = this.GetCell(row, column + 1);
        upsilonCell.West = this.GetCell(row, column - 1);

        if ((row + column) % 2 == 0)
        {
          upsilonCell.Northeast = this.GetCell(row - 1, column + 1);
          upsilonCell.Northwest = this.GetCell(row - 1, column - 1);
          upsilonCell.Southeast = this.GetCell(row + 1, column + 1);
          upsilonCell.Southwest = this.GetCell(row + 1, column - 1);
        }
      }
    }
  }
}
