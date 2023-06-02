using Microsoft.Xna.Framework;

namespace StarDefenderss;

public interface IOperator: IObject
{
    bool IsSpawned { get; set; }
    Grid _grid { get; set; }
    
    int Currency { get; set; }
    void Update(GameTime gameTime);
    bool isSniper { get; }

    void ActivUltimate();
}