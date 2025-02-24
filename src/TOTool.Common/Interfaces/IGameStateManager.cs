using System;
using TOTool.Common.Models;

namespace TOTool.Common.Interfaces
{
    public interface IGameStateManager
    {
        GameState CurrentState { get; }
        event EventHandler<GameState> GameStateChanged;
        void Initialize();
        void Update();
        bool IsGameRunning { get; }
    }
} 