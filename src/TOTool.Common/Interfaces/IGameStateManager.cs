using System;
using TOTool.Common.Models;

namespace TOTool.Common.Interfaces
{
    public interface IGameStateManager
    {
        GameState CurrentState { get; }
        bool IsGameRunning { get; }
        event EventHandler<GameState>? GameStateChanged;
        bool Initialize();
        void Update();
    }
} 