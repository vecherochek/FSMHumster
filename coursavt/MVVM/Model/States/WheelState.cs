using System;
using System.Threading.Tasks;
using System.Timers;
using coursavt.MVVM.Model.Consts;

namespace coursavt.MVVM.Model.States
{
    public class WheelState : HamsterState
    {
        public WheelState(Hamster hamster) : base(hamster)
        {
            _wheelTime = 3;
        }

        private byte _wheelTime;

        public override async Task Enter()
        {
            Hamster.ButtonEnabled = false;
            Hamster.EatButtonEnabled = false;
            await Logic();
        }

        protected override async Task Logic()
        {
            await Move(Hamster.Location, HamsterPoints.WheelPoint);
            Hamster.ImagePath = HamsterImages.WheelImage;
            Hamster.LeisurePBar = Hamster.LeisurePBar + 40 > 100 ? 100 : Hamster.LeisurePBar + 40;
            Hamster.CountFright = 0;
            _timer.Start();
            _timer.Elapsed += timer_Tick;
        }

        public override void Exit()
        {
            Status = "Реально думали, что будет бегать?";
        }

        private void timer_Tick(Object source, ElapsedEventArgs e)
        {
            Status = $"Осталось бегать: {_wheelTime}";
            _wheelTime--;
            if (_wheelTime != 0) return;
            _timer.Stop();
            _timer.Dispose();
            Hamster.TransitionToState(new InitState(Hamster));
        }
    }
}