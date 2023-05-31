using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarDefenderss;

public class Operator: Character, IObject, IAttackable
{
    public override int Speed { get; set; }
    public float AttackRange { get; }
    public override int Attack { get; set; }
    public override int Defense { get; set; }
    public override int DamageResistance { get; set; }
    public override int GuaranteedAttack { get; set; }
    public override int Currency { get; set; }
    public float Scale { get; set; }
    public float Rotation { get; set; }
    public Color Color { get; set; }
    public Vector2 Pos{ get; set; }
    public bool IsSpawned { get; set; }
    public override Direction Direction { get; set; }

    public Operator(int healthPoints, int attack, int defense, int speed, int damageResistance,
        Vector2 position, int guaranteedAttack, int currency, GameObjects operType) : base(healthPoints, attack, defense, speed, damageResistance,
        position, guaranteedAttack,operType,currency,Color.Blue)
    {
        Color = Color.White;
        Scale = 1f;
    }
    public override void TakeDamage(int damage)
    {
    }

    public Grid _grid { get; set; }

    public void Update(GameTime gameTime)
    {
    }
}