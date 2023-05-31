using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StarDefenderss;

public interface IGameplayModel
{
    event EventHandler<GameplayEventArgs> Updated;
    event EventHandler<CurrencyEventArgs> CurrencyChange;
    public int Currency { get; set; }
    public Dictionary<int, IObject> Objects { get; set; }
    void Initialize();   
    void Update(GameTime gameTime);

    void MoveEnemy(GameTime gameTime);
    void SpawnCharacter(Vector2 position, IObject character);

}


public class GameplayEventArgs : EventArgs
{
    public Dictionary<int, IObject> Objects { get; set; }
}

public class CurrencyEventArgs : EventArgs
{
    public int Currencys { get; set; }
}