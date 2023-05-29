using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarDefenderss;

public abstract class Character
{
    public GameObjects ImageId { get; set; }
    public int UnicId { get; set; }
    public void Update(GameTime gameTime)
    {
    }
    public int Ditrection { get; set; }
    public Vector2 Pos{ get; set; }
    public abstract int HealthPoints { get; set; }
    public abstract int Speed { get; set; }
    public abstract int Attack { get; set; }
    public abstract int Defense { get; set; }
    public abstract int DamageResistance { get; set; }
    public abstract  int GuaranteedAttack{ get; set; }
    public abstract int Currency { get; set; }

    public Character(int healthPoints, int attack, int defense, int speed,
        int damageResistance, Vector2 position, int guaranteedAttack, GameObjects objectType, int curency)
    {
        Currency = curency;
        HealthPoints = healthPoints;
        Attack = attack;
        Defense = defense;
        DamageResistance = damageResistance;
        Pos = position;
        Speed = speed;
        GuaranteedAttack = guaranteedAttack;
        ImageId = objectType;
    }
    public virtual void TakeDamage(int damage)
    {
        var damageFromPlayer = (damage - DamageResistance);
        HealthPoints -= damageFromPlayer >= 0 ? damageFromPlayer : GuaranteedAttack;
    }

    public virtual void AttackOperator()
    {
    }
    
}