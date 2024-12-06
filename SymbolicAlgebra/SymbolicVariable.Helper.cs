namespace SymbolicAlgebra;

public sealed partial class SymbolicVariable
{

    public static SymbolicVariable Number(double number)
    {
        var sv = new SymbolicVariable(number.ToString(CultureInfo.InvariantCulture));
        return sv;
    }

    private static SymbolicVariable _NegativeOne = new SymbolicVariable("-1");
    private static SymbolicVariable _Zero = new SymbolicVariable("0");
    private static SymbolicVariable _One = new SymbolicVariable("1");
    private static SymbolicVariable _Two = new SymbolicVariable("2");
    private static SymbolicVariable _Three = new SymbolicVariable("3");
    private static SymbolicVariable _Four = new SymbolicVariable("4");
    private static SymbolicVariable _Five = new SymbolicVariable("5");
    private static SymbolicVariable _Six = new SymbolicVariable("6");
    private static SymbolicVariable _Seven = new SymbolicVariable("7");
    private static SymbolicVariable _Eight = new SymbolicVariable("8");
    private static SymbolicVariable _Nine = new SymbolicVariable("9");
    private static SymbolicVariable _Ten = new SymbolicVariable("10");
    private static SymbolicVariable _Eleven = new SymbolicVariable("11");
    private static SymbolicVariable _Twelve = new SymbolicVariable("12");

    public static SymbolicVariable NegativeOne => _NegativeOne.Clone();

    public static SymbolicVariable Zero => _Zero.Clone();

    public static SymbolicVariable One => _One.Clone();

    public static SymbolicVariable Two => _Two.Clone();

    public static SymbolicVariable Three => _Three.Clone();

    public static SymbolicVariable Four => _Four.Clone();

    public static SymbolicVariable Five => _Five.Clone();

    public static SymbolicVariable Six => _Six.Clone();

    public static SymbolicVariable Seven => _Seven.Clone();

    public static SymbolicVariable Eight => _Eight.Clone();

    public static SymbolicVariable Nine => _Nine.Clone();

    public static SymbolicVariable Ten => _Ten.Clone();

    public static SymbolicVariable Eleven => _Eleven.Clone();

    public static SymbolicVariable Twelve => _Twelve.Clone();
}