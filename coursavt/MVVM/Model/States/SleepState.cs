using System;
using System.Threading.Tasks;
using System.Timers;

using coursavt.MVVM.Model.Consts;

namespace coursavt.MVVM.Model.States
{
    public class SleepState : HamsterState
    {
        public SleepState(Hamster hamster) : base(hamster)
        {
            _sleepTime = 5;
        }

        private byte _sleepTime;

        public override async Task Enter()
        {
            Hamster.ButtonEnabled = false;
            Hamster.EatButtonEnabled = false;
            Status = "Хомяк отправляется спать.";
            await Logic();
        }

        protected override async Task Logic()
        {
            await Move(Hamster.Location, HamsterPoints.HomePoint);
            Hamster.ImagePath = HamsterImages.SleepImage;
            Hamster.SleepPBar = 100;
            _timer.Start();
            _timer.Elapsed += timer_Tick;
            Hamster.CountFright = 0;
        }

        public override void Exit()
        {
            Status = "Выспался";
        }

        private void timer_Tick(Object source, ElapsedEventArgs e)
        {
            Status = $"Осталось спать: {_sleepTime}";
            _sleepTime--;
            if (_sleepTime != 0) return;
            _timer.Stop();
            _timer.Dispose();
            Hamster.TransitionToState(new InitState(Hamster));
        }
    }
}