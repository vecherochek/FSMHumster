using System;
using System.Threading.Tasks;
using System.Timers;

using coursavt.MVVM.Model.Consts;

namespace coursavt.MVVM.Model.States
{
    public class PatState : HamsterState
    {
        public PatState(Hamster hamster) : base(hamster)
        {
            _patTime = 3;
        }

        private byte _patTime;

        public override async Task Enter()
        {
            Hamster.ButtonEnabled = false;
            Hamster.EatButtonEnabled = false;
            await Logic();
        }

        protected override async Task Logic()
        {
            await Move(Hamster.Location, HamsterPoints.PatPoint);
            Hamster.ImagePath = HamsterImages.PatImage;
            Hamster.LeisurePBar = Hamster.LeisurePBar + 40 > 100 ? 100 : Hamster.LeisurePBar + 40;
            Hamster.CountFright = 0;
            _timer.Start();
            _timer.Elapsed += timer_Tick;
        }

        public override void Exit()
        {
            Status = "Хомяк тоже Вас любит.";
            Hamster.CountEscape = (byte)(Hamster.CountEscape - 1 < 0 ? 0 : Hamster.CountEscape - 1);
        }

        private void timer_Tick(Object source, ElapsedEventArgs e)
        {
            _patTime--;
            if (_patTime != 0) return;
            _timer.Stop();
            _timer.Dispose();
            Hamster.TransitionToState(new InitState(Hamster));
        }
    }
}