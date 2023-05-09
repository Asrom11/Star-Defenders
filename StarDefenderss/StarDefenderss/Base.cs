using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarDefenderss;

namespace StarDefenders;

public class Base: IObject
{
    public GameObjects ImageId { get; set; }
    public int UnicId { get; set; }
    public Color Color { get; set; }
    public Vector2 Pos { get; set; }
    public Texture2D Texture { get; set; }
    public Vector2 Position { get; set; }
    public bool IsEmpty { get; set; }
    public Rectangle Rectangle {get;}
    
    public bool isEnemyBase;
 
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Rectangle, Color);
    }
    public void Update(GameTime gameTime)
    {
    }
}