using System;
using Microsoft.Xna.Framework;

namespace StarDefenderss;

public interface IGameplayView
{
    event EventHandler CycleFinished;
    void LoadGameCycleParameters();
    void Run();
}
public class ControlsEventArgs : EventArgs
{
}