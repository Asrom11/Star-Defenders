namespace StarDefenderss;

public interface IAttackable:  IObject
{
    int BlockCount { get; set; }
    int CurrentBlock { get; set; }
    float AttackRange { get; }
    int Attack { get; }
    void TakeDamage(int damage);
    void Heal(int heal);
    
    bool IsOnWall { get; set; }

    Grid _grid { get; set; }
}