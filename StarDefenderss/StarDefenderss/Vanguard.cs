using System.Timers;
using Microsoft.Xna.Framework;

namespace StarDefenderss;

public class Vanguard:Character, IAttackable, IOperator
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
    public bool IsHealer { get; }
    public Grid _grid { get; set; }
    public bool IsOnWall { get; set; }
    public bool IsSniper { get;}
    public int TempAttack { get; set; }
    public int TempDefense { get; set; }
    public Timer ultimateTimer { get; }
    public GameObjects operatorType { get; }
    private  double _attackTimeCounter;
    private float _tempSpeedAttackUp = 0;

    public Vanguard(int healthPoints, int attack, int defense, int speed, int damageResistance,
        Vector2 position, int guaranteedAttack, int currency, GameObjects operType) : base(healthPoints, attack, defense, speed, damageResistance,
        position, guaranteedAttack,operType,currency,Color.Blue)
    {
        operatorType = operType;
        Currency = currency;
        Color = Color.White;
        Scale = 1f;
        MaxMana = 100;
        BlockCount = 2;
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
        _tempSpeedAttackUp = 0;
        TempDefense = 0;
    }
    public override void TakeDamage(int damage)
    {
        var damageFromEnemy = (damage - (DamageResistance + Defense )) <= 0 ? (int)(damage  * 0.5) + GuaranteedAttack : damage - (DamageResistance  + Defense);
        CurrentHealth -= damageFromEnemy;
    }

    public void Update(GameTime gameTime)
    {
        if (!ultimateTimer.Enabled)
            CurrentMana = CurrentMana + 10 > 100 ? 100 : CurrentMana + 10;
        CurrentBlock = 0;
        var nearbyObjects = _grid.GetNearbyObjects(Pos, AttackRange);
        foreach (var obj in nearbyObjects)
        {
            _attackTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (_attackTimeCounter < 1 - _tempSpeedAttackUp) return;
            
            obj.TakeDamage(Attack + TempAttack);
            _attackTimeCounter = 0;
            return;
        }
        _attackTimeCounter = 0;
    }
    public void Heal(int heal)
    {
        var healthWithHeal = CurrentHealth + heal;
        CurrentHealth = healthWithHeal > MaxHealth ? MaxHealth: healthWithHeal;
    }

    public void ActivUltimate()
    {
        if (ultimateTimer.Enabled) return;
        CurrentMana = 0;
        _tempSpeedAttackUp = 0.5f;
        TempDefense = (int)(Defense * 0.2);
        ultimateTimer.Start();
    }
    
}