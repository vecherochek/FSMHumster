using System.Threading.Tasks;

using coursavt.MVVM.Model.Consts;

namespace coursavt.MVVM.Model.States
{
    public class OtkinylsiaState : HamsterState
    {
        public OtkinylsiaState(Hamster hamster) : base(hamster)
        {
        }

        public override async Task Enter()
        {
            Hamster.ButtonEnabled = false;
            Hamster.EatButtonEnabled = false;
            Status = "Вы не уследили за хомяком и он ушел :(";
            await Logic();
        }

        protected override async Task Logic()
        {
            Hamster.Timer.Stop();
            Hamster.IsAlive = false;
            Hamster.ImagePath = HamsterImages.OtkinylsiaImage;
            await Move(Hamster.Location, Hamster.Location with { Y = 0 });
            Exit();
        }

        public override void Exit()
        {
            Status = "Игра закончена";
        }
    }
}