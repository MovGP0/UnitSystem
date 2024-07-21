using Qs;
using Qs.Types;
using QuantitySystem.Quantities.BaseQuantities;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QsRoot.Processor;

/// <summary>
/// Z80 Class is for testing purposes only
/// there are no important thing here
/// </summary>
public sealed class Z80 : EightBitProcessor
{
    public static QsScalar R { get; set; } = QsScalar.Zero;

    public double Sum(double a, double b)
        => a + b;

    public static Length<double> SumQ(Length<double> x, Length<double> y)
        => (Length<double>)(x + y);

    public static AnyQuantity<double> Square(AnyQuantity<double> x)
        => x * x;

    public static SymbolicAlgebra.SymbolicVariable? Register()
    {
        return SymbolicAlgebra.SymbolicVariable.Parse("R+HL");
    }

    public static QsVector Func(
        QsFunction f,
        DimensionlessQuantity<double> from,
        DimensionlessQuantity<double> to)
    {
        var increment = (to - from) / (40).ToQuantity();
        var v = new QsVector(40);

        for (AnyQuantity<double> dt = from; dt <= to; dt += increment)
        {
            v.AddComponent(f.Invoke(dt).ToScalar());
        }

        return v;
    }

    public QsValue Rpc => R + Pc;

    public double Accumulate(params double[] all)
        => all.Sum();

    public Z80 GetZ80(int pc, Z80 z)
    {
        return new()
        {
            Pc = Pc + z.Pc
        };
    }

    public static Z80 LoadPc(int step)
    {
        return new()
        {
            Pc = ((double)step).ToQuantity().ToScalar()
        };
    }

    public static double Length(QsVector v)
        => v.Count;

    public QsValue AddHl(Z80 z80)
        => Hl + z80.Hl;

    public static Z80 operator +(Z80 left, Z80 right)
    {
        return new()
        {
            Af = left.Af + right.Af
        };
    }

    public static Z80 operator -(Z80 left, Z80 right)
    {
        return new()
        {
            Af = left.Af - right.Af
        };
    }

    public static Z80 operator *(Z80 left, Z80 right)
    {
        return new()
        {
            Af = left.Af * right.Af
        };
    }

    public static Z80 operator /(Z80 left, Z80 right)
    {
        return new()
        {
            Af = left.Af / right.Af
        };
    }

    public QsValue this[string register]
    {
        get
        {
            return register switch
            {
                "AF" => Af,
                "BC" => Bc,
                "DE" => De,
                "HL" => Hl,
                "IX" => Ix,
                "IY" => Iy,
                "SP" => Sp,
                "PC" => Pc,
                _ => throw new QsException("Register not found")
            };
        }
        set
        {
            switch (register)
            {
                case "AF":
                    Af = value;
                    break;
                case "BC":
                    Bc = value;
                    break;
                case "DE":
                    De = value;
                    break;
                case "HL":
                    Hl = value;
                    break;
                case "IX":
                    Ix = value;
                    break;
                case "IY":
                    Iy = value;
                    break;
                case "SP":
                    Sp = value;
                    break;
                case "PC":
                    Pc = value;
                    break;
                default:
                    throw new QsException("Register not found");
            }
        }
    }
}