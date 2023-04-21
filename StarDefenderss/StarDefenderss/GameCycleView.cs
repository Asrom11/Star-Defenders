using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StarDefenderss;

public class GameCycleView : Game,IGameplayView
{
    private Dictionary<int, IObject> _objects = new ();
    private Dictionary<int, Texture2D> _textures = new ();
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Field _field;
    public event EventHandler CycleFinished;

    public GameCycleView()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _field = new Field(10, 10, Content.Load<Texture2D>("square"), 64);
        // _textures.Add(1, Content.Load<Texture2D>("square"));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        _spriteBatch.Begin();
        _field.Draw(_spriteBatch); 
        //foreach (var o in _objects.Values)
        //{
         //   _spriteBatch.Draw(_textures[o.ImageId],  o.Pos, Color.White);
       // }  		
        _spriteBatch.End();
        base.Draw(gameTime);
    }
    public void LoadGameCycleParameters(Dictionary<int, IObject> Objects)
    {
        _objects = Objects;  
    }
}