//using var game = new StarDefenderss.GameCycleView();
//game.Run();

using System;
using Microsoft.Xna.Framework;
using StarDefenderss;

public static class Program
{
    [STAThread]
    static void Main()
    {
        //using (var game = new GameCycleView())
        //var game = new GameCycleView();
        //game.Run();         
        var game = new GameplayPresenter(
            new GameCycleView(), new GameCycle());
        game.LaunchGame();
    }
}