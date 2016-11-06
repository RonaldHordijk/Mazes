namespace MazesTests
{
  using Mazes;
  using NUnit.Framework;

  [TestFixture]
  public class TestCell
  {
    [Test]
    public void TestCreate()
    {
      Cell cell = new Cell(2, 3);

      Assert.AreEqual(2, cell.Row);
      Assert.AreEqual(3, cell.Column);
    }

    [Test]
    public void TestIsLinked()
    {
      Cell cell = new Cell(2, 3);
      Cell other = new Cell(2, 3);

      Assert.IsFalse(cell.IsLinked(other));
      Assert.IsFalse(cell.IsLinked(cell));
      Assert.IsFalse(cell.IsLinked(null));
      Assert.IsFalse(other.IsLinked(cell));
    }

    [Test]
    public void TestLinking()
    {
      Cell cell = new Cell(2, 3);
      Cell other = new Cell(2, 3);

      Assert.IsFalse(cell.IsLinked(other));
      Assert.IsFalse(other.IsLinked(cell));

      cell.Link(other);

      Assert.IsTrue(cell.IsLinked(other));
      Assert.IsTrue(other.IsLinked(cell));
    }

    [Test]
    public void TestLinkingOneDir()
    {
      Cell cell = new Cell(2, 3);
      Cell other = new Cell(2, 3);

      Assert.IsFalse(cell.IsLinked(other));
      Assert.IsFalse(other.IsLinked(cell));

      cell.Link(other, false);

      Assert.IsTrue(cell.IsLinked(other));
      Assert.IsFalse(other.IsLinked(cell));
    }

    [Test]
    public void TestUnLinking()
    {
      Cell cell = new Cell(2, 3);
      Cell other = new Cell(2, 3);

      cell.Link(other);

      Assert.IsTrue(cell.IsLinked(other));
      Assert.IsTrue(other.IsLinked(cell));

      cell.Unlink(other);

      Assert.IsFalse(cell.IsLinked(other));
      Assert.IsFalse(other.IsLinked(cell));
    }
  }
}
