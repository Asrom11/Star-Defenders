using System;
using Microsoft.Xna.Framework;

namespace StarDefenderss;

public class GameplayPresenter
{
    private IGameplayView _gameplayView;
    private IGameplayModel _gameplayModel;


    public GameplayPresenter(IGameplayView gameplayView, 
        IGameplayModel gameplayModel)
    {
        _gameplayView = gameplayView;
        _gameplayModel = gameplayModel;

        _gameplayView.CycleFinished += ViewModelUpdate;
        _gameplayModel.Updated += ModelViewUpdate;
        _gameplayView.EnemyMoved += ViewModelMove;
        _gameplayModel.CurrencyChange += ModelViewUpdateCurrency;
        _gameplayView.CharacterSpawned += OnCharacterSpawned;
        _gameplayModel.Initialize();
    }
    private void ModelViewUpdate(object sender, GameplayEventArgs e)
    {
        _gameplayView.LoadGameCycleParameters(e.Objects, e.EnemyObjects);
    }

    private void ViewModelUpdate(object sender, EventArgs e)
    {
        _gameplayModel.Update();
    }
    private void ViewModelMove(object sender, EnemyMovedEventArgs e)
    {
        _gameplayModel.MoveEnemy(e.GameTime);
    }

    private void ModelViewUpdateCurrency(object sender, CurrencyEventArgs e)
    {
        _gameplayView.LoadCurrencyValue(e.Currencys);
    }
    private void OnCharacterSpawned(object sender, CharacterSpawnedEventArgs e)
    {
        _gameplayModel.SpawnCharacter(e.Position,e.SpawnedCharacter);
    }
    public void LaunchGame()
    {
        _gameplayView.Run();
    }
}