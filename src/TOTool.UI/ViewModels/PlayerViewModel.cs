using System;
using System.Threading.Tasks;
using TOTool.Common.Interfaces;
using TOTool.Common.Models;
using Microsoft.Extensions.Logging;
using TOTool.Core.Memory;

namespace TOTool.UI.ViewModels
{
    public class PlayerViewModel : ViewModelBase
    {
        private readonly MemoryManager _memoryReader;
        private readonly ILogger<PlayerViewModel> _logger;

        private int _hp;
        private int _maxHp;
        private int _mp;
        private int _maxMp;
        private int _exp;
        private int _maxExp;
        private float _posX;
        private float _posY;

        public PlayerViewModel(MemoryManager memoryReader, ILogger<PlayerViewModel> logger)
        {
            _memoryReader = memoryReader;
            _logger = logger;
            StartUpdateLoop();
        }

        public int HP
        {
            get => _hp;
            set => SetProperty(ref _hp, value);
        }

        public int MaxHP
        {
            get => _maxHp;
            set => SetProperty(ref _maxHp, value);
        }

        public int MP
        {
            get => _mp;
            set => SetProperty(ref _mp, value);
        }

        public int MaxMP
        {
            get => _maxMp;
            set => SetProperty(ref _maxMp, value);
        }

        public int EXP
        {
            get => _exp;
            set => SetProperty(ref _exp, value);
        }

        public int MaxEXP
        {
            get => _maxExp;
            set => SetProperty(ref _maxExp, value);
        }

        public float PosX
        {
            get => _posX;
            set => SetProperty(ref _posX, value);
        }

        public float PosY
        {
            get => _posY;
            set => SetProperty(ref _posY, value);
        }

        private void StartUpdateLoop()
        {
            var timer = new System.Timers.Timer(100); // 更新頻率 100ms
            timer.Elapsed += async (s, e) =>
            {
                await UpdatePlayerInfo();
            };
            timer.Start();
        }

        private async Task UpdatePlayerInfo()
        {
            try
            {
                var info = _memoryReader.GetPlayerInfo();
                if (info != null)
                {
                    HP = info.HP;
                    MaxHP = info.MaxHP;
                    MP = info.MP;
                    MaxMP = info.MaxMP;
                    EXP = (int)info.Experience;
                    MaxEXP = (int)info.MaxExperience;
                    PosX = info.PositionX;
                    PosY = info.PositionY;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新玩家資訊失敗");
            }
        }
    }
} 