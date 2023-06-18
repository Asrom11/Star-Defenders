using System.Timers;
using Microsoft.Xna.Framework;

namespace StarDefenderss.Content;

public class PlayerSniper:Character, IAttackable, IOperator
{
    public override int Speed { get; set; }
    public int BlockCount { get; set; }
    public int CurrentBlock { get; set; }
    public float AttackRange { get; }
    public override int Attack { get; set; }
    public bool IsOnWall { get; set; }
    public bool IsHealer { get; }
    public Grid _grid { get; set; }
    public override int Defense { get; set; }
    public override int DamageResistance { get; set; }
    public override int GuaranteedAttack { get; set; }
    public override int Currency { get; set; }
    public float Scale { get; set; }
    public float Rotation { get; set; }
    public Vector2 Pos { get; set; }
    public bool IsSniper { get; }
    public int TempAttack { get; set; }
    public int TempDefense { get; }
    public Timer ultimateTimer { get; }
    public GameObjects operatorType { get; }
    public override bool IsSpawned { get; set; }
    private  double _attackTimeCounter;
    private float _tempSpeedAttackUp;
    public PlayerSniper(int healthPoints, int attack, int defense, int speed, int damageResistance, Vector2 position, int guaranteedAttack, 
        GameObjects objectType, int curency) : base(healthPoints, attack, defense, speed, damageResistance, position, guaranteedAttack, objectType, curency, Color.Blue)
    {
        operatorType = objectType;
        Color = Color.White;
        Scale = 0.5f;
        MaxHealth = healthPoints;
        CurrentHealth = healthPoints;
        AttackRange = 300;
        Attack = attack;
        MaxMana = 100;
        CurrentMana = 0;
        IsSniper = true;
        IsOnWall = IsSniper;
        Defense = defense;
        AttackRange = 60;
        ultimateTimer = new Timer(3000);
        ultimateTimer.Elapsed += OnUltimateTimerElapsed;
        ultimateTimer.AutoReset = false;
    }

    private void OnUltimateTimerElapsed(object sender, ElapsedEventArgs e)
    {
        _tempSpeedAttackUp = 0;
        TempAttack = 0;
    }

    public void Update(GameTime gameTime)
    {
        
        var nearbyObjects = _grid.GetNearbyObjects(Pos, AttackRange);
        if (!ultimateTimer.Enabled)
            CurrentMana = CurrentMana + 1.5 > 100 ? 100 : CurrentMana + 1;
        foreach (var obj in nearbyObjects)
        {
            _attackTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (_attackTimeCounter < 1.4) return;
            
            
            obj.TakeDamage(Attack + TempAttack);
            _attackTimeCounter = 0;
        }
    }
    public void Heal(int heal)
    {
        var healthWithHeal = CurrentHealth + heal;
        CurrentHealth = healthWithHeal > MaxHealth ? MaxHealth: healthWithHeal;
    }

    public override void TakeDamage(int damage)
    {
        var damageFromEnemy = (damage - (DamageResistance + Defense )) <= 0 ? (int)(damage  * 0.5) + GuaranteedAttack : damage - (DamageResistance  + Defense);
        CurrentHealth -= damageFromEnemy;
    }

    public void ActivUltimate()
    {
        if (ultimateTimer.Enabled) return;
        CurrentMana = 0;
        _tempSpeedAttackUp = 0.6f;
        TempAttack = (int)(Attack * 0.15);
        ultimateTimer.Start();
    }

}