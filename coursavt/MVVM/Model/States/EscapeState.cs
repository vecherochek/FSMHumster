using System.Threading.Tasks;
using coursavt.MVVM.Model.Consts;

namespace coursavt.MVVM.Model.States;

public class EscapeState : HamsterState
{
    public EscapeState(Hamster hamster) : base(hamster)
    {
    }

    public override async Task Enter()
    {
        Hamster.ButtonEnabled = false;
        Hamster.EatButtonEnabled = false;
        Status = "Кажется, хомяк сбегает!!";
        await Logic();
    }

    protected override async Task Logic()
    {
        Status = "Хомяку здесь не нравится. Пока-пока!!";
        Hamster.Timer.Stop();
        Hamster.IsAlive = false;
        Hamster.ImagePath = HamsterImages.DoorImage;
        await Move(Hamster.Location, HamsterPoints.DoorPoint, 2);
        Hamster.ImagePath = HamsterImages.NibbleImage;
        Exit();
    }

    public override void Exit()
    {
        Status = "Сбежал, ъ";
        Status = "Игра закончена";
    }
}