using System.Collections.ObjectModel;
using System.Numerics;
using ShInUeXx.Numerics;

namespace UnitSystem.Gauge;

/// <summary>
/// The decomposition of a tensor product into irreducible gauge representations.
/// </summary>
public sealed class GaugeProduct
{
    private GaugeProduct(Dictionary<GaugeRepresentation, BigInteger> channels, Rational hypercharge)
    {
        Channels = new ReadOnlyDictionary<GaugeRepresentation, BigInteger>(channels);
        Hypercharge = hypercharge;
    }

    /// <summary>Gets the irreducible channels and the multiplicity of each.</summary>
    public IReadOnlyDictionary<GaugeRepresentation, BigInteger> Channels { get; }

    /// <summary>Gets the sum of the factors' hypercharges.</summary>
    public Rational Hypercharge { get; }

    /// <summary>Gets the number of independent gauge-singlet channels.</summary>
    public BigInteger SingletMultiplicity => Channels.GetValueOrDefault(GaugeRepresentation.Singlet);

    /// <summary>Gets whether a contraction to a gauge singlet exists.</summary>
    public bool HasSinglet => SingletMultiplicity > BigInteger.Zero;

    /// <summary>Gets whether every component of the product transforms trivially.</summary>
    public bool IsGaugeInvariant => Channels.Count == 1 && HasSinglet;

    /// <summary>
    /// Selects the unique singlet symbolically. A product with multiple singlets requires
    /// an explicit choice of contraction.
    /// </summary>
    /// <exception cref="InvalidOperationException">There is no unique singlet.</exception>
    public GaugeRepresentation SelectUniqueSinglet()
    {
        var multiplicity = SingletMultiplicity;
        if (!multiplicity.IsOne)
        {
            throw new InvalidOperationException($"Expected one gauge-singlet channel, found {multiplicity}.");
        }

        return GaugeRepresentation.Singlet;
    }

    /// <summary>
    /// Decomposes a product. Supported color factors are 1, 3, conjugate 3, and 8.
    /// Weak factors use twice their isospin. The empty product is a singlet.
    /// </summary>
    /// <exception cref="OverflowException">A resulting representation label exceeds its numeric range.</exception>
    public static GaugeProduct Analyze(params GaugeRepresentation[] factors)
    {
        ArgumentNullException.ThrowIfNull(factors);
        var colors = new Dictionary<ColorRepresentation, BigInteger> { [ColorRepresentation.Singlet] = BigInteger.One };
        var weakIsospins = new Dictionary<uint, BigInteger> { [0] = BigInteger.One };
        var hypercharge = Rational.Zero;

        foreach (var factor in factors)
        {
            ArgumentNullException.ThrowIfNull(factor);
            colors = MultiplyColor(colors, factor.Color);
            weakIsospins = MultiplyWeak(weakIsospins, factor.TwiceWeakIsospin);
            hypercharge += factor.Hypercharge;
        }

        var channels = new Dictionary<GaugeRepresentation, BigInteger>();
        foreach (var (color, colorMultiplicity) in colors)
        {
            foreach (var (twiceWeakIsospin, weakMultiplicity) in weakIsospins)
            {
                var representation = new GaugeRepresentation(color, twiceWeakIsospin, hypercharge);
                channels.Add(representation, colorMultiplicity * weakMultiplicity);
            }
        }

        return new GaugeProduct(channels, hypercharge);
    }

    private static Dictionary<ColorRepresentation, BigInteger> MultiplyColor(
        Dictionary<ColorRepresentation, BigInteger> product, ColorRepresentation factor)
        => factor switch
        {
            { P: 0, Q: 0 } => product,
            { P: 1, Q: 0 } => MultiplyTriplet(product, conjugate: false),
            { P: 0, Q: 1 } => MultiplyTriplet(product, conjugate: true),
            { P: 1, Q: 1 } => MultiplyOctet(product),
            _ => throw new NotSupportedException($"Unsupported input color representation ({factor.P}, {factor.Q})."),
        };

    private static Dictionary<ColorRepresentation, BigInteger> MultiplyTriplet(
        Dictionary<ColorRepresentation, BigInteger> product, bool conjugate)
    {
        var result = new Dictionary<ColorRepresentation, BigInteger>();
        foreach (var (representation, multiplicity) in product)
        {
            var basis = conjugate ? representation.Conjugate() : representation;
            var p = basis.P;
            var q = basis.Q;

            // Add each of the three fundamental weights and keep dominant weights.
            AddChannel(new ColorRepresentation(checked(p + 1), q));
            if (p > 0) AddChannel(new ColorRepresentation(p - 1, checked(q + 1)));
            if (q > 0) AddChannel(new ColorRepresentation(p, q - 1));

            void AddChannel(ColorRepresentation channel)
                => AddMultiplicity(result, conjugate ? channel.Conjugate() : channel, multiplicity);
        }

        return result;
    }

    private static Dictionary<ColorRepresentation, BigInteger> MultiplyOctet(
        Dictionary<ColorRepresentation, BigInteger> product)
    {
        // A triplet times an antitriplet is a singlet plus an octet.
        var tripletProduct = MultiplyTriplet(product, conjugate: false);
        var result = MultiplyTriplet(tripletProduct, conjugate: true);
        foreach (var (representation, multiplicity) in product)
        {
            var remainder = result[representation] - multiplicity;
            if (remainder.IsZero)
            {
                result.Remove(representation);
            }
            else
            {
                result[representation] = remainder;
            }
        }

        return result;
    }

    private static Dictionary<uint, BigInteger> MultiplyWeak(Dictionary<uint, BigInteger> product, uint factor)
    {
        var result = new Dictionary<uint, BigInteger>();
        foreach (var (twiceIsospin, multiplicity) in product)
        {
            var maximum = checked(twiceIsospin + factor);
            for (long channel = Math.Abs((long)twiceIsospin - factor); channel <= maximum; channel += 2)
            {
                AddMultiplicity(result, (uint)channel, multiplicity);
            }
        }

        return result;
    }

    private static void AddMultiplicity<TKey>(Dictionary<TKey, BigInteger> channels, TKey key, BigInteger multiplicity)
        where TKey : notnull
        => channels[key] = channels.GetValueOrDefault(key) + multiplicity;
}
