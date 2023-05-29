using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarDefenderss;

namespace StarDefenders;

public class Base: IObject
{
    public GameObjects ImageId { get; set; }
    public int Currency { get; set; }
    
    public int UnicId { get; set; }
    public float Scale { get; set; }
    public float Rotation { get; set; }
    public Color Color { get; set; }
    public Direction dir { get; set; }
    public Vector2 Pos { get; set; }
    public bool IsSpawned { get; set; }
    public Texture2D Texture { get; set; }
    public Vector2 Position { get; set; }
    public bool IsEmpty { get; set; }
    public Rectangle Rectangle {get;}
    
    public bool isEnemyBase;

    public Base()
    {
        Scale = 1f;
    }
    public void Update(GameTime gameTime)
    {
    }
}