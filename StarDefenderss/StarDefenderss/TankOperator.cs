using System.Collections.Generic;
using System.Timers;
using Microsoft.Xna.Framework;

namespace StarDefenderss;

public class TankOperator: Character, IAttackable,IOperator
{
    public override int Speed { get; set; }
    public int BlockCount { get; set; }
    public int CurrentBlock { get; set; }
    public float AttackRange { get; }
    public override int Attack { get; set; }
    public bool IsHealer { get; }
    public Grid _grid { get; set; }
    public override int Defense { get; set; }
    public override int DamageResistance { get; set; }
    public override int GuaranteedAttack { get; set; }
    public override int Currency { get; set; }
    public bool IsOnWall { get; set; }
    public float Scale { get; set; }
    public float Rotation { get; set; }
    public new Color Color { get; set; }
    public Vector2 Pos { get; set; }
    public override bool IsSpawned { get; set; }
    public bool IsSniper { get; }
    public int TempAttack { get; set; }
    public int TempDefense { get; set; }
    public Timer ultimateTimer { get; }
    public GameObjects operatorType { get; }
    private  double _attackTimeCounter;

    public TankOperator(int healthPoints, int attack, int defense, int speed, int damageResistance, Vector2 position, int guaranteedAttack, int currency, GameObjects OperatorType) : 
        base(healthPoints, attack, defense, speed, damageResistance, position, guaranteedAttack, OperatorType, currency, Color.Blue)
    {
        operatorType = OperatorType;
        Currency = currency;
        Color = Color.White;
        Scale = 1f;
        MaxMana = 100;
        BlockCount = 4;
        MaxHealth = healthPoints;
        CurrentHealth = healthPoints;
        Defense = defense;
        AttackRange = 60;
        Attack = attack;
        ultimateTimer = new Timer(3000);
        ultimateTimer.Elapsed += OnUltimateTimerElapsed;
        ultimateTimer.AutoReset = false;
    }
    public void Heal(int heal)
    {
        var healthWithHeal = CurrentHealth + heal;
        CurrentHealth = healthWithHeal > MaxHealth ? MaxHealth: healthWithHeal;
    }
    private void OnUltimateTimerElapsed(object sender, ElapsedEventArgs e)
    {
        TempAttack = 0;
        TempDefense = 0;
    }
    public void Update(GameTime gameTime)
    {
        if (!ultimateTimer.Enabled)
            CurrentMana = CurrentMana + 10 > 100 ? 100 : CurrentMana + 10;
        var nearbyObjects = _grid.GetNearbyObjects(Pos, AttackRange);
        CurrentBlock = 0;
        foreach (var obj in nearbyObjects)
        {
            if (obj.IsOnWall) continue;
            _attackTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (_attackTimeCounter < 1) return;
            
            obj.TakeDamage(Attack + TempAttack);
            _attackTimeCounter = 0;
        }
    }
    

    public void ActivUltimate()
    {
        if (ultimateTimer.Enabled) return;
        CurrentMana = 0;
        TempDefense = 1000;
        ultimateTimer.Start();
    }

    public override void TakeDamage(int damage)
    {
        var damageFromEnemy = (damage - (DamageResistance + Defense )) <= 0 ? (int)(damage  * 0.5) + GuaranteedAttack : damage - (DamageResistance  + Defense);
        CurrentHealth -= damageFromEnemy;
    }
}