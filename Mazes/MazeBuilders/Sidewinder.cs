namespace Mazes
{
  using System;

  public class Sidewinder : IMazeBuilder
  {
    private static Random rnd = new Random();

    public void Build(Grid grid)
    {
      if (grid == null)
        return;

      var run = new CellCollection();
      foreach (Cell cell in grid.GetCells())
      {
        ProcessCell(run, cell);
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

      ProcessCell(stepInfo.Path, stepInfo.CurrentCell);
      return true;
    }

    public override string ToString()
    {
      return "SideWinder";
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

    private static void ProcessCell(CellCollection run, Cell cell)
    {
      if ((run == null) || (cell == null))
        return;

      run.Add(cell);

      bool atEasterBoundary = cell.East == null;
      bool atNorthernBoundary = cell.North == null;

      bool shouldCloseOut = atEasterBoundary ||
        (!atNorthernBoundary && rnd.Next(2) == 0);

      if (shouldCloseOut)
      {
        Cell member = run.Sample();
        if (member.North != null)
          member.Link(member.North);

        run.Clear();
      }
      else
      {
        cell.Link(cell.East);
      }
    }
  }
}
