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
    public Color Color { get; set; }
    public Vector2 Pos{ get; set; }
    public override bool IsSpawned { get; set; }
    public Grid _grid { get; set; }
    public bool isOnWall { get; set; }
    public bool isSniper { get; }

    public Operator(int healthPoints, int attack, int defense, int speed, int damageResistance,
        Vector2 position, int guaranteedAttack, int currency, GameObjects operType) : base(healthPoints, attack, defense, speed, damageResistance,
        position, guaranteedAttack,operType,currency,Color.Blue)
    {
        Color = Color.White;
        Scale = 1f;
        MaxHealth = healthPoints;
        CurrentHealth = healthPoints;
        AttackRange = 1;
        Attack = 100;
        MaxMana = 100;
        CurrentMana = 0;
        BlockCount = 2;
    }
    public override void TakeDamage(int damage)
    {
        var damageFromPlayer = damage;
        CurrentHealth -= damageFromPlayer >= 0 ? damageFromPlayer : GuaranteedAttack;
    }

    public void Update(GameTime gameTime)
    {
        var nearbyObjects = _grid.GetNearbyObjects(Pos, 80);
        CurrentMana = CurrentMana + 10 > 100 ? 100 : CurrentMana + 10;
        CurrentBlock = 0;
        foreach (var obj in nearbyObjects)
        {
            obj.TakeDamage(100);
            return;
        }
    }

    public void ActivUltimate()
    {
        if (CurrentMana != 100) return;
        CurrentHealth = 0;
        CurrentMana = 0;
    }
}