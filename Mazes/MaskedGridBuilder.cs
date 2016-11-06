namespace Mazes
{
  using System;
  using System.Drawing;
  using System.IO;

  public static class MaskedGridBuilder
  {
    public static MaskedGrid MakeCircle(int rows, int columns)
    {
      var mask = new Mask(rows, columns);

      double borderSqr = Math.Pow(0.5 * Math.Min(rows - 1, columns - 1), 2);

      for (int i = 0; i < rows; i++)
      {
        for (int j = 0; j < columns; j++)
        {
          var rdist = i - (0.5 * rows) + 0.5;
          var cdist = j - (0.5 * columns) + 0.5;

          double distSqr = (rdist * rdist) + (cdist * cdist);
          if (distSqr > borderSqr)
            mask.SetMask(i, j, false);
        }
      }

      return MaskedGrid.CreateGrid(mask);
    }

    public static MaskedGrid FromTextFile(string fileName)
    {
      var lines = File.ReadAllLines(fileName);

      int rows = lines.Length;
      int columns = lines[0].Length;

      var mask = new Mask(rows, columns);

      for (int i = 0; i < rows; i++)
      {
        for (int j = 0; j < columns; j++)
        {
         if (lines[i][j] == 'X')
           mask.SetMask(i, j, false);
        }
      }

      return MaskedGrid.CreateGrid(mask);
    }

    public static MaskedGrid FromImage(string fileName)
    {
      var image = Image.FromFile(fileName);
      Bitmap bitmap = new Bitmap(image);

      int rows = bitmap.Height;
      int columns = bitmap.Width;

      var mask = new Mask(rows, columns);

      for (int i = 0; i < rows; i++)
      {
        for (int j = 0; j < columns; j++)
        {
          if (bitmap.GetPixel(j, i).GetBrightness() < 0.1)
            mask.SetMask(i, j, false);
        }
      }

      return MaskedGrid.CreateGrid(mask);
    }
  }
}
