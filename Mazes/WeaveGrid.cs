namespace Mazes
{
  public class WeaveGrid : Grid
  {
    private CellCollection underCells = new CellCollection();

    protected WeaveGrid(int rows, int columns) : base(rows, columns)
    {
    }

    public static WeaveGrid CreateWeaveGrid(int rows, int columns)
    {
      var grid = new WeaveGrid(rows, columns);

      grid.Init();

      return grid;
    }

    public void CreateUnderCell(OverCell cell)
    {
      this.underCells.Add(UnderCell.CreateUnderCell(cell));
    }

    public override Cell[] GetCells()
    {
      var cells = new Cell[this.Size() + this.underCells.Count];

      base.GetCells().CopyTo(cells, 0);

      for (var i = 0; i < this.underCells.Count; i++)
      {
        cells[this.Size() + i] = this.underCells[i];
      }

      return cells;
    }

    protected override void PrepareGrid()
    {
      for (int row = 0; row < this.Rows; row++)
      {
        for (int column = 0; column < this.Columns; column++)
        {
          this.SetCell(row, column, new OverCell(row, column, this.CreateUnderCell));
        }
      }
    }
  }
}
