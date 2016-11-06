namespace Mazes
{
  public class GridStepInfo
  {
    public GridStepInfo(Grid grid)
    {
      Grid = grid;
    }

    public Grid Grid
    {
      get;
      private set;
    }

    public Cell CurrentCell
    {
      get;
      set;
    } = null;

    public CellCollection Path
    {
      get;
    } = new CellCollection();
  }
}
