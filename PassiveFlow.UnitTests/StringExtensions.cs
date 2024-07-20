namespace PassiveFlow.UnitTests;

public static class StringExtensions
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string NormalizeLineEndings(this string input)
        => input.Replace("\r\n", "\n").Replace("\r", "\n");
}