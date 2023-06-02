using Microsoft.Xna.Framework;

namespace StarDefenderss;

public interface IHasBar
{
    Vector2 Position { get; }
    int CurrentHealth { get; set; }
    int MaxHealth { get; set; }
    int CurrentMana { get; set; }

    int MaxMana { get; set; }
    Color _HealthColor { get; }
}