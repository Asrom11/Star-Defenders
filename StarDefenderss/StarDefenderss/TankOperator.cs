using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StarDefenderss;

public class TankOperator: Character, IObject, IAttackable,IOperator
{
    public override int Speed { get; set; }
    public Vector2 Position { get; }
    public float AttackRange { get; }
    public override int Attack { get; set; }
    public Grid _grid { get; set; }
    public override int Defense { get; set; }
    public override int DamageResistance { get; set; }
    public override int GuaranteedAttack { get; set; }
    public override int Currency { get; set; }
    public float Scale { get; set; }
    public float Rotation { get; set; }
    public Color Color { get; set; }
    public override bool IsSpawned { get; set; }

    public TankOperator(int healthPoints, int attack, int defense, int speed, int damageResistance, Vector2 position, int guaranteedAttack, int currency, GameObjects OperatorType) : 
        base(healthPoints, attack, defense, speed, damageResistance, position, guaranteedAttack, OperatorType, currency, Color.Blue)
    {
        Currency = currency;
        Color = Color.White;
        Scale = 1f;
    }
    public void Update(GameTime gameTime)
    {
        var nearbyObjects = _grid.GetNearbyObjects(Pos, AttackRange);
        foreach (var obj in nearbyObjects)
        {
            obj.TakeDamage(Attack);
        }
    }

    public void ActivUltimate()
    {
        CurrentHealth = 0;
    }

    public override void TakeDamage(int damage)
    {
        var damageFromEnemy = (damage - DamageResistance);
        CurrentHealth -= damageFromEnemy >= 0 ? damageFromEnemy : GuaranteedAttack;
    }
}

// Убрать стоимость у асбтрактного персонажа, она тут нафиг не нужна