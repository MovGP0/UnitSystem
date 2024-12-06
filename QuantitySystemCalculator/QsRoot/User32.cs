using System.Runtime.InteropServices;

namespace QsRoot;

internal static partial class User32
{
    [LibraryImport("user32.dll")]
    internal static partial IntPtr GetForegroundWindow();
}