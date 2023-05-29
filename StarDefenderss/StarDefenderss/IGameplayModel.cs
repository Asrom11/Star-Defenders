using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StarDefenderss;

public interface IGameplayModel
{
    event EventHandler<GameplayEventArgs> Updated;
    event EventHandler<EnemyMovedEventArgs> EnemyMoved;

    event EventHandler<CurrencyEventArgs> CurrencyChange; 
    public int PlayerId { get; set; }
    
    public int Currency { get; set; } 
    event EventHandler<CharacterSpawnedEventArgs> CharacterSelected;
    public Dictionary<int, IObject> Objects { get; set; }
    public Dictionary<int, IObject> EnemyObjects { get; set; }
    void Initialize();   
    void Update();

    void MoveEnemy(GameTime gameTime);
    void SpawnCharacter(Vector2 position, IObject character);

}


public class GameplayEventArgs : EventArgs
{
    public Dictionary<int, IObject> Objects { get; set; }
    public Dictionary<int, IObject> EnemyObjects { get; set; }
}

public class CurrencyEventArgs : EventArgs
{
    public int Currencys { get; set; }
}