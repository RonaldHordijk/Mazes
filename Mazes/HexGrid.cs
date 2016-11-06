namespace Mazes
{
  public class HexGrid : Grid
  {
    protected HexGrid(int rows, int columns) : base(rows, columns)
    {
    }

    public static HexGrid CreateHexGrid(int rows, int columns)
    {
      var grid = new HexGrid(rows, columns);

      grid.Init();

      return grid;
    }

    public static HexGrid CreateHexGrid(int size)
    {
      var width = (size * 2) - 1;
      var height = (size * 2) - 1;

      var grid = new HexGrid(height, width);

      grid.Init();

      // Disable cells
      for (int col = 0; col < size; col++)
      {
        var end = size - 1 - col;
        if (size % 2 == 0)
          end++;

        for (int j = 0; j < end / 2; j++)
        {
          grid.SetCell(j, col, null);
          grid.SetCell(j, width - col - 1, null);
        }
      }

      for (int col = 0; col < size; col++)
      {
        var end = size - 1 - col;
        if (size % 2 == 1)
          end++;

        for (int j = 0; j < end / 2; j++)
        {
          grid.SetCell(height - j - 1, col, null);
          grid.SetCell(height - j - 1, width - col - 1, null);
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
          this.SetCell(row, column, new HexCell(row, column));
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

        int northDiagonal = row;
        int southDiagonal = row + 1;

        if (column % 2 == 0)
        {
          northDiagonal = row - 1;
          southDiagonal = row;
        }

        var hexCell = cell as HexCell;

        hexCell.North = this.GetCell(row - 1, column);
        hexCell.South = this.GetCell(row + 1, column);
        hexCell.Northeast = this.GetCell(northDiagonal, column + 1);
        hexCell.Northwest = this.GetCell(northDiagonal, column - 1);
        hexCell.Southeast = this.GetCell(southDiagonal, column + 1);
        hexCell.Southwest = this.GetCell(southDiagonal, column - 1);
      }
    }
  }
}
