using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarDefenderss;

public class Enemy: Character, IObject
{
    public override int HealthPoints { get; set; }
    public override int Speed { get; set; }
    public override int Attack { get; set; }
    public override int Defense { get; set; }
    public override int DamageResistance { get; set; }
    public static Texture2D Texture2D { get; set; }
    public override int GuaranteedAttack { get; set; }
    public Color Color { get; set; }
    public Vector2 Pos { get; set; }
    private Vector2 _basePos;

    public Enemy(int healthPoints, int attack, int defense, int speed, int damageResistance,
        Vector2 position, int guaranteedAttack, Vector2 basePos):
        base(healthPoints, attack, defense, speed, damageResistance, position, guaranteedAttack)
    {
        ImageId = GameObjects.Enemy;
        Pos = position;
        _basePos = basePos;
        Color = Color.White;
    }
    public void Update(GameTime gameTime)
    {
        var direction = _basePos - Pos;
        direction.Normalize();
        Pos += direction * (float)gameTime.ElapsedGameTime.TotalSeconds * 100f;
    }
    public override void TakeDamage(int damage)
    {
        
    }

}