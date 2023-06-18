using System.Collections.Generic;
using Microsoft.Xna.Framework;
using StarDefenders;

namespace StarDefenderss;

public static class MapGenerator
{
    public const int TileSize = 40;
    public static IObject GenerateObject (
        char sign, int xTile, int yTile, ref Vector2 EnemyPos, ref Vector2 BasePos)
    {
        IObject generatedObject = null;
        switch (sign)
        {
            case 'T':
                generatedObject = CreateBase(GameObjects.Base, xTile * TileSize,yTile * TileSize, Color.Red);
                EnemyPos = new Vector2(xTile * TileSize,yTile * TileSize) ;
                break;
            case 'C':
                generatedObject = CreateBase(GameObjects.Base, xTile * TileSize,yTile * TileSize, Color.Blue);
                BasePos = new Vector2(xTile * TileSize,yTile * TileSize);
                break;
            case 'W':
                generatedObject = CreateWall(GameObjects.Wall, xTile * TileSize,yTile * TileSize, Color.White);
                break;
            default:
                generatedObject = CreatePath(GameObjects.Path, xTile*TileSize, yTile*TileSize, Color.White);
                break;
        }
        return generatedObject;
    }
    private static Base CreateBase(GameObjects ImageId, int x, int y, Color color)
    {
        return new Base()
        {
            Pos = new Vector2(x,
                y ),
            ImageId = ImageId,
            Color = color
        };
    }
    
    private static Path CreatePath(GameObjects ImageId, int x, int y, Color color)
    {
        return new Path()
        {
            Pos = new Vector2(x ,
                y  ),
            ImageId = ImageId,
            Color = color
        };
    }
    
    private static Wall CreateWall(GameObjects ImageId, int x, int y, Color color)
    {
        return new Wall()
        {
            Pos = new Vector2(x ,
                y  ),
            ImageId = ImageId,
            Color = color
        };
    }
}