namespace MazesTests
{
  using Mazes;
  using NUnit.Framework;

  [TestFixture]
  public class TestGrid
  {
    [Test]
    public void TestFromGrid()
    {
      var grid = Grid.CreateGrid(3, 4);
      var mazeBuilder = new BinaryTree();
      mazeBuilder.Build(grid);

      var originalDefinition = GridTextConversion.ToText(grid);
      var newGrid = GridTextConversion.FromText(originalDefinition);

      Assert.NotNull(newGrid);

      var newDefinition = GridTextConversion.ToText(newGrid);

      StringAssert.Contains(originalDefinition, newDefinition);
    }
  }
}
