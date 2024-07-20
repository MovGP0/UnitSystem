using QuantitySystem.Attributes;
using QuantitySystem.Quantities.DimensionlessQuantities;

namespace QuantitySystem.Units.Misc
{



    [DefaultUnit("sp", typeof(SolidAngle<>))]
    [ReferenceUnit(4 * Math.PI)]
    public sealed class Spat : Unit
    {
    }



}
