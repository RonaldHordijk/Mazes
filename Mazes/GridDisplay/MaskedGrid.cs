namespace Mazes
{
  public class MaskedGrid : Grid
  {
    private Mask mask = null;

    private MaskedGrid(Mask mask, int rows, int columns) : base(rows, columns)
    {
      this.mask = mask;
    }

    public static MaskedGrid CreateGrid(Mask mask)
    {
      if (mask == null)
        return null;

      var grid = new MaskedGrid(mask, mask.Rows, mask.Columns);

      grid.Init();

      return grid;
    }

    public override Cell RandomCell()
    {
      int row;
      int column;
      this.mask.RandomLocation(out row, out column);

      return this.GetCell(row, column);
    }

    public override int UsedCells()
    {
      return this.mask.Count();
    }

    protected override void PrepareGrid()
    {
      if (this.mask == null)
        return;

      for (int i = 0; i < this.Rows; i++)
      {
        for (int j = 0; j < this.Columns; j++)
        {
          if (this.mask.IsMasked(i, j))
          {
            this.SetCell(i, j, null);
          }
          else
          {
            this.SetCell(i, j, new Cell(i, j));
          }
        }
      }
    }
  }
}
