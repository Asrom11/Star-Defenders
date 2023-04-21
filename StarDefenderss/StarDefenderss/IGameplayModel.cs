using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StarDefenderss;

public interface IGameplayModel
{
    event EventHandler<GameplayEventArgs> Updated;
    void Initialize();   
    void Update();
}
public class GameplayEventArgs : EventArgs
{
    public Dictionary<int, IObject> Objects { get; set; }   
}