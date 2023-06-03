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
        _gameplayView.CharacterSpawned += OnCharacterSpawned;
        _gameplayView.ActivateUltimate += OnUltimateUsed;
        _gameplayModel.GameStatus += OnGameStatus;
        _gameplayModel.Initialize("FirstLevel.txt");
    }

    private void OnGameStatus(object sender, GamePlayStatus e)
    {
        _gameplayView.SetGameStatus(e.GameIsWin);
    }
    private void ModelViewUpdate(object sender, GameplayEventArgs e)
    {
        _gameplayView.LoadGameCycleParameters(e.Objects,e.Currencys,e.PlayerLives,e.spawnedCharacters);
    }

    private void ViewModelUpdate(object sender, CycleHasFinished e)
    {
        _gameplayModel.Update(e.GameTime);
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