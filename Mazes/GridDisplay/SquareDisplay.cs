namespace Mazes
{
  using System;
  using System.Drawing;

  public class SquareDisplay : GridDisplay
  {
    public override void CalculateCellSize(int width, int height)
    {
      if (this.Grid == null)
        return;

      double cellWidth = (1.0 * width - 1.0) / this.Grid.Columns;
      double cellHeight = (1.0 * height - 1.0) / this.Grid.Rows;
      this.CellSize = Math.Min(cellWidth, cellHeight);
    }

    public override int CalculateImageWidth()
    {
      if (this.Grid == null)
        return 0;

      return Convert.ToInt32((this.CellSize * this.Grid.Columns) + 1);
    }

    public override int CalculateImageHeight()
    {
      if (this.Grid == null)
        return 0;

      return Convert.ToInt32((this.CellSize * this.Grid.Rows) + 1);
    }

    public override Bitmap MakeImage(Distances distances)
    {
      if (this.Grid == null)
        return null;

      Bitmap bmp = new Bitmap(
        this.CalculateImageWidth(),
        this.CalculateImageHeight());
      this.FillBackground(bmp);

      Graphics g = Graphics.FromImage(bmp);

      foreach (Cell cell in this.Grid.GetCells())
      {
        if (cell != null)
          this.DrawCellBackground(g, cell, distances);
      }

      foreach (Cell cell in this.Grid.GetCells())
      {
        if (cell != null)
          this.DrawCellContour(g, cell);
      }

      return bmp;
    }

    protected virtual void DrawCellBackground(Graphics graphics, Cell cell, Distances distances)
    {
      if ((graphics == null) || (cell == null))
        return;

      double x1 = cell.Column * this.CellSize;
      double y1 = cell.Row * this.CellSize;

      Brush brush = this.GetCellBrush(distances, cell);
      graphics.FillRectangle(
        brush,
        Convert.ToSingle(x1),
        Convert.ToSingle(y1),
        Convert.ToSingle(this.CellSize),
        Convert.ToSingle(this.CellSize));
    }

    protected virtual void DrawCellContour(Graphics graphics, Cell cell)
    {
      if ((graphics == null) || (cell == null))
        return;

      var x1 = Convert.ToSingle(cell.Column * this.CellSize);
      var y1 = Convert.ToSingle(cell.Row * this.CellSize);
      var x2 = Convert.ToSingle(x1 + this.CellSize);
      var y2 = Convert.ToSingle(y1 + this.CellSize);

      Pen wallPen = new Pen(this.WallColor);

      if (cell.North == null)
        graphics.DrawLine(wallPen, x1, y1, x2, y1);

      if (cell.West == null)
        graphics.DrawLine(wallPen, x1, y1, x1, y2);

      if (!cell.IsLinked(cell.East))
        graphics.DrawLine(wallPen, x2, y1, x2, y2);

      if (!cell.IsLinked(cell.South))
        graphics.DrawLine(wallPen, x1, y2, x2, y2);
    }
  }
}
