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
    public bool IsOnWall { get; set; }
    public float Rotation { get; set; }
    public Vector2 Pos { get; set; }
    public override bool IsSpawned { get; set; }
    private readonly int _tileSize;
    static List<Node> _path;
    private Vector2 _startPos;
    private Vector2 _endPos;

    public EnemyDrone(int healthPoints, int attack, int defense, int speed, int damageResistance, 
        Vector2 position, List<Node> bestPath,int guaranteedAttack, GameObjects objectType, int curency, int tileSize) : base(healthPoints, attack, defense, speed, damageResistance, position, guaranteedAttack, objectType, curency, Color.Red)
    {
        _tileSize = tileSize;
        Pos = position;
        Color = Color.White;
        _path = bestPath;
        this._startPos = new Vector2(_startPos.X, _startPos.Y);
        this._endPos = new Vector2(_endPos.X, _endPos.Y);
        Scale = 1f;
        IsOnWall = true;
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
    
    public override void TakeDamage(int damage)
    {
        var damageFromEnemy = (damage - (DamageResistance + Defense )) <= 0 ? (int)(damage  * 0.5) + GuaranteedAttack : damage - (DamageResistance  + Defense);
        CurrentHealth -= damageFromEnemy;
    }
    public void Heal(int heal)
    {
    }
}