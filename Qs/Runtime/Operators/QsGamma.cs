using Qs.Types;
using QuantitySystem.Quantities.BaseQuantities;

namespace Qs.Runtime.Operators
{
    /// <summary>
    /// Gives the factorial of real numbers.
    /// </summary>
    public static class QsGamma
    {
        /// <summary>
        /// Get factorial for the <see>QsValue</see> whether it is Scalar, Vector, Matrix, and later Tensor.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static QsValue Factorial(QsValue value)
        {
            if (value is QsBoolean)
            {
                var b = (QsBoolean)value;
                return new QsBoolean { Value = !b.Value };
            }

            if (value is QsScalar)
            {
                var sv = (QsScalar)value;
                if (sv.ScalarType == ScalarTypes.NumericalQuantity)
                {
                    var number = sv.NumericalQuantity;
                    return new QsScalar { NumericalQuantity = QuantityFactorial(number) };
                }

                throw new QsException("Unsupported scalar object");
            }

            if (value is QsVector)
            {
                var vec = value as QsVector;

                var rvec = new QsVector(vec.Count);

                foreach (var v in vec)
                {
                    rvec.AddComponent((QsScalar)Factorial(v));
                }

                return rvec;
            }
            if (value is QsMatrix)
            {
                var mat = value as QsMatrix;
                var Total = new QsMatrix();

                for (var IY = 0; IY < mat.RowsCount; IY++)
                {
                    List<QsScalar> row = new(mat.ColumnsCount);

                    for (var IX = 0; IX < mat.ColumnsCount; IX++)
                    {
                        row.Add((QsScalar)Factorial(mat[IY, IX]));
                    }

                    Total.AddRow(row.ToArray());
                }
                return Total;
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// Get the factorial of quantity
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static AnyQuantity<double> QuantityFactorial(AnyQuantity<double> number)
        {
            var v = (int)number.Value;

            //if (v > 170) throw new ArgumentOutOfRangeException("Number", number, "Number is greater than 170");
            if (v < 0) throw new ArgumentOutOfRangeException("Number", "Number is less than 0");

            var num = (AnyQuantity<double>)number.Clone();
            num.Value = Math.Floor(num.Value);

            var Total = num.Value == 0 ? 1 : num.Value;

            //I am calculating the value part first for fast calculation
            for (var i = v; i > 1; i--)
            {
                num.Value--;
                Total = Total * num.Value;
            }

            //   Raise the power of the unit.
            //    I think 3.5<kg>! = 11.631728<kg^3> is wrong and of course not <kg^4>  but it is <kg^3.5>
            var TotalUnit = num.Unit.RaiseUnitPower((float)number.Value);


            // if we have fraction correct the calculation with Gamma Factorial.
            if (number.Value > v)
            {
                Total = GammaFactorial(number.Value);
            }

            var TotalQuantity = TotalUnit.GetThisUnitQuantity(Total);

            return TotalQuantity;
        }


        /// <summary>
        /// Factorial for real numbers.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static double GammaFactorial(double number)
        {
            //I checked this http://www.rskey.org/gamma.htm
            // then this ...  http://www.luschny.de/System.Math/factorial/approx/SimpleCases.html



            //the used function  near to the windows calculator
            var StieltjesFactorial = delegate(double x)
            {
                var y = x + 1; var p = 1.0;
                while (y < 7) { p = p * y; y = y + 1; }
                var r = Math.Exp(y * Math.Log(y) - y + 1 / (12 * y + 2 / (5 * y + 53 / (42 * y))));
                if (x < 7) r = r / p;
                return r * Math.Sqrt(2 * Math.PI / y);
            };


            var StieltjesLnFactorial = delegate(double z)
            {
                var a0 = 1 / 12; var a1 = 1 / 30; var a2 = 53 / 210; var a3 = 195 / 371;
                var a4 = 22999 / 22737; var a5 = 29944523 / 19733142;
                var a6 = 109535241009 / 48264275462;
                var Z = z + 1;

                return (1 / 2) * Math.Log(2 * Math.PI) + (Z - 1 / 2) * Math.Log(Z) - Z +
                    a0 / (Z + a1 / (Z + a2 / (Z + a3 / (Z + a4 / (Z + a5 / (Z + a6 / Z))))));
            };

            //gives value of the normal factorial :S
            var StieltjesFactorialHP = delegate(double x)
            {
                var y = x; var p = 1.0;
                while (y < 8)
                {
                    p = p*y;
                    y = y+1;
                };
                var r = Math.Exp(StieltjesLnFactorial(y));

                if (x < 8) return  (r * x) / (p * y);
                return r;
            };


            //gives value less the normal factorial :(
            var Nemes = delegate(double x)
            {
                var temp = x;
                temp += (1 / 12) * (1 / x);
                temp += (1 / 1440) * (1 / x * x * x);
                temp += (239 / 362880) * (1 / x * x * x * x * x);

                return Math.Pow(temp, x) * Math.Exp(-x) * Math.Sqrt(2) * Math.Sqrt(Math.PI * x);

            };


            return StieltjesFactorial(number);

        }

    }
}
