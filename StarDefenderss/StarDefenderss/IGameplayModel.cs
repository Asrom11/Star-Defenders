using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StarDefenderss;

public interface IGameplayModel
{
    event EventHandler<GameplayEventArgs> Updated;
    event EventHandler<EnemyMovedEventArgs> EnemyMoved;
    public int PlayerId { get; set; }
    public Dictionary<int, IObject> Objects { get; set; }
    void Initialize();   
    void Update();

    void MoveEnemy(GameTime gameTime);
}


public class GameplayEventArgs : EventArgs
{
    public Dictionary<int, IObject> Objects { get; set; }   
}