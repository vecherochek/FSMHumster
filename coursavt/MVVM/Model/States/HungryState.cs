using System;
using System.Threading.Tasks;
using System.Timers;

using coursavt.MVVM.Model.Consts;

namespace coursavt.MVVM.Model.States
{
    public class HungryState : HamsterState
    {
        public HungryState(Hamster hamster) : base(hamster)
        {
        }

        private string _status = null;

        public override async Task Enter()
        {
            Status = "Хомяк хочет есть((";
            Hamster.ButtonEnabled = false;
            Hamster.EatButtonEnabled = true;
            Hamster.CountFright = 0;
            Hamster.CountEscape++;
            await Logic();
        }

        protected override async Task Logic()
        {
            if (Hamster.CountEscape > 5)
            {
                _status = "О, нет..";
                Hamster.TransitionToState(new EscapeState(Hamster));
                return;
            }

            _status = "Ура, еда!!!!";
            await Move(Hamster.Location, HamsterPoints.HungryPoint);
            _timer.Start();
            _timer.Elapsed += timer_Tick;
            Hamster.ImagePath = HamsterImages.HungryImage;
        }

        public override void Exit()
        {
            _timer.Stop();
            _timer.Dispose();
            Status = _status;
        }

        private void timer_Tick(Object source, ElapsedEventArgs e)
        {
            if (Hamster.HungryPBar > -10) return;
            _timer.Stop();
            _timer.Dispose();
            Hamster.TransitionToState(new OtkinylsiaState(Hamster));
        }
    }
}