using Qs.Types;
using SymbolicAlgebra;
using Qs;

namespace QsRoot.Processor;

public class Curve(SymbolicVariable fx, SymbolicVariable fy, SymbolicVariable fz)
{
    public QsVector Point(double t)
    {
        var x = fx.Execute(t);
        var y = fy.Execute(t);
        var z = fz.Execute(t);

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