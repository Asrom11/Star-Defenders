using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarDefenderss;

public abstract class Character: IHasHealth
{
    public GameObjects ImageId { get; set; }
    public int UnicId { get; set; }
    public Vector2 Pos{ get; set; }
    public int CurrentHealth { get; set; }
    public int MaxHealth { get; set; }
    public Color _HealthColor { get; }
    public Color Color { get; set; }
    public abstract int Speed { get; set; }
    public abstract int Attack { get; set; }
    public abstract int Defense { get; set; }
    public abstract int DamageResistance { get; set; }
    public abstract  int GuaranteedAttack{ get; set; }
    public abstract int Currency { get; set; }

    public abstract bool IsSpawned { get; set; }

    public Character(int healthPoints, int attack, int defense, int speed,
        int damageResistance, Vector2 position, int guaranteedAttack, GameObjects objectType, int curency, Color healthColor)
    {
        _HealthColor = healthColor;
        Currency = curency;
        MaxHealth = healthPoints;
        CurrentHealth = healthPoints;
        Attack = attack;
        Defense = defense;
        DamageResistance = damageResistance;
        Speed = speed;
        GuaranteedAttack = guaranteedAttack;
        ImageId = objectType;
    }
    public virtual void TakeDamage(int damage)
    {
        var damageFromPlayer = (damage * DamageResistance);
        CurrentHealth -= damageFromPlayer >= 0 ? damageFromPlayer : GuaranteedAttack;
    }
}