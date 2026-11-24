using System.Globalization;

namespace Qs.Numerics;

/// <summary>
/// Qs Complex storage type.
/// </summary>
public struct Complex(double real, double imaginary) : IEquatable<Complex>
{
    public double Real { get; } = real;

    public double Imaginary { get; } = imaginary;

    public static readonly Complex Zero = new(0, 0);

    public static readonly Complex One = new(1.0, 0.0);

    public static readonly Complex ImaginaryOne = new(0.0, 1.0);

    public override string ToString()
    {
        return "(" + Real.ToString(CultureInfo.InvariantCulture) + ", " + Imaginary.ToString(CultureInfo.InvariantCulture) + "i)";
    }

    public string ToQsSyntax()
    {
        return "C{" + Real.ToString(CultureInfo.InvariantCulture) + ", " + Imaginary.ToString(CultureInfo.InvariantCulture) + "}";
    }

    public static implicit operator Complex(double d)
    {
        return new Complex(d, 0);
    }

    public bool Equals(Complex other)
    {
        return (Real == other.Real) && (Imaginary == other.Imaginary);
    }

    public static bool operator ==(Complex lhs, Complex rhs)
    {
        return (lhs.Real == rhs.Real) && (lhs.Imaginary == rhs.Imaginary);
    }

    public static bool operator !=(Complex lhs, Complex rhs)
    {
        return (lhs.Real != rhs.Real) || (lhs.Imaginary != rhs.Imaginary);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Complex lhs)
        {
            return Equals(lhs);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return Real.GetHashCode() ^ Imaginary.GetHashCode();
    }

    public bool IsZero => Real == 0.0 && Imaginary == 0.0;

    #region Operations
    public static Complex operator +(Complex a, Complex b)
    {
        return new Complex(a.Real + b.Real, a.Imaginary + b.Imaginary);
    }

    public static Complex operator -(Complex a, Complex b)
    {
        return new Complex(a.Real - b.Real, a.Imaginary - b.Imaginary);
    }

    public static Complex operator *(Complex a, Complex b)
    {
        return new Complex((a.Real * b.Real) - (a.Imaginary * b.Imaginary), (a.Real * b.Imaginary) + (a.Imaginary * b.Real));
    }

    public static Complex operator /(Complex a, Complex b)
    {
        if (b.IsZero)
        {
            throw new DivideByZeroException("complex division by zero");
        }

        double real, imag, den, r;

        if (Math.Abs(b.Real) >= Math.Abs(b.Imaginary))
        {
            r = b.Imaginary / b.Real;
            den = b.Real + (r * b.Imaginary);
            real = (a.Real + (a.Imaginary * r)) / den;
            imag = (a.Imaginary - (a.Real * r)) / den;
        }
        else
        {
            r = b.Real / b.Imaginary;
            den = b.Imaginary + (r * b.Real);
            real = ((a.Real * r) + a.Imaginary) / den;
            imag = ((a.Imaginary * r) - a.Real) / den;
        }

        return new Complex(real, imag);
    }

    public static Complex operator /(double a, Complex b)
    {
        var result = new Complex(a, 0) / b;
        return result;
    }

    public static Complex operator /(Complex a, double b)
    {
        var result = a / new Complex(b, 0);
        return result;
    }

    public static Complex operator *(double a, Complex b)
    {
        var result = new Complex(a, 0) * b;
        return result;
    }

    public static Complex operator *(Complex a, double b)
    {
        var result = a * new Complex(b, 0);
        return result;
    }

    public Complex Power(Complex y)
    {
        var c = y.Real;
        var d = y.Imaginary;
        var power = (int)c;

        if (power == c && power >= 0 && d == .0)
        {
            var result = One;
            if (power == 0)
            {
                return result;
            }

            var factor = this;
            while (power != 0)
            {
                if ((power & 1) != 0)
                {
                    result *= factor;
                }

                factor *= factor;
                power >>= 1;
            }

            return result;
        }

        if (IsZero)
        {
            return y.IsZero ? One : Zero;
        }

        var a = Real;
        var b = Imaginary;
        var powers = (a * a) + (b * b);
        var arg = Math.Atan2(b, a);
        var mul = Math.Pow(powers, c / 2) * Math.Exp(-d * arg);
        var common = (c * arg) + (.5 * d * Math.Log(powers));
        return new Complex(mul * Math.Cos(common), mul * Math.Sin(common));
    }

    public static Complex Pow(Complex a, Complex power)
    {
        return a.Power(power);
    }

    public static Complex Pow(Complex a, double power)
    {
        var result = a.Power(new Complex(power, 0));
        return result;
    }

    #endregion
}
