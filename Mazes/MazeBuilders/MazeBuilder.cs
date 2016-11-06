namespace Mazes
{
  public interface IMazeBuilder
  {
    void Build(Grid grid);

    GridStepInfo StartStep(Grid grid);

    bool BuildStep(GridStepInfo stepInfo);
  }
}
