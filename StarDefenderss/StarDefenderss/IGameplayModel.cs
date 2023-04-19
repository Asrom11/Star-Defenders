using System;
using Microsoft.Xna.Framework;

namespace StarDefenderss;

public interface IGameplayModel
{
    event EventHandler<GameplayEventArgs> Updated;
    void Update();
}
public class GameplayEventArgs : EventArgs
{
}