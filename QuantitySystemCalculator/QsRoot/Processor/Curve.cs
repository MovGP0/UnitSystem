using Qs.Types;
using SymbolicAlgebra;
using Qs;

namespace QsRoot.Processor;

public class Curve
{
    readonly SymbolicVariable _fx;
    readonly SymbolicVariable _fy;
    readonly SymbolicVariable _fz;

    public Curve(SymbolicVariable x, SymbolicVariable y, SymbolicVariable z)
    {
        _fx = x;
        _fy = y;
        _fz = z;
    }

    public QsVector Point(double t)
    {
        var x = _fx.Execute(t);
        var y = _fy.Execute(t);
        var z = _fz.Execute(t);

        return new QsVector(x.ToQuantity().ToScalar(), y.ToQuantity().ToScalar(), z.ToQuantity().ToScalar());

    }

    public static Curve GetCurve(SymbolicVariable x, SymbolicVariable y, SymbolicVariable z)
    {
        return new Curve(x, y, z);
    }



    /// <summary>
    /// Test method for returning array of objects
    /// handled in qs as a tuple containing the native objects
    /// </summary>
    /// <returns></returns>
    public static Z80[] GetProcessors()
    {
        Z80[] z = new Z80[5];

        var r = new Random();
        for (var i = 0; i < 5; i++)
        {
            z[i] = new Z80() { Pc = r.NextDouble().ToQuantity().ToScalar() };
        }

        return z;
    }

}