namespace QuantitySystem;

public static class DynamicQuantitySystem
{
    /// <summary>
    /// Holds number of arbitrary functions that give the conversion factor to specific unit types.
    /// has been created for the currency conversion.
    /// </summary>
    internal static Dictionary<string, Func<string, double>> DynamicSourceFunctions = new();

    public static string[] SourceFunctionNames => DynamicSourceFunctions.Keys.ToArray();

    public static void AddDynamicUnitConverterFunction(string name, Func<string, double> converter)
    {
        DynamicSourceFunctions[name] = converter;
    }
}