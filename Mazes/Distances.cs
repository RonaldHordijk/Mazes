namespace Mazes
{
  using System.Collections.Generic;
  using System.Linq;

  public class Distances
  {
    private Cell root;
    private Dictionary<Cell, int> cells = new Dictionary<Cell, int>();

    public Distances(Cell cell)
    {
      this.root = cell;
    }

    public static Distances Build(Cell start)
    {
      Distances fullGrid = new Distances(start);
      fullGrid.BuildDistances();

      return fullGrid;
    }

    public static Distances PathTo(Cell start, Cell goal)
    {
      Cell current = goal;

      Distances fullGrid = Build(start);

      Distances breadcrumbs = new Distances(start);
      breadcrumbs.Add(current, fullGrid.GetDistance(current));

      do
      {
        foreach (Cell neighbour in current.Links)
        {
          if (fullGrid.GetDistance(neighbour) < fullGrid.GetDistance(current))
          {
            breadcrumbs.Add(neighbour, fullGrid.GetDistance(neighbour));
            current = neighbour;
            break;
          }
        }
      }
      while (current != start);

      return breadcrumbs;
    }

    public static Distances LongestPath(Grid grid)
    {
      if (grid == null)
        return null;

      Distances distance = Build(grid.RandomCell());
      Distances distanceBack = Build(distance.MaxDistance().Key);
      return PathTo(distance.MaxDistance().Key, distanceBack.MaxDistance().Key);
    }

    public static Distances DeadEnds(Grid grid)
    {
      if (grid == null)
        return null;

      Distances distance = new Distances(null);

      foreach (Cell cell in grid.GetCells())
      {
        if (cell == null)
          continue;

        if (cell.Links.Count == 1)
          distance.Add(cell, 1);
      }

      return distance;
    }

    public KeyValuePair<Cell, int> MaxDistance()
    {
      KeyValuePair<Cell, int> max = this.cells.First();

      foreach (KeyValuePair<Cell, int> item in this.cells)
      {
        if (item.Value > max.Value)
          max = item;
      }

      return max;
    }

    public bool KnowsCell(Cell cell)
    {
      return this.cells.Keys.Contains(cell);
    }

    public int GetDistance(Cell cell)
    {
      int cellValue = 0;
      this.cells.TryGetValue(cell, out cellValue);

      return cellValue;
    }

    public int Count()
    {
      return this.cells.Count();
    }

    private void Clear()
    {
      this.cells.Clear();
    }

    private void Add(Cell cell, int distance)
    {
      this.cells.Add(cell, distance);
    }

    private void BuildDistances()
    {
      this.Clear();
      this.cells.Add(this.root, 0);

      List<Cell> frontier = new List<Cell>();
      frontier.Add(this.root);

      while (frontier.Count != 0)
      {
        List<Cell> newFrontier = new List<Cell>();

        foreach (Cell cell in frontier)
        {
          int nextDistance = this.GetDistance(cell) + 1;

          foreach (Cell linked in cell.Links)
          {
            if (this.cells.ContainsKey(linked))
              continue;

            this.Add(linked, nextDistance);

            newFrontier.Add(linked);
          }
        }

        frontier = newFrontier;
      }
    }
  }
}
