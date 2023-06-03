using Microsoft.Xna.Framework;

namespace StarDefenderss;

public class Wall: IObject
{
    public GameObjects ImageId { get; set; }
    public int Currency { get; set; }
    public int UnicId { get; set; }
    public float Scale { get; set; }
    public float Rotation { get; set; }

    public Color Color
    {
        get
        {return  Color.White;}
        set{}
    }

    public Wall()
    {
        Scale = 1f;
    }
    public Vector2 Pos { get; set; }
}