using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarDefenderss;

namespace StarDefenders;

public class Base: IObject
{
    public GameObjects ImageId { get; set; }
    public int UnicId { get; set; }
    public float Scale { get; set; }
    public float Rotation { get; set; }
    public Color Color { get; set; }
    public Vector2 Pos { get; set; }
    
    public Base()
    {
        Scale = 1f;
    }

}