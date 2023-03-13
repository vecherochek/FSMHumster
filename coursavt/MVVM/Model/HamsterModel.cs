using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using coursavt.Core;
using coursavt.MVVM.Model.Consts;
using coursavt.UserControls;
using Point = System.Drawing.Point;

namespace coursavt.MVVM.Model;

public class HamsterModel : ObservableObject
{
    private Point _location;
    private string _imagePath;
    private System.Timers.Timer _timer;
    private int _hungryPBar;
    private int _sleepPBar;
    private int _leisurePBar;
    private byte _countNibble;
    private byte _countFright;

    public ObservableCollection<MessageControl> MessageControls { get; set; } =
        new ObservableCollection<MessageControl>();

    public HamsterModel()
    {
        ImagePath = HamsterImages.InitImage;
        _countNibble = 0;
        _countFright = 0;
        SleepPBar = 100;
        HungryPBar = 100;
        LeisurePBar = 100;
        Location = new(240, 420);
        _timer = new System.Timers.Timer(500);
        VitalSigns();
        Memo("Хомяк приветствует Вас :)");
    }

    public Point Location
    {
        get => _location;
        set
        {
            _location = value;
            OnPropertyChanged();
        }
    }

    public string ImagePath
    {
        get => _imagePath;
        set
        {
            _imagePath = value;
            OnPropertyChanged();
        }
    }

    public int LeisurePBar
    {
        get => _leisurePBar;
        set
        {
            _leisurePBar = value;
            OnPropertyChanged();
        }
    }

    public int HungryPBar
    {
        get => _hungryPBar;
        set
        {
            _hungryPBar = value;
            OnPropertyChanged();
        }
    }

    public int SleepPBar
    {
        get => _sleepPBar;
        set
        {
            _sleepPBar = value;
            OnPropertyChanged();
        }
    }

    private void VitalSigns()
    {
        _timer.Elapsed += timer_Tick;
        _timer.Start();
    }

    void timer_Tick(Object source, ElapsedEventArgs e)
    {
        HungryPBar--;
        if (HungryPBar == 30) GoHungry();
        else if (HungryPBar == 15) GoNibble();
        else if (HungryPBar == -10) GoOtkinylsia();

        SleepPBar--;
        if (SleepPBar == -10) GoOtkinylsia();

        LeisurePBar--;
        if (LeisurePBar == 20) GoNibble();
    }

    public async void GoEat()
    {
        if (Location != HamsterPoints.HungryPoint)
        {
            ImagePath = HamsterImages.InitImage;
            await Move(Location, HamsterPoints.InitPoint);
        }

        await Move(Location, HamsterPoints.EatPoint);
        ImagePath = HamsterImages.EatImage;
        HungryPBar = 100;
        _countFright = 0;
    }

    private async void GoOtkinylsia()
    {
        _timer.Stop();
        ImagePath = HamsterImages.OtkinylsiaImage;
        await Move(Location, new Point(Location.X, 0));
    }

    private async void GoHungry()
    {
        ImagePath = HamsterImages.InitImage;
        await Move(Location, HamsterPoints.InitPoint);
        await Move(Location, HamsterPoints.HungryPoint);
        ImagePath = HamsterImages.HungryImage;
        _countFright = 0;
    }

    private async void GoNibble()
    {
        _countNibble++;
        if (_countNibble == 4)
        {
            GoEscape();
            return;
        }

        ImagePath = HamsterImages.NibbleImage;
        await Move(Location, HamsterPoints.InitPoint, 3);
        await Move(Location, HamsterPoints.NibblePoint, 3);
        _countFright = 0;
    }

    private async void GoEscape()
    {
        await Move(Location, HamsterPoints.DoorPoint, 3);
        ImagePath = HamsterImages.DoorImage;
        _timer.Stop();
    }

    public async Task GoPat()
    {
        Memo("Хомяк тоже Вас любит");
        ImagePath = HamsterImages.InitImage;
        await Move(Location, HamsterPoints.InitPoint);
        await Move(Location, HamsterPoints.PatPoint);
        ImagePath = HamsterImages.PatImage;
        LeisurePBar = LeisurePBar + 40 > 100 ? 100 : LeisurePBar + 40;
        _countFright = 0;
    }

    public async void GoWheel()
    {
        ImagePath = HamsterImages.InitImage;
        await Move(Location, HamsterPoints.InitPoint);
        await Move(Location, HamsterPoints.WheelPoint);
        ImagePath = HamsterImages.WheelImage;
        LeisurePBar = LeisurePBar + 40 > 100 ? 100 : LeisurePBar + 40;
        _countFright = 0;
    }

    public async void GoSleep()
    {
        if (Location != HamsterPoints.HomePoint)
        {
            ImagePath = HamsterImages.InitImage;
            await Move(Location, HamsterPoints.InitPoint);
            await Move(Location, HamsterPoints.HomePoint);
        }

        ImagePath = HamsterImages.SleepImage;
        SleepPBar = 100;
        _countFright = 0;
    }

    public async void GoScare()
    {
        _countFright++;
        ImagePath = HamsterImages.FrightImage1;
        switch (_countFright)
        {
            case 1:
                await Move(Location, HamsterPoints.DoorPoint, 4);
                await Move(Location, HamsterPoints.HungryPoint, 4);
                await Move(Location, HamsterPoints.HomePoint, 4);
                await Move(Location, HamsterPoints.EatPoint, 4);
                await Move(Location, HamsterPoints.WheelPoint, 4);
                await Move(Location, HamsterPoints.PatPoint, 4);
                break;
            case 2:
                await Move(Location, HamsterPoints.HomePoint, 3);
                break;
            case 3:
                ImagePath = HamsterImages.FrightImage2;
                break;
            default:
                GoOtkinylsia();
                break;
        }
    }

    private int GetY(int x, Point a, Point c)
    {
        double k = (double)(a.Y - c.Y) / (double)(a.X - c.X);
        double b = (double)c.Y - (double)(k * c.X);
        return (int)(k * x + b);
    }

    private async Task Move(Point currentPoint, Point destinationPoint, int speed = 2)
    {
        if (currentPoint.X < destinationPoint.X)
        {
            for (var i = currentPoint.X; i <= destinationPoint.X; i += speed)
            {
                Location = new Point(i, GetY(i, currentPoint, destinationPoint));
                //await Task.Run(() => { Thread.Sleep(10); });
                await Task.Delay(1);
            }
        }
        else if (currentPoint.X == destinationPoint.X)
        {
            for (var i = currentPoint.Y; i >= 0; i--)
            {
                Location = new Point(currentPoint.X, i);
                //await Task.Run(() => { Thread.Sleep(10); });
                await Task.Delay(1);
            }
        }
        else
        {
            for (var i = currentPoint.X; i > destinationPoint.X; i -= speed)
            {
                Location = new Point(i, GetY(i, currentPoint, destinationPoint));
                //await Task.Run(() => { Thread.Sleep(10); });
                await Task.Delay(1);
            }
        }
    }

    private void Memo(string massage)
    {
        InvokeInUiThread(() => MessageControls.Add(new MessageControl(massage)));
    }

    /// <summary>
    /// Invoke <see cref="Action"/> in default UI thread.
    /// </summary>
    /// <param name="action"></param>
    public void InvokeInUiThread(Action action)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                return;
            }
        }, DispatcherPriority.Normal);
    }
}