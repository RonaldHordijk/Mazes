namespace Mazes
{
  public class UnderCell : Cell
  {
    public UnderCell(int row, int column) : base(row, column)
    {
    }

    public static UnderCell CreateUnderCell(OverCell overCell)
    {
      if (overCell == null)
        return null;

      var underCell = new UnderCell(overCell.Row, overCell.Column);

      if (overCell.IsHorizontalPassage())
      {
        underCell.North = overCell.North;
        overCell.North.South = underCell;
        underCell.South = overCell.South;
        overCell.South.North = underCell;

        underCell.Link(underCell.North);
        underCell.Link(underCell.South);
      }
      else
      {
        underCell.East = overCell.East;
        overCell.East.West = underCell;
        underCell.West = overCell.West;
        overCell.West.East = underCell;

        underCell.Link(underCell.East);
        underCell.Link(underCell.West);
      }

      return underCell;
    }

    public bool IsHorizontalPassage()
    {
      return (this.East != null) || (this.West != null);
    }

    public bool IsVerticalPassage()
    {
      return (this.North != null) || (this.South != null);
    }
  }
}
