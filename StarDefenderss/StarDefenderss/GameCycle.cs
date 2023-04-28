using System;
using System.Collections.Generic;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarDefenderss;

public class GameCycle: IGameplayModel
{
    public Dictionary<int, IObject> Objects { get; set; }
    public event EventHandler<GameplayEventArgs> Updated;
    public event EventHandler<EnemyMovedEventArgs> EnemyMoved;
    Timer _spawnTimer; 
    private int _currentId = 5;
    public int PlayerId { get; set; }
    private const float spawnInterval = 1000; 
    public void Initialize()
    {
        Objects = new Dictionary<int, IObject>();
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
        var enemy = new Enemy (100,1,2,4, 0, new Vector2(0,0), 3, new Vector2(1280,720));
        _currentId++;
        lock (Objects)
            Objects.Add (_currentId, enemy);
    }
}