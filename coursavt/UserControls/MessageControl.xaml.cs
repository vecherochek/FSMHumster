using System.Windows.Controls;

namespace coursavt.UserControls;

public partial class MessageControl : UserControl
{
    public MessageControl(string message)
    {
        InitializeComponent();
        Message = message;
    }
    public string Message { get; set; }
}