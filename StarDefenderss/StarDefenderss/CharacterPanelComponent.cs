using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarDefenderss;

public class CharacterMenu
{
    private List<GameObjects> character = new ();
    private Dictionary<GameObjects, Texture2D> _textures = new();
    private int selectedCharacterIndex;
    private const int characterSize = 50;
    private int yPos;
    private Dictionary<GameObjects, int> _costs = new();
    private SpriteFont _font;

    public CharacterMenu(List<GameObjects> characters,Dictionary<GameObjects, Texture2D> textures, Dictionary<GameObjects, int> operatorCost, SpriteFont font)
    {
        _font = font;
        _textures = textures;
        _costs = operatorCost;
        character = characters;
        selectedCharacterIndex = -1;
        
    }

    public void Draw(SpriteBatch spriteBatch, int y, HashSet<GameObjects> spawnedCharacters)
    {
        int x = 0;
        yPos = y;
        y -= characterSize;
        for (var i = 0; i < character.Count; i++)
        {
            var color = Color.White;
            if (spawnedCharacters.Contains(character[i]))
                color = Color.Red;
            else if (i == selectedCharacterIndex)
            {
                color = Color.Gray;
            }
            spriteBatch.Draw(_textures[character[i]], new Rectangle(x, y, characterSize, characterSize), color);
            spriteBatch.DrawString(_font, _costs[character[i]].ToString(), new Vector2(x, y - 20), Color.Black);
            x += characterSize;
        }
    }

    public void Update()
    {
        if (!InputManager.IsMouseLeftButtonPressed()) return;
        
        var mousePosition = InputManager.GetMousePosition();
        var characterIndex = mousePosition.X / characterSize;
        if (IsMousePositionNotValid(mousePosition)) return;
        
        selectedCharacterIndex = characterIndex;
    }

    private bool IsMousePositionNotValid(Point mousePosition)
    {
        return mousePosition.X < 0 || mousePosition.X >= character.Count * characterSize ||
               mousePosition.Y < yPos - characterSize || mousePosition.Y >= yPos;
    }

    public GameObjects GetSelectedCharacter()
    {
        return character[selectedCharacterIndex];
    }
    
    public bool IsCharacterSelected()
    {
        return selectedCharacterIndex >= 0 && selectedCharacterIndex < character.Count;
    }
}
