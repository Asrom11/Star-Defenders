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
    public event EventHandler<GamePlayStatus> GameStatus;
    public event EventHandler<GameplayEventArgs> Updated;
    public int PlayerLives { get; set; }
    public Dictionary<int, IObject> Objects { get; set; }
    
    private Grid _gridWithOperators;
    private Grid _gridWithEnemys;

    private HashSet<Point> _wallCoordinats = new ();
    public int Currency { get; set; }
    private HashSet<GameObjects> _spawnedCharacters = new (); 
    private Dictionary<GameObjects, IOperator> _operators;
    private Dictionary<GameObjects, int> _opertorsHp = new();
    private HashSet<IOperator> _activeOperators = new ();
    private HashSet<IEnemy> _spawnedEnemys = new ();
    private Queue<IEnemy> _enemiesToSpawn = new ();
    
    private Timer _currencyTimer;
    private int _currentId = 1;
    private Vector2 _basePos;
    private Vector2 _enemyPos;
    private Node[,] _nodes;
    private int _waveCount;
    private const float currencyInterval = 1000;
    private const int valuetAdd = 1;
    private  int _width;
    private  int _height;
    private bool _isEnd;
    private const int SpawnDelay = 1000;
    private int _timeSinceLastSpawn = 0;
    public void Initialize(string levelName)
    {
        PlayerLives = 3;
        var textMap = File.ReadAllText("Maps/" + levelName);
        var lines = textMap.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        Objects = new Dictionary<int, IObject>();
        _width = lines[0].Length;
        _height = lines.Length;
        InitializeMap(lines);
        InitializeGraph(lines);

        _opertorsHp.Add(GameObjects.FirstOp, 1410);
        _opertorsHp.Add(GameObjects.TankOp, 1728);
        _opertorsHp.Add(GameObjects.Sniper, 1016);
        _opertorsHp.Add(GameObjects.Healer, 1076);
        _opertorsHp.Add(GameObjects.Vanguard,996);
        
        _operators = new Dictionary<GameObjects, IOperator>();
        _operators.Add(GameObjects.FirstOp, new Operator(_opertorsHp[GameObjects.FirstOp],426,238,0,15, new Vector2(0,0), 10,18, GameObjects.FirstOp));
        _operators.Add(GameObjects.TankOp, new TankOperator(_opertorsHp[GameObjects.TankOp],295,364,0,15, new Vector2(0,0), 10, 23,GameObjects.TankOp));
        _operators.Add(GameObjects.Sniper, new PlayerSniper(_opertorsHp[GameObjects.Sniper],305,95,0,100,new Vector2(0,0),10,GameObjects.Sniper, 14));
        _operators.Add(GameObjects.Healer, new Healer(_opertorsHp[GameObjects.Healer],210,107,0,100,new Vector2(0,0),10,GameObjects.Healer, 18));
        _operators.Add(GameObjects.Vanguard, new Vanguard(_opertorsHp[GameObjects.Vanguard],299,208,0,15, new Vector2(0,0), 10,11, 
            GameObjects.Vanguard));
        
        _gridWithOperators = new Grid(TileSize);
        _gridWithEnemys = new Grid(TileSize);
        _currencyTimer = new Timer(currencyInterval);
        _currencyTimer.Elapsed += AddCurrency;
        _currencyTimer.AutoReset = true;
        _currencyTimer.Start();
    }
    private void RealeseEnemyWave()
    {
        if (_enemiesToSpawn.Count <= 0) return;
        var enemy = _enemiesToSpawn.Dequeue();
        _currentId++;
        enemy.UnicId = _currentId;
        _spawnedEnemys.Add(enemy);
        _gridWithEnemys.Add((IAttackable)enemy);
        Objects.Add(_currentId, enemy);
    }
    private void InitializeMap(string[] lines)
    {
        for (var y = 0; y < lines.Length; y++)
        for (var x = 0; x < lines[0].Length; x++)
        {
            var generatedObject = MapGenerator.GenerateObject(
                lines[x][y], x, y, ref _enemyPos, ref _basePos);
            if (lines[x][y] == 'W') _wallCoordinats.Add(new Point(x, y));
            Objects.Add(_currentId, generatedObject);
            _currentId++;
        }
    }
    private void AddCurrency(object sender, ElapsedEventArgs e)
    {
        if (Currency + valuetAdd >= 99)
            Currency = 99;
        else
            Currency += valuetAdd;
    }

    private void InitializeGraph(string[] lines)
    {
        CreateNodes();
        AddNeighborsNode();
    }

    private void CreateNodes()
    {     
        _nodes = new Node[_width, _height];
        for (var x = 0; x < _width; x++)
        {
            for (var y = 0; y < _height; y++)
            {
                _nodes[x, y] = new Node(x, y);
            }
        }
        
    }
    private void AddNeighborsNode()
    {
        
        for (var x = 0; x < _width; x++)
        {
            for (var y = 0; y < _height; y++)
            {
                var node = _nodes[x, y];
                var neighbors = new List<Point>
                {
                    new (x - 1, y),
                    new (x + 1, y),
                    new (x, y - 1),
                    new (x, y + 1)
                };

                foreach (var neighbor in neighbors.Where(neighbor => neighbor.X >= 0 && neighbor.X < _width && neighbor.Y >= 0 && neighbor.Y < _height &&
                                                                     !_wallCoordinats.Contains(neighbor)))
                {
                    node.Neighbors.Add(_nodes[neighbor.X, neighbor.Y]);
                }
            }
        }
    }
    public void Update(GameTime gameTime)
    {
        if (_isEnd) return;
        
        
        _gridWithEnemys.Clear();
        MoveEnemy(gameTime);
        UpdatePlayer(gameTime);
        if (CheckEnemysGrid() && _enemiesToSpawn.Count == 0 )
            SpawnEnemy();
        _timeSinceLastSpawn += gameTime.ElapsedGameTime.Milliseconds;
        CheckWaweSpawn();
        CheckGameStatus();
        Updated.Invoke(this, new GameplayEventArgs { Objects = this.Objects, Currencys = Currency, 
            PlayerLives = PlayerLives, spawnedCharacters = _spawnedCharacters});
    }

    private void CheckGameStatus()
    {
        if (PlayerLives <= 0)
        {
            _isEnd = true;
            _currencyTimer.Stop();
            GameStatus.Invoke(this, new GamePlayStatus()
            {
                GameIsWin = false
            });
            return;
        }

        if (_waveCount != 3 || _enemiesToSpawn.Count != 0 || _spawnedEnemys.Count != 0) return;

        _isEnd = true;
        GameStatus.Invoke(this, new GamePlayStatus {
            GameIsWin = true
        });
    }

    private void CheckWaweSpawn()
    {
        if (_timeSinceLastSpawn >= SpawnDelay)
        {
            RealeseEnemyWave();
            _timeSinceLastSpawn = 0;
        }
    }

    private bool CheckEnemysGrid()
    {
        return _gridWithEnemys.IsEmpty();
    }
    private void MoveEnemy(GameTime gameTime)
    {
        foreach (var obj in _spawnedEnemys)
        {
            obj.Update(gameTime);
            var currentEnemy = obj as Character;
            if (HpIsZero(currentEnemy, currentEnemy.CurrentHealth))
            {
                Objects.Remove(obj.UnicId);
                _spawnedEnemys.Remove(obj);
                continue;
            }
            
            if (DistanceToBase(obj) < 5)
            {
                ChangePlayerLives(obj);
                continue;
            }
            _gridWithEnemys.Add((IAttackable)obj);
        }
    }

    private float DistanceToBase(IEnemy obj)
    {
        return Vector2.Distance(obj.Pos, _basePos);
    }

    private void ChangePlayerLives(IEnemy obj)
    {
        PlayerLives--;
        _spawnedEnemys.Remove(obj);
        Objects.Remove(obj.UnicId);
        CheckGameStatus();
    }
    private bool HpIsZero(Character character, int health)
    {
        if (character.CurrentHealth > 0) return false;
        
        Objects.Remove(character.UnicId);
        return true;

    }
    
    private void UpdatePlayer(GameTime gameTime)
    {
        foreach (var obj in _activeOperators)
        {
            var currentPlayer = obj as Character;
            
            if (HpIsZero(currentPlayer, currentPlayer.CurrentHealth))
            {
                obj.IsSpawned = false;
                Objects.Remove(obj.UnicId);
                _spawnedCharacters.Remove(obj.ImageId);
                _gridWithOperators.Remove(obj as IAttackable);
                _activeOperators.Remove(obj);
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

            if (playerOperator.operatorType == GameObjects.Vanguard)
            {
                Currency += 14;
                playerOperator.ActivUltimate();
                return;
            }
            playerOperator.ActivUltimate();
            return;

        }
    }
    private void SpawnEnemy ()
    {
        var nodeEnemy = _nodes[(int)(_enemyPos.X / TileSize), (int)(_enemyPos.Y / TileSize)];
        var nodeBase = _nodes[(int)_basePos.X / TileSize, (int)_basePos.Y / TileSize];
        _waveCount++;
        var enemysToSpawn = new List<IEnemy>();
        
        switch (_waveCount)
        {
            case 1:
            {
                for (var i = 0; i < 1; i++)
                {
                    var speed = 3 + i;
                    var enemy = new Enemy(1650, 200, 100, speed, 0, _enemyPos,  nodeEnemy,nodeBase,3,  TileSize, GameObjects.Enemy, _gridWithOperators);
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
                    var sniper = new SniperEnemy(1400, 240, 100,
                        4, 0,  _enemyPos, nodeEnemy,nodeBase,10, GameObjects.EnemySniper, 0, 40, _gridWithOperators);
                    var enemy = new Enemy(1650, 200, 100, speed, 0, _enemyPos,  nodeEnemy,nodeBase,3,  TileSize, GameObjects.Enemy, _gridWithOperators);
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
            _enemiesToSpawn.Enqueue(enemys);
        }
        
    }
    
    public void SpawnCharacter(Vector2 position, GameObjects charactere)
    {
        var getCharacterToSpawn = _operators[charactere];
        if (!IsMousePositionOnTileMap(position) || Currency < getCharacterToSpawn.Currency || getCharacterToSpawn.IsSpawned) return;

        var tiledPos = new Vector2((int)position.X / TileSize, (int)position.Y / TileSize);

        if (getCharacterToSpawn.IsSniper && _wallCoordinats.Contains(tiledPos.ToPoint()))
        {
            AddSpawnCharachters(getCharacterToSpawn,tiledPos, charactere);
        }
        else if(!_wallCoordinats.Contains(tiledPos.ToPoint()) && !getCharacterToSpawn.IsSniper)
        {
            AddSpawnCharachters(getCharacterToSpawn,tiledPos,charactere);
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
        _activeOperators.Add(charachter);
        if (charachter.IsHealer)
            charachter._grid = _gridWithOperators;
        else
            charachter._grid = _gridWithEnemys;
        if (charachter is not IHasBar health) return;
        health.CurrentHealth = _opertorsHp[operatorType];
        if (charachter is not IAttackable attackableCharacter) return;
        _gridWithOperators.Add(attackableCharacter);
    }
    private bool IsMousePositionOnTileMap(Vector2 mousePosition)
    {
        var tiledMousPos = new Vector2((int)mousePosition.X / TileSize, (int)mousePosition.Y / TileSize);

        var tileMapWidth = _width;
        var tileMapHeight = _height;
        return tiledMousPos.X >= 0 && tiledMousPos.X  < tileMapWidth  &&
               tiledMousPos.Y >= 0 && tiledMousPos.Y <  tileMapHeight ;
    }
}