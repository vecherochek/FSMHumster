using System;
using System.Threading.Tasks;
using System.Timers;
using coursavt.MVVM.Model.Consts;

namespace coursavt.MVVM.Model.States;

public class EatState : HamsterState
{
    public EatState(Hamster hamster) : base(hamster)
    {
        _eatTime = 3;
    }

    private byte _eatTime;

    public override async Task Enter()
    {
        Hamster.ButtonEnabled = false;
        Hamster.EatButtonEnabled = false;
        Status = "Хомяк отправился кушоц.";
        await Logic();
    }

    protected override async Task Logic()
    {
        Hamster.ImagePath = HamsterImages.InitImage;
        await Move(Hamster.Location, HamsterPoints.EatPoint);
        Hamster.ImagePath = HamsterImages.EatImage;
        _timer.Start();
        _timer.Elapsed += timer_Tick;
        Hamster.HungryPBar = 100;
        Hamster.CountFright = 0;
    }

    public override void Exit()
    {
        Status = "Сытно покушал.";
        Hamster.ButtonEnabled = true;
        Hamster.EatButtonEnabled = true;
    }

    private void timer_Tick(Object source, ElapsedEventArgs e)
    {
        Status = $"Осталось есть: {_eatTime}";
        _eatTime--;
        if (_eatTime != 0) return;
        _timer.Stop();
        _timer.Dispose();
        Hamster.TransitionToState(new InitState(Hamster));
    }
}