namespace TOTool.Core.Game
{
    public interface IGameStateManager
    {
        GameState? CurrentState { get; }
        void Update();
    }
} 