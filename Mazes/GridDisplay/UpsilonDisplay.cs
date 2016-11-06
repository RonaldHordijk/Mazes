namespace Mazes
{
  using System;
  using System.Drawing;
  using System.Drawing.Drawing2D;

  public class UpsilonDisplay : GridDisplay
  {
    public override void CalculateCellSize(int width, int height)
    {
      if (this.Grid == null)
        return;

      double cellWidth = ((1.0 * width) - 1.0) / (this.Grid.Columns + 0.7);
      double cellHeight = ((1.0 * height) - 1.0) / (this.Grid.Rows + 0.7);
      this.CellSize = Math.Min(cellWidth, cellHeight);
    }

    public override int CalculateImageWidth()
    {
      if (this.Grid == null)
        return 0;

      var offset = this.CellSize / 3.0;
      return Convert.ToInt32((this.CellSize * this.Grid.Columns) + offset + 1);
    }

    public override int CalculateImageHeight()
    {
      if (this.Grid == null)
        return 0;

      var offset = this.CellSize / 3.0;
      return Convert.ToInt32((this.CellSize * this.Grid.Rows) + offset + 1);
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
      g.SmoothingMode = SmoothingMode.HighQuality;

      foreach (Cell cell in this.Grid.GetCells())
      {
        if (cell != null)
          this.DrawCellBackGround(g, cell, distances);
      }

      foreach (Cell cell in this.Grid.GetCells())
      {
        if (cell == null)
          continue;

        if ((cell.Row + cell.Column) % 2 == 0)
          this.DrawBigCellContour(g, cell);
        else
          this.DrawSmallCellContour(g, cell);
      }

      return bmp;
    }

    private void DrawCellBackGround(Graphics g, Cell cell, Distances distances)
    {
      var points = this.GetPoints(cell);
      Brush brush = this.GetCellBrush(distances, cell);
      g.FillPolygon(brush, points);
      Pen pen = this.GetCellPen(distances, cell);
      g.DrawPolygon(pen, points);
    }

    private void DrawBigCellContour(Graphics g, Cell cell)
    {
      Pen wallPen = new Pen(this.WallColor);
      var points = this.GetPoints(cell);
      var upsilonCell = cell as UpsilonCell;

      if (upsilonCell.Northeast == null)
        g.DrawLine(wallPen, points[1].X, points[1].Y, points[2].X, points[2].Y);

      if (upsilonCell.North == null)
        g.DrawLine(wallPen, points[0].X, points[0].Y, points[1].X, points[1].Y);

      if (upsilonCell.Northwest == null)
        g.DrawLine(wallPen, points[0].X, points[0].Y, points[7].X, points[7].Y);

      if (upsilonCell.West == null)
        g.DrawLine(wallPen, points[6].X, points[6].Y, points[7].X, points[7].Y);

      if (!upsilonCell.IsLinked(upsilonCell.East))
        g.DrawLine(wallPen, points[2].X, points[2].Y, points[3].X, points[3].Y);

      if (!upsilonCell.IsLinked(upsilonCell.Southeast))
        g.DrawLine(wallPen, points[3].X, points[3].Y, points[4].X, points[4].Y);

      if (!upsilonCell.IsLinked(upsilonCell.South))
        g.DrawLine(wallPen, points[4].X, points[4].Y, points[5].X, points[5].Y);

      if (!upsilonCell.IsLinked(upsilonCell.Southwest))
        g.DrawLine(wallPen, points[5].X, points[5].Y, points[6].X, points[6].Y);
    }

    private void DrawSmallCellContour(Graphics g, Cell cell)
    {
      Pen wallPen = new Pen(this.WallColor);
      var points = this.GetPoints(cell);

      if (cell.North == null)
        g.DrawLine(wallPen, points[0].X, points[0].Y, points[1].X, points[1].Y);

      if (cell.West == null)
        g.DrawLine(wallPen, points[0].X, points[0].Y, points[3].X, points[3].Y);

      if (!cell.IsLinked(cell.East))
        g.DrawLine(wallPen, points[2].X, points[2].Y, points[1].X, points[1].Y);

      if (!cell.IsLinked(cell.South))
        g.DrawLine(wallPen, points[2].X, points[2].Y, points[3].X, points[3].Y);
    }

    private PointF[] GetPoints(Cell cell)
    {
      var offset = this.CellSize / 3.0;

      var cx = offset + (cell.Column * this.CellSize);
      var cy = offset + (cell.Row * this.CellSize);

      if ((cell.Row + cell.Column) % 2 == 0)
      {
        return new PointF[] {
          new PointF { X = Convert.ToSingle(cx), Y = Convert.ToSingle(cy - offset) },
          new PointF { X = Convert.ToSingle(cx + this.CellSize - offset), Y = Convert.ToSingle(cy - offset) },
          new PointF { X = Convert.ToSingle(cx + this.CellSize), Y = Convert.ToSingle(cy) },
          new PointF { X = Convert.ToSingle(cx + this.CellSize), Y = Convert.ToSingle(cy + this.CellSize - offset) },
          new PointF { X = Convert.ToSingle(cx + this.CellSize - offset), Y = Convert.ToSingle(cy + this.CellSize) },
          new PointF { X = Convert.ToSingle(cx), Y = Convert.ToSingle(cy + this.CellSize) },
          new PointF { X = Convert.ToSingle(cx - offset), Y = Convert.ToSingle(cy + this.CellSize - offset) },
          new PointF { X = Convert.ToSingle(cx - offset), Y = Convert.ToSingle(cy) },
        };
      }

      return new PointF[] {
        new PointF { X = Convert.ToSingle(cx), Y = Convert.ToSingle(cy) },
        new PointF { X = Convert.ToSingle(cx + this.CellSize - offset), Y = Convert.ToSingle(cy) },
        new PointF { X = Convert.ToSingle(cx + this.CellSize - offset), Y = Convert.ToSingle(cy + this.CellSize - offset) },
        new PointF { X = Convert.ToSingle(cx), Y = Convert.ToSingle(cy + this.CellSize - offset) },
      };
    }
  }
}
