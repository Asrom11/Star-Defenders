using Microsoft.Xna.Framework;

namespace StarDefenderss;

public interface IObject
{
    int ImageId { get; set; }   

    Vector2 Pos { get; }
    
    void Update();  
}