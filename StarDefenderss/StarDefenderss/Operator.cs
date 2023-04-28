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
    public Vector2 Pos{ get; set; }
    private Vector2 _endPosition;
    public Operator(int healthPoints, int attack, int defense, int speed,  int damageResistance,
        Vector2 position, int guaranteedAttack) : base(healthPoints, attack, defense, speed, damageResistance,
        position, guaranteedAttack) {}

    public void Update()
    {
        var direction = _endPosition - Pos;
        direction.Normalize();
        Pos += direction * 100f;
    }
    public override void TakeDamage(int damage)
    {
        
    }
    
}