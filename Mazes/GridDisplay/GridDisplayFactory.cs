namespace Mazes
{
  public static class GridDisplayFactory
  {
    public static GridDisplay GetDisplayForGrid(Grid grid)
    {
      GridDisplay result = null;

      result = new SquareDisplay();

      if (grid is PolarGrid)
        result = new PolarDisplay();

      if (grid is HexGrid)
        result = new HexDisplay();

      if (grid is TriangleGrid)
        result = new TriangleDisplay();

      if (grid is UpsilonGrid)
        result = new UpsilonDisplay();

      if (grid is WeaveGrid)
        result = new WeaveDisplay();

      result.InitGrid(grid);
      return result;
    }

    public static GridDisplay GetDisplayForGridStep(GridStepInfo gridStepInfo)
    {
      if (gridStepInfo == null)
        return null;

      var result = GetDisplayForGrid(gridStepInfo.Grid);
      result.InitGridStepInfo(gridStepInfo);
      return result;
    }
  }
}
