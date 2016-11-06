namespace Mazes
{
  using System;
  using System.Drawing;
  using System.Drawing.Drawing2D;

  public class PolarDisplay : GridDisplay
  {
    public override void CalculateCellSize(int width, int height)
    {
      if (this.Grid == null)
        return;

      double cellWidth = (1.0 * width - 1.0) / (2.0 * this.Grid.Rows);
      double cellHeight = (1.0 * height - 1.0) / (2.0 * this.Grid.Rows);
      this.CellSize = Math.Min(cellWidth, cellHeight);
    }

    public override int CalculateImageWidth()
    {
      if (this.Grid == null)
        return 0;

      return Convert.ToInt32(2.0 * this.Grid.Rows * this.CellSize + 1);
    }

    public override int CalculateImageHeight()
    {
      return this.CalculateImageWidth();
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

      var center = this.CalculateImageWidth() / 2.0;

      foreach (Cell cell in this.Grid.GetCells())
      {
        if (cell == null)
          continue;

        Brush brush = this.GetCellBrush(distances, cell);

        if (cell.Row == 0)
        {
          g.FillEllipse(
            brush,
            Convert.ToSingle(center - this.CellSize),
            Convert.ToSingle(center - this.CellSize),
            Convert.ToSingle(2 * this.CellSize),
            Convert.ToSingle(2 * this.CellSize));
          continue;
        }

        var theta = 2 * Math.PI / this.Grid.ColumnSize(cell.Row);
        var innerRadius = cell.Row * this.CellSize;
        var outerRadius = innerRadius + this.CellSize;
        var thetaCounterClockWise = cell.Column * theta;
        var thetaClockWise = thetaCounterClockWise + theta;

        var ax = Convert.ToSingle(center + innerRadius * Math.Cos(thetaCounterClockWise));
        var ay = Convert.ToSingle(center + innerRadius * Math.Sin(thetaCounterClockWise));
        var bx = Convert.ToSingle(center + outerRadius * Math.Cos(thetaCounterClockWise));
        var by = Convert.ToSingle(center + outerRadius * Math.Sin(thetaCounterClockWise));
        var cx = Convert.ToSingle(center + innerRadius * Math.Cos(thetaClockWise));
        var cy = Convert.ToSingle(center + innerRadius * Math.Sin(thetaClockWise));
        var dx = Convert.ToSingle(center + outerRadius * Math.Cos(thetaClockWise));
        var dy = Convert.ToSingle(center + outerRadius * Math.Sin(thetaClockWise));

        var path = new GraphicsPath();

        path.StartFigure(); // Start the first figure.
        path.AddArc(
          Convert.ToSingle(center - innerRadius),
          Convert.ToSingle(center - innerRadius),
          Convert.ToSingle(2 * innerRadius),
          Convert.ToSingle(2 * innerRadius),
          Convert.ToSingle(thetaCounterClockWise * 180 / Math.PI),
          Convert.ToSingle(theta * 180 / Math.PI));
        path.AddLine(cx, cy, dx, dy);
        path.AddArc(
          Convert.ToSingle(center - outerRadius),
          Convert.ToSingle(center - outerRadius),
          Convert.ToSingle(2 * outerRadius),
          Convert.ToSingle(2 * outerRadius),
          Convert.ToSingle(thetaClockWise * 180 / Math.PI),
          -Convert.ToSingle(theta * 180 / Math.PI));
        path.AddLine(bx, by, ax, ay);
        path.CloseFigure();

        g.FillPath(brush, path);
        Pen pen = this.GetCellPen(distances, cell);
        g.DrawPath(pen, path);
      }

      Pen wallPen = new Pen(this.WallColor);

      foreach (Cell cell in this.Grid.GetCells())
      {
        if (cell == null)
          continue;

        if (cell.Row == 0)
          continue;

        var theta = 2 * Math.PI / this.Grid.ColumnSize(cell.Row);
        var innerRadius = cell.Row * this.CellSize;
        var outerRadius = innerRadius + this.CellSize;
        var thetaCounterClockWise = cell.Column * theta;
        var thetaClockWise = thetaCounterClockWise + theta;

        var cx = Convert.ToSingle(center + innerRadius * Math.Cos(thetaClockWise));
        var cy = Convert.ToSingle(center + innerRadius * Math.Sin(thetaClockWise));
        var dx = Convert.ToSingle(center + outerRadius * Math.Cos(thetaClockWise));
        var dy = Convert.ToSingle(center + outerRadius * Math.Sin(thetaClockWise));

        if (!cell.IsLinked((cell as PolarCell).Inward))
          g.DrawArc(
            wallPen,
            Convert.ToSingle(center - innerRadius),
            Convert.ToSingle(center - innerRadius),
            Convert.ToSingle(2 * innerRadius),
            Convert.ToSingle(2 * innerRadius),
            Convert.ToSingle(thetaCounterClockWise * 180 / Math.PI),
            Convert.ToSingle(theta * 180 / Math.PI));

        if (!cell.IsLinked((cell as PolarCell).Clockwise))
          g.DrawLine(wallPen, cx, cy, dx, dy);
      }

      g.DrawArc(wallPen, 1, 1, this.CalculateImageWidth() - 1, this.CalculateImageWidth() - 1, 0, 360);

      return bmp;
    }
  }
}
