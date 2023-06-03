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
    void Initialize(string levelName);   
    void Update(GameTime gameTime);
    void SpawnCharacter(Vector2 position, GameObjects character);
    void TryActivateUltimate(Point pointClick);

}