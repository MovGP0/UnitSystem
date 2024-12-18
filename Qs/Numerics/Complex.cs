﻿using System.Globalization;

namespace Qs.Numerics;

/// <summary>
/// Qs Complex storage type.
/// </summary>
public struct Complex(double real, double imaginary) : IEquatable<Complex>
{
    private double _real = real, _imaginary = imaginary;

    public double Real => _real;

    public double Imaginary => _imaginary;


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
        if ((_real == other._real) && (_imaginary == other._imaginary))
            return true;
        return false;
    }


    public static bool operator ==(Complex lhs, Complex rhs)
    {
        if ((lhs._real == rhs._real) && (lhs._imaginary == rhs._imaginary))
            return true;
        return false;
    }

    public static bool operator !=(Complex lhs, Complex rhs)
    {
        if ((lhs._real != rhs._real) || (lhs._imaginary != rhs._imaginary))
            return true;
        return false;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Complex lhs)
            return Equals(lhs);

        return false;
    }


    public override int GetHashCode()
    {
        return _real.GetHashCode() ^ _imaginary.GetHashCode();
    }

    public bool IsZero => _real == 0.0 && _imaginary == 0.0;


    #region Operations
    public static Complex operator +(Complex a, Complex b)
    {
        return new Complex(a._real + b._real, a._imaginary + b._imaginary);
    }

    public static Complex operator -(Complex a, Complex b)
    {
        return new Complex(a._real - b._real, a._imaginary - b._imaginary);
    }

    public static Complex operator *(Complex a, Complex b)
    {
        return new Complex(a._real * b._real - a._imaginary * b._imaginary, a._real * b._imaginary + a._imaginary * b._real);
    }

    public static Complex operator /(Complex a, Complex b)
    {
        if (b.IsZero)
        {
            throw new DivideByZeroException("complex division by zero");
        }

        double real, imag, den, r;

        if (Math.Abs(b._real) >= Math.Abs(b._imaginary))
        {
            r = b._imaginary / b._real;
            den = b._real + r * b._imaginary;
            real = (a._real + a._imaginary * r) / den;
            imag = (a._imaginary - a._real * r) / den;
        }
        else
        {
            r = b._real / b._imaginary;
            den = b._imaginary + r * b._real;
            real = (a._real * r + a._imaginary) / den;
            imag = (a._imaginary * r - a._real) / den;
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
        return (result);
    }



    public Complex Power(Complex y)
    {
        var c = y._real;
        var d = y._imaginary;
        var power = (int)c;

        if (power == c && power >= 0 && d == .0)
        {
            var result = One;
            if (power == 0) return result;
            var factor = this;
            while (power != 0)
            {
                if ((power & 1) != 0)
                {
                    result = result * factor;
                }
                factor = factor * factor;
                power >>= 1;
            }
            return result;
        }

        if (IsZero)
        {
            return y.IsZero ? One : Zero;
        }
        var a = _real;
        var b = _imaginary;
        var powers = a * a + b * b;
        var arg = Math.Atan2(b, a);
        var mul = Math.Pow(powers, c / 2) * Math.Exp(-d * arg);
        var common = c * arg + .5 * d * Math.Log(powers);
        return new Complex(mul * Math.Cos(common), mul * Math.Sin(common));
    }


    public static Complex Pow(Complex a, Complex power)
    {
        return a.Power(power);
    }


    public static Complex Pow(Complex a, double power)
    {
        var result = a.Power(new Complex(power, 0));
        return (result);
    }




    #endregion
}