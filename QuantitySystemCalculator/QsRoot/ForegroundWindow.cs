using System.Windows.Forms;

namespace QsRoot;

/// <summary>
/// to get the console window
/// </summary>
internal sealed class ForegroundWindow : IWin32Window
{
    private static readonly ForegroundWindow Window = new();

    private ForegroundWindow() { }

    public static IWin32Window Instance => Window;

    IntPtr IWin32Window.Handle => User32.GetForegroundWindow();
}
