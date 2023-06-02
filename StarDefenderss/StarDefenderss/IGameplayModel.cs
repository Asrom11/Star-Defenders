using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StarDefenderss;

public interface IGameplayModel
{
    event EventHandler<GameplayEventArgs> Updated;
    event EventHandler<GamePlayStatus> GameStatus;
    int Currency { get;  }

    int PlayerLives { get;  }
    public Dictionary<int, IObject> Objects { get; set; }
    void Initialize();   
    void Update(GameTime gameTime);

    private void MoveEnemy(GameTime gameTime)
    {
    }

    void SpawnCharacter(Vector2 position, GameObjects character);
    void TryActivateUltimate(Point pointClick);

}


public class GameplayEventArgs : EventArgs
{
    public Dictionary<int, IObject> Objects { get; set; }
    public int Currencys { get; set; }
    public int PlayerLives { get; set; }
}

public class GamePlayStatus : EventArgs
{
    public bool GameIsWin { get; set; }
}