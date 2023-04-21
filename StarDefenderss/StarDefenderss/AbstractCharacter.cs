using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarDefenderss;

public abstract class Character : IObject
{
    public int ImageId { get; set; }
    public Vector2 Pos{ get; set; }
    public Texture2D Texture { get; set; }
    public abstract int HealthPoints { get; set; }
    public abstract int Speed { get; set; }
    public abstract int Attack { get; set; }
    public abstract int Defense { get; set; }
    public abstract int DamageResistance { get; set; }
    public abstract  int GuaranteedAttack{ get; set; }

    public Character(int healthPoints, int attack, int defense, int speed,
        int damageResistance, Texture2D texture, Vector2 position, int guaranteedAttack)
    {
        HealthPoints = healthPoints;
        Attack = attack;
        Defense = defense;
        DamageResistance = damageResistance;
        Texture = texture;
        Pos = position;
        Speed = speed;
        GuaranteedAttack = guaranteedAttack;
    }
    public void Update()
    {
    }
    public void TakeDamage(int damage)
    {
        var damageFromPlayer = (damage - DamageResistance);
        HealthPoints -= damageFromPlayer >= 0 ? damageFromPlayer : GuaranteedAttack;
    }

    public void AttackOperator()
    {
    }
    
}