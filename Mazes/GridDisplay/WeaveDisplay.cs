namespace Mazes
{
  using System;
  using System.Drawing;

  public class WeaveDisplay : SquareDisplayInlets
  {
    protected override void DrawCellBackground(Graphics graphics, Cell cell, Distances distances)
    {
      if ((graphics == null) || (cell == null))
        return;

      if (cell is OverCell)
      {
        base.DrawCellBackground(graphics, cell, distances);
        return;
      }

      // undercell
      var x1 = Convert.ToSingle(cell.Column * this.CellSize);
      var y1 = Convert.ToSingle(cell.Row * this.CellSize);
      var x2 = Convert.ToSingle(x1 + this.CellSize);
      var y2 = Convert.ToSingle(y1 + this.CellSize);

      var cellOffset = Convert.ToSingle(this.Inlet * this.CellSize);
      var x3 = x1 + cellOffset;
      var x4 = x2 - cellOffset;
      var y3 = y1 + cellOffset;
      var y4 = y2 - cellOffset;

      Brush brush = this.GetCellBrush(distances, cell);

      if ((cell as UnderCell).IsVerticalPassage())
      {
        graphics.FillRectangle(
        brush,
        x3,
        y1,
        Convert.ToSingle(this.CellSize - (2 * cellOffset)),
        cellOffset);

        graphics.FillRectangle(
          brush,
          x3,
          y4,
          Convert.ToSingle(this.CellSize - (2 * cellOffset)),
          cellOffset);
      }
      else
      {
        graphics.FillRectangle(
          brush,
          x1,
          y3,
          cellOffset,
          Convert.ToSingle(this.CellSize - (2 * cellOffset)));

        graphics.FillRectangle(
         brush,
         x4,
         y3,
         cellOffset,
         Convert.ToSingle(this.CellSize - (2 * cellOffset)));
      }
    }

    protected override void DrawCellContour(Graphics graphics, Cell cell)
    {
      if ((graphics == null) || (cell == null))
        return;

      if (cell is OverCell)
      {
        base.DrawCellContour(graphics, cell);
        return;
      }

      // undercell
      var x1 = Convert.ToSingle(cell.Column * this.CellSize);
      var y1 = Convert.ToSingle(cell.Row * this.CellSize);
      var x2 = Convert.ToSingle(x1 + this.CellSize);
      var y2 = Convert.ToSingle(y1 + this.CellSize);

      var cellOffset = Convert.ToSingle(this.Inlet * this.CellSize);
      var x3 = x1 + cellOffset;
      var x4 = x2 - cellOffset;
      var y3 = y1 + cellOffset;
      var y4 = y2 - cellOffset;

      Pen wallPen = new Pen(this.WallColor);

      if ((cell as UnderCell).IsVerticalPassage())
      {
        graphics.DrawLine(wallPen, x3, y3, x3, y1);
        graphics.DrawLine(wallPen, x4, y3, x4, y1);
        graphics.DrawLine(wallPen, x3, y4, x3, y2);
        graphics.DrawLine(wallPen, x4, y4, x4, y2);
      }
      else
      {
        graphics.DrawLine(wallPen, x4, y3, x2, y3);
        graphics.DrawLine(wallPen, x4, y4, x2, y4);
        graphics.DrawLine(wallPen, x3, y3, x1, y3);
        graphics.DrawLine(wallPen, x3, y4, x1, y4);
      }
    }
  }
}
