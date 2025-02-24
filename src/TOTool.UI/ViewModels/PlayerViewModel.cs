using System;
using System.Threading.Tasks;
using TOTool.Core.Memory;
using TOTool.Core.Patterns;

namespace TOTool.UI.ViewModels
{
    public class PlayerViewModel : ViewModelBase
    {
        private readonly MemoryManager _memoryManager;
        private readonly PatternFinder _patternFinder;

        private int _hp;
        private int _maxHp;
        private int _mp;
        private int _maxMp;
        private int _exp;
        private int _maxExp;
        private float _posX;
        private float _posY;

        public PlayerViewModel(MemoryManager memoryManager)
        {
            _memoryManager = memoryManager;
            _patternFinder = new PatternFinder(memoryManager);
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

        private async void StartUpdateLoop()
        {
            while (true)
            {
                await UpdatePlayerInfo();
                await Task.Delay(100); // 更新頻率 100ms
            }
        }

        private async Task UpdatePlayerInfo()
        {
            try
            {
                var info = _memoryManager.GetPlayerInfo();
                if (info != null)
                {
                    HP = info.HP;
                    MaxHP = info.MaxHP;
                    MP = info.MP;
                    MaxMP = info.MaxMP;
                    EXP = info.Experience;
                    MaxEXP = info.MaxExperience;
                    PosX = info.PositionX;
                    PosY = info.PositionY;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("更新玩家資訊失敗", ex);
            }
        }
    }
} 