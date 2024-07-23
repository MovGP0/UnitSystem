namespace QuantitySystem.Units;

internal struct UnitValues
{
    public string _Symbol;
    public bool _IsDefaultUnit;
    public bool _IsBaseUnit;
    public Type _QuantityType;
    public QuantityDimension _UnitDimension;
    public Unit _ReferenceUnit;
    public double _ReferenceUnitNumerator;
    public double _ReferenceUnitDenominator;
}