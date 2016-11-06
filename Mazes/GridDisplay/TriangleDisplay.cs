namespace Mazes
{
  using System;
  using System.Drawing;
  using System.Drawing.Drawing2D;

  public class TriangleDisplay : GridDisplay
  {
    public override void CalculateCellSize(int width, int height)
    {
      if (this.Grid == null)
        return;

      double cellWidth = (1.0 * width - 1.0) / (0.5 * this.Grid.Columns + 0.5);
      double cellHeight = (1.0 * height - 1.0) / (0.5 * this.Grid.Rows * Math.Sqrt(3.0));
      this.CellSize = Math.Min(cellWidth, cellHeight);
    }

    public override int CalculateImageWidth()
    {
      if (this.Grid == null)
        return 0;

      return Convert.ToInt32(0.5 * this.CellSize * (this.Grid.Columns + 1) + 1);
    }

    public override int CalculateImageHeight()
    {
      if (this.Grid == null)
        return 0;

      var height = 0.5 * this.CellSize * Math.Sqrt(3.0);
      return Convert.ToInt32(height * this.Grid.Rows + 1);
    }

    public override Bitmap MakeImage(Distances distances)
    {
      if (this.Grid == null)
        return null;

      var height = 0.5 * this.CellSize * Math.Sqrt(3.0);
      var width = this.CellSize;

      Bitmap bmp = new Bitmap(
       this.CalculateImageWidth(),
       this.CalculateImageHeight());
      this.FillBackground(bmp);
      Graphics g = Graphics.FromImage(bmp);
      g.SmoothingMode = SmoothingMode.HighQuality;

      foreach (Cell cell in this.Grid.GetCells())
      {
        if (cell == null)
          continue;

        var cx = 0.5 * width * (cell.Column + 1);
        var cy = 0.5 * height + cell.Row * height;

        var westX = cx - 0.5 * width;
        var midX = cx;
        var eastX = cx + 0.5 * width;

        var apexY = cy + 0.5 * height;
        var baseY = cy - 0.5 * height;

        if ((cell as TriangleCell).Upright())
        {
          apexY = cy - 0.5 * height;
          baseY = cy + 0.5 * height;
        }

        Brush brush = this.GetCellBrush(distances, cell);
        PointF[] points = new PointF[]
        {
          new PointF { X = Convert.ToSingle(westX), Y = Convert.ToSingle(baseY) },
          new PointF { X = Convert.ToSingle(midX),  Y = Convert.ToSingle(apexY) },
          new PointF { X = Convert.ToSingle(eastX), Y = Convert.ToSingle(baseY) }
        };

        g.FillPolygon(brush, points);
        Pen pen = this.GetCellPen(distances, cell);
        g.DrawPolygon(pen, points);
      }

      foreach (Cell cell in this.Grid.GetCells())
      {
        if (cell == null)
          continue;

        var cx = 0.5 * width * (cell.Column + 1);
        var cy = 0.5 * height + cell.Row * height;

        var westX = cx - 0.5 * width;
        var midX = cx;
        var eastX = cx + 0.5 * width;

        var apexY = cy + 0.5 * height;
        var baseY = cy - 0.5 * height;

        if ((cell as TriangleCell).Upright())
        {
          apexY = cy - 0.5 * height;
          baseY = cy + 0.5 * height;
        }

        Pen wallPen = new Pen(this.WallColor);
        if (cell.West == null)
          g.DrawLine(
            wallPen,
            Convert.ToSingle(westX),
            Convert.ToSingle(baseY),
            Convert.ToSingle(midX),
            Convert.ToSingle(apexY));

        if (!cell.IsLinked(cell.East))
          g.DrawLine(
            wallPen,
            Convert.ToSingle(eastX),
            Convert.ToSingle(baseY),
            Convert.ToSingle(midX),
            Convert.ToSingle(apexY));

        var noSouth = (cell as TriangleCell).Upright() && (cell.South == null);
        var notLinked = !(cell as TriangleCell).Upright() && !cell.IsLinked(cell.North);

        if (noSouth || notLinked)
          g.DrawLine(
            wallPen,
            Convert.ToSingle(eastX),
            Convert.ToSingle(baseY),
            Convert.ToSingle(westX),
            Convert.ToSingle(baseY));
      }

      return bmp;
    }
  }
}
