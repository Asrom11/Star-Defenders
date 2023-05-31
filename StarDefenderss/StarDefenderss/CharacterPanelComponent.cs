using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StarDefenderss;

public class CharacterMenu
{
    private List<IObject> character = new ();
    private Dictionary<GameObjects, Texture2D> _textures = new();
    private int selectedCharacterIndex;
    public event EventHandler<CharacterSpawnedEventArgs> CharacterSelected;
    private const int characterSize = 50;
    private int yPos;
    
    public CharacterMenu(List<IObject> characters,Dictionary<GameObjects, Texture2D> textures )
    {
        _textures = textures;
        character = characters;
        selectedCharacterIndex = -1;
    }

    public void Draw(SpriteBatch spriteBatch, int y)
    {
        int x = 0;
        yPos = y;
        y -= characterSize;
        for (var i = 0; i < character.Count; i++)
        {
            var color = Color.White;
            if (character[i].IsSpawned)
                color = Color.Red;
            else if (i == selectedCharacterIndex)
            {
                color = Color.Gray;
            }
            spriteBatch.Draw(_textures[character[i].ImageId], new Rectangle(x, y, characterSize, characterSize), color);
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

    public IObject GetSelectedCharacter()
    {
        return character[selectedCharacterIndex];
    }

    public bool IsCharacterSelected()
    {
        return selectedCharacterIndex >= 0 && selectedCharacterIndex < character.Count;
    }
}