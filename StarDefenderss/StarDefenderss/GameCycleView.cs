﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StarDefenderss;

public class GameCycleView : Game, IGameplayView
{
    private Dictionary<int, IObject> _objects = new();
    private Dictionary<GameObjects, Texture2D> _textures = new();
    private CharacterMenu characterMenu;
    private GraphicsDeviceManager _graphics;

    public int playerLives;
    private int currency;
    private TimeSpan currencyTimer;
    private SpriteBatch _spriteBatch;
    private float elapsedTime;
    private SpriteFont font;
    private HashSet<GameObjects> spawnedCharacters;
    private Texture2D _blankTexture;
    private bool GameStatus;
    private bool isDone;
    public event EventHandler<CycleHasFinished> CycleFinished;
    public event EventHandler<ActivateUltimate> ActivateUltimate;
    public event EventHandler<CharacterSpawnedEventArgs> CharacterSpawned;


    public GameCycleView(List<Texture2D> characters)
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _textures.Add(GameObjects.Base, Content.Load<Texture2D>("square"));
        _textures.Add(GameObjects.Enemy, Content.Load<Texture2D>("easyEnemy"));
        _textures.Add(GameObjects.Wall, Content.Load<Texture2D>("Grass"));
        _textures.Add(GameObjects.Path, Content.Load<Texture2D>("Path"));
        _textures.Add(GameObjects.FirstOp,Content.Load<Texture2D>("firstOper"));
        _textures.Add(GameObjects.TankOp, Content.Load<Texture2D>("tankOper")); 
        _textures.Add(GameObjects.EnemySniper, Content.Load<Texture2D>("EnemySniper"));
        _textures.Add(GameObjects.Sniper, Content.Load<Texture2D>("sniper"));
        _textures.Add(GameObjects.EnemyDrone, Content.Load<Texture2D>("EnemyDrone"));
        font = Content.Load<SpriteFont>("Font");
        
        _blankTexture = new Texture2D(GraphicsDevice, 1, 1);
        _blankTexture.SetData(new[] { Color.White });
        var operators = new List<GameObjects>();
        operators.Add(GameObjects.FirstOp);
        operators.Add(GameObjects.TankOp);
        operators.Add(GameObjects.Sniper);
        characterMenu = new CharacterMenu(operators,_textures,spawnedCharacters);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
    
        base.Update(gameTime);
        InputManager.Update();
        characterMenu.Update();
        SpawnCharacter();
        CheckUltimate();
        CycleFinished(this,new CycleHasFinished(){GameTime = gameTime});
    }
    
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        foreach (var o in _objects.Values)
        {
            if (o is IHasBar hasBar)
            {
                var position =o.Pos;
                var currentHealth = hasBar.CurrentHealth;
                var maxHealth = hasBar.MaxHealth;
                DrawHealthBar(position, currentHealth, maxHealth, hasBar._HealthColor);
                if (o is IOperator)
                    DrawManaBar(position, hasBar.CurrentMana, hasBar.MaxMana);
            }
            _spriteBatch.Draw(_textures[o.ImageId], o.Pos, null, o.Color, o.Rotation, Vector2.Zero, o.Scale,
                SpriteEffects.None, 0f);
        }

        switch (GameStatus)
        {
            case false when isDone:
            {
                var gameOverText = "GAME OVER\nPress ESC to exit";
                DrawGameStatus(gameOverText);
                break;
            }
            case true when isDone:
            {
                var gameWinText = "GAME WIN!\nPress ESC to exit";
                DrawGameStatus(gameWinText);
                break;
            }
        }
        
        characterMenu.Draw(_spriteBatch, GraphicsDevice.Viewport.Height);
        _spriteBatch.DrawString(font, $"Lives: {playerLives}",
            new Vector2(GraphicsDevice.Viewport.Width - 150, GraphicsDevice.Viewport.Height - 50), Color.White);
        _spriteBatch.DrawString(font, $"Currency: {currency}",
            new Vector2(GraphicsDevice.Viewport.Width - 150, GraphicsDevice.Viewport.Height - 30), Color.White);
        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private void DrawGameStatus(string gameText)
    {
        var textSize = font.MeasureString(gameText);
        var textPosition = new Vector2(
            GraphicsDevice.Viewport.Width / 2 - textSize.X / 2,
            GraphicsDevice.Viewport.Height / 2 - textSize.Y / 2);
        _spriteBatch.DrawString(font, gameText, textPosition, Color.Red, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
    }
    private void CheckUltimate()
    {
        if (InputManager.IsMouseRightButtonPressed())
        {
            ActivateUltimate?.Invoke(this, new ActivateUltimate { Position = InputManager.GetMousePosition()});
        }
    }

    private void SpawnCharacter()
    {
        if (!InputManager.IsMouseLeftButtonPressed() || !characterMenu.IsCharacterSelected()) return;
        var mousePosition = InputManager.GetMousePosition();
        var selectedCharacter = characterMenu.GetSelectedCharacter();
        CharacterSpawned?.Invoke(this, new CharacterSpawnedEventArgs { Position = mousePosition.ToVector2(), SpawnedCharacter = selectedCharacter });
    }
    private void DrawHealthBar( Vector2 position, int currentHealth, int maxHealth, Color color)
    {
        var width = 50;
        var height = 5;
        var border = 2;
        var yOffset = 64;

        float healthPercentage = (float)currentHealth / maxHealth;
        var healthBarWidth = (int)(width * healthPercentage);

        _spriteBatch.Draw(_blankTexture, new Rectangle((int)position.X - border, (int)position.Y - border + yOffset, width + border * 2, height + border * 2), Color.Black);
        _spriteBatch.Draw(_blankTexture, new Rectangle((int)position.X, (int)position.Y + yOffset, healthBarWidth, height), color);
    }
    private void DrawManaBar(Vector2 position, int currentMana, int maxMana)
    {
        var width = 50;
        var height = 5;
        var border = 2;
        var yOffset = 74; 

        float manaPercentage = (float)currentMana / maxMana;
        var manaBarWidth = (int)(width * manaPercentage);

        _spriteBatch.Draw(_blankTexture, new Rectangle((int)position.X - border, (int)position.Y - border + yOffset, width + border * 2, height + border * 2), Color.Black);
        _spriteBatch.Draw(_blankTexture, new Rectangle((int)position.X, (int)position.Y + yOffset, manaBarWidth, height), Color.Green);
    }


    public void LoadGameCycleParameters(Dictionary<int, IObject> Objects, int currency, int PlayerLives)
    {
        _objects = Objects;
        this.currency = currency;
        playerLives = PlayerLives;
    }

    public void SetGameStatus(bool GameStatus)
    {
        this.GameStatus = GameStatus;
        isDone = true;
    }
}