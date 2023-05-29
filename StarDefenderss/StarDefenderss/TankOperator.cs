using Microsoft.Xna.Framework;

namespace StarDefenderss;

public class TankOperator: Character, IObject
{
    public override int HealthPoints { get; set; }
    public override int Speed { get; set; }
    public override int Attack { get; set; }
    public override int Defense { get; set; }
    public override int DamageResistance { get; set; }
    public override int GuaranteedAttack { get; set; }
    public override int Currency { get; set; }
    public float Scale { get; set; }
    public float Rotation { get; set; }
    public Color Color { get; set; }
    public Direction dir { get; set; }
    public bool IsSpawned { get; set; }

    public TankOperator(int healthPoints, int attack, int defense, int speed, int damageResistance, Vector2 position, int guaranteedAttack, int currency, GameObjects OperatorType) : base(healthPoints, attack, defense, speed, damageResistance, position, guaranteedAttack, OperatorType, currency)
    {
        Currency = currency;
        Color = Color.White;
        Scale = 1f;
    }
}

// Убрать стоимость у асбтрактного персонажа, она тут нафиг не нужна