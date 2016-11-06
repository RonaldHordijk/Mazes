namespace Mazes
{
  using System.Collections.Generic;

  public class RecursiveBacktracker : IMazeBuilder
  {
    public void Build(Grid grid)
    {
      if (grid == null)
        return;

      Stack<Cell> stack = new Stack<Cell>();
      stack.Push(grid.RandomCell());

      while (stack.Count != 0)
      {
        Cell current = stack.Peek();

        var unusedNeighbors = current.UnusedNeighbors;
        if (unusedNeighbors.IsEmpty())
        {
          stack.Pop();
        }
        else
        {
          Cell neighbour = unusedNeighbors.Sample();
          current.Link(neighbour);
          stack.Push(neighbour);
        }
      }
    }

    public GridStepInfo StartStep(Grid grid)
    {
      var stepInfo = new RecursiveBacktrackerGridStepInfo(grid);
      stepInfo.CurrentCell = grid.RandomCell();
      stepInfo.Push(stepInfo.CurrentCell);
      return stepInfo;
    }

    public bool BuildStep(GridStepInfo stepInfo)
    {
      if (stepInfo == null)
        return false;

      Cell current = stepInfo.CurrentCell;

      var unusedNeighbors = current.UnusedNeighbors;
      if (unusedNeighbors.IsEmpty())
      {
        (stepInfo as RecursiveBacktrackerGridStepInfo).Pop();
      }
      else
      {
        Cell neighbour = unusedNeighbors.Sample();
        current.Link(neighbour);
        (stepInfo as RecursiveBacktrackerGridStepInfo).Push(neighbour);
      }

      stepInfo.CurrentCell = (stepInfo as RecursiveBacktrackerGridStepInfo).Peek();

      return stepInfo.CurrentCell != null;
    }

    public override string ToString()
    {
      return "Recursive Backtracker";
    }

    private class RecursiveBacktrackerGridStepInfo : GridStepInfo
    {
      public RecursiveBacktrackerGridStepInfo(Grid grid) : base(grid)
      {
      }

      public Cell Pop()
      {
        if (Path.IsEmpty())
          return null;

        var result = Path[Path.Count - 1];
        Path.RemoveAt(Path.Count - 1);
        return result;
      }

      public Cell Peek()
      {
        if (Path.IsEmpty())
          return null;

        return Path[Path.Count - 1];
      }

      public void Push(Cell cell)
      {
        Path.Add(cell);
      }
    }
  }
}
