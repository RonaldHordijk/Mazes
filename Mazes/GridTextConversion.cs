namespace Mazes
{
  using System;

  public static class GridTextConversion
  {
    public static Grid FromText(string definition)
    {
      if (string.IsNullOrEmpty(definition))
        return null;

      var lines = definition.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

      if ((lines.Length < 2) || ((lines.Length - 1) % 2 != 0))
        return null;

      var rows = lines.Length / 2;

      // all lines have the same lenght
      int length = lines[0].Length;
      for (int i = 1; i < lines.Length; i++)
      {
        if (length != lines[i].Length)
        {
          return null;
        }
      }

      if ((length < 3) || ((length - 1) % 4 != 0))
        return null;

      int columns = length / 4;

      var grid = Grid.CreateGrid(rows, columns);

      for (int row = 0; row < rows; row++)
      {
        for (int column = 0; column < columns; column++)
        {
          var cell = grid.GetCell(row, column);
          if (lines[(row * 2) + 1][(column * 4) + 4] == ' ')
            cell.Link(cell.East);
          if (lines[(row * 2) + 2][(column * 4) + 2] == ' ')
            cell.Link(cell.South);
        }
      }

      return grid;
    }

    public static string ToText(Grid grid)
    {
      return ToText(grid, null);
    }

    public static string ToText(Grid grid, Distances distances)
    {
      if (grid == null)
        return string.Empty;

      string output = "+";

      for (int i = 0; i < grid.Columns; i++)
      {
        output += "---+";
      }

      output += "\n";

      for (int r = 0; r < grid.Rows; r++)
      {
        string top = "|";
        string bottom = "+";
        for (int i = 0; i < grid.Columns; i++)
        {
          Cell cell = grid.GetCell(r, i);
          if (cell == null)
            cell = new Cell(-1, -1);

          string cellContent = "   ";
          if ((distances != null) && distances.KnowsCell(cell))
          {
            cellContent = " " + (char)(distances.GetDistance(cell) + 65) + " ";
          }

          top += cellContent + (cell.IsLinked(cell.East) ? " " : "|");
          bottom += (cell.IsLinked(cell.South) ? "   " : "---") + "+";
        }

        output += top + "\n" + bottom + "\n";
      }

      return output;
    }
  }
}
