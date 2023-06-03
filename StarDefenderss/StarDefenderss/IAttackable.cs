namespace StarDefenderss;

public interface IAttackable:  IObject
{
    int BlockCount { get; set; }
    int CurrentBlock { get; set; }
    float AttackRange { get; }
    int Attack { get; }
    void TakeDamage(int damage);
    
    bool isOnWall { get; set; }

    Grid _grid { get; set; }
}