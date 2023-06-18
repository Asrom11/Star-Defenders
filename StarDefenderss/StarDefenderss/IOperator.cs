using Microsoft.Xna.Framework;
using System.Timers;
namespace StarDefenderss;

public interface IOperator: IObject
{
    bool IsSpawned { get; set; }
    bool IsHealer { get; }
    Grid _grid { get; set; }
    
    int Currency { get; set; }
    void Update(GameTime gameTime);
    bool IsSniper { get; }

    int TempAttack { get; }
    int TempDefense { get; }
    Timer ultimateTimer { get; }
    GameObjects operatorType { get;}
    
    void OnUltimateTimerElapsed(object sender, ElapsedEventArgs e){}

    void ActivUltimate();
}