using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarDefenderss;

public interface IGameplayView
{
    event EventHandler CycleFinished;
    void LoadGameCycleParameters(Dictionary<int, IObject> Objects);
    void Run();
}
public class ControlsEventArgs : EventArgs
{
}