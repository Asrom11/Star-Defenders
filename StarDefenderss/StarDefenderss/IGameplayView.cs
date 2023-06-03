using System;
using System.Collections.Generic;

namespace StarDefenderss;

public interface IGameplayView
{
    event EventHandler<CycleHasFinished> CycleFinished;
    event EventHandler<CharacterSpawnedEventArgs> CharacterSpawned;
     event EventHandler<ActivateUltimate> ActivateUltimate;

    void LoadGameCycleParameters(Dictionary<int, IObject> Objects, int currency, int PlayerLives,HashSet<GameObjects> spawnedCharacters);

    void SetGameStatus(bool GameStatus);
    void Run();
}