using System.Threading.Tasks;
using System.Timers;
using coursavt.UserControls;

namespace coursavt.MVVM.Model;

public abstract class HamsterState
{
    public Hamster Hamster { get; set; }
    public abstract Task Enter();

    protected abstract Task Logic();

    public abstract void Exit();
    protected Timer _timer;

    protected string Status
    {
        set { Hamster.InvokeInUiThread(() => Hamster.MessageControls.Add(new MessageControl(value))); }
    }

    protected HamsterState(Hamster hamster)
    {
        Hamster = hamster;
        _timer = new Timer(1000);
    }

    private int GetY(int x, System.Drawing.Point a, System.Drawing.Point c)
    {
        double k = (double)(a.Y - c.Y) / (double)(a.X - c.X);
        double b = (double)c.Y - (double)(k * c.X);
        return (int)(k * x + b);
    }

    protected async Task Move(System.Drawing.Point currentPoint, System.Drawing.Point destinationPoint, int speed = 2)
    {
        if (currentPoint.X < destinationPoint.X)
        {
            for (var i = currentPoint.X; i <= destinationPoint.X; i += speed)
            {
                Hamster.Location = new System.Drawing.Point(i, GetY(i, currentPoint, destinationPoint));
                //await Task.Run(() => { Thread.Sleep(10); });
                await Task.Delay(1);
            }
        }
        else if (currentPoint.X == destinationPoint.X)
        {
            for (var i = currentPoint.Y; i >= 0; i--)
            {
                Hamster.Location = new System.Drawing.Point(currentPoint.X, i);
                //await Task.Run(() => { Thread.Sleep(10); });
                await Task.Delay(1);
            }
        }
        else
        {
            for (var i = currentPoint.X; i > destinationPoint.X; i -= speed)
            {
                Hamster.Location = new System.Drawing.Point(i, GetY(i, currentPoint, destinationPoint));
                //await Task.Run(() => { Thread.Sleep(10); });
                await Task.Delay(1);
            }
        }
    }
}