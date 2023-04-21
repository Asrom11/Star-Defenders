using System;

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
        
        _gameplayModel.Initialize();

    }
    
    private void ModelViewUpdate(object sender, GameplayEventArgs e)
    {
        _gameplayView.LoadGameCycleParameters(e.Objects);
    }

    private void ViewModelUpdate(object sender, EventArgs e)
    {
        _gameplayModel.Update();
    }

    public void LaunchGame()
    {
        _gameplayView.Run();
    }
}