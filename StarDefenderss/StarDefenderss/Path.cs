using Microsoft.Xna.Framework;

namespace StarDefenderss;

public class Path: IObject
{
    public GameObjects ImageId { get; set; }
    public int Currency { get; set; }
    public int UnicId { get; set; }
    public float Scale { get; set; }
    public float Rotation { get; set; }
    public Color Color { get; set; }
    public Vector2 Pos { get; set; }

    public Path()
    {
        Scale = 1f;
    }
    public void Update(GameTime gameTime)
    {
    }

    public Grid _grid { get; set; }
}