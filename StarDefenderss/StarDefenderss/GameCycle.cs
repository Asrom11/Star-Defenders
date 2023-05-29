using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarDefenders;

namespace StarDefenderss;

public class GameCycle: IGameplayModel
{
    private const int TileSize = 40;

    public int PlayerId { get; set; }
    public int Currency { get; set; }
    public event EventHandler<CharacterSpawnedEventArgs> CharacterSelected;
    public Dictionary<int, IObject> Objects { get; set; }
    public Dictionary<int, IObject> EnemyObjects { get; set; }
    public event EventHandler<GameplayEventArgs> Updated;
    public event EventHandler<EnemyMovedEventArgs> EnemyMoved;
    public event EventHandler<CurrencyEventArgs> CurrencyChange;
    private List<Operator> characters;
    private Timer _spawnTimer;
    private Timer _currencyTimer;
    
    private int _currentId = 1;
    private Vector2 BasePos;
    private Vector2 EnemyPos;
    private Node[,] _nodes;

    private const float spawnInterval = 1000;
    private const float currencyInterval = 100;
    private const int valuetAdd = 5;
    private  int shirina;
    private  int visota;
    private GameObjects selectedDirection = 0;

    private Dictionary<Keys, GameObjects> _keyDirectionMap = new()
    {
        { Keys.Up, GameObjects.Up },
        { Keys.Down, GameObjects.Down },
        { Keys.Left, GameObjects.Left },
        { Keys.Right, GameObjects.Right }
    };

    private Dictionary<GameObjects, float> _directionRotationMap = new()
    {
        { GameObjects.Up, 0 },
        { GameObjects.Down, MathHelper.Pi },
        { GameObjects.Left, -MathHelper.PiOver2 },
        { GameObjects.Right, MathHelper.PiOver2 }
    };
    private Dictionary<GameObjects, Vector2> _directionOffsetMap = new()
    {
        { GameObjects.Up, new Vector2(0, -TileSize) },
        { GameObjects.Down, new Vector2(TileSize, TileSize) },
        { GameObjects.Left, new Vector2(0, TileSize*2) },
        { GameObjects.Right, new Vector2(TileSize, 0) }
    };
    
    
    
    public void Initialize()
    {
        var textMap = MapLoader.Load(LevelName.LevelFirst);
        var lines = textMap.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        Objects = new Dictionary<int, IObject>();
        EnemyObjects = new Dictionary<int, IObject>();
        InitializeMap(lines);
        InitializeGraph(lines);
        shirina = lines[0].Length;
        visota = lines.Length;
        _spawnTimer = new Timer (spawnInterval);
        _currencyTimer = new Timer(currencyInterval);
        _currencyTimer.Elapsed += AddCurrency;
        _spawnTimer.Elapsed += SpawnEnemy; 
        _spawnTimer.AutoReset = true; 
        _currencyTimer.AutoReset = true;
        _currencyTimer.Start();
        _spawnTimer.Start (); 
    }

    private void InitializeMap(string[] lines)
    {
        for (var y = 0; y < lines.Length; y++)
        for (var x = 0; x < lines[0].Length; x++)
        {
            var generatedObject = GenerateObject(
                lines[x][y], x, y);
            Objects.Add(_currentId, generatedObject);
            _currentId++;
        }
    }
    private void AddCurrency(object sender, ElapsedEventArgs e)
    {
        Currency += valuetAdd;
        CurrencyChange.Invoke(this, new CurrencyEventArgs{Currencys = Currency});
    }

    private void InitializeGraph(string[] lines)
    {
        var width = TileSize;
        var height = TileSize;
        _nodes = new Node[width, height];

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                _nodes[x, y] = new Node(x, y);
            }
        }

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                // захардкожено, но потом будет исправлено ;)
                var node = _nodes[x, y];
                if (x > 0) node.Neighbors.Add(_nodes[x - 1, y]);
                if (x < width - 1) node.Neighbors.Add(_nodes[x + 1, y]);
                if (y > 0) node.Neighbors.Add(_nodes[x, y - 1]);
                if (y < height - 1) node.Neighbors.Add(_nodes[x, y + 1]);
            }
        }
    }
    public void Update()
    {
        Updated.Invoke(this, new GameplayEventArgs { Objects = this.Objects, EnemyObjects = this.EnemyObjects });
    }

    public void MoveEnemy(GameTime gameTime)
    {
        lock (Objects)
            foreach (var enemy in EnemyObjects.Values)
            {
                enemy.Update(gameTime);
            }
    }
    
    private void SpawnEnemy (object sender, ElapsedEventArgs e)
    {
        var nodeEnemy = _nodes[(int)(EnemyPos.X / TileSize), (int)(EnemyPos.Y / TileSize)];
        var nodeBase = _nodes[(int)BasePos.X / TileSize, (int)BasePos.Y / TileSize];
        lock (Objects)
        {
            var enemy = new Enemy(100, 1, 2, 4, 0, EnemyPos, nodeEnemy, 3, nodeBase, TileSize, GameObjects.Enemy);
            _currentId++;
            EnemyObjects.Add (_currentId, enemy);
            Objects.Add(_currentId,enemy);
        }
    }
    
    private IObject GenerateObject (
        char sign, int xTile, int yTile)
    {
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
    public void SpawnCharacter(Vector2 position, IObject charactere)
    {
        if (!IsMousePositionOnTileMap(position) || Currency < charactere.Currency || charactere.IsSpawned) return;
        Currency -= charactere.Currency;
        lock (Objects)
        {
            position.X = (int) position.X / TileSize;
            position.Y = (int) position.Y / TileSize;
            _currentId++; 
            charactere.Pos = position * TileSize;
            var dir = new Direction { ImageId = GameObjects.Ditection, Pos = charactere.Pos };
            HandleDirectionSelection(dir);
            charactere.IsSpawned = true;
            Objects.Add(_currentId,charactere);
        }
    }
    private bool IsMousePositionOnTileMap(Vector2 mousePosition)
    {
        var tileMapX = 0;
        var tileMapY = 0;
        mousePosition.X = (int) mousePosition.X / TileSize;
        mousePosition.Y = (int) mousePosition.Y / TileSize;

        var tileMapWidth = shirina;
        var tileMapHeight = visota;
        return mousePosition.X >= tileMapX && mousePosition.X  < tileMapX + tileMapWidth  &&
               mousePosition.Y >= tileMapY && mousePosition.Y < tileMapY + tileMapHeight ;
    }
    private async Task HandleDirectionSelection (Direction direction)
    {
        selectedDirection = 0;
        while (selectedDirection == 0)
        {
            foreach (var kvp in _keyDirectionMap)
            {
                if (InputManager.IsKeyPressed(kvp.Key))
                {
                    selectedDirection = kvp.Value;
                    break;
                }
            }
            await Task.Delay(1);
        }

        direction.Rotation = _directionRotationMap[selectedDirection];
        direction.Pos += _directionOffsetMap[selectedDirection];
        lock (Objects)
        {
            _currentId++;
            Objects.Add(_currentId,direction);
        }
    }
}
public class CharacterSpawnedEventArgs : EventArgs
{
    public IObject SpawnedCharacter { get; set; }
    public Vector2 Position { get; set; }
}

public class Direction: IObject
{
    public GameObjects ImageId { get; set; }
    public int Currency { get; set; }
    public int UnicId { get; set; }
    public float Scale { get; set; }
    public float Rotation { get; set; }
    public Color Color { get; set; }
    public Direction dir { get; set; }
    public Vector2 Pos { get; set; }
    public bool IsSpawned { get; set; }

    public void Update(GameTime gameTime)
    {
    }

    public Direction()
    {
        Color = Color.White;
        Scale = 0.5f;
    }
}