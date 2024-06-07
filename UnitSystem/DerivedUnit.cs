using UnitSystem.Dimensions;

namespace UnitSystem;

internal sealed class DerivedUnit : Unit
{
    internal DerivedUnit(IUnitSystem system, double factor, double offset, Dimension dimension)
        : base(system, factor, offset, dimension)
    {
    }
}
