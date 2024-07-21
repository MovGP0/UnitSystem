using Qs.Runtime;
using Qs.Types;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Qs;

namespace QsRoot;

/// <summary>
/// Qs Module as namespace.
/// </summary>
public static class Windows
{
    public static QsValue MessageBox(QsParameter text)
    {

        IWin32Window? fw = null;
        try { fw = ForegroundWindow.Instance; }
        catch { }

        var d =
            System.Windows.Forms.MessageBox.Show(
                fw,
                text.UnknownValueText,
                "Quantity System"
            );

        return ((int)d).ToScalarValue();
        //return d;
    }

    public static QsValue MessageBox(
        QsParameter text,
        [QsParamInfo(QsParamType.Text)]QsParameter caption
    )
    {

        IWin32Window? fw = null;
        try { fw = ForegroundWindow.Instance; }
        catch { }

        var d =
            System.Windows.Forms.MessageBox.Show(
                fw,
                text.UnknownValueText,
                caption.UnknownValueText
            );

        return ((int)d).ToScalarValue();
    }

    public static QsValue? Beep()
    {
        System.Media.SystemSounds.Beep.Play();
        return null;
    }
}

/// <summary>
/// to get the console window
/// </summary>
internal class ForegroundWindow : IWin32Window
{
    private static ForegroundWindow _window = new();
    private ForegroundWindow() { }

    public static IWin32Window Instance => _window;

    [LibraryImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    IntPtr IWin32Window.Handle => GetForegroundWindow();
}