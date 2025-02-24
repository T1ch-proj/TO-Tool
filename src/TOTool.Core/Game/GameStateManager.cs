namespace TOTool.Core.Game
{
    public class GameStateManager : IGameStateManager
    {
        public GameState? CurrentState { get; private set; }

        public void Update()
        {
            // 實作遊戲狀態更新邏輯
        }
    }
} 