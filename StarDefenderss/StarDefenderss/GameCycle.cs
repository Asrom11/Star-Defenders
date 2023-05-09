using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarDefenders;

namespace StarDefenderss;

public class GameCycle: IGameplayModel
{
    private const int TileSize = 40;
    public int PlayerId { get; set; }
    public Dictionary<int, IObject> Objects { get; set; }
    public event EventHandler<GameplayEventArgs> Updated;
    public event EventHandler<EnemyMovedEventArgs> EnemyMoved;
    Timer _spawnTimer; 
    private int _currentId = 1;
    private Vector2 BasePos;
    private Vector2 EnemyPos;

    private const float spawnInterval = 1000; 
    public void Initialize()
    {
        var textMap = MapLoader.Load(LevelName.LevelFirst);
        var lines = textMap.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        Objects = new Dictionary<int, IObject>();
        for (var y = 0; y < lines.Length; y++)
        for (var x = 0; x < lines[0].Length; x++)
        {
            var generatedObject = GenerateObject(
               lines[x][y], x, y);
            Objects.Add(_currentId, generatedObject);
            _currentId++;
        }       
        _spawnTimer = new Timer (spawnInterval);
        _spawnTimer.Elapsed += SpawnEnemy; 
        _spawnTimer.AutoReset = true; 
        _spawnTimer.Start (); 
    }

    public void Update()
    {
        Updated.Invoke(this, new GameplayEventArgs { Objects = this.Objects });
    }

    public void MoveEnemy(GameTime gameTime)
    {
        lock (Objects)
            foreach (var enemy in Objects.Values)
            {
                enemy.Update(gameTime);
            }
    }
    
    private void SpawnEnemy (object sender, ElapsedEventArgs e)
    {
        var enemy = new Enemy (100,1,2,4, 0, EnemyPos, 3, BasePos);
        _currentId++;
        lock (Objects)
            Objects.Add (_currentId, enemy);
    }
    
    private IObject GenerateObject (
        char sign, int xTile, int yTile)
    {
        float x = xTile*TileSize;
        float y = yTile*TileSize;
        IObject generatedObject = null;
        switch (sign)
        {
            case 'T':
                generatedObject = CreateBase(GameObjects.Base, xTile * TileSize,yTile * TileSize, Color.Red,false);
                EnemyPos = new Vector2(xTile * TileSize,yTile * TileSize) ;
                break;
            case 'C':
                generatedObject = CreateBase(GameObjects.Base, xTile * TileSize,yTile * TileSize, Color.Blue, true);
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
    
    private Base CreateBase(GameObjects ImageId, int x, int y, Color color,bool isEnemyBase)
    {
        return new Base()
        {
            Pos = new Vector2(x,
                y ),
            ImageId = ImageId,
            isEnemyBase = isEnemyBase,
            Color = color
        };
    }
    
    private Path CreatePath(GameObjects ImageId, int x, int y, Color color)
    {
        return new Path()
        {
            Pos = new Vector2(x ,
                y  ),
            ImageId = ImageId,
            Color = color
        };
    }
    
    private Wall CreateWall(GameObjects ImageId, int x, int y, Color color)
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