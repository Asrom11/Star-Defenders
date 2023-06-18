using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using StarDefenders;

namespace StarDefenderss;

public class GameCycleView : Game, IGameplayView
{
    private Dictionary<int, IObject> _objects = new();
    private Dictionary<GameObjects, int> _operatorsCost = new();
    private Dictionary<GameObjects, Texture2D> _textures = new();
    private Dictionary<GameObjects, Texture2D> _sizedUpOperators = new();
    private CharacterMenu _characterMenu;
    private GraphicsDeviceManager _graphics;
    private Song backgroundMusic;
    private int _playerLives;
    private int _currency;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
    private Texture2D _blankTexture;
    private bool _gameStatus;
    private bool _isDone;
    private Texture2D _backgroundTexture;
    private HashSet<GameObjects> _spawnedCharacters;
    public event EventHandler<CycleHasFinished> CycleFinished;
    public event EventHandler<ActivateUltimate> ActivateUltimate;
    public event EventHandler<CharacterSpawnedEventArgs> CharacterSpawned;


    public GameCycleView()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _spawnedCharacters = new HashSet<GameObjects>();
    }

    protected override void Initialize()
    {
        base.Initialize();
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        MediaPlayer.Play(backgroundMusic);
        MediaPlayer.IsRepeating = true;
        _graphics.ApplyChanges();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        backgroundMusic = Content.Load<Song>("backgroundMusic");
        _backgroundTexture = Content.Load<Texture2D>("mapBack");
        _textures.Add(GameObjects.Base, Content.Load<Texture2D>("square"));
        _textures.Add(GameObjects.Enemy, Content.Load<Texture2D>("easyEnemy"));
        _textures.Add(GameObjects.Wall, Content.Load<Texture2D>("Grass"));
        _textures.Add(GameObjects.Path, Content.Load<Texture2D>("Path"));
        _textures.Add(GameObjects.FirstOp,Content.Load<Texture2D>("firstOper"));
        _textures.Add(GameObjects.TankOp, Content.Load<Texture2D>("tankOper")); 
        _textures.Add(GameObjects.EnemySniper, Content.Load<Texture2D>("EnemySniper"));
        _textures.Add(GameObjects.Sniper, Content.Load<Texture2D>("sniper"));
        _textures.Add(GameObjects.EnemyDrone, Content.Load<Texture2D>("EnemyDrone"));
        _textures.Add(GameObjects.Healer, Content.Load<Texture2D>("Healer"));
        _textures.Add(GameObjects.Vanguard, Content.Load<Texture2D>("Vanguard"));
        
        _sizedUpOperators.Add(GameObjects.FirstOp, Content.Load<Texture2D>("SizedUpOp"));
        _sizedUpOperators.Add(GameObjects.TankOp, Content.Load<Texture2D>("SizedUpTank"));
        _sizedUpOperators.Add(GameObjects.Vanguard, Content.Load<Texture2D>("SizedUpVanguard"));
        _sizedUpOperators.Add(GameObjects.Healer, Content.Load<Texture2D>("HealerSizedUp"));
        _sizedUpOperators.Add(GameObjects.Sniper, Content.Load<Texture2D>("sniper"));
        _font = Content.Load<SpriteFont>("Font");

        _operatorsCost.Add(GameObjects.FirstOp, 18);
        _operatorsCost.Add(GameObjects.TankOp, 23);
        _operatorsCost.Add(GameObjects.Vanguard, 11);
        _operatorsCost.Add(GameObjects.Healer, 18);
        _operatorsCost.Add(GameObjects.Sniper, 14);

        _blankTexture = new Texture2D(GraphicsDevice, 1, 1);
        _blankTexture.SetData(new[] { Color.White });
        var operators = new List<GameObjects>();
        operators.Add(GameObjects.FirstOp);
        operators.Add(GameObjects.TankOp);
        operators.Add(GameObjects.Sniper);
        operators.Add(GameObjects.Healer);
        operators.Add(GameObjects.Vanguard);
        _characterMenu = new CharacterMenu(operators,_sizedUpOperators, _operatorsCost, _font);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        base.Update(gameTime);
        InputManager.Update();
        _characterMenu.Update();
        SpawnCharacter();
        CheckUltimate();
        CycleFinished(this,new CycleHasFinished(){GameTime = gameTime});
    }
    
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        _spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
        foreach (var o in _objects.Values)
        {
            _spriteBatch.Draw(_textures[o.ImageId], o.Pos, null, o.Color, o.Rotation, Vector2.Zero, o.Scale,
                SpriteEffects.None, 0f);
            if (o is not IHasBar hasBar) continue;
            
            var position =o.Pos;
            var currentHealth = hasBar.CurrentHealth;
            var maxHealth = hasBar.MaxHealth;
            DrawHealthBar(position, currentHealth, maxHealth, hasBar._HealthColor);
            if (o is IOperator)
                DrawManaBar(position, hasBar.CurrentMana, hasBar.MaxMana);
        }
        
        switch (_gameStatus)
        {
            case false when _isDone:
            {
                var gameOverText = "GAME OVER\nPress ESC to exit";
                DrawGameStatus(gameOverText);
                break;
            }
            case true when _isDone:
            {
                var gameWinText = "GAME WIN!\nPress ESC to exit";
                DrawGameStatus(gameWinText);
                break;
            }
        }
        
        _characterMenu.Draw(_spriteBatch, GraphicsDevice.Viewport.Height,_spawnedCharacters);
        _spriteBatch.DrawString(_font, $"Lives: {_playerLives}",
            new Vector2(GraphicsDevice.Viewport.Width - 150, GraphicsDevice.Viewport.Height - 50), Color.White);
        _spriteBatch.DrawString(_font, $"Currency: {_currency}",
            new Vector2(GraphicsDevice.Viewport.Width - 150, GraphicsDevice.Viewport.Height - 30), Color.White);
        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private void DrawGameStatus(string gameText)
    {
        var textSize = _font.MeasureString(gameText);
        var textPosition = new Vector2(
            GraphicsDevice.Viewport.Width / 2 - textSize.X / 2,
            GraphicsDevice.Viewport.Height / 2 - textSize.Y / 2);
        _spriteBatch.DrawString(_font, gameText, textPosition, Color.Red, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
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
        if (!InputManager.IsMouseLeftButtonPressed() || !_characterMenu.IsCharacterSelected()) return;
        var mousePosition = InputManager.GetMousePosition();
        var selectedCharacter = _characterMenu.GetSelectedCharacter();
        CharacterSpawned?.Invoke(this, new CharacterSpawnedEventArgs { Position = mousePosition.ToVector2(), SpawnedCharacter = selectedCharacter });
    }
    private void DrawHealthBar( Vector2 position, int currentHealth, int maxHealth, Color color)
    {
        var width = 50;
        var height = 5;
        var border = 2;
        var yOffset = 50;

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
        var yOffset = 60; 

        float manaPercentage = (float)currentMana / maxMana;
        var manaBarWidth = (int)(width * manaPercentage);

        _spriteBatch.Draw(_blankTexture, new Rectangle((int)position.X - border, (int)position.Y - border + yOffset, width + border * 2, height + border * 2), Color.Black);
        _spriteBatch.Draw(_blankTexture, new Rectangle((int)position.X, (int)position.Y + yOffset, manaBarWidth, height), Color.Green);
    }


    public void LoadGameCycleParameters(Dictionary<int, IObject> Objects, int currency, int PlayerLives, HashSet<GameObjects> spawnedCharacters)
    {
        _objects = Objects;
        this._currency = currency;
        _playerLives = PlayerLives;
        _spawnedCharacters = spawnedCharacters;
    }

    public void SetGameStatus(bool GameStatus)
    {
        this._gameStatus = GameStatus;
        _isDone = true;
    }
}