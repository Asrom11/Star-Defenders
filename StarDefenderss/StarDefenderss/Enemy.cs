using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarDefenderss;

public class Enemy: Character,  IAttackable,IEnemy
{
    public override int Speed { get; set; }
    public int BlockCount { get; set; }
    public int CurrentBlock { get; set; }
    public float AttackRange { get; }
    public sealed override int Attack { get; set; }
    public override int Defense { get; set; }
    public override int DamageResistance { get; set; }
    public static Texture2D Texture2D { get; set; }
    public override int GuaranteedAttack { get; set; }
    public Grid _grid { get; set; }
    public override int Currency { get; set; }
    private readonly int _tileSize;
    public float Scale { get; set; }
    public float Rotation { get; set; }
    public Color Color { get; set; }
    public Vector2 Pos { get; set; }
    public bool isOnWall { get; set; }
    public override bool IsSpawned { get; set; }
    private Vector2 _basePos;
    private Node _startNode;
    private Node _endNode;
    private List<Node> _path;
    public Enemy(int healthPoints, int attack, int defense, int speed, int damageResistance,
        Vector2 position, Node startNode, Node endNode, int guaranteedAttack, int tileSize, GameObjects enemyType, Grid grid):
        base(healthPoints, attack, defense, speed, damageResistance, position, guaranteedAttack,enemyType,  0, Color.Red)
    {
        _tileSize = tileSize;
        Pos = position;
        Color = Color.White;
        _path = PathFinding.AStar(startNode,endNode);
        _path.RemoveAt(0);
        Scale = 1f;
        AttackRange = tileSize;
        Attack = 100;
        _grid = grid;
    }
    public override void TakeDamage(int damage)
    {
        var damageFromPlayer = damage;
        CurrentHealth -= damageFromPlayer >= 0 ? damageFromPlayer : GuaranteedAttack;
    }
    
    public void Update(GameTime gameTime)
    {
        if (_path is not { Count: > 0 } ) return;
        
        var nextNode = _path[0];
        var direction = new Vector2(nextNode.X  - (int)(Pos.X / _tileSize), nextNode.Y - (int)(Pos.Y / _tileSize));
        direction.Normalize();
        
        var attackPosition = Pos + direction * _tileSize;
        var nearbyObjects = _grid.GetNearbyObjects(attackPosition, _tileSize/2);
        
        foreach (var obj in nearbyObjects)
        {   
            if (obj.CurrentBlock >= obj.BlockCount || obj.isOnWall)
                continue;
            obj.CurrentBlock++;
            obj.TakeDamage(0);
            return;
        }
        
        Pos += direction * (float)gameTime.ElapsedGameTime.TotalSeconds * Speed * 10;
        if (Vector2.Distance(Pos/ _tileSize, new Vector2(nextNode.X, nextNode.Y)) < 0.1f)
        {
            _path.Remove(nextNode);
        }
    }
}