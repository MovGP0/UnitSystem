using QuantitySystem.Quantities.BaseQuantities;
using Qs.Types;
using QuantitySystem.Units;
using QuantitySystem.Units.Shared;
using QuantitySystem.Quantities;

namespace QsGraphics;

public class Circle : Shape
{
    readonly AnyQuantity<double>? _x, _y, _radius;

    public Circle(AnyQuantity<double> x, AnyQuantity<double> y, AnyQuantity<double> radius)
    {
        _x = x;
        _y = y;
        _radius = radius;

        var t = new System.Timers.Timer();
    }

    readonly QsFunction? xfunc, yfunc, radfunc;

    /// <summary>
    /// constructor accepts quantities and qs functions also
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="radius"></param>
    public Circle(QsScalar x, QsScalar y, QsScalar radius)
    {
        if (x.ScalarType == ScalarTypes.FunctionQuantity)
        {
            xfunc = x.FunctionQuantity.Value;
        }
        else
        {
            _x = x.NumericalQuantity;
        }

        if (y.ScalarType == ScalarTypes.FunctionQuantity)
        {
            yfunc = y.FunctionQuantity.Value;
        }
        else
        {
            _y = y.NumericalQuantity;
        }

        if (radius.ScalarType == ScalarTypes.FunctionQuantity)
        {
            radfunc = radius.FunctionQuantity.Value;
        }
        else
        {
            _radius = radius.NumericalQuantity;
        }
    }

    private readonly Length<double> zm = (Length<double>)Unit.ParseQuantity("0<m>");

    private Length<double> ResolveLength(AnyQuantity<double>? quantity, QsFunction? function, Time<double> time)
    {
        if (function is not null)
        {
            return (Length<double>)(zm + function.Invoke(time));
        }

        var value = quantity ?? throw new InvalidOperationException("The circle dimension is missing.");
        if (value.GetType() == typeof(Speed<double>))
        {
            value *= time;
        }

        return (Length<double>)(zm + value);
    }

    public override void Draw(Graphics graphics, float pixelPerMeter)
    {
        var p1 = new PointF();
        var p2 = new PointF();

        // include time into the picture
        var TQ = (Time<double>)MetricUnit.Milli<Second>(0);

        // prepare for time
        if (Timer.IsRunning)
        {
            TQ = (Time<double>)MetricUnit.Milli<Second>(Timer.ElapsedMilliseconds);
        }

        var ActualRadius = ResolveLength(_radius, radfunc, TQ);
        var ActualX = ResolveLength(_x, xfunc, TQ);
        var ActualY = ResolveLength(_y, yfunc, TQ);
        p1.X = (float)((ActualX - ActualRadius).Value * pixelPerMeter);
        p2.X = (float)((ActualX + ActualRadius).Value * pixelPerMeter);

        p1.Y = (float)((ActualY - ActualRadius).Value * pixelPerMeter);
        p2.Y = (float)((ActualY + ActualRadius).Value * pixelPerMeter);

        var sf = new SizeF(p2.X - p1.X, p2.Y - p1.Y);

        var rc = new RectangleF(p1, sf);

        graphics.DrawEllipse(
            Pens.Blue
            ,
            rc
        );

        // start the timer after drawing the first round
        if (!Timer.IsRunning)
        {
            Timer.Start();
        }
    }
}
