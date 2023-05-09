using Microsoft.Xna.Framework;

namespace StarDefenderss;

public class Wall: IObject
{
    public GameObjects ImageId { get; set; }
    public int UnicId { get; set; }
    public Color Color
    {
        get
        {return  Color.White;}
        set{}
    }

    public Vector2 Pos { get; set; }
    public void Update(GameTime gameTime)
    {
    }
}