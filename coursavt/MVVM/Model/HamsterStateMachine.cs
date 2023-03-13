using System;
using coursavt.MVVM.Model.States;

namespace coursavt.MVVM.Model;

public class HamsterStateMachine
{
    public Hamster Hamster { get; set; }

    public HamsterStateMachine(Hamster hamster)
    {
        Hamster = hamster;
    }

    public void ChangeState(HamsterCommand command)
    {
        switch (command)
        {
            case HamsterCommand.EAT:
                GoEat();
                break;
            case HamsterCommand.PAT:
                GoPat();
                break;
            case HamsterCommand.SCARE:
                GoScare();
                break;
            case HamsterCommand.SLEEP:
                GoSleep();
                break;
            case HamsterCommand.WHEEL:
                GoWheel();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void GoEat()
    {
        Hamster.TransitionToState(new EatState(Hamster));
    }

    private void GoPat()
    {
        Hamster.TransitionToState(new PatState(Hamster));
    }

    private void GoScare()
    {
        Hamster.TransitionToState(new ScareState(Hamster));
    }

    private void GoSleep()
    {
        Hamster.TransitionToState(new SleepState(Hamster));
    }

    private void GoWheel()
    {
        Hamster.TransitionToState(new WheelState(Hamster));
    }
}