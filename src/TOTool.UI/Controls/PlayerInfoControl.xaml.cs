using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TOTool.UI.Controls
{
    public partial class PlayerInfoControl : UserControl, INotifyPropertyChanged
    {
        private int _hp;
        private int _maxHp;
        private int _mp;
        private int _maxMp;
        private int _exp;
        private int _maxExp;

        public PlayerInfoControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public double HPPercentage => _maxHp == 0 ? 0 : (_hp * 100.0 / _maxHp);
        public string HPText => $"{_hp}/{_maxHp}";
        
        public double MPPercentage => _maxMp == 0 ? 0 : (_mp * 100.0 / _maxMp);
        public string MPText => $"{_mp}/{_maxMp}";
        
        public double EXPPercentage => _maxExp == 0 ? 0 : (_exp * 100.0 / _maxExp);
        public string EXPText => $"{_exp}/{_maxExp}";

        public void UpdateInfo(int hp, int maxHp, int mp, int maxMp, int exp, int maxExp)
        {
            _hp = hp;
            _maxHp = maxHp;
            _mp = mp;
            _maxMp = maxMp;
            _exp = exp;
            _maxExp = maxExp;

            OnPropertyChanged(nameof(HPPercentage));
            OnPropertyChanged(nameof(HPText));
            OnPropertyChanged(nameof(MPPercentage));
            OnPropertyChanged(nameof(MPText));
            OnPropertyChanged(nameof(EXPPercentage));
            OnPropertyChanged(nameof(EXPText));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 