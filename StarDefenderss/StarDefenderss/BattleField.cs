using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarDefenders;

public class Field
{
    private Square[,] _squares;
    private Texture2D _texture;
    private int _cellSize = 64;

    public Field(int rows, int columns, Texture2D texture)
    {
        _squares = new Square[rows, columns];
        _texture = texture;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                _squares[i, j] = new Square(_texture, new Vector2(i * _cellSize, j * _cellSize), new Rectangle(j * _cellSize, i * _cellSize, _cellSize, _cellSize));
            }
        }
    }
 
    public void Draw(SpriteBatch spriteBatch)
    {
        _squares[0,0].Draw(spriteBatch,Color.Red);
        _squares[9,6].Draw(spriteBatch,Color.Blue);
    }
}