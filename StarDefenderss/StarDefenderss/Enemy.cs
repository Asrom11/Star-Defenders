using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarDefenderss;

public class Enemy: Character, IObject, IAttackable
{
    public override Direction Direction { get; set; }
    public override int Speed { get; set; }
    public float AttackRange { get; }
    public override int Attack { get; set; }
    public override int Defense { get; set; }
    public override int DamageResistance { get; set; }
    public static Texture2D Texture2D { get; set; }
    public override int GuaranteedAttack { get; set; }
    public override int Currency { get; set; }
    private readonly int _tileSize;
    public float Scale { get; set; }
    public float Rotation { get; set; }
    public Color Color { get; set; }
    public Vector2 Pos { get; set; }
    public bool IsSpawned { get; set; }
    private Vector2 _basePos;
    private Node _startNode;
    private Node _endNode;
    private List<Node> _path;
    public Enemy(int healthPoints, int attack, int defense, int speed, int damageResistance,
        Vector2 position, Node starPos, int guaranteedAttack, Node basePos, int tileSize, GameObjects enemyType):
        base(healthPoints, attack, defense, speed, damageResistance, position, guaranteedAttack,enemyType,  0, Color.Red)
    {
        _tileSize = tileSize;
        Pos = position;
        Color = Color.White;
        _path = PathFinding.AStar(starPos, basePos);
        _path.RemoveAt(0);
        Scale = 1f;
        AttackRange = tileSize;
    }
    public override void TakeDamage(int damage)
    {
    }

    public Grid _grid { get; set; }

    // todo character.BlocksPath т.к ограничение на блок
    public void Update(GameTime gameTime)
    {
        if (_path is not { Count: > 0 } ) return;
        var nearbyObjects = _grid.GetNearbyObjects(Pos, AttackRange);
        foreach (var obj in nearbyObjects)
        {
            obj.TakeDamage(Attack);
            return;
        }

        var nextNode = _path[0];
        var direction = new Vector2(nextNode.X - (Pos.X / _tileSize), nextNode.Y - (Pos.Y / _tileSize));
        direction.Normalize();

        Pos += direction * (float)gameTime.ElapsedGameTime.TotalSeconds * Speed * 10;
        if (Vector2.Distance(Pos/ _tileSize, new Vector2(nextNode.X, nextNode.Y)) < 0.1f)
        {
            _path.RemoveAt(0);
        }
    }
}