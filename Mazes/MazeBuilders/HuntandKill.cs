namespace Mazes
{
  public class HuntAndKill : IMazeBuilder
  {
    public void Build(Grid grid)
    {
      if (grid == null)
        return;

      Cell current = grid.RandomCell();

      while (current != null)
      {
        var unvisitedNeighbors = current.UnusedNeighbors;

        if (!unvisitedNeighbors.IsEmpty())
        {
          Cell neighbour = unvisitedNeighbors.Sample();
          current.Link(neighbour);
          current = neighbour;
        }
        else
        {
          current = null;

          foreach (Cell cell in grid.GetCells())
          {
            if (cell == null)
              continue;

            var visitedNeighbors = cell.UsedNeighbors;

            if (cell.Links.IsEmpty() && !visitedNeighbors.IsEmpty())
            {
              current = cell;
              Cell neighbour = visitedNeighbors.Sample();
              current.Link(neighbour);

              break;
            }
          }
        }
      }
    }

    public GridStepInfo StartStep(Grid grid)
    {
      var stepInfo = new HuntAndKillGridStepInfo(grid);
      stepInfo.CurrentCell = grid.RandomCell();
      return stepInfo;
    }

    private Cell LinkToNeighbor(Cell current)
    {
      var unvisitedNeighbors = current.UnusedNeighbors;
      if (unvisitedNeighbors.IsEmpty())
        return null;

      Cell neighbour = unvisitedNeighbors.Sample();
      current.Link(neighbour);
      return neighbour;
    }

    public bool BuildStep(GridStepInfo stepInfo)
    {
      if (stepInfo == null)
        return false;

      // Killing
      if ((stepInfo as HuntAndKillGridStepInfo).IsHunting == false)
      {
        var newCurrent = LinkToNeighbor(stepInfo.CurrentCell);
        if (newCurrent != null)
        {
          stepInfo.CurrentCell = newCurrent;
        }
        else
        {
          (stepInfo as HuntAndKillGridStepInfo).IsHunting = true;
          stepInfo.CurrentCell = null;
        }

        return true;
      }

      // Hunting
      foreach (Cell cell in stepInfo.Grid.GetCells())
      {
        if (cell == null)
          continue;

        var visitedNeighbors = cell.UsedNeighbors;

        if (cell.Links.IsEmpty() && !visitedNeighbors.IsEmpty())
        {
          stepInfo.CurrentCell = cell;
          Cell neighbour = visitedNeighbors.Sample();
          stepInfo.CurrentCell.Link(neighbour);

          return true;
        }
      }

      stepInfo.CurrentCell = null;
      return false;
    }

    public override string ToString()
    {
      return "Hunt and Kill";
    }

    private class HuntAndKillGridStepInfo : GridStepInfo
    {
      public HuntAndKillGridStepInfo(Grid grid) : base(grid)
      {
      }

      public bool IsHunting
      {
        get;
        set;
      } = false;
    }
  }
}