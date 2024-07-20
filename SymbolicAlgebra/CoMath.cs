namespace SymbolicAlgebra;

/// <summary>
/// Co Math class for the rest of functions
/// </summary>
public static class CoMath
{
    public static readonly string[] AvailableFunctions =
    [
        "Sec",
        "Csc",
        "Cot",
        "Sech",
        "Csch",
        "Coth" ,
        "ACosh",
        "ASinh",
        "ATanh",
        "ASec",
        "ACsc",
        "ACot",
        "ASech",
        "ACsch",
        "ACoth",
        "Int"
    ];

    public static double Sec(double x) => 1.0 / Math.Cos(x);

    public static double Csc(double x) => 1.0 / Math.Sin(x);

    public static double Cot(double x) => 1.0 / Math.Tan(x);

    public static double Sech(double x) => 1.0 / Math.Cosh(x);

    public static double Csch(double x) => 1.0 / Math.Sinh(x);

    public static double Coth(double x) => 1.0 / Math.Tanh(x);

    public static double Asec(double x) => Math.Acos(1.0 / x);

    public static double Acsc(double x) => Math.Asin(1.0 / x);

    public static double Acot(double x) => Math.Atan(1.0 / x);

    public static double Acosh(double x) => Math.Log(x + Math.Sqrt(x * x - 1.0));

    public static double Asinh(double x) => Math.Log(x + Math.Sqrt(x * x + 1.0));

    public static double Atanh(double x) => 0.5 * Math.Log((1.0 + x) / (1.0 - x));

    public static double Asech(double x) => Acosh(1.0 / x);

    public static double Acsch(double x) => Asinh(1.0 / x);

    public static double Acoth(double x) => Atanh(1.0 / x);

    public static double Int(double x) => (int)x;
}