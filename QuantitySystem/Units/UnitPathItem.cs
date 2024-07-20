using System.Globalization;

namespace QuantitySystem.Units
{
    public class UnitPathItem
    {

        public Unit Unit { get; set; }

        public double Times
        {
            get
            {
                return Numerator / Denominator;
            }
        }

        public double Numerator { get; set; }

        public double Denominator { get; set; }

        public bool IsInverted => Unit.IsInverted;

        /// <summary>
        /// Invert the item with its underlying unit.
        /// </summary>
        public void Invert()
        {
            double num = Numerator;
            Numerator = Denominator;
            Denominator = num;

            Unit = Unit.Invert();
        }


        public override bool Equals(object obj)
        {
            if (obj is UnitPathItem upi)
            {
                if ((Unit.GetType() == upi.Unit.GetType())
                    && (Numerator == upi.Numerator)
                    && (Denominator == upi.Denominator))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {

            return Unit.Symbol + ": " + Times.ToString(CultureInfo.InvariantCulture);
        }

    }
}
