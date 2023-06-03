using System.Timers;
using Microsoft.Xna.Framework;

namespace StarDefenderss;

public class Operator: Character, IAttackable, IOperator
{
    public override int Speed { get; set; }
    public int BlockCount { get; set; }
    public int CurrentBlock { get; set; }
    public float AttackRange { get; }
    public override int Attack { get; set; }
    public override int Defense { get; set; }
    public override int DamageResistance { get; set; }
    public override int GuaranteedAttack { get; set; }
    public override int Currency { get; set; }
    public float Scale { get; set; }
    public float Rotation { get; set; }
    public new Color Color { get; set; }
    public Vector2 Pos{ get; set; }
    public override bool IsSpawned { get; set; }
    public Grid _grid { get; set; }
    public bool IsOnWall { get; set; }
    public bool IsSniper { get;}
    public int TempAttack { get; set; }
    public int TempDefense { get; set; }
    public Timer ultimateTimer { get; }

    public Operator(int healthPoints, int attack, int defense, int speed, int damageResistance,
        Vector2 position, int guaranteedAttack, int currency, GameObjects operType) : base(healthPoints, attack, defense, speed, damageResistance,
        position, guaranteedAttack,operType,currency,Color.Blue)
    {
        Currency = currency;
        Color = Color.White;
        Scale = 1f;
        MaxMana = 100;
        BlockCount = 4;
        MaxHealth = healthPoints;
        CurrentHealth = healthPoints;
        Defense = defense;
        AttackRange = 80;
        Attack = attack;
        ultimateTimer = new Timer(3000);
        ultimateTimer.Elapsed += OnUltimateTimerElapsed;
        ultimateTimer.AutoReset = false;
    }
    
    private void OnUltimateTimerElapsed(object sender, ElapsedEventArgs e)
    {
        TempAttack = 0;
        TempDefense = 0;
        CurrentHealth -= 50;
    }
    public override void TakeDamage(int damage)
    {
        var damageFromEnemy = (damage - (DamageResistance + TempDefense)) < 0 ? 0 : damage - (DamageResistance + TempDefense);
        CurrentHealth -= damageFromEnemy >= 0 ? damageFromEnemy : GuaranteedAttack;
    }

    public void Update(GameTime gameTime)
    {
        var nearbyObjects = _grid.GetNearbyObjects(Pos, AttackRange);
        CurrentMana = CurrentMana + 10 > 100 ? 100 : CurrentMana + 10;
        CurrentBlock = 0;
        foreach (var obj in nearbyObjects)
        {
            obj.TakeDamage(Attack + TempAttack);
            return;
        }
    }

    public void ActivUltimate()
    {
        if (ultimateTimer.Enabled) return;
        CurrentMana = 0;
        TempAttack = 300;
        TempDefense = 300;
        ultimateTimer.Start();
    }
    
}