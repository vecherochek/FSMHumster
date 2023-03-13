using System.Windows.Input;
using coursavt.Core;
using coursavt.MVVM.Commands;
using coursavt.MVVM.Model;

namespace coursavt.MVVM.ViewModel;

public class MainWindowViewModel : ObservableObject
{
    private HamsterStateMachine _hamster;
    public ICommand EatCommand { get; set; }
    public ICommand PatCommand { get; set; }
    public ICommand SleepCommand { get; set; }
    public ICommand ScareCommand { get; set; }
    public ICommand WheelCommand { get; set; }
    public ICommand StartCommand { get; set; }

    public HamsterStateMachine Hamster
    {
        get => _hamster;
        set
        {
            _hamster = value;
            OnPropertyChanged();
        }
    }

    public MainWindowViewModel()
    {
        _hamster = new HamsterStateMachine(new Hamster());

        EatCommand = new RelayCommand(Eat);
        PatCommand = new RelayCommand(Pat);
        SleepCommand = new RelayCommand(Sleep);
        ScareCommand = new RelayCommand(Scare);
        WheelCommand = new RelayCommand(Wheel);
        StartCommand = new RelayCommand(Start);
    }

    private void Eat(object o)
    {
        _hamster.ChangeState(HamsterCommand.EAT);
    }

    private void Pat(object o)
    {
        _hamster.ChangeState(HamsterCommand.PAT);
    }

    private void Sleep(object o)
    {
        _hamster.ChangeState(HamsterCommand.SLEEP);
    }

    private void Scare(object o)
    {
        _hamster.ChangeState(HamsterCommand.SCARE);
    }

    private void Wheel(object o)
    {
        _hamster.ChangeState(HamsterCommand.WHEEL);
    }

    private void Start(object o)
    {
        Hamster = new HamsterStateMachine(new Hamster());
    }
}