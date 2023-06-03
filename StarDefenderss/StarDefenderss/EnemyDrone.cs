using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StarDefenderss;

public class EnemyDrone: Character, IAttackable,IEnemy
{
    public override int Speed { get; set; }
    public int BlockCount { get; set; }
    public int CurrentBlock { get; set; }
    public float AttackRange { get; }
    public override int Attack { get; set; }
    public Grid _grid { get; set; }
    public override int Defense { get; set; }
    public override int DamageResistance { get; set; }
    public override int GuaranteedAttack { get; set; }
    public override int Currency { get; set; }
    public float Scale { get; set; }
    public bool isOnWall { get; set; }
    public float Rotation { get; set; }
    public Vector2 Pos { get; set; }
    public override bool IsSpawned { get; set; }
    private readonly int _tileSize;
    static List<Node> _path;
    private Vector2 startPos;
    private Vector2 endPos;

    public EnemyDrone(int healthPoints, int attack, int defense, int speed, int damageResistance, 
        Vector2 position, List<Node> bestPath,int guaranteedAttack, GameObjects objectType, int curency, int tileSize) : base(healthPoints, attack, defense, speed, damageResistance, position, guaranteedAttack, objectType, curency, Color.Red)
    {
        _tileSize = tileSize;
        Pos = position;
        Color = Color.White;
        _path = bestPath;
        this.startPos = new Vector2(startPos.X, startPos.Y);
        this.endPos = new Vector2(endPos.X, endPos.Y);
        Scale = 1f;
    }
    
    public void Update(GameTime gameTime)
    {
        if (_path is not { Count: > 0 } ) return;
        
        var nextNode = _path[0];
        var direction = new Vector2(nextNode.X  - (int)(Pos.X / _tileSize), nextNode.Y - (int)(Pos.Y / _tileSize));
        direction.Normalize();

        Pos += direction * (float)gameTime.ElapsedGameTime.TotalSeconds * Speed * 10;
        if (Vector2.Distance(Pos/ _tileSize, new Vector2(nextNode.X, nextNode.Y)) < 0.1f)
        {
            _path.Remove(nextNode);
        }
    }
    
    public void TakeDamage(int damage)
    {
        var damageFromPlayer = damage;
        CurrentHealth -= damageFromPlayer >= 0 ? damageFromPlayer : GuaranteedAttack;
    }
}