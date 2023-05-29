using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarDefenderss;

public interface IGameplayView
{
    event EventHandler CycleFinished;
    event EventHandler<EnemyMovedEventArgs> EnemyMoved;
    event EventHandler<CharacterSpawnedEventArgs> CharacterSpawned;

    void LoadGameCycleParameters(Dictionary<int, IObject> Objects, Dictionary<int, IObject> EnemyObjects);

    void LoadCurrencyValue(int currentCurrency);
    void Run();
}
public class EnemyMovedEventArgs : EventArgs
{
    public GameTime GameTime { get; set; } 
}

