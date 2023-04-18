using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarDefenders;

public class Square
{
    public Texture2D Texture { get; set; }
    public Vector2 Position { get; set; }
    public bool IsEmpty { get; set; }
 
    public Square(Texture2D texture, Vector2 position)
    {
        Texture = texture;
        Position = position;
        IsEmpty = true;
    }
 
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
}