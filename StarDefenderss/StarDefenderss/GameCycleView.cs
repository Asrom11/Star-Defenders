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
    private Dictionary<int, Texture2D> _textures = new();
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Field _field;
    private float elapsedTime;
    public event EventHandler CycleFinished;
    public event EventHandler<EnemyMovedEventArgs> EnemyMoved;


public GameCycleView()
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
        _field = new Field(10, 14, Content.Load<Texture2D>("square"));
        Enemy.Texture2D = Content.Load<Texture2D>("tempEnemy");
        _textures.Add(1, Content.Load<Texture2D>("square"));
        _textures.Add(2,Content.Load<Texture2D>("tempEnemy"));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        // TODO: Add your update logic here
        base.Update(gameTime);
        EnemyMoved.Invoke(this, new EnemyMovedEventArgs()
        {
            GameTime = gameTime,
        });
        CycleFinished(this, EventArgs.Empty);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        _field.Draw(_spriteBatch);

        lock (_objects)
            foreach (var o in _objects.Values) 
            {
                _spriteBatch.Draw(_textures[o.ImageId], o.Pos, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            }
        _spriteBatch.End();
        base.Draw(gameTime);
    }
    public void LoadGameCycleParameters(Dictionary<int, IObject> Objects)
    {
        _objects = Objects;  
    }
}