using System;
using System.Collections.Generic;

namespace StarDefenderss;

public class GameCycle: IGameplayModel
{
    public Dictionary<int, IObject> Objects { get; set; }
    public event EventHandler<GameplayEventArgs> Updated;
    public void Initialize()
    {
        Objects = new Dictionary<int, IObject>();
    }

    public void Update()
    {
        foreach (var o in Objects.Values)
        {
            o.Update();
        }
        Updated.Invoke(this, new GameplayEventArgs { Objects = this.Objects });      
        Updated.Invoke(this, new GameplayEventArgs());
    }

    public void ChangeEnemyPosition()
    {
        // Сначала по тупому для теста чисто его в право, а потом уже сделать по умному
        
    }
}