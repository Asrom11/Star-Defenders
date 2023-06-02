using System;
using System.Collections.Generic;
using System.Timers;
using Microsoft.Xna.Framework;

namespace StarDefenderss;

public class GameCycle: IGameplayModel
{
    private const int TileSize = 40;
    public int Currency { get; set; }
    public event EventHandler<CharacterSpawnedEventArgs> CharacterSelected;
    public Dictionary<int, IObject> Objects { get; set; }
    
    private Grid _gridWithOperators;
    private Grid _gridWithEnemys;

    private HashSet<Point> _wallCoordinats = new ();
    public event EventHandler<GameplayEventArgs> Updated;
    public event EventHandler<CurrencyEventArgs> CurrencyChange;
    private Dictionary<GameObjects, IOperator> _operators;
    private Dictionary<GameObjects, Enemy> _enemies;
    private HashSet<IOperator> spawnedOperators = new ();
    private HashSet<Enemy> spawnedEnemys = new ();
    private HashSet<Vector2> operatorPos = new();

    private Timer _spawnTimer;
    private Timer _currencyTimer;
    private int _currentId = 1;
    private Vector2 BasePos;
    private Vector2 EnemyPos;
    private Node[,] _nodes;
    
    private const float spawnInterval = 1000;
    private const float currencyInterval = 100;
    private const int valuetAdd = 5;
    private  int width;
    private  int height;
    private GameObjects selectedDirection = 0;
    public void Initialize()
    {
        var textMap = MapLoader.Load(LevelName.LevelFirst);
        var lines = textMap.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        Objects = new Dictionary<int, IObject>();
        width = lines[0].Length;
        height = lines.Length;
        InitializeMap(lines);
        InitializeGraph(lines);
        _operators = new Dictionary<GameObjects, IOperator>();
        _operators.Add(GameObjects.FirstOp, new Operator(100,10,1,1,1, new Vector2(0,0), 1,100, GameObjects.FirstOp));
        _operators.Add(GameObjects.TankOp, new TankOperator(100,10,1,1,1, new Vector2(0,0), 1, 100,GameObjects.TankOp));
        
        _gridWithOperators = new Grid(TileSize);
        _gridWithEnemys = new Grid(TileSize);
        _currencyTimer = new Timer(currencyInterval);
        _spawnTimer = new Timer(currencyInterval);
        _currencyTimer.Elapsed += AddCurrency;
        _spawnTimer.Elapsed += SpawnEnemy; 
        _spawnTimer.AutoReset = false; 
        _currencyTimer.AutoReset = true;
        _currencyTimer.Start();
        _spawnTimer.Start (); 
    }

    private void InitializeMap(string[] lines)
    {
        for (var y = 0; y < lines.Length; y++)
        for (var x = 0; x < lines[0].Length; x++)
        {
            var generatedObject = MapGenerator.GenerateObject(
                lines[x][y], x, y, ref EnemyPos, ref BasePos);
            if (lines[x][y] == 'W') _wallCoordinats.Add(new Point(x, y));
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
                if (x > 0 && !_wallCoordinats.Contains(new Point(x,y))) node.Neighbors.Add(_nodes[x - 1, y]);
                if (x < width - 1 && !_wallCoordinats.Contains(new Point(x,y))) node.Neighbors.Add(_nodes[x + 1, y]);
                if (y > 0 && !_wallCoordinats.Contains(new Point(x,y))) node.Neighbors.Add(_nodes[x, y - 1]);
                if (y < height - 1 && !_wallCoordinats.Contains(new Point(x,y))) node.Neighbors.Add(_nodes[x, y + 1]);
            }
        }
    }
    public void Update(GameTime gameTime)
    {
        _gridWithEnemys.Clear();
        MoveEnemy(gameTime);
        UpdatePlayer(gameTime);
        Updated.Invoke(this, new GameplayEventArgs { Objects = this.Objects});
    }
    
    private void MoveEnemy(GameTime gameTime)
    {
        foreach (var obj in spawnedEnemys)
        {
            obj.Update(gameTime);
            _gridWithEnemys.Add(obj);
        }
    }

    //Todo добавить в интерфейс
    private void UpdatePlayer(GameTime gameTime)
    {
        foreach (var obj in spawnedOperators)
        {
            obj.Update(gameTime);
        }
    }

    public void TryActivateUltimate(Point pointClick)
    {
        var currentPoit = pointClick.ToVector2();
        var near = _gridWithOperators.GetNearbyObjects(currentPoit,30);
        foreach (var operators in near)
        {
            if (operators is not IOperator playerOperator) continue;
            playerOperator.ActivUltimate();
            return;

        }
    }
    private void SpawnEnemy (object sender, ElapsedEventArgs e)
    {
        var nodeEnemy = _nodes[(int)(EnemyPos.X / TileSize), (int)(EnemyPos.Y / TileSize)];
        var nodeBase = _nodes[(int)BasePos.X / TileSize, (int)BasePos.Y / TileSize];
        var enemy = new Enemy(100, 1, 2, 4, 0, EnemyPos, nodeEnemy, 3, nodeBase, TileSize, GameObjects.Enemy);
        _currentId++;
        enemy._grid = _gridWithOperators;
        spawnedEnemys.Add(enemy);
        _gridWithEnemys.Add(enemy);
        Objects.Add(_currentId,enemy);
    }
    
    public void SpawnCharacter(Vector2 position, GameObjects charactere)
    {
        var getCharacterToSpawn = _operators[charactere];
        if (!IsMousePositionOnTileMap(position) || Currency < getCharacterToSpawn.Currency || getCharacterToSpawn.IsSpawned) return;
        
        Currency -= getCharacterToSpawn.Currency;
        operatorPos.Add(position);
        position.X = (int) position.X / TileSize;
        position.Y = (int) position.Y / TileSize;
        _currentId++; 
        getCharacterToSpawn.Pos = position * TileSize;
        getCharacterToSpawn.IsSpawned = true;
        Objects.Add(_currentId, (IObject)getCharacterToSpawn);
        spawnedOperators.Add(getCharacterToSpawn);
        getCharacterToSpawn._grid = _gridWithEnemys;
        if (getCharacterToSpawn is not IAttackable attackableCharacter) return;
        _gridWithOperators.Add(attackableCharacter);
    }
    private bool IsMousePositionOnTileMap(Vector2 mousePosition)
    {
        var tileMapX = 0;
        var tileMapY = 0;
        mousePosition.X = (int) mousePosition.X / TileSize;
        mousePosition.Y = (int) mousePosition.Y / TileSize;

        var tileMapWidth = width;
        var tileMapHeight = height;
        return mousePosition.X >= tileMapX && mousePosition.X  < tileMapX + tileMapWidth  &&
               mousePosition.Y >= tileMapY && mousePosition.Y < tileMapY + tileMapHeight ;
    }
}
public class CharacterSpawnedEventArgs : EventArgs
{
    public GameObjects SpawnedCharacter { get; set; }
    public Vector2 Position { get; set; }
}
