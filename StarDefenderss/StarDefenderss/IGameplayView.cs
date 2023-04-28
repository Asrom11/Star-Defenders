using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarDefenderss;

public interface IGameplayView
{
    event EventHandler CycleFinished;
    event EventHandler<EnemyMovedEventArgs> EnemyMoved;
    void LoadGameCycleParameters(Dictionary<int, IObject> Objects);
    void Run();
}
public class EnemyMovedEventArgs : EventArgs
{
    public GameTime GameTime { get; set; } 
}
public class ControlsEventArgs : EventArgs
{
    public Dictionary<int, IObject> Objects { get; set; }
    public GameTime GameTime { get; set; }
}

