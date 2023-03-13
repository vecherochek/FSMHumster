using System;
using System.Threading.Tasks;
using System.Timers;

using coursavt.MVVM.Model.Consts;

namespace coursavt.MVVM.Model.States
{
    public class InitState : HamsterState
    {
        public InitState(Hamster hamster) : base(hamster)
        {
            _timer = new Timer(500);
        }

        public override async Task Enter()
        {
            Hamster.ButtonEnabled = false;
            Hamster.EatButtonEnabled = false;
            await Logic();
        }

        protected override async Task Logic()
        {
            Hamster.ImagePath = HamsterImages.InitImage;
            await Move(Hamster.Location, HamsterPoints.InitPoint);
            _timer.Start();
            _timer.Elapsed += timer_Tick;
            Hamster.ButtonEnabled = true;
            Hamster.EatButtonEnabled = true;
        }

        public override void Exit()
        {
            _timer.Dispose();
        }

        private void timer_Tick(Object source, ElapsedEventArgs e)
        {
            if (Hamster.CountEscape > 5)
            {
                _timer.Stop();
                Hamster.TransitionToState(new EscapeState(Hamster));
                return;
            }

            switch (Hamster.SleepPBar)
            {
                case <= -5:
                    _timer.Stop();
                    Hamster.TransitionToState(new SleepState(Hamster));
                    return;
                case 30:
                    Status = "Кажется, пора спать...";
                    break;
            }

            if (Hamster.HungryPBar <= 30)
            {
                _timer.Stop();
                Hamster.TransitionToState(new HungryState(Hamster));
                return;
            }

            switch (Hamster.LeisurePBar)
            {
                case <= 5:
                    _timer.Stop();
                    Hamster.CountEscape++;
                    Hamster.TransitionToState(new WheelState(Hamster));
                    return;
                case 30:
                    Status = "Кажется, хомяку скучно...";
                    break;
            }
        }
    }
}