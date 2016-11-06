namespace Mazes
{
  public class Wilsons : IMazeBuilder
  {
    public void Build(Grid grid)
    {
      if (grid == null)
        return;

      var unvisited = new CellCollection();
      foreach (Cell cell in grid.GetCells())
      {
        if (cell != null)
          unvisited.Add(cell);
      }

      unvisited.Remove(unvisited.Sample());

      while (!unvisited.IsEmpty())
      {
        Cell cell = unvisited.Sample();
        var path = new CellCollection();
        path.Add(cell);

        while (unvisited.Contains(cell))
        {
          cell = cell.Neighbors.Sample();

          int position = path.IndexOf(cell);
          if (position >= 0)
          {
            path.RemoveRange(position + 1, path.Count - position - 1);
          }
          else
          {
            path.Add(cell);
          }
        }

        for (int i = 0; i < path.Count - 1; i++)
        {
          path[i].Link(path[i + 1]);
          unvisited.Remove(path[i]);
        }
      }
    }

    public GridStepInfo StartStep(Grid grid)
    {
      var stepInfo = new WilsonGridStepInfo(grid);
      this.BuildStep(stepInfo);
      return stepInfo;
    }

    public bool BuildStep(GridStepInfo stepInfo)
    {
      if (stepInfo == null)
        return false;

      if ((stepInfo as WilsonGridStepInfo).Unvisited.IsEmpty())
      {
        stepInfo.CurrentCell = null;
        return false;
      }

      var unvisited = (stepInfo as WilsonGridStepInfo).Unvisited;
      if (stepInfo.Path.Count == 0)
      {
        stepInfo.CurrentCell = unvisited.Sample();
        stepInfo.Path.Add(stepInfo.CurrentCell);
        return true;
      }

      if (unvisited.Contains(stepInfo.CurrentCell))
      {
        stepInfo.CurrentCell = stepInfo.CurrentCell.Neighbors.Sample();
        int position = stepInfo.Path.IndexOf(stepInfo.CurrentCell);
        if (position >= 0)
        {
          stepInfo.Path.RemoveRange(position + 1, stepInfo.Path.Count - position - 1);
        }
        else
        {
          stepInfo.Path.Add(stepInfo.CurrentCell);
        }

        return true;
      }

      for (int i = 0; i < stepInfo.Path.Count - 1; i++)
      {
        stepInfo.Path[i].Link(stepInfo.Path[i + 1]);
        unvisited.Remove(stepInfo.Path[i]);
      }

      stepInfo.Path.Clear();
      return true;
    }

    public override string ToString()
    {
      return "Wilsons";
    }

    private class WilsonGridStepInfo : GridStepInfo
    {
      public WilsonGridStepInfo(Grid grid) : base(grid)
      {
        this.Unvisited = new CellCollection();
        foreach (Cell cell in grid.GetCells())
        {
          if (cell != null)
            this.Unvisited.Add(cell);
        }

        this.Unvisited.Remove(this.Unvisited.Sample());
      }

      public CellCollection Unvisited
      {
        get;
        private set;
      }
    }
  }
}
