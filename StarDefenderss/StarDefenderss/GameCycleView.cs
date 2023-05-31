using System;
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
    
    private int currency;
    private TimeSpan currencyTimer;
    private SpriteBatch _spriteBatch;
    private float elapsedTime;
    private SpriteFont font;
    private Texture2D _blankTexture;
    public event EventHandler<CycleHasFinished> CycleFinished;
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
        _textures.Add(GameObjects.Enemy, Content.Load<Texture2D>("tempEnemy"));
        _textures.Add(GameObjects.Wall, Content.Load<Texture2D>("Grass"));
        _textures.Add(GameObjects.Path, Content.Load<Texture2D>("Path"));
        _textures.Add(GameObjects.FirstOp,Content.Load<Texture2D>("firstOper"));
        _textures.Add(GameObjects.TankOp, Content.Load<Texture2D>("tankOper"));
        _textures.Add(GameObjects.Ditection, Content.Load<Texture2D>("Direction"));
        font = Content.Load<SpriteFont>("Font");
        
        _blankTexture = new Texture2D(GraphicsDevice, 1, 1);
        _blankTexture.SetData(new[] { Color.White });
        var operators = new List<IObject>();
        operators.Add(new Operator(100,10,1,1,1, new Vector2(0,0), 1,100, GameObjects.FirstOp));
        operators.Add(new TankOperator(100,10,1,1,1, new Vector2(0,0), 1, 100,GameObjects.TankOp));
        characterMenu = new CharacterMenu(operators,_textures);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
    
        base.Update(gameTime);
        InputManager.Update();
        characterMenu.Update();

        if (InputManager.IsMouseLeftButtonPressed() && characterMenu.IsCharacterSelected()) 
        {
            var mousePosition = InputManager.GetMousePosition();
            var selectedCharacter = characterMenu.GetSelectedCharacter();
            CharacterSpawned?.Invoke(this, new CharacterSpawnedEventArgs { Position = mousePosition.ToVector2(), SpawnedCharacter = selectedCharacter });
        }
        CycleFinished(this,new CycleHasFinished(){GameTime = gameTime});
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        foreach (var o in _objects.Values)
        {
            if (o is IHasHealth hasHealth)
            {
                var position =o.Pos;
                var currentHealth = hasHealth.CurrentHealth;
                var maxHealth = hasHealth.MaxHealth;
                DrawHealthBar(_spriteBatch, position, currentHealth, maxHealth, hasHealth._HealthColor, o is Enemy);
            }
            _spriteBatch.Draw(_textures[o.ImageId], o.Pos, null, o.Color, o.Rotation, Vector2.Zero, o.Scale,
                SpriteEffects.None, 0f);
        }
        characterMenu.Draw(_spriteBatch, GraphicsDevice.Viewport.Height);
        _spriteBatch.DrawString(font, $"Currency: {currency}",
            new Vector2(GraphicsDevice.Viewport.Width - 150, GraphicsDevice.Viewport.Height - 30), Color.White);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
    public void DrawHealthBar(SpriteBatch spriteBatch, Vector2 position, int currentHealth, int maxHealth, Color color,bool isEnemy)
    {
        var width = 50;
        var height = 5;
        var border = 2;
        var yOffset = isEnemy ? 64 : 50;

        float healthPercentage = (float)currentHealth / maxHealth;
        var healthBarWidth = (int)(width * healthPercentage);

        spriteBatch.Draw(_blankTexture, new Rectangle((int)position.X - border, (int)position.Y - border + yOffset, width + border * 2, height + border * 2), Color.Black);
        spriteBatch.Draw(_blankTexture, new Rectangle((int)position.X, (int)position.Y + yOffset, healthBarWidth, height), color);
    }


    public void LoadGameCycleParameters(Dictionary<int, IObject> Objects)
    {
        _objects = Objects;
    }


    public void LoadCurrencyValue(int currentCurrency)
    {
        currency = currentCurrency;
    }
}