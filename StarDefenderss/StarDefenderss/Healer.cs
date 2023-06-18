using System.Timers;
using Microsoft.Xna.Framework;

namespace StarDefenderss;

public class Healer:Character, IAttackable, IOperator
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
    public bool IsSniper { get; }
    public int TempAttack { get; set; }
    public int TempDefense { get; }
    public Timer ultimateTimer { get; }
    public GameObjects operatorType { get; }
    public override bool IsSpawned { get; set; }
    public float Scale { get; set; }
    public float Rotation { get; set; }
    public Vector2 Pos { get; set; }
    private  double _attackTimeCounter;
    
    public Healer(int healthPoints, int attack, int defense, int speed, int damageResistance, Vector2 position, int guaranteedAttack, 
        GameObjects objectType, int curency) : base(healthPoints, attack, defense, speed, damageResistance, position, guaranteedAttack, objectType, curency, Color.Blue)
    {
        operatorType = objectType;
        IsHealer = true;
        Color = Color.White;
        Scale = 1f;
        MaxHealth = healthPoints;
        CurrentHealth = healthPoints;
        Attack = attack/2;
        MaxMana = 100;
        CurrentMana = 0;
        Defense = defense;
        AttackRange = 300;
        IsSniper = true;
        IsOnWall = true;
        ultimateTimer = new Timer(10000);
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
    }

    public void Update(GameTime gameTime)
    {
        var nearbyObjects = _grid.GetNearbyObjects(Pos, AttackRange);
        if (!ultimateTimer.Enabled)
            CurrentMana = CurrentMana + 10 > 100 ? 100 : CurrentMana + 10;
        
        foreach (var obj in nearbyObjects)
        {
            _attackTimeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            if (_attackTimeCounter < 0.5) return;
            
            
            obj.Heal(Attack + TempAttack);
            _attackTimeCounter = 0;
        }
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
        TempAttack = (int) (Attack * 0.5);
        ultimateTimer.Start();
    }
}