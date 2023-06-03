using System.Timers;
using Microsoft.Xna.Framework;

namespace StarDefenderss.Content;

public class PlayerSniper:Character, IObject, IAttackable, IOperator
{
    public override int Speed { get; set; }
    public int BlockCount { get; set; }
    public int CurrentBlock { get; set; }
    public float AttackRange { get; }
    public override int Attack { get; set; }
    public bool IsOnWall { get; set; }
    public Grid _grid { get; set; }
    public override int Defense { get; set; }
    public override int DamageResistance { get; set; }
    public override int GuaranteedAttack { get; set; }
    public override int Currency { get; set; }
    public float Scale { get; set; }
    public float Rotation { get; set; }
    public Vector2 Pos { get; set; }
    public bool IsSniper { get; }
    public int TempAttack { get; }
    public int TempDefense { get; }
    public Timer ultimateTimer { get; }
    public override bool IsSpawned { get; set; }
    public PlayerSniper(int healthPoints, int attack, int defense, int speed, int damageResistance, Vector2 position, int guaranteedAttack, 
        GameObjects objectType, int curency) : base(healthPoints, attack, defense, speed, damageResistance, position, guaranteedAttack, objectType, curency, Color.Blue)
    {
        Color = Color.White;
        Scale = 0.5f;
        MaxHealth = healthPoints;
        CurrentHealth = healthPoints;
        AttackRange = 150;
        Attack = attack;
        MaxMana = 100;
        CurrentMana = 0;
        IsSniper = true;
        IsOnWall = IsSniper;
        Defense = defense;
        AttackRange = 60;
    }
    public void Update(GameTime gameTime)
    {
        
        var nearbyObjects = _grid.GetNearbyObjects(Pos, AttackRange);
        CurrentMana = CurrentMana + 10 > 100 ? 100 : CurrentMana + 10;
        foreach (var obj in nearbyObjects)
        {
            obj.TakeDamage(Attack);
            return;
        }
    }

    public override void TakeDamage(int damage)
    {
        var damageFromEnemy = (damage - (DamageResistance + TempDefense)) < 0 ? 0 : damage - (DamageResistance + TempDefense);
        CurrentHealth -= damageFromEnemy >= 0 ? damageFromEnemy : GuaranteedAttack;
    }

    public void ActivUltimate()
    {
        CurrentMana = 0;
        CurrentHealth = 0;
    }

}