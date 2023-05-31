using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarDefenderss;

public interface IGameplayView
{
    event EventHandler<CycleHasFinished> CycleFinished;
    event EventHandler<CharacterSpawnedEventArgs> CharacterSpawned;

    void LoadGameCycleParameters(Dictionary<int, IObject> Objects);

    void LoadCurrencyValue(int currentCurrency);
    void Run();
}

public class CycleHasFinished :EventArgs
{
    public GameTime GameTime { get; set; } 
}

