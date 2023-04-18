using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarDefenders;

public class Field
{
    private Square[,] _squares;
    private Texture2D _texture;
    private int _squareSize;
 
    public Field(int rows, int columns, Texture2D texture, int squareSize)
    {
        _squares = new Square[rows, columns];
        _texture = texture;
        _squareSize = squareSize;
 
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                _squares[i, j] = new Square(_texture, new Vector2(i * _squareSize, j * _squareSize));
            }
        }
    }
 
    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Square square in _squares)
        {
            square.Draw(spriteBatch);
        }
    }
}