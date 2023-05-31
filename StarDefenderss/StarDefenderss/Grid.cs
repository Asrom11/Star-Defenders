using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StarDefenderss;

public class Grid
{
    private int cellSize;
    private Dictionary<Point, List<IAttackable>> cells;

    public Grid(int cellSize)
    {
        this.cellSize = cellSize;
        this.cells = new Dictionary<Point, List<IAttackable>>();
    }

    public void Clear()
    {
        this.cells.Clear();
    }

    public void Add(IAttackable obj)
    {
        var cell = GetCell(obj.Pos);
        if (!this.cells.TryGetValue(cell, out var objects))
        {
            objects = new List<IAttackable>();
            this.cells.Add(cell, objects);
        }

        objects.Add(obj);
    }

    public IEnumerable<IAttackable> GetNearbyObjects(Vector2 position, float range)
    {
        var minCell = GetCell(new Vector2(position.X - range, position.Y - range));
        var maxCell = GetCell(new Vector2(position.X + range, position.Y + range));
        for (var x = minCell.X; x <= maxCell.X; x++)
        {
            for (var y = minCell.Y; y <= maxCell.Y; y++)
            {
                if (this.cells.TryGetValue(new Point(x, y), out var objects))
                {
                    foreach (var obj in objects)
                    {
                        if (Vector2.Distance(position, obj.Pos) <= range)
                        {
                            yield return obj;
                        }
                    }
                }
            }
        }
    }

    private Point GetCell(Vector2 position)
    {
        return new Point((int)(position.X / this.cellSize), (int)(position.Y / this.cellSize));
    }
}
public interface IAttackable
{
    public Vector2 Pos{ get; set; }
    float AttackRange { get; }
    int Attack { get; }
    void TakeDamage(int damage);

     Grid _grid { get; set; }
}