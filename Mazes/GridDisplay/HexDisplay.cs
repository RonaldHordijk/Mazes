namespace Mazes
{
  using System;
  using System.Drawing;
  using System.Drawing.Drawing2D;

  public class HexDisplay : GridDisplay
  {
    public override void CalculateCellSize(int width, int height)
    {
      if (this.Grid == null)
        return;

      double cellWidth = (1.0 * width - 1.0) / ((1.5 * this.Grid.Columns) + 0.5);
      double cellHeight = (1.0 * height - 1.0) / ((this.Grid.Rows + 0.5) * Math.Sqrt(3));
      this.CellSize = Math.Min(cellWidth, cellHeight);
    }

    public override int CalculateImageWidth()
    {
      if (this.Grid == null)
        return 0;

      double halfWidth = this.CellSize / 2.0;
      return Convert.ToInt32(3 * Grid.Columns * halfWidth + halfWidth + 1);
    }

    public override int CalculateImageHeight()
    {
      if (this.Grid == null)
        return 0;

      double halfHeight = 0.5 * this.CellSize * Math.Sqrt(3);
      return Convert.ToInt32(2 * halfHeight * this.Grid.Rows + halfHeight + 1);
    }

    public override Bitmap MakeImage(Distances distances)
    {
      if (this.Grid == null)
        return null;

      double aSize = this.CellSize / 2.0;
      double bSize = this.CellSize * Math.Sqrt(3) / 2.0;
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

        var cx = this.CellSize + 3 * aSize * cell.Column;
        var cy = bSize + 2 * bSize * cell.Row;
        if (cell.Column % 2 != 0)
          cy += bSize;

        var x_fw = Convert.ToInt32(cx - this.CellSize);
        var x_nw = Convert.ToInt32(cx - aSize);
        var x_ne = Convert.ToInt32(cx + aSize);
        var x_fe = Convert.ToInt32(cx + this.CellSize);

        var y_n = Convert.ToInt32(cy - bSize);
        var y_m = Convert.ToInt32(cy);
        var y_s = Convert.ToInt32(cy + bSize);

        Brush brush = this.GetCellBrush(distances, cell);
        Point[] points = new Point[]
        {
          new Point { X = x_fw, Y = y_m },
          new Point { X = x_nw, Y = y_n },
          new Point { X = x_ne, Y = y_n },
          new Point { X = x_fe, Y = y_m },
          new Point { X = x_ne, Y = y_s },
          new Point { X = x_nw, Y = y_s },
        };

        g.FillPolygon(brush, points);
        Pen pen = this.GetCellPen(distances, cell);
        g.DrawPolygon(pen, points);
      }

      foreach (Cell cell in this.Grid.GetCells())
      {
        if (cell == null)
          continue;

        var cx = this.CellSize + 3 * aSize * cell.Column;
        var cy = bSize + 2 * bSize * cell.Row;
        if (cell.Column % 2 != 0)
          cy += bSize;

        var x_fw = Convert.ToInt32(cx - this.CellSize);
        var x_nw = Convert.ToInt32(cx - aSize);
        var x_ne = Convert.ToInt32(cx + aSize);
        var x_fe = Convert.ToInt32(cx + this.CellSize);

        var y_n = Convert.ToInt32(cy - bSize);
        var y_m = Convert.ToInt32(cy);
        var y_s = Convert.ToInt32(cy + bSize);

        var hexCell = cell as HexCell;

        Pen wallPen = new Pen(this.WallColor);
        if (hexCell.Southwest == null)
          g.DrawLine(wallPen, x_fw, y_m, x_nw, y_s);

        if (hexCell.Northwest == null)
          g.DrawLine(wallPen, x_fw, y_m, x_nw, y_n);

        if (hexCell.North == null)
          g.DrawLine(wallPen, x_nw, y_n, x_ne, y_n);

        if (!hexCell.IsLinked(hexCell.Northeast))
          g.DrawLine(wallPen, x_ne, y_n, x_fe, y_m);

        if (!hexCell.IsLinked(hexCell.Southeast))
          g.DrawLine(wallPen, x_fe, y_m, x_ne, y_s);

        if (!hexCell.IsLinked(hexCell.South))
          g.DrawLine(wallPen, x_ne, y_s, x_nw, y_s);
      }

      return bmp;
    }
  }
}
