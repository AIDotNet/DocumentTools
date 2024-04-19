using System.Runtime.InteropServices;

namespace AIDotNet.Document.Client;

[ClassInterface(ClassInterfaceType.AutoDual)]
[ComVisible(true)]
public class EventForwarder(IntPtr target)
{
    public const int WM_NCLBUTTONDOWN = 0xA1;
    public const int HT_CAPTION = 0x2;

    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    
    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();

    public void MouseDownDrag()
    {
        ReleaseCapture();
        SendMessage(target, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
    }
}