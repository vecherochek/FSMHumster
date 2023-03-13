using System;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using coursavt.Core;
using coursavt.MVVM.Model.Consts;
using coursavt.MVVM.Model.States;
using coursavt.UserControls;
using Point = System.Drawing.Point;

namespace coursavt.MVVM.Model;

public class Hamster : ObservableObject
{
    private Point _location;
    private string _imagePath;
    public System.Timers.Timer Timer;
    private int _hungryPBar;
    private int _sleepPBar;
    private int _leisurePBar;
    private bool _buttonEnabled;
    public byte CountFright;
    private HamsterState _currentState;
    private bool _eatButtonEnabled;
    private bool _isAlive;
    private byte _countEscape;

    public ObservableCollection<MessageControl> MessageControls { get; set; } =
        new ObservableCollection<MessageControl>();

    public byte CountEscape
    {
        get => _countEscape;
        set
        {
            _countEscape = value;
            OnPropertyChanged();
        }
    }

    public bool IsAlive
    {
        get => !_isAlive;
        set
        {
            _isAlive = value;
            OnPropertyChanged();
        }
    }

    public bool ButtonEnabled
    {
        get => _buttonEnabled;
        set
        {
            _buttonEnabled = value;
            OnPropertyChanged();
        }
    }

    public bool EatButtonEnabled
    {
        get => _eatButtonEnabled;
        set
        {
            _eatButtonEnabled = value;
            OnPropertyChanged();
        }
    }

    public Point Location
    {
        get => _location;
        set
        {
            _location = value;
            OnPropertyChanged();
        }
    }

    public string ImagePath
    {
        get => _imagePath;
        set
        {
            _imagePath = value;
            OnPropertyChanged();
        }
    }

    public int LeisurePBar
    {
        get => _leisurePBar;
        set
        {
            _leisurePBar = value;
            OnPropertyChanged();
        }
    }

    public int HungryPBar
    {
        get => _hungryPBar;
        set
        {
            _hungryPBar = value;
            OnPropertyChanged();
        }
    }

    public int SleepPBar
    {
        get => _sleepPBar;
        set
        {
            _sleepPBar = value;
            OnPropertyChanged();
        }
    }

    public Hamster()
    {
        ImagePath = HamsterImages.InitImage;
        CountEscape = 0;
        CountFright = 0;
        SleepPBar = 100;
        HungryPBar = 100;
        LeisurePBar = 100;
        Location = new Point(240, 420);
        IsAlive = true;
        _eatButtonEnabled = true;
        Timer = new System.Timers.Timer(500);
        TransitionToState(new InitState(this));
        VitalSigns();
    }

    private void VitalSigns()
    {
        Timer.Elapsed += timer_Tick;
        Timer.Start();
    }

    private void timer_Tick(Object source, ElapsedEventArgs e)
    {
        HungryPBar--;
        SleepPBar--;
        LeisurePBar--;
    }

    public void TransitionToState(HamsterState state)
    {
        _currentState?.Exit();
        _currentState = state;
        _currentState.Enter();
    }

    /// <summary>
    /// Invoke <see cref="Action"/> in default UI thread.
    /// </summary>
    /// <param name="action"></param>
    public void InvokeInUiThread(Action action)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                return;
            }
        }, DispatcherPriority.Normal);
    }
}