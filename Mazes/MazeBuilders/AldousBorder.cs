namespace Mazes
{
  public class AldousBorder : IMazeBuilder
  {
    public void Build(Grid grid)
    {
      if (grid == null)
        return;

      int unvisited = grid.UsedCells() - 1;
      Cell cell = grid.RandomCell();

      while (unvisited > 0)
      {
        bool newCell;
        cell = ProcessCell(cell, out newCell);
        if (newCell)
          unvisited--;
      }
    }

    public GridStepInfo StartStep(Grid grid)
    {
      var stepInfo = new AldousBorderGridStepInfo(grid);
      stepInfo.CurrentCell = grid.RandomCell();

      return stepInfo;
    }

    public bool BuildStep(GridStepInfo stepInfo)
    {
      if (stepInfo == null)
        return false;

      if ((stepInfo as AldousBorderGridStepInfo).Unvisited == 0)
      {
        stepInfo.CurrentCell = null;
        return false;
      }

      bool newCell;
      stepInfo.CurrentCell = ProcessCell(stepInfo.CurrentCell, out newCell);
      if (newCell)
        (stepInfo as AldousBorderGridStepInfo).UsedCell();

      return true;
    }

    public override string ToString()
    {
      return "AldousBorder";
    }

    private static Cell ProcessCell(Cell cell, out bool newCell)
    {
      var neighbors = cell.Neighbors;
      var neighbor = neighbors.Sample();
      newCell = false;

      if (neighbor.Links.IsEmpty)
      {
        cell.Link(neighbor);
        newCell = true;
      }

      return neighbor;
    }

    private class AldousBorderGridStepInfo : GridStepInfo
    {
      public AldousBorderGridStepInfo(Grid grid) : base(grid)
      {
        this.Unvisited = grid.UsedCells() - 1;
      }

      public int Unvisited
      {
        get;
        private set;
      }

      public void UsedCell()
      {
        this.Unvisited = this.Unvisited - 1;
      }
    }
  }
}
