namespace Mazes
{
  using System.Collections.Generic;

  public class AldousBorderWilsons : IMazeBuilder
  {
    public void Build(Grid grid)
    {
      if (grid == null)
        return;

      int unvisitedCount = grid.UsedCells() - 1;
      int switchPoint = 2 * grid.UsedCells() / 3;
      Cell current = grid.RandomCell();

      // AldousBorder for first third
      while (unvisitedCount > switchPoint)
      {
        var neighbors = current.Neighbors;
        Cell neighbour = neighbors.Sample();

        if (neighbour.Links.IsEmpty())
        {
          current.Link(neighbour);
          unvisitedCount--;
        }

        current = neighbour;
      }

      // and finish with Wilson
      var unvisited = new CellCollection();
      foreach (Cell cell in grid.GetCells())
      {
        if ((cell != null) &&
            cell.Links.IsEmpty())
          unvisited.Add(cell);
      }

      while (!unvisited.IsEmpty())
      {
        Cell cell = unvisited.Sample();
        List<Cell> path = new List<Cell>();
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
      var stepInfo = new GridStepInfo(grid);
      return stepInfo;
    }

    public bool BuildStep(GridStepInfo stepInfo)
    {
      return false;
    }

    public override string ToString()
    {
      return "AldousBorderWilson";
    }
  }
}
