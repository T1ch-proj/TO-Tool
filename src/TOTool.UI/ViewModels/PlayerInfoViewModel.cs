using System;
using System.Windows;
using System.Windows.Threading;
using TOTool.Core.Interfaces;
using TOTool.UI.ViewModels;

public class PlayerInfoViewModel : ViewModelBase
{
    private readonly IMemoryReader _memoryReader;
    private double _hpPercentage;
    private string _hpText = "0/0";
    private double _mpPercentage;
    private string _mpText = "0/0";
    private double _expPercentage;
    private string _expText = "0/0";

    public PlayerInfoViewModel(IMemoryReader memoryReader)
    {
        _memoryReader = memoryReader;
        StartUpdateTimer();
    }

    private void StartUpdateTimer()
    {
        var timer = new System.Timers.Timer(1000);
        timer.Elapsed += (s, e) => UpdatePlayerInfo();
        timer.Start();
    }

    private void UpdatePlayerInfo()
    {
        var info = _memoryReader.GetPlayerInfo();
        if (info != null)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                HPPercentage = info.HP * 100.0 / info.MaxHP;
                HPText = $"{info.HP}/{info.MaxHP}";
                MPPercentage = info.MP * 100.0 / info.MaxMP;
                MPText = $"{info.MP}/{info.MaxMP}";
                EXPPercentage = info.Experience * 100.0 / info.MaxExperience;
                EXPText = $"{info.Experience}/{info.MaxExperience}";
            });
        }
    }

    // 屬性定義...
} 