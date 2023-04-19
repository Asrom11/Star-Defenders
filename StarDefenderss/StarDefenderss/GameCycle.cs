using System;

namespace StarDefenderss;

public class GameCycle: IGameplayModel
{
    public event EventHandler<GameplayEventArgs> Updated;
    public void Update()
    {
        Updated.Invoke(this, new GameplayEventArgs());
    }
}