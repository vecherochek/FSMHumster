using System;
using System.Threading.Tasks;
using System.Timers;
using coursavt.MVVM.Model.Consts;

namespace coursavt.MVVM.Model.States
{
    public class ScareState : HamsterState
    {
        public ScareState(Hamster hamster) : base(hamster)
        {
            _timer = new Timer(500);
        }

        public override async Task Enter()
        {
            Hamster.ButtonEnabled = false;
            Hamster.EatButtonEnabled = false;
            Status = Hamster.CountFright switch
            {
                0 => "Зачем пугаете малышарика?",
                1 => "Бедняга....",
                2 => "Он же боится...",
                _ => "Зря-зря..."
            };
            Hamster.CountEscape++;
            _timer.Start();
            _timer.Elapsed += timer_Tick;
            await Logic();
        }

        protected override async Task Logic()
        {
            if (Hamster.CountEscape > 5)
            {
                Hamster.TransitionToState(new EscapeState(Hamster));
                return;
            }
            Hamster.ImagePath = HamsterImages.FrightImage1;
            switch (Hamster.CountFright)
            {
                case 0:
                    await Move(Hamster.Location, HamsterPoints.HungryPoint, 5);
                    await Move(Hamster.Location, HamsterPoints.HomePoint, 5);
                    await Move(Hamster.Location, HamsterPoints.EatPoint, 5);
                    await Move(Hamster.Location, HamsterPoints.WheelPoint, 5);
                    await Move(Hamster.Location, HamsterPoints.PatPoint, 5);
                    Hamster.ButtonEnabled = true;
                    Hamster.EatButtonEnabled = true;
                    break;
                case 1:
                    await Move(Hamster.Location, HamsterPoints.HomePoint, 4);
                    Hamster.ButtonEnabled = true;
                    Hamster.EatButtonEnabled = true;
                    break;
                case 2:
                    Hamster.ImagePath = HamsterImages.FrightImage2;
                    Hamster.ButtonEnabled = true;
                    Hamster.EatButtonEnabled = true;
                    break;
                default:
                    Hamster.TransitionToState(new OtkinylsiaState(Hamster));
                    break;
            }
        }

        public override void Exit()
        {
            Hamster.CountFright++;
        }
    
        private void timer_Tick(Object source, ElapsedEventArgs e)
        {
            if (Hamster.HungryPBar > 30) return;
            _timer.Stop();
            Hamster.TransitionToState(new HungryState(Hamster));
        }
    }
}