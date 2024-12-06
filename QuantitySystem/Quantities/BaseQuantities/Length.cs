using QuantitySystem.DimensionDescriptors;

namespace QuantitySystem.Quantities.BaseQuantities;

public class Length<T> : AnyQuantity<T>
{

    public TensorRank LengthRank { get; set; }

    public Length() : base(1)
    {
        LengthRank = TensorRank.Scalar;
    }

    public Length(float exponent)
        : base(exponent)
    {
        LengthRank = TensorRank.Scalar;
    }

    public Length(float exponent, TensorRank lengthType)
        : base(exponent)
    {
        LengthRank = lengthType;
    }

    public override QuantityDimension Dimension
    {
        get
        {
            var LengthDimension = new QuantityDimension();

            switch (LengthRank)
            {
                case TensorRank.Scalar:
                    LengthDimension.Length = new LengthDescriptor(Exponent,  0);
                    break;
                case TensorRank.Vector:
                    LengthDimension.Length = new LengthDescriptor(0,  Exponent);
                    break;
            }

            return LengthDimension;
        }
    }


    public static implicit operator Length<T>(T value)
    {
        Length<T> Q = new()
        {
            Value = value
        };

        return Q;
    }

}