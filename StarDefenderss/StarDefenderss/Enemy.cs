using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarDefenderss;

public class Enemy: Character
{
    public override int HealthPoints { get; set; }
    public override int Speed { get; set; }
    public override int Attack { get; set; }
    public override int Defense { get; set; }
    public override int DamageResistance { get; set; }
    public override int GuaranteedAttack { get; set; }
    
    public Enemy(int healthPoints, int attack, int defense, int speed, int damageResistance, Texture2D texture, Vector2 position, int guaranteedAttack) : 
        base(healthPoints, attack, defense, speed, damageResistance, texture, position, guaranteedAttack) {}
}