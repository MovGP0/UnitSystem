namespace QuantitySystem.Units;

public struct MetricPrefix
{
    public string Prefix { get; }
    public string Symbol { get; }

    private readonly int _prefixExponent;

    public MetricPrefix(string prefix, string symbol, int exponent)
    {
        Prefix = prefix;
        Symbol = symbol;
        _prefixExponent = exponent;
    }

    /// <summary>
    /// Gets the factor to convert to the required prefix.
    /// </summary>
    /// <param name="prefix"></param>
    /// <returns>Conversion factor</returns>
    public double GetFactorForConvertTo(MetricPrefix prefix)
    {
        return prefix.Factor / Factor;
    }

    #region SI Standard prefixes as static properties


    #region Positive
    public static MetricPrefix Quetta => new("quetta", "Q", 30);
    public static MetricPrefix Ronna => new("ronna", "R", 27);
    public static MetricPrefix Yotta => new("yotta", "Y", 24);
    public static MetricPrefix Zetta => new("zetta", "Z", 21);
    public static MetricPrefix Exa => new("exa", "E", 18);
    public static MetricPrefix Peta => new("peta", "P", 15);
    public static MetricPrefix Tera => new("tera", "T", 12);
    public static MetricPrefix Giga => new("giga", "G", 9);
    public static MetricPrefix Mega => new("mega", "M", 6);
    public static MetricPrefix Kilo => new("kilo", "k", 3);
    public static MetricPrefix Hecto => new("hecto", "h", 2);
    public static MetricPrefix Deka => new("deka", "da", 1);

    #endregion

    public static MetricPrefix None => new("", "", 0);

    #region Negative
    public static MetricPrefix Deci => new("deci", "d", -1);
    public static MetricPrefix Centi => new("centi", "c", -2);
    public static MetricPrefix Milli => new("milli", "m", -3);
    public static MetricPrefix Micro => new("micro", "µ", -6);
    public static MetricPrefix Nano => new("nano", "n", -9);
    public static MetricPrefix Pico => new("pico", "p", -12);
    public static MetricPrefix Femto => new("femto", "f", -15);
    public static MetricPrefix Atto => new("atto", "a", -18);
    public static MetricPrefix Zepto => new("zepto", "z", -21);
    public static MetricPrefix Yocto => new("yocto", "y", -24);
    public static MetricPrefix Ronto => new("ronto", "Rt", -27);
    public static MetricPrefix Quento => new("quento", "Qt", -30);

    #endregion

    #endregion

    #region static constructor
    public static MetricPrefix GetPrefix(int index)
    {

        switch (index)
        {
            case 12: return Quetta;
            case 11: return Ronna;
            case 10: return Yotta;
            case 9: return Zetta;
            case 8: return Exa;
            case 7: return Peta;
            case 6: return Tera;
            case 5: return Giga;
            case 4: return Mega;
            case 3: return Kilo;
            case 2: return Hecto;
            case 1: return Deka;

            case 0: return None;

            case -1: return Deci;
            case -2: return Centi;
            case -3: return Milli;
            case -4: return Micro;
            case -5: return Nano;
            case -6: return Pico;
            case -7: return Femto;
            case -8: return Atto;
            case -9: return Zepto;
            case -10: return Yocto;
            case -11: return Ronto;
            case -12: return Quento;
        }

        throw new MetricPrefixException("Index out of range");
    }

    public static MetricPrefix FromExponent(double exponent)
    {
        CheckExponent(exponent);
        var exp = (int)exponent;
        return exp switch
        {
            -30 => Quento,
            -27 => Ronto,
            -24 => Yocto,
            -21 => Zepto,
            -18 => Atto,
            -15 => Femto,
            -12 => Pico,
            -9 => Nano,
            -6 => Micro,
            -3 => Milli,
            -2 => Centi,
            -1 => Deci,
            0 => None,
            1 => Deka,
            2 => Hecto,
            3 => Kilo,
            6 => Mega,
            9 => Giga,
            12 => Tera,
            15 => Peta,
            18 => Exa,
            21 => Zetta,
            24 => Yotta,
            27 => Ronna,
            30 => Quetta,
            _ => throw new MetricPrefixException("No SI Prefix found.") { WrongExponent = (int)exponent }
        };
    }

    public static MetricPrefix FromPrefixName(string prefixName)
    {
        return prefixName.ToLowerInvariant() switch
        {
            "yocto" => Yocto,
            "zepto" => Zepto,
            "atto" => Atto,
            "femto" => Femto,
            "pico" => Pico,
            "nano" => Nano,
            "micro" => Micro,
            "milli" => Milli,
            "centi" => Centi,
            "deci" => Deci,
            "none" => None,
            "deka" => Deka,
            "hecto" => Hecto,
            "kilo" => Kilo,
            "mega" => Mega,
            "giga" => Giga,
            "tera" => Tera,
            "peta" => Peta,
            "exa" => Exa,
            "zetta" => Zetta,
            "yotta" => Yotta,
            _ => throw new MetricPrefixException("No SI Prefix found for prefix = " + prefixName)
        };
    }

    #endregion

    #region Properties
    public int Exponent => _prefixExponent;

    public double Factor => Math.Pow(10, _prefixExponent);

    #endregion

    #region Operations
    public MetricPrefix Invert() => FromExponent(0 - _prefixExponent);

    public static MetricPrefix Add(MetricPrefix firstPrefix, MetricPrefix secondPrefix)
    {
        var exp = firstPrefix.Exponent + secondPrefix.Exponent;
        CheckExponent(exp);
        return FromExponent(exp);
    }

    public static MetricPrefix operator +(MetricPrefix firstPrefix, MetricPrefix secondPrefix)
    {
        return Add(firstPrefix, secondPrefix);
    }

    public static MetricPrefix Subtract(MetricPrefix firstPrefix, MetricPrefix secondPrefix)
    {
        var exp = firstPrefix.Exponent - secondPrefix.Exponent;

        CheckExponent(exp);

        var prefix = FromExponent(exp);
        return prefix;
    }

    public static MetricPrefix operator -(MetricPrefix firstPrefix, MetricPrefix secondPrefix)
    {
        return Subtract(firstPrefix, secondPrefix);
    }

    public static MetricPrefix Multiply(MetricPrefix firstPrefix, MetricPrefix secondPrefix)
    {
        var exp = firstPrefix.Exponent * secondPrefix.Exponent;
        CheckExponent(exp);
        return FromExponent(exp);
    }

    public static MetricPrefix operator *(MetricPrefix firstPrefix, MetricPrefix secondPrefix)
    {
        return Multiply(firstPrefix, secondPrefix);
    }

    public static MetricPrefix Divide(MetricPrefix firstPrefix, MetricPrefix secondPrefix)
    {
        var exp = firstPrefix.Exponent / secondPrefix.Exponent;
        CheckExponent(exp);
        return FromExponent(exp);
    }

    public static MetricPrefix operator /(MetricPrefix firstPrefix, MetricPrefix secondPrefix)
    {
        return Divide(firstPrefix, secondPrefix);
    }

    #endregion

    /// <summary>
    /// Check the exponent if it can found or
    /// if it exceeds 24 or precedes -25 <see cref="MetricPrefixException"/> occur with the closest
    /// <see cref="MetricPrefix"/> and overflow the rest of it.
    /// </summary>
    /// <param name="exp"></param>
    public static void CheckExponent(double expo)
    {
        var exp = (int)Math.Floor(expo);

        var ov = expo - exp;

        if (exp > 24) throw new MetricPrefixException("Exponent Exceed 24")
        {
            WrongExponent = exp,
            CorrectPrefix = FromExponent(24),
            OverflowExponent = exp - 24 + ov
        };

        if (exp < -24) throw new MetricPrefixException("Exponent Precede -24")
        {
            WrongExponent = exp,
            CorrectPrefix = FromExponent(-24),
            OverflowExponent = exp + 24 + ov
        };

        int[] wrongexp = [4, 5, 7, 8, 10, 11, 13, 14, 16, 17, 19, 20, 22, 23];
        int[] correctexp = [3, 3, 6, 6, 9, 9, 12, 12, 15, 15, 18, 18, 21, 21];

        for (var i = 0; i < wrongexp.Length; i++)
        {
            //find if exponent in wrong powers
            if (Math.Abs(exp) == wrongexp[i])
            {
                var cexp = 0;
                if (exp > 0) cexp = correctexp[i];
                if (exp < 0) cexp = -1 * correctexp[i];

                throw new MetricPrefixException("Exponent not aligned")
                {
                    WrongExponent = exp,
                    CorrectPrefix = FromExponent(cexp),
                    OverflowExponent = exp - cexp + ov           //5-3 = 2  ,  -5--3 =-2
                };
            }
        }

        if (ov != 0)
        {
            //then the exponent must be 0.5
            throw new MetricPrefixException("Exponent not aligned")
            {
                WrongExponent = exp,
                CorrectPrefix = FromExponent(0),
                OverflowExponent = ov           //5-3 = 2  ,  -5--3 =-2
            };
        }
    }
}