namespace Mazes
{
  using System.Drawing;

  public class GridDisplay
  {
    public double CellSize
    {
      get;
      set;
    } = 10.0;

    public Color BackgroundColor
    {
      get;
      set;
    } = Color.FromArgb(255, 64, 64, 64);

    public Color WallColor
    {
      get;
      set;
    } = Color.Black;

    public Color DistanceFromColor
    {
      get;
      set;
    } = Color.Black;

    public Color DistanceToColor
    {
      get;
      set;
    } = Color.FromArgb(255, 127, 127, 255);

    public Color StepCurrentCellColor
    {
      get;
      set;
    } = Color.FromArgb(255, 255, 127, 127);

    public Color StepCurrentCellNeighborColor
    {
      get;
      set;
    } = Color.FromArgb(255, 127, 255, 127);

    public Color StepPathColor
    {
      get;
      set;
    } = Color.FromArgb(255, 255, 208, 127);

    protected Grid Grid
    {
      get;
      private set;
    } = null;

    protected GridStepInfo GridStepInfo
    {
      get;
      private set;
    } = null;

    public void InitGrid(Grid grid)
    {
      if (grid == null)
        return;

      this.Grid = grid;
      this.GridStepInfo = null;
    }

    public void InitGridStepInfo(GridStepInfo gridStepInfo)
    {
      if (gridStepInfo == null)
        return;

      this.GridStepInfo = gridStepInfo;
      this.Grid = gridStepInfo.Grid;
    }

    public Bitmap MakeImage()
    {
      return this.MakeImage(null);
    }

    public virtual Bitmap MakeImage(Distances distances)
    {
      return null;
    }

    public virtual void CalculateCellSize(int width, int height)
    {
    }

    public virtual int CalculateImageWidth()
    {
      return 0;
    }

    public virtual int CalculateImageHeight()
    {
      return 0;
    }

    protected Color GetCellColor(Distances distances, Cell cell)
    {
      if ((this.GridStepInfo != null) && (this.GridStepInfo.CurrentCell != null))
      {
        if (cell == this.GridStepInfo.CurrentCell)
          return this.StepCurrentCellColor;
        if (this.GridStepInfo.Path.Contains(cell))
          return this.StepPathColor;
        if (this.GridStepInfo.CurrentCell.IsLinked(cell))
          return this.StepCurrentCellNeighborColor;
      }

      if (distances == null)
        return Color.FromArgb(255, 208, 208, 208);

      if (!distances.KnowsCell(cell))
        return Color.FromArgb(255, 208, 208, 208);

      int dist = distances.GetDistance(cell);
      int maxdist = distances.MaxDistance().Value;

      double intensity = 1.0 * (maxdist - dist) / maxdist;
      byte r = (byte)((this.DistanceFromColor.R * intensity) +
        (this.DistanceToColor.R * (1 - intensity)));
      byte g = (byte)((this.DistanceFromColor.G * intensity) +
        (this.DistanceToColor.G * (1 - intensity)));
      byte b = (byte)((this.DistanceFromColor.B * intensity) +
        (this.DistanceToColor.B * (1 - intensity)));

      return Color.FromArgb(255, r, g, b);
    }

    protected Brush GetCellBrush(Distances distances, Cell cell)
    {
      return new SolidBrush(this.GetCellColor(distances, cell));
    }

    protected Pen GetCellPen(Distances distances, Cell cell)
    {
      return new Pen(this.GetCellColor(distances, cell));
    }

    protected void FillBackground(Image image)
    {
      if (image == null)
        return;

      Graphics g = Graphics.FromImage(image);

      g.FillRectangle(new SolidBrush(this.BackgroundColor), 0, 0, image.Width, image.Height);
    }
  }
}
