using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarDefenders;

public class Square
{
    public Texture2D Texture { get; set; }
    public Vector2 Position { get; set; }
    public bool IsEmpty { get; set; }
    public Rectangle Rectangle {get;}
 
    public Square(Texture2D texture, Vector2 position, Rectangle rectangle)
    {
        Texture = texture;
        Position = position;
        IsEmpty = true;
        Rectangle = rectangle;
    }
 
    public void Draw(SpriteBatch spriteBatch,Color color)
    {
        spriteBatch.Draw(Texture, Position, Rectangle, color);
    }
}