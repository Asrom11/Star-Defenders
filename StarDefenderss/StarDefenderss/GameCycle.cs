using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StarDefenderss.Content;

namespace StarDefenderss;

public class GameCycle: IGameplayModel
{
    private const int TileSize = 40;

    public event EventHandler<CharacterSpawnedEventArgs> CharacterSelected;
    public event EventHandler<GamePlayStatus> GameStatus;
    public int PlayerLives { get; set; }
    public Dictionary<int, IObject> Objects { get; set; }
    
    private Grid _gridWithOperators;
    private Grid _gridWithEnemys;

    private HashSet<Point> _wallCoordinats = new ();
    public event EventHandler<GameplayEventArgs> Updated;
    public int Currency { get; set; }
    private HashSet<GameObjects> _spawnedCharacters = new (); 
    private Dictionary<GameObjects, IOperator> _operators;
    private Dictionary<GameObjects, IEnemy> _enemies;
    private Dictionary<GameObjects, int> _opertorsHp = new();
    private HashSet<IOperator> activeOperators = new ();
    private HashSet<IEnemy> spawnedEnemys = new ();
    
    private Timer _currencyTimer;
    private int _currentId = 1;
    private Vector2 BasePos;
    private Vector2 EnemyPos;
    private Node[,] _nodes;
    private int waveCount;
    private const float currencyInterval = 100;
    private const int valuetAdd = 5;
    private  int width;
    private  int height;
    private GameObjects selectedDirection = 0;
    private bool isEnd;
    private int spawnDelay = 1000;
    private int timeSinceLastSpawn = 0;
    private Queue<IEnemy> enemiesToSpawn = new ();
    public void Initialize(string levelName)
    {
        PlayerLives = 3;
        var textMap = File.ReadAllText("Maps/" + levelName);
        var lines = textMap.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        Objects = new Dictionary<int, IObject>();
        width = lines[0].Length;
        height = lines.Length;
        InitializeMap(lines);
        InitializeGraph(lines);

        _opertorsHp.Add(GameObjects.FirstOp, 500);
        _opertorsHp.Add(GameObjects.TankOp, 1000);
        _opertorsHp.Add(GameObjects.Sniper, 200);
        
        _operators = new Dictionary<GameObjects, IOperator>();
        _operators.Add(GameObjects.FirstOp, new Operator(_opertorsHp[GameObjects.FirstOp],10,1,1,1, new Vector2(0,0), 1,100, GameObjects.FirstOp));
        _operators.Add(GameObjects.TankOp, new TankOperator(_opertorsHp[GameObjects.TankOp],10,1,1,1, new Vector2(0,0), 1, 100,GameObjects.TankOp));
        _operators.Add(GameObjects.Sniper, new PlayerSniper(_opertorsHp[GameObjects.Sniper],100,100,0,100,new Vector2(0,0),10,GameObjects.Sniper, 100));

        _gridWithOperators = new Grid(TileSize);
        _gridWithEnemys = new Grid(TileSize);
        _currencyTimer = new Timer(currencyInterval);
        _currencyTimer.Elapsed += AddCurrency;
        _currencyTimer.AutoReset = true;
        _currencyTimer.Start();
    }
    private void RealeseEnemyWave()
    {
        if (enemiesToSpawn.Count <= 0) return;
        var enemy = enemiesToSpawn.Dequeue();
        _currentId++;
        enemy.UnicId = _currentId;
        spawnedEnemys.Add(enemy);
        _gridWithEnemys.Add((IAttackable)enemy);
        Objects.Add(_currentId, enemy);
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
                var node = _nodes[x, y];
                var neighbors = new List<Point>
                {
                    new (x - 1, y),
                    new (x + 1, y),
                    new (x, y - 1),
                    new (x, y + 1)
                };

                foreach (var neighbor in neighbors.Where(neighbor => neighbor.X >= 0 && neighbor.X < width && neighbor.Y >= 0 && neighbor.Y < height &&
                                                                     !_wallCoordinats.Contains(neighbor)))
                {
                    node.Neighbors.Add(_nodes[neighbor.X, neighbor.Y]);
                }
            }
        }
    }
    public void Update(GameTime gameTime)
    {
        if (isEnd) return;
        
        _gridWithEnemys.Clear();
        MoveEnemy(gameTime);
        UpdatePlayer(gameTime);
        if (CheckEnemysGrid() && enemiesToSpawn.Count == 0 )
            SpawnEnemy();
        timeSinceLastSpawn += gameTime.ElapsedGameTime.Milliseconds;
        if (timeSinceLastSpawn >= spawnDelay)
        {
            RealeseEnemyWave();
            timeSinceLastSpawn = 0;
        }

        if (waveCount == 3 && enemiesToSpawn.Count == 0 && spawnedEnemys.Count == 0)
        {
            isEnd = true;
            GameStatus.Invoke(this, new GamePlayStatus
            {
                GameIsWin = true
            });
        }
        Updated.Invoke(this, new GameplayEventArgs { Objects = this.Objects, Currencys = Currency, 
            PlayerLives = PlayerLives, spawnedCharacters = _spawnedCharacters});
    }

    private bool CheckEnemysGrid()
    {
        return _gridWithEnemys.IsEmpty();
    }
    private void MoveEnemy(GameTime gameTime)
    {
        foreach (var obj in spawnedEnemys)
        {
            obj.Update(gameTime);
            var currentEnemy = obj as Character;
            if (CheckHp(currentEnemy, currentEnemy.CurrentHealth))
            {
                Objects.Remove(obj.UnicId);
                spawnedEnemys.Remove(obj);
                continue;
            }
            
            if ( !(Vector2.Distance(obj.Pos,BasePos) > 5f))
            {
                PlayerLives--;
                spawnedEnemys.Remove(obj);
                Objects.Remove(obj.UnicId);
                
                if (PlayerLives <= 0)
                {
                    isEnd = true;
                    _currencyTimer.Stop();
                    GameStatus.Invoke(this, new GamePlayStatus()
                    {
                        GameIsWin = false
                    });
                }
                continue;
            }
            _gridWithEnemys.Add((IAttackable)obj);
        }
    }

    private bool CheckHp(Character character, int health)
    {
        if (character.CurrentHealth > 0) return false;
        
        Objects.Remove(character.UnicId);
        return true;

    }
    
    private void UpdatePlayer(GameTime gameTime)
    {
        foreach (var obj in activeOperators)
        {
            var currentPlayer = obj as Character;
            
            if (CheckHp(currentPlayer, currentPlayer.CurrentHealth))
            {
                obj.IsSpawned = false;
                Objects.Remove(obj.UnicId);
                _spawnedCharacters.Remove(obj.ImageId);
                _gridWithOperators.Remove(obj as IAttackable);
                activeOperators.Remove(obj);
            }
            obj.Update(gameTime);
        }
    }

    public void TryActivateUltimate(Point pointClick)
    {
        var currentPoit = Mouse.GetState().Position.ToVector2();
        var near = _gridWithOperators.GetNearbyObjects(currentPoit,40);
        foreach (var operators in near)
        {
            if (operators is not IOperator playerOperator) continue;
            playerOperator.ActivUltimate();
            return;

        }
    }
    private void SpawnEnemy ()
    {
        var nodeEnemy = _nodes[(int)(EnemyPos.X / TileSize), (int)(EnemyPos.Y / TileSize)];
        var nodeBase = _nodes[(int)BasePos.X / TileSize, (int)BasePos.Y / TileSize];
        waveCount++;
        var enemysToSpawn = new List<IEnemy>();
        
        switch (waveCount)
        {
            case 1:
            {
                for (var i = 0; i < 4; i++)
                {
                    var speed = 3 + i;
                    var enemy = new Enemy(100, 1, 2, speed, 0, EnemyPos,  nodeEnemy,nodeBase,3,  TileSize, GameObjects.Enemy, _gridWithOperators);
                    enemysToSpawn.Add(enemy);
                }
                AddEnemy(enemysToSpawn);
                break;
            }

            case 2:
            {
                for (var i = 0; i < 1; i++)
                {
                    var speed = 3 + i;
                    var sniper = new SniperEnemy(100, 1, 2,
                        4, 0,  EnemyPos, nodeEnemy,nodeBase,10, GameObjects.EnemySniper, 0, 40, _gridWithOperators);
                    var enemy = new Enemy(100, 1, 2, speed, 0, EnemyPos,  nodeEnemy,nodeBase,3,  TileSize, GameObjects.Enemy, _gridWithOperators);
                    enemysToSpawn.Add(sniper);
                    enemysToSpawn.Add(enemy);
                }
                AddEnemy(enemysToSpawn);
                break;
            }
        }
    }

    private void AddEnemy(List<IEnemy> enemysToSpawn)
    {
        foreach (var enemys in enemysToSpawn)
        {
            enemiesToSpawn.Enqueue(enemys);
        }
        
    }
    
    public void SpawnCharacter(Vector2 position, GameObjects charactere)
    {
        var getCharacterToSpawn = _operators[charactere];
        if (!IsMousePositionOnTileMap(position) || Currency < getCharacterToSpawn.Currency || getCharacterToSpawn.IsSpawned) return;
        
        position.X = (int) position.X / TileSize;
        position.Y = (int) position.Y / TileSize;

        if (getCharacterToSpawn.isSniper && _wallCoordinats.Contains(position.ToPoint()))
        {
            AddSpawnCharachters(getCharacterToSpawn,position, charactere);
        }
        else if(!_wallCoordinats.Contains(position.ToPoint()) && !getCharacterToSpawn.isSniper)
        {
            AddSpawnCharachters(getCharacterToSpawn,position,charactere);
        }
    }

    private void AddSpawnCharachters(IOperator charachter, Vector2 position,GameObjects operatorType)
    {
        Currency -= charachter.Currency;
        _spawnedCharacters.Add(charachter.ImageId);
        _currentId++;
        charachter.UnicId = _currentId;
        charachter.Pos = position * TileSize;
        charachter.IsSpawned = true;
        Objects.Add(_currentId, charachter);
        activeOperators.Add(charachter);
        charachter._grid = _gridWithEnemys;
        if (charachter is not IHasBar health) return;
        health.CurrentHealth = _opertorsHp[operatorType];
        if (charachter is not IAttackable attackableCharacter) return;
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