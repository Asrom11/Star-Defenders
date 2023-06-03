using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StarDefenderss;

public class CycleHasFinished :EventArgs
{
    public GameTime GameTime { get; set; } 
}

public class ActivateUltimate : EventArgs
{
    public Point Position;
}
public class GameplayEventArgs : EventArgs
{
    public Dictionary<int, IObject> Objects { get; set; }
    public int Currencys { get; set; }
    public int PlayerLives { get; set; }

    public HashSet<GameObjects> spawnedCharacters { get; set; }
}

public class GamePlayStatus : EventArgs
{
    public bool GameIsWin { get; set; }
}
public class CharacterSpawnedEventArgs : EventArgs
{
    public GameObjects SpawnedCharacter { get; set; }
    public Vector2 Position { get; set; }
}
