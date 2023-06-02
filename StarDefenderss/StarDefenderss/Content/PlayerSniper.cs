using Microsoft.Xna.Framework;

namespace StarDefenderss.Content;

public class PlayerSniper:Character, IObject, IAttackable, IOperator
{
    public override int Speed { get; set; }
    public int BlockCount { get; set; }
    public int CurrentBlock { get; set; }
    public float AttackRange { get; }
    public override int Attack { get; set; }
    public Grid _grid { get; set; }
    public override int Defense { get; set; }
    public override int DamageResistance { get; set; }
    public override int GuaranteedAttack { get; set; }
    public override int Currency { get; set; }
    public float Scale { get; set; }
    public float Rotation { get; set; }
    public Vector2 Pos { get; set; }
    public override bool IsSpawned { get; set; }
    public bool isSniper { get; }
    public PlayerSniper(int healthPoints, int attack, int defense, int speed, int damageResistance, Vector2 position, int guaranteedAttack, 
        GameObjects objectType, int curency) : base(healthPoints, attack, defense, speed, damageResistance, position, guaranteedAttack, objectType, curency, Color.Blue)
    {
        Color = Color.White;
        Scale = 1f;
        MaxHealth = 100;
        CurrentHealth = 100;
        AttackRange = 150;
        Attack = 100;
        MaxMana = 100;
        CurrentMana = 0;
        isSniper = true;
        BlockCount = 1;
    }
    public void Update(GameTime gameTime)
    {
        
        var nearbyObjects = _grid.GetNearbyObjects(Pos, AttackRange);
        CurrentMana = CurrentMana + 10 > 100 ? 100 : CurrentMana + 10;
        foreach (var obj in nearbyObjects)
        {
            obj.TakeDamage(1);
            return;
        }
    }
    

    public void ActivUltimate()
    {
    }

}