using Microsoft.Xna.Framework;

namespace StarDefenderss;

public interface IObject
{
    int ImageId { get; set; }

    int UnicId
    {
        get;
        set;
    }

    Vector2 Pos { get; set; }

    void Update(GameTime gameTime);  
}