using Microsoft.Xna.Framework;

namespace StarDefenderss;

public interface IObject
{
    GameObjects ImageId { get; set; }
    int UnicId { get; set; }
    float Scale { get; set; }
    float Rotation { get; set; }
    Color Color { get; set;}
    Vector2 Pos { get; set; }
}