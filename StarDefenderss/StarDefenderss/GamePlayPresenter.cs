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
        _gameplayModel.CurrencyChange += ModelViewUpdateCurrency;
        _gameplayView.CharacterSpawned += OnCharacterSpawned;
        _gameplayView.ActivateUltimate += OnUltimateUsed;
        _gameplayModel.Initialize();
    }
    private void ModelViewUpdate(object sender, GameplayEventArgs e)
    {
        _gameplayView.LoadGameCycleParameters(e.Objects);
    }

    private void ViewModelUpdate(object sender, CycleHasFinished e)
    {
        _gameplayModel.Update(e.GameTime);
    }
    private void ModelViewUpdateCurrency(object sender, CurrencyEventArgs e)
    {
        _gameplayView.LoadCurrencyValue(e.Currencys);
    }
    private void OnCharacterSpawned(object sender, CharacterSpawnedEventArgs e)
    {
        _gameplayModel.SpawnCharacter(e.Position,e.SpawnedCharacter);
    }

    private void OnUltimateUsed(object sender, ActivateUltimate e)
    {
        _gameplayModel.TryActivateUltimate(e.Position);
    }
    public void LaunchGame()
    {
        _gameplayView.Run();
    }
}