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
        //using (var game = new GameCycleView())
        //var game = new GameCycleView();
        //game.Run();
        var characterTextures = new List<Texture2D>();
        var game = new GameplayPresenter(
            new GameCycleView(characterTextures), new GameCycle());
        game.LaunchGame();
    }
}