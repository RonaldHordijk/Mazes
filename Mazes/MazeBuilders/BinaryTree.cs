namespace Mazes
{
  public class BinaryTree : IMazeBuilder
  {
    public void Build(Grid grid)
    {
      if (grid == null)
        return;

      foreach (Cell cell in grid.GetCells())
      {
        ProcessCell(cell);
      }
    }

    public GridStepInfo StartStep(Grid grid)
    {
      var stepInfo = new GridStepInfo(grid);
      this.BuildStep(stepInfo);
      return stepInfo;
    }

    public bool BuildStep(GridStepInfo stepInfo)
    {
      if (stepInfo == null)
        return false;

      if (!GetNextCell(stepInfo))
        return false;

      ProcessCell(stepInfo.CurrentCell);
      return true;
    }

    public override string ToString()
    {
      return "BinaryTree";
    }

    private static bool GetNextCell(GridStepInfo stepInfo)
    {
      var cells = stepInfo.Grid.GetCells();

      // at end
      if ((cells.Length == 0) || (stepInfo.CurrentCell == cells[cells.Length - 1]))
      {
        stepInfo.CurrentCell = null;
        return false;
      }

      if (stepInfo.CurrentCell == null)
      {
        stepInfo.CurrentCell = cells[0];
        return true;
      }

      for (int i = 0; i < cells.Length - 1; i++)
      {
        if (cells[i] == stepInfo.CurrentCell)
        {
          stepInfo.CurrentCell = cells[i + 1];
          return true;
        }
      }

      stepInfo.CurrentCell = null;
      return false;
    }

    private static void ProcessCell(Cell cell)
    {
      var neighbors = new CellCollection();

      if (cell.North != null)
        neighbors.Add(cell.North);

      if (cell.East != null)
        neighbors.Add(cell.East);

      if (neighbors.IsEmpty())
        return;

      cell.Link(neighbors.Sample());
    }
  }
}
