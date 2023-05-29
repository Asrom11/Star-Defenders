using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarDefenderss;

public class Operator: Character, IObject
{
    public override int HealthPoints { get; set; }
    public override int Speed { get; set; }
    public override int Attack { get; set; }
    public override int Defense { get; set; }
    public override int DamageResistance { get; set; }
    public override int GuaranteedAttack { get; set; }
    public override int Currency { get; set; }
    public Vector2 position { get; set; }

    public Texture2D IconTexture { get; set; }
    public float Scale { get; set; }
    public float Rotation { get; set; }
    public Color Color { get; set; }
    public Direction dir { get; set; }
    public Vector2 Pos{ get; set; }
    public bool IsSpawned { get; set; }
    private Vector2 _endPosition;

    public Operator(int healthPoints, int attack, int defense, int speed, int damageResistance,
        Vector2 position, int guaranteedAttack, int currency, GameObjects operType) : base(healthPoints, attack, defense, speed, damageResistance,
        position, guaranteedAttack,operType,currency)
    {
        Color = Color.White;
        Scale = 1f;
    }

    public void Update()
    {
    }
    public override void TakeDamage(int damage)
    {
        
    }
}