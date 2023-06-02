using Microsoft.Xna.Framework;

namespace StarDefenderss;

public interface IOperator
{
    Vector2 Pos { get; set; }
    bool IsSpawned { get; set; }
    Grid _grid { get; set; }
    
    int Currency { get; set; }
    void Update(GameTime gameTime);

    void ActivUltimate();
}