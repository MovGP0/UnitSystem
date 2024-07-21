using QuantitySystem.Quantities.BaseQuantities;
using Qs.Types;

namespace QsRoot;

public struct PlanetElement
{
    public string ElementName { get; set; }
    public int ElementNumber { get; set; }
    public bool Rare { get; set; }
}

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

    public QsScalar Volume
    {
        get;
        set;
    }

    public string Name { get; set; }


    public long[] Rocks { get; set; }

    static int _planets;

    public Planet NextPlanet => new();

    readonly Dictionary<string, Planet> _subPlanets = new();
    public Planet this[string planetName]
    {
        get => _subPlanets[planetName];
        set => _subPlanets[planetName] = value;
    }

    public Planet[] SubPlanets => _subPlanets.Values.ToArray();

    public int Sum(int a, int b) => a+b;

    public Planet GetNextPlanet() => new();

    public override string ToString() => Name;
}