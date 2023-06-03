using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StarDefenderss;

public class SniperEnemy: Character, IObject, IAttackable,IEnemy
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
    public override bool IsSpawned { get; set; }
    public bool isOnWall { get; set; }
    public float Scale { get; set; }
    public float Rotation { get; set; }
    public Vector2 Pos { get; set; }
    static List<Node> _path;
    private readonly int _tileSize;
    private  double _attackTimeCounter;
    private bool _distanceAtackTime;
    private bool _canMove;

    public SniperEnemy(int healthPoints, int attack, int defense, int speed, int damageResistance, 
        Vector2 position, Node start, Node end, int guaranteedAttack, GameObjects objectType, int curency, int tileSize, Grid grid) : base(healthPoints, attack, defense, speed, damageResistance, position, guaranteedAttack, objectType, curency, Color.Red)
    {
        _tileSize = tileSize;
        Pos = position;
        Color = Color.White;
        _path = PathFinding.AStar(start,end);
        _path.RemoveAt(0);
        Scale =1;
        AttackRange = 1000;
        Attack = 100;
        _attackTimeCounter = 0;
        _distanceAtackTime = true;
        _canMove = true;
        _grid = grid;
        DamageResistance = 1;

    }
        
    public void Update(GameTime gameTime)
    {
        if (_path is not { Count: > 0 } ) return;

        var nextNode = _path[0];
        var direction = new Vector2(nextNode.X  - (int)(Pos.X / _tileSize), nextNode.Y - (int)(Pos.Y / _tileSize));
        direction.Normalize();
        
        var attackPosition = Pos + direction * _tileSize;
        var nearbyObjects = _grid.GetNearbyObjects(attackPosition, AttackRange);
        _attackTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
        foreach (var obj in nearbyObjects)
        {
            obj.BlockCount++;
            if (obj.CurrentBlock >= obj.BlockCount)
                continue;
            if (Vector2.Distance(Pos, obj.Pos) > _tileSize && _distanceAtackTime)
            {
                obj.TakeDamage(1);
                _distanceAtackTime = false;
                _canMove = false;
                return;
            }

            if (!(Vector2.Distance(Pos, obj.Pos) < _tileSize)) continue;
            
            obj.TakeDamage(1);
            return;
        }

        if (_attackTimeCounter >=3)
        {
            _attackTimeCounter = 0;
            _distanceAtackTime = true;
        }

        if (_attackTimeCounter >= 1)
            _canMove = true;

        if (!_canMove) return;
        Pos += direction * (float)gameTime.ElapsedGameTime.TotalSeconds * Speed * 10;
        if (Vector2.Distance(Pos/ _tileSize, new Vector2(nextNode.X, nextNode.Y)) < 0.1f)
        {
            _path.Remove(nextNode);
        }
    }
    
    public void TakeDamage(int damage)
    {
        var damageFromPlayer = (damage * DamageResistance);
        CurrentHealth -= damageFromPlayer >= 0 ? damageFromPlayer : GuaranteedAttack;
    }
}