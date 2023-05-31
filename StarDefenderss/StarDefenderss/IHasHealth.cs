using Microsoft.Xna.Framework;

namespace StarDefenderss;

public interface IHasHealth
{
    Vector2 Pos { get; }
    int CurrentHealth { get; set; }
    int MaxHealth { get; set; }
    Color _HealthColor { get; }
}