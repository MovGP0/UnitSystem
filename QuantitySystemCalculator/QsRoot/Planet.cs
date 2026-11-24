using QuantitySystem.Quantities.BaseQuantities;
using Qs.Types;

namespace QsRoot;

public sealed class Planet
{
    public Mass<double> Mass { get; set; }

    public PlanetElement Element { get; set; } = new()
    {
        ElementName = "7agar",
        ElementNumber = 34
    };

    public bool Rare { get; set; }

    public Planet()
    {
        _planets++;
        Name = "Planet " + _planets;
    }

    public Planet(PlanetElement pe)
    {
        Element = pe;
    }

    public QsScalar Volume { get; set; }

    public string Name { get; set; }

    public long[] Rocks { get; set; }

    static int _planets;

    public Planet NextPlanet => GetNextPlanet();

    readonly Dictionary<string, Planet> _subPlanets = new();

    public Planet this[string planetName]
    {
        get => _subPlanets[planetName];
        set => _subPlanets[planetName] = value;
    }

    public Planet[] SubPlanets => _subPlanets.Values.ToArray();

#pragma warning disable CA1822 // The Qs scripting host resolves this member on object instances.
    public int Sum(int a, int b) => a+b;
#pragma warning restore CA1822

#pragma warning disable CA1822 // The Qs scripting host resolves this member on object instances.
    public Planet GetNextPlanet() => new();
#pragma warning restore CA1822

    public override string ToString() => Name;
}
