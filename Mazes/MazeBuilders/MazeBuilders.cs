namespace Mazes
{
  using System.Collections.Generic;

  public static class MazeBuilders
  {
    public static IEnumerable<IMazeBuilder> Builders
    {
      get
      {
        IMazeBuilder[] builders = {
          new BinaryTree(),
          new Sidewinder(),
          new AldousBorder(),
          new Wilsons(),
          new AldousBorderWilsons(),
          new HuntAndKill(),
          new RecursiveBacktracker()
        };

        return builders;
      }
    }

    public static IEnumerable<IMazeBuilder> NonSquareBuilders
    {
      get
      {
        IMazeBuilder[] builders = {
          new AldousBorder(),
          new Wilsons(),
          new AldousBorderWilsons(),
          new HuntAndKill(),
          new RecursiveBacktracker()
        };

        return builders;
      }
    }
  }
}
