namespace QuantitySystem.Units;

public partial class UnitPathStack : Stack<UnitPathItem>, ICloneable
{
    public double ConversionFactor
    {
        get
        {
            double cf = 1;
            for (var ix = 0; ix < Count; ix++)
            {
                cf *= this.ElementAt(ix).Times;
            }

            return cf;
        }
    }

    object ICloneable.Clone() => Clone();

    public UnitPathStack Clone()
    {
        var up = new UnitPathStack();
        foreach (var upi in this.Reverse())
        {
            up.Push(new UnitPathItem
            {
                Denominator = upi.Denominator,
                Numerator = upi.Numerator,
                //Shift = upi.Shift,
                Unit = (Unit)upi.Unit.Clone()
            });
        }

        return up;
    }
}
