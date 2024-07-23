using Qs.Types;
using Qs;

namespace QsRoot;

/// <summary>
/// System Environment but in unitized properties
/// </summary>
public static class Environment
{
    public static QsValue TickCount => System.Environment.TickCount.ToQuantity("ms").ToScalarValue();

    public static QsValue ProcessorCount => System.Environment.ProcessorCount.ToQuantity().ToScalar();

#if !WINRT

    public static QsText CurrentDirectory => new(System.Environment.CurrentDirectory);

    public static QsText MachineName => new(System.Environment.MachineName);

    public static QsObject OsVersion => QsObject.CreateNativeObject( System.Environment.OSVersion);

    public static QsText SystemDirectory => new(System.Environment.SystemDirectory);

    public static QsText UserName => new(System.Environment.UserName);

#endif
}