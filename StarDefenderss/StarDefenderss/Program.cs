//using var game = new StarDefenderss.GameCycleView();
//game.Run();

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarDefenderss;

public static class Program
{
    [STAThread]
    static void Main()
    {
        var game = new GameplayPresenter(
            new GameCycleView(), new GameCycle());
        game.LaunchGame();
    }
}