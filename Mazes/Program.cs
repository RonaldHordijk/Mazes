using System;
using System.Drawing;
using System.Drawing.Imaging;

[assembly: CLSCompliant(true)]

namespace Mazes
{
  public static class Program
  {
    public static void Main()
    {
      // RunTest();
      // BuildMasked();
      // BuildPolar();
      // BuildHex();
      // BuildTriangle();
      BuildUpsilon();

      Console.ReadLine();
    }

    private static void BuildUpsilon()
    {
      Console.WriteLine("Upsilon");

      var grid = UpsilonGrid.CreateUpsilonGrid(20, 20);

      var mazeBuilder = new RecursiveBacktracker();
      mazeBuilder.Build(grid);

      var distances = Distances.Build(grid.GetCell(0, 0));

      var gd = GridDisplayFactory.GetDisplayForGrid(grid);
      var bitmap = gd.MakeImage(distances);
      bitmap.Save("Upsilon.png", ImageFormat.Png);
      Console.WriteLine("Done");
    }

    private static void BuildTriangle()
    {
      Console.WriteLine("Triangle");

      var grid = TriangleGrid.CreateTriangleGrid(30, 50);

      var mazeBuilder = new RecursiveBacktracker();
      mazeBuilder.Build(grid);

      var distances = Distances.Build(grid.GetCell(0, 0));

      var gd = GridDisplayFactory.GetDisplayForGrid(grid);
      var bitmap = gd.MakeImage(distances);
      bitmap.Save("Triangle.png", ImageFormat.Png);
      Console.WriteLine("Done");
    }

    private static void BuildHex()
    {
      Console.WriteLine("Hex");

      var grid = HexGrid.CreateHexGrid(20, 20);

      var mazeBuilder = new RecursiveBacktracker();
      mazeBuilder.Build(grid);

      var distances = Distances.Build(grid.GetCell(0, 0));

      var gd = GridDisplayFactory.GetDisplayForGrid(grid);
      var bitmap = gd.MakeImage(distances);
      bitmap.Save("Hex.png", ImageFormat.Png);
      Console.WriteLine("Done");
    }

    private static void BuildPolar()
    {
      Console.WriteLine("Polar");

      var grid = PolarGrid.CreatePolarGrid(40);

      var mazeBuilder = new RecursiveBacktracker();
      mazeBuilder.Build(grid);

      var distances = Distances.Build(grid.GetCell(0, 0));

      var gd = GridDisplayFactory.GetDisplayForGrid(grid);
      var bitmap = gd.MakeImage(distances);
      bitmap.Save("Polar.png", ImageFormat.Png);
      Console.WriteLine("Done");
    }

    private static void BuildMasked()
    {
      Grid grid = MaskedGridBuilder.MakeCircle(13, 23);

      var mazeBuilder = new AldousBorderWilsons();
      mazeBuilder.Build(grid);

      Console.WriteLine(GridTextConversion.ToText(grid));
      var gd = GridDisplayFactory.GetDisplayForGrid(grid);
      var bitmap = gd.MakeImage();
      bitmap.Save("mask.png", ImageFormat.Png);
    }

    private static void RunTest()
    {
      foreach (IMazeBuilder builder in MazeBuilders.Builders)
      {
        Test(builder, 20, 20, 100);
      }
    }

    private static void Test(IMazeBuilder builder, int rows, int columns, int samples)
    {
      if (builder == null)
        return;

      Console.WriteLine("{0}x{1}: {2}", rows, columns, builder);

      int sumPathLength = 0;
      int sumDeadEnds = 0;
      for (int i = 0; i < samples; i++)
      {
        var grid = Grid.CreateGrid(rows, columns);
        builder.Build(grid);

        Distances longestPath = Distances.LongestPath(grid);
        sumPathLength += longestPath.MaxDistance().Value;

        Distances deadEnds = Distances.DeadEnds(grid);
        sumDeadEnds += deadEnds.Count;
      }

      Console.WriteLine("Longest Path: {0}", (double)sumPathLength / samples);
      Console.WriteLine("Nr Dead Ends: {0}", (double)sumDeadEnds / samples);
    }
  }
}
