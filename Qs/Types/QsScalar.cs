using QuantitySystem.Quantities.BaseQuantities;
using Qs.Runtime;
using SymbolicAlgebra;
using System.Globalization;
using QuantitySystem.Units;
using Qs.Numerics;


namespace Qs.Types
{
    /// <summary>
    /// Wrapper for AnyQuantit&lt;double&gt; and it serve the basic number in the Qs
    /// </summary>
    public sealed class QsScalar : QsValue, IConvertible
    {

        #region Scalar Types Storage

        /// <summary>
        /// Tells the current storage type of the scalar.
        /// </summary>
        private readonly ScalarTypes _ScalarType;

        public ScalarTypes ScalarType
        {
            get { return _ScalarType; }
        }

        /// <summary>
        /// Quantity that its storage is symbol.
        /// </summary>
        public AnyQuantity<SymbolicVariable> SymbolicQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// Double Number Quantity
        /// Default behaviour.
        /// </summary>
        public AnyQuantity<double> NumericalQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// Quantity that its storage is a complex number
        /// </summary>
        public AnyQuantity<Complex> ComplexQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// Quantity that its storage is a Quaternion number
        /// </summary>
        public AnyQuantity<Quaternion> QuaternionQuantity
        {
            get;
            set;
        }


        /// <summary>
        /// Quantity that its storage is a Function.
        /// </summary>
        public AnyQuantity<QsFunction> FunctionQuantity
        {
            get;
            set;
        }

        /// <summary>
        /// Hold qs operations like @ and \/ operations
        /// </summary>
        public QsOperation Operation
        {
            get;
            set;
        }

        /// <summary>
        /// Rational Number Quantity.
        /// </summary>
        public AnyQuantity<Rational> RationalQuantity
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Return the current scalar unit.
        /// </summary>
        public Unit Unit
        {
            get
            {
                switch (_ScalarType)
                {
                    case ScalarTypes.NumericalQuantity:
                        return NumericalQuantity.Unit;
                    case ScalarTypes.SymbolicQuantity:
                        return SymbolicQuantity.Unit;
                    case ScalarTypes.ComplexNumberQuantity:
                        return ComplexQuantity.Unit;
                    case ScalarTypes.QuaternionNumberQuantity:
                        return QuaternionQuantity.Unit;
                    case ScalarTypes.FunctionQuantity:
                        return FunctionQuantity.Unit;
                    case ScalarTypes.QsOperation:
                        return new Unit(typeof(QuantitySystem.Quantities.DimensionlessQuantities.DimensionlessQuantity<>));
                    case ScalarTypes.RationalNumberQuantity:
                        return RationalQuantity.Unit;
                    default:
                        throw new NotImplementedException("Not implemented for " + _ScalarType);
                }
            }
        }

        /// <summary>
        /// Try to convert the current quantity into symbolic quantity.
        /// </summary>
        /// <returns></returns>
        public AnyQuantity<SymbolicVariable> ToSymbolicQuantity()
        {
            switch(_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    var sv =
                        NumericalQuantity.Unit.GetThisUnitQuantity(
                        new SymbolicVariable(NumericalQuantity.Value.ToString(CultureInfo.InvariantCulture)));
                    return sv;
                case ScalarTypes.RationalNumberQuantity:
                    return
                        RationalQuantity.Unit.GetThisUnitQuantity<SymbolicVariable>(
                        new SymbolicVariable(RationalQuantity.Value.Value.ToString(CultureInfo.InvariantCulture)));
                case ScalarTypes.SymbolicQuantity:
                    return (AnyQuantity<SymbolicVariable>)SymbolicQuantity.Clone();
                default:
                    throw new NotImplementedException();
            }
        }

        public QsScalar()
        {
            _ScalarType = ScalarTypes.NumericalQuantity;
        }

        public QsScalar(ScalarTypes scalarType)
        {
            _ScalarType = scalarType;
        }

        public override string ToString()
        {
            var scalar = string.Empty;
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    scalar = NumericalQuantity.ToString();
                    break;
                case ScalarTypes.ComplexNumberQuantity:
                    scalar = ComplexQuantity.ToString();
                    break;
                case ScalarTypes.QuaternionNumberQuantity:
                    scalar = QuaternionQuantity.ToString();
                    break;
                case ScalarTypes.SymbolicQuantity:
                    scalar = SymbolicQuantity.ToString();
                    break;
                case ScalarTypes.FunctionQuantity:
                    var plist = string.Join(", ", FunctionQuantity.Value.ParametersNames);
                    scalar = FunctionQuantity.ToString();
                    break;
                case ScalarTypes.QsOperation:
                    scalar = Operation.ToString() ?? string.Empty;
                    break;
                case ScalarTypes.RationalNumberQuantity:
                    scalar = RationalQuantity.ToString();
                    break;
                default:
                    throw new NotImplementedException(_ScalarType + " Operation not implemented yet");
            }

            return scalar;
        }

        /// <summary>
        /// This gives the inner value string representation of this scalar without any unit and in parer way.
        /// </summary>
        /// <returns></returns>
        public string ToParsableValuedString()
        {
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    return NumericalQuantity.Value.ToString(CultureInfo.InvariantCulture);
                case ScalarTypes.ComplexNumberQuantity:
                    return ComplexQuantity.Value.ToQsSyntax();
                case ScalarTypes.QuaternionNumberQuantity:
                    return QuaternionQuantity.Value.ToQsSyntax();
                case ScalarTypes.SymbolicQuantity:
                    return SymbolicQuantity.Value.ToString();
                case ScalarTypes.FunctionQuantity:
                    return FunctionQuantity.Value.ToSymbolicVariable().ToString();
                case ScalarTypes.QsOperation:
                    return Operation.ToShortString();
                case ScalarTypes.RationalNumberQuantity:
                    return RationalQuantity.Value.ToQsSyntax();
                default:
                    throw new NotImplementedException(_ScalarType + " Operation not implemented yet");
            }
        }

        public override string ToShortString()
        {
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    return NumericalQuantity.ToShortString();
                case ScalarTypes.ComplexNumberQuantity:
                    return ComplexQuantity.ToShortString();
                case ScalarTypes.QuaternionNumberQuantity:
                    return QuaternionQuantity.ToShortString();
                case ScalarTypes.SymbolicQuantity:
                    return SymbolicQuantity.ToShortString();
                case ScalarTypes.FunctionQuantity:
                    return FunctionQuantity.Value.ToShortString();
                case ScalarTypes.QsOperation:
                    return Operation.ToShortString();
                case ScalarTypes.RationalNumberQuantity:
                   return RationalQuantity.ToShortString();
                default:
                    throw new NotImplementedException(_ScalarType + " Operation not implemented yet");
            }
        }

        /// <summary>
        /// Text that is able to be parsed.
        /// </summary>
        /// <returns></returns>
        public string ToExpressionParsableString()
        {
            var rv = string.Empty;

            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    rv =  NumericalQuantity.ToShortString();
                    break;

                case ScalarTypes.ComplexNumberQuantity:  // return C{Real, Imaginary}
                    rv =  "C{" + ComplexQuantity.Value.Real.ToString(CultureInfo.InvariantCulture) + ", "
                        + ComplexQuantity.Value.Imaginary.ToString(CultureInfo.InvariantCulture) + "}"
                        + ComplexQuantity.UnitText;
                    break;

                case ScalarTypes.QuaternionNumberQuantity: // return H{a, b, c, d}
                    rv =  "H{" + QuaternionQuantity.Value.Real.ToString(CultureInfo.InvariantCulture) + ", "
                        + QuaternionQuantity.Value.i.ToString(CultureInfo.InvariantCulture) + ", "
                        + QuaternionQuantity.Value.j.ToString(CultureInfo.InvariantCulture) + ", "
                        + QuaternionQuantity.Value.k.ToString(CultureInfo.InvariantCulture) + "}"
                        + QuaternionQuantity.UnitText;
                    break;

                case ScalarTypes.SymbolicQuantity: // return symbolic quantity into parsable format
                    //add $ before any symbol
                    var sq = SymbolicQuantity.Value.ToString();
                    foreach (var sym in SymbolicQuantity.Value.InvolvedSymbols)
                        sq = sq.Replace(sym, "$" + sym);
                    rv =  sq + SymbolicQuantity.UnitText;
                    break;

                case ScalarTypes.FunctionQuantity:    // return the body of the function
                    rv = FunctionQuantity.Value.SymbolicBodyText;
                    break;
                case ScalarTypes.RationalNumberQuantity:  // return Q{a, b}
                    rv = "Q{" + RationalQuantity.Value.num.ToString(CultureInfo.InvariantCulture) + ", "
                        + RationalQuantity.Value.den.ToString(CultureInfo.InvariantCulture) + "}"
                        + RationalQuantity.UnitText;
                    break;
                default:
                    throw new NotImplementedException(_ScalarType + " ToExpression String is not implemented yet");
            }

            if (rv.EndsWith("<1>")) return rv.Substring(0, rv.Length - 3);
            return rv;

        }

        /// <summary>
        /// Gets the value hosted in this scalar as text.
        /// </summary>
        /// <returns></returns>
        public override string ToValueString()
        {
            if (_ScalarType == ScalarTypes.NumericalQuantity) return NumericalQuantity.Value.ToString(CultureInfo.InvariantCulture);

            if (_ScalarType == ScalarTypes.SymbolicQuantity) return SymbolicQuantity.Value.ToString();

            if (_ScalarType == ScalarTypes.ComplexNumberQuantity) return ComplexQuantity.Value.ToString();

            if (_ScalarType == ScalarTypes.QuaternionNumberQuantity) return QuaternionQuantity.Value.ToString();

            if (_ScalarType == ScalarTypes.FunctionQuantity) return FunctionQuantity.Value.ToShortString();

            if (_ScalarType == ScalarTypes.QsOperation) return Operation.ToShortString();

            if (_ScalarType == ScalarTypes.RationalNumberQuantity) return RationalQuantity.Value.ToString();

            throw new NotImplementedException("Unknow scalar type: " + _ScalarType);
        }

        #region Operations

        #region Scalar Operations
        /// <summary>
        /// Add the passed scalar to the current scalar.
        /// </summary>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public QsScalar AddScalar(QsScalar scalar)
        {
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:  // lhs := number
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar { NumericalQuantity = NumericalQuantity + scalar.NumericalQuantity };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = ToSymbolicQuantity() + scalar.SymbolicQuantity };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = NumericalQuantity.ToComplex() + scalar.ComplexQuantity };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = NumericalQuantity.ToQuaternion() + scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar { NumericalQuantity = NumericalQuantity + scalar.RationalQuantity.Unit.GetThisUnitQuantity(scalar.RationalQuantity.Value.Value) };
                        default:
                            throw new NotImplementedException(_ScalarType + " + " + scalar.ScalarType);
                    }
                case ScalarTypes.SymbolicQuantity: // lhs := symbolic variable
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = SymbolicQuantity + scalar.ToSymbolicQuantity() };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = SymbolicQuantity + scalar.SymbolicQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = SymbolicQuantity + scalar.ToSymbolicQuantity() };
                        case ScalarTypes.FunctionQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = SymbolicQuantity + scalar.FunctionQuantity.Value.ToSymbolicQuantity() };
                        default:
                            throw new NotImplementedException(_ScalarType + " + " + scalar.ScalarType);
                    }
                case ScalarTypes.ComplexNumberQuantity: // lhs := complex
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = ComplexQuantity + scalar.NumericalQuantity.ToComplex() };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = ComplexQuantity + scalar.ComplexQuantity };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = ComplexQuantity.ToQuaternion() + scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            {
                                var result = scalar.RationalQuantity.Unit.GetThisUnitQuantity(new Complex((double)scalar.RationalQuantity.Value.Value, 0.0));
                                result = ComplexQuantity + result;
                                return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = result };
                            }

                        case ScalarTypes.SymbolicQuantity:
                        case ScalarTypes.FunctionQuantity:
                        case ScalarTypes.QsOperation:
                        default:
                            throw new NotImplementedException(_ScalarType + " + " + scalar.ScalarType);
                    }

                case ScalarTypes.QuaternionNumberQuantity: // lhs := quanternion
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = QuaternionQuantity + scalar.NumericalQuantity.ToQuaternion() };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = QuaternionQuantity + scalar.ComplexQuantity.ToQuaternion() };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = QuaternionQuantity + scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            {
                                var result = scalar.RationalQuantity.Unit.GetThisUnitQuantity(new Quaternion((double)scalar.RationalQuantity.Value.Value, 0.0, 0, 0));
                                result = QuaternionQuantity + result;
                                return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = result };
                            }
                        default:
                            throw new NotImplementedException(_ScalarType + " + " + scalar.ScalarType);
                    }
                case ScalarTypes.FunctionQuantity:  // lhs := function
                    {
                        return ((QsFunction)FunctionQuantity.Value.AddOperation(scalar)).ToQuantity().ToScalar();
                    }
                case ScalarTypes.RationalNumberQuantity: // lhs := Rational
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar(ScalarTypes.RationalNumberQuantity) { RationalQuantity = RationalQuantity + scalar.RationalQuantity };
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.RationalNumberQuantity) { RationalQuantity = RationalQuantity + scalar.NumericalQuantity.ToRational() };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity =ToSymbolicQuantity() + scalar.SymbolicQuantity };
                       case ScalarTypes.ComplexNumberQuantity:
                            {
                                AnyQuantity<Complex>? result = null;
                                result = RationalQuantity.Unit.GetThisUnitQuantity(new Complex((double)RationalQuantity.Value.Value, 0.0));
                                result = result + scalar.ComplexQuantity;
                                return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = result };
                            }
                        case ScalarTypes.QuaternionNumberQuantity:
                            {
                                AnyQuantity<Quaternion>? result = null;
                                result = RationalQuantity.Unit.GetThisUnitQuantity(new Quaternion((double)RationalQuantity.Value.Value, 0.0, 0, 0));
                                result = result + scalar.QuaternionQuantity;
                                return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = result };
                            }
                        default:
                            throw new NotImplementedException(_ScalarType + " + " + scalar.ScalarType);
                    }
                default:
                    throw new NotImplementedException(_ScalarType + " + " + scalar.ScalarType);
            }
        }

        public QsScalar SubtractScalar(QsScalar scalar)
        {
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar { NumericalQuantity = NumericalQuantity - scalar.NumericalQuantity };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = ToSymbolicQuantity() - scalar.SymbolicQuantity };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = NumericalQuantity.ToComplex() - scalar.ComplexQuantity };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = NumericalQuantity.ToQuaternion() - scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar { NumericalQuantity = NumericalQuantity - scalar.RationalQuantity.Unit.GetThisUnitQuantity(scalar.RationalQuantity.Value.Value) };
                        default:
                            throw new NotImplementedException(_ScalarType + " + " + scalar.ScalarType);
                    }
                case ScalarTypes.SymbolicQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = SymbolicQuantity - scalar.ToSymbolicQuantity() };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = SymbolicQuantity - scalar.SymbolicQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = SymbolicQuantity - scalar.ToSymbolicQuantity() };
                        case ScalarTypes.FunctionQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = SymbolicQuantity - scalar.FunctionQuantity.Value.ToSymbolicQuantity() };

                        default:
                            throw new NotImplementedException(_ScalarType + " + " + scalar.ScalarType);
                    }
                case ScalarTypes.ComplexNumberQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = ComplexQuantity - scalar.NumericalQuantity.ToComplex() };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = ComplexQuantity - scalar.ComplexQuantity };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = ComplexQuantity.ToQuaternion() - scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            {
                                var result = scalar.RationalQuantity.Unit.GetThisUnitQuantity(new Complex((double)scalar.RationalQuantity.Value.Value, 0.0));
                                result = ComplexQuantity - result;
                                return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = result };
                            }
                        default:
                            throw new NotImplementedException(_ScalarType + " + " + scalar.ScalarType);
                    }
                case ScalarTypes.QuaternionNumberQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = QuaternionQuantity - scalar.NumericalQuantity.ToQuaternion() };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = QuaternionQuantity - scalar.ComplexQuantity.ToQuaternion() };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = QuaternionQuantity - scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            {
                                var result = scalar.RationalQuantity.Unit.GetThisUnitQuantity(new Quaternion((double)scalar.RationalQuantity.Value.Value, 0.0, 0, 0));
                                result = QuaternionQuantity - result;
                                return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = result };
                            }
                        default:
                            throw new NotImplementedException(_ScalarType + " + " + scalar.ScalarType);
                    }
                case ScalarTypes.FunctionQuantity:
                    {
                        return ((QsFunction)FunctionQuantity.Value.SubtractOperation(scalar)).ToQuantity().ToScalar();
                    }
                case ScalarTypes.RationalNumberQuantity: // lhs := Rational
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar(ScalarTypes.RationalNumberQuantity) { RationalQuantity = RationalQuantity - scalar.RationalQuantity };
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.RationalNumberQuantity) { RationalQuantity = RationalQuantity - scalar.NumericalQuantity.ToRational() };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = ToSymbolicQuantity() - scalar.SymbolicQuantity };
                        case ScalarTypes.ComplexNumberQuantity:
                            {
                                var result = RationalQuantity.Unit.GetThisUnitQuantity(new Complex((double)RationalQuantity.Value.Value, 0.0));
                                result = result - scalar.ComplexQuantity;
                                return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = result };
                            }
                        case ScalarTypes.QuaternionNumberQuantity:
                            {
                                var result = RationalQuantity.Unit.GetThisUnitQuantity(new Quaternion((double)RationalQuantity.Value.Value, 0.0, 0, 0));
                                result = result - scalar.QuaternionQuantity;
                                return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = result };
                            }

                        default:
                            throw new NotImplementedException(_ScalarType + " + " + scalar.ScalarType);
                    }
                default:
                    throw new NotImplementedException(_ScalarType + " - " + scalar.ScalarType);
            }

        }

        /// <summary>
        /// Multiply the passed scalar to the current scalar instance.
        /// </summary>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public QsScalar MultiplyScalar(QsScalar scalar)
        {
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar { NumericalQuantity = NumericalQuantity * scalar.NumericalQuantity };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = ToSymbolicQuantity() * scalar.SymbolicQuantity };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = NumericalQuantity.ToComplex() * scalar.ComplexQuantity };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = NumericalQuantity.ToQuaternion() * scalar.QuaternionQuantity };
                        case ScalarTypes.FunctionQuantity:
                            return ((QsFunction)scalar.FunctionQuantity.Value.MultiplyOperation(this)).ToQuantity().ToScalar();
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar { NumericalQuantity = NumericalQuantity * scalar.RationalQuantity.Unit.GetThisUnitQuantity(scalar.RationalQuantity.Value.Value) };

                        default:
                            throw new NotImplementedException(_ScalarType + " * " + scalar.ScalarType);
                    }
                case ScalarTypes.SymbolicQuantity:
                    {
                        switch (scalar.ScalarType)
                        {
                            case ScalarTypes.NumericalQuantity:
                                return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = SymbolicQuantity * scalar.ToSymbolicQuantity() };
                            case ScalarTypes.SymbolicQuantity:
                                return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = SymbolicQuantity * scalar.SymbolicQuantity };
                            case ScalarTypes.RationalNumberQuantity:
                                return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = SymbolicQuantity * scalar.ToSymbolicQuantity() };
                            case ScalarTypes.FunctionQuantity:
                                return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = SymbolicQuantity * scalar.FunctionQuantity.Value.ToSymbolicQuantity() };
                            default:
                                throw new NotImplementedException(_ScalarType + " * " + scalar.ScalarType);
                        }
                    }
                case ScalarTypes.ComplexNumberQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = ComplexQuantity * scalar.NumericalQuantity.ToComplex() };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = ComplexQuantity * scalar.ComplexQuantity };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = ComplexQuantity.ToQuaternion() * scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            {
                                var result = scalar.RationalQuantity.Unit.GetThisUnitQuantity(new Complex((double)scalar.RationalQuantity.Value.Value, 0.0));
                                result = ComplexQuantity * result;
                                return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = result };
                            }
                        default:
                            throw new NotImplementedException(_ScalarType + " * " + scalar.ScalarType);
                    }
                case ScalarTypes.QuaternionNumberQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = QuaternionQuantity * scalar.NumericalQuantity.ToQuaternion() };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = QuaternionQuantity * scalar.ComplexQuantity.ToQuaternion() };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = QuaternionQuantity * scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            {
                                var result = scalar.RationalQuantity.Unit.GetThisUnitQuantity(new Quaternion((double)scalar.RationalQuantity.Value.Value, 0.0, 0, 0));
                                result = QuaternionQuantity * result;
                                return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = result };
                            }
                        default:
                            throw new NotImplementedException(_ScalarType + " * " + scalar.ScalarType);
                    }
                case ScalarTypes.FunctionQuantity:
                    {
                        switch (scalar.ScalarType)
                        {
                            case ScalarTypes.QsOperation:
                                return (QsScalar)scalar.Operation.MultiplyOperation(this);
                            default:
                                return ((QsFunction)FunctionQuantity.Value.MultiplyOperation(scalar)).ToQuantity().ToScalar();
                        }
                    }
                case ScalarTypes.RationalNumberQuantity: // lhs := Rational
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar(ScalarTypes.RationalNumberQuantity) { RationalQuantity = RationalQuantity * scalar.RationalQuantity };
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.RationalNumberQuantity) { RationalQuantity = RationalQuantity * scalar.NumericalQuantity.ToRational() };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = ToSymbolicQuantity() * scalar.SymbolicQuantity };
                        case ScalarTypes.ComplexNumberQuantity:
                            {
                                var result = RationalQuantity.Unit.GetThisUnitQuantity(new Complex((double)RationalQuantity.Value.Value, 0.0));
                                result = result * scalar.ComplexQuantity;
                                return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = result };
                            }
                        case ScalarTypes.QuaternionNumberQuantity:
                            {
                                var result = RationalQuantity.Unit.GetThisUnitQuantity(new Quaternion((double)RationalQuantity.Value.Value, 0.0, 0, 0));
                                result = result * scalar.QuaternionQuantity;
                                return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = result };
                            }

                        default:
                            throw new NotImplementedException(_ScalarType + " * " + scalar.ScalarType);
                    }
                case ScalarTypes.QsOperation:
                    return (QsScalar)Operation.MultiplyOperation(scalar);
                default:
                    throw new NotImplementedException(_ScalarType + " * " + scalar.ScalarType);
            }

        }

        public QsScalar DivideScalar(QsScalar scalar)
        {
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar { NumericalQuantity = NumericalQuantity / scalar.NumericalQuantity };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = ToSymbolicQuantity() / scalar.SymbolicQuantity };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = NumericalQuantity.ToComplex() / scalar.ComplexQuantity };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = NumericalQuantity.ToQuaternion() / scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar { NumericalQuantity = NumericalQuantity / scalar.RationalQuantity.Unit.GetThisUnitQuantity(scalar.RationalQuantity.Value.Value) };
                        default:
                            throw new NotImplementedException(_ScalarType + " / " + scalar.ScalarType);
                    }
                case ScalarTypes.SymbolicQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = SymbolicQuantity / scalar.ToSymbolicQuantity() };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = SymbolicQuantity / scalar.SymbolicQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = SymbolicQuantity / scalar.ToSymbolicQuantity() };
                        case ScalarTypes.FunctionQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = SymbolicQuantity / scalar.FunctionQuantity.Value.ToSymbolicQuantity() };
                        default:
                            throw new NotImplementedException(_ScalarType + " / " + scalar.ScalarType);
                    }
                case ScalarTypes.ComplexNumberQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = ComplexQuantity / scalar.NumericalQuantity.ToComplex() };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = ComplexQuantity / scalar.ComplexQuantity };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = ComplexQuantity.ToQuaternion() / scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            {
                                var result = scalar.RationalQuantity.Unit.GetThisUnitQuantity(new Complex((double)scalar.RationalQuantity.Value.Value, 0.0));
                                result = ComplexQuantity / result;
                                return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = result };
                            }
                        default:
                            throw new NotImplementedException(_ScalarType + " / " + scalar.ScalarType);
                    }
                case ScalarTypes.QuaternionNumberQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = QuaternionQuantity / scalar.NumericalQuantity.ToQuaternion() };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = QuaternionQuantity / scalar.ComplexQuantity.ToQuaternion() };
                        case ScalarTypes.QuaternionNumberQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = QuaternionQuantity / scalar.QuaternionQuantity };
                        case ScalarTypes.RationalNumberQuantity:
                            {
                                var result = scalar.RationalQuantity.Unit.GetThisUnitQuantity(new Quaternion((double)scalar.RationalQuantity.Value.Value, 0.0, 0, 0));
                                result = QuaternionQuantity / result;
                                return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = result };
                            }
                        default:
                            throw new NotImplementedException(_ScalarType + " / " + scalar.ScalarType);
                    }
                case ScalarTypes.FunctionQuantity:
                    {
                        return ((QsFunction)FunctionQuantity.Value.DivideOperation(scalar)).ToQuantity().ToScalar();
                    }
                case ScalarTypes.RationalNumberQuantity: // lhs := Rational
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.RationalNumberQuantity:
                            return new QsScalar(ScalarTypes.RationalNumberQuantity) { RationalQuantity = RationalQuantity / scalar.RationalQuantity };
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.RationalNumberQuantity) {
                                RationalQuantity = RationalQuantity / scalar.NumericalQuantity.ToRational()
                            };
                        case ScalarTypes.SymbolicQuantity:
                            return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = ToSymbolicQuantity() / scalar.SymbolicQuantity };
                        case ScalarTypes.ComplexNumberQuantity:
                            {
                                var result = RationalQuantity.Unit.GetThisUnitQuantity(new Complex((double)RationalQuantity.Value.Value, 0.0));
                                result = result / scalar.ComplexQuantity;
                                return new QsScalar(ScalarTypes.ComplexNumberQuantity) { ComplexQuantity = result };
                            }
                        case ScalarTypes.QuaternionNumberQuantity:
                            {
                                var result = RationalQuantity.Unit.GetThisUnitQuantity(new Quaternion((double)RationalQuantity.Value.Value, 0.0, 0, 0));
                                result = result / scalar.QuaternionQuantity;
                                return new QsScalar(ScalarTypes.QuaternionNumberQuantity) { QuaternionQuantity = result };
                            }

                        default:
                            throw new NotImplementedException(_ScalarType + " / " + scalar.ScalarType);
                    }

                default:
                    throw new NotImplementedException(_ScalarType + " / " + scalar.ScalarType);
            }
        }

        public QsScalar PowerScalar(QsScalar power)
        {
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    {
                        switch (power.ScalarType)
                        {
                            case ScalarTypes.NumericalQuantity:
                            {
                                if (NumericalQuantity.Value < 0.0 && power.NumericalQuantity.Value == 0.5)
                                {
                                    var av = Math.Sqrt(Math.Abs(NumericalQuantity.Value));
                                    return new Complex(0, av).ToQuantity().ToScalar();
                                }

                                return new QsScalar { NumericalQuantity = AnyQuantity<double>.Power(NumericalQuantity, power.NumericalQuantity) };
                            }
                            case ScalarTypes.SymbolicQuantity:
                            {
                                if (NumericalQuantity.Dimension.IsDimensionless)
                                {
                                    var sv = new SymbolicVariable(NumericalQuantity.Value.ToString(CultureInfo.InvariantCulture));
                                    return new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = AnyQuantity<SymbolicVariable>.Power(sv.ToQuantity(), power.SymbolicQuantity) };
                                }

                                throw new QsException("Raising none dimensionless quantity to symbolic quantity is not supported");
                            }
                            case ScalarTypes.RationalNumberQuantity:
                                return new QsScalar { NumericalQuantity = AnyQuantity<double>.Power(NumericalQuantity, power.RationalQuantity.Unit.GetThisUnitQuantity(power.RationalQuantity.Value.Value)) };
                            default:
                                throw new NotImplementedException(_ScalarType + " ^ " + power.ScalarType);
                        }

                    }
                case ScalarTypes.SymbolicQuantity:
                    {
                        switch (power.ScalarType)
                        {
                            case ScalarTypes.NumericalQuantity:
                            {
                                var dpower = power.NumericalQuantity.Value;
                                var nsq = new QsScalar(ScalarTypes.SymbolicQuantity) { SymbolicQuantity = AnyQuantity<SymbolicVariable>.Power(SymbolicQuantity, power.NumericalQuantity) };

                                return nsq;
                            }
                            case ScalarTypes.SymbolicQuantity:
                            {
                                if (power.SymbolicQuantity.Dimension.IsDimensionless && SymbolicQuantity.Dimension.IsDimensionless)
                                {
                                    // get the raised symbolic variable
                                    var RaisedSymbolicVariable = SymbolicVariable.SymbolicPower(SymbolicQuantity.Value, power.SymbolicQuantity.Value);

                                    // make it into quantity
                                    AnyQuantity<SymbolicVariable> NewSymbolicQuantity = RaisedSymbolicVariable.ToQuantity();

                                    // assign into SymbolicQuantity property in new QsScalar object.

                                    var NewSymbolicQuantityScalar = NewSymbolicQuantity.ToScalar();

                                    return NewSymbolicQuantityScalar;

                                }

                                throw new QsException("Raising Symbolic Quantity to Symbolic Quantity is only valid when the two quantities are dimensionless");
                            }
                            default:
                            throw new NotImplementedException("Raising Symbolic Quantity to " + power.ScalarType + " is not implemented yet");
                        }

                    }

                case ScalarTypes.ComplexNumberQuantity:
                    switch (power.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity)
                            {
                                ComplexQuantity = AnyQuantity<Complex>.Power(ComplexQuantity, power.NumericalQuantity)
                            };
                        case ScalarTypes.ComplexNumberQuantity:
                            return new QsScalar(ScalarTypes.ComplexNumberQuantity)
                            {
                                ComplexQuantity = AnyQuantity<Complex>.Power(ComplexQuantity, power.ComplexQuantity)
                            };
                        default:
                            throw new NotImplementedException("Raising Complex Quantity to " + power.ScalarType + " is not implemented yet");
                    }

                case ScalarTypes.QuaternionNumberQuantity:
                    switch (power.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.QuaternionNumberQuantity)
                            {
                                QuaternionQuantity = AnyQuantity<Quaternion>.Power(QuaternionQuantity, power.NumericalQuantity)
                            };
                        default:
                            throw new NotImplementedException("Raising Quaternion Quantity to " + power.ScalarType + " is not implemented yet");
                    }
                case ScalarTypes.FunctionQuantity:
                    {
                        return ((QsFunction)FunctionQuantity.Value.PowerOperation(power)).ToQuantity().ToScalar();
                    }
                case ScalarTypes.RationalNumberQuantity: // lhs := Rational
                    switch (power.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar(ScalarTypes.RationalNumberQuantity)
                            {
                                RationalQuantity = AnyQuantity<Rational>.Power(RationalQuantity,  power.NumericalQuantity)
                            };

                        default:
                            throw new NotImplementedException(_ScalarType + " ^ " + power.ScalarType);
                    }

                default:
                    throw new NotImplementedException(_ScalarType + " ^ " + power.ScalarType);
            }
        }

        public QsScalar ModuloScalar(QsScalar scalar)
        {
            switch (_ScalarType)
            {
                case ScalarTypes.NumericalQuantity:
                    switch (scalar.ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return new QsScalar { NumericalQuantity = NumericalQuantity % scalar.NumericalQuantity };
                        default:
                            throw new NotImplementedException(_ScalarType + " % " + scalar.ScalarType);
                    }
                default:
                    throw new NotImplementedException(_ScalarType + " % " + scalar.ScalarType);
            }
        }

        public QsScalar DifferentiateScalar(QsScalar scalar)
        {
            switch (_ScalarType)
            {
                case ScalarTypes.FunctionQuantity:
                    return ((QsFunction)FunctionQuantity.Value.DifferentiateOperation(scalar)).ToQuantity().ToScalar();
                case ScalarTypes.SymbolicQuantity:
                        switch (scalar._ScalarType)
                        {
                            case ScalarTypes.SymbolicQuantity:
                                {
                                    var dsv = scalar.SymbolicQuantity.Value;
                                    var nsv = SymbolicQuantity.Value;
                                    var times = (int)dsv.SymbolPower;
                                    while (times > 0)
                                    {
                                        nsv = nsv.Differentiate(dsv.Symbol);
                                        times--;
                                    }

                                    return nsv.ToQuantity().ToScalar();
                                }
                            default:
                                throw new NotImplementedException();
                        }

                case ScalarTypes.QsOperation:
                        {
                            var o = (QsScalar)Clone();
                            return (QsScalar)o.Operation.DifferentiateOperation(scalar);
                        }
                case ScalarTypes.NumericalQuantity:
                        {
                            return Zero;
                        }
                default:
                        throw new NotImplementedException(_ScalarType + " | " + scalar.ScalarType);
            }
        }

        #endregion

        #region Vector Operations

        public QsVector AddVector(QsVector vector)
        {
            var v = new QsVector(vector.Count);

            for (var i = 0; i < vector.Count; i++)
            {
                v.AddComponent(this + vector[i]);
            }

            return v;
        }

        public QsVector SubtractVector(QsVector vector)
        {
            var v = new QsVector(vector.Count);

            for (var i = 0; i < vector.Count; i++)
            {
                v.AddComponent(this - vector[i]);
            }

            return v;
        }

        /// <summary>
        /// Multiply Scalar by Vector.
        /// </summary>
        /// <param name="q"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public QsVector MultiplyVector(QsVector vector)
        {
            var v = new QsVector(vector.Count);

            for (var i = 0; i < vector.Count; i++)
            {
                v.AddComponent(MultiplyScalar(vector[i]));
            }

            return v;
        }

        #endregion

        #region Matrix Operations

        /// <summary>
        /// Scalar + Matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public QsMatrix AddMatrix(QsMatrix matrix)
        {
            var Total = new QsMatrix();
            for (var IY = 0; IY < matrix.RowsCount; IY++)
            {
                List<QsScalar> row = new(matrix.ColumnsCount);

                for (var IX = 0; IX < matrix.ColumnsCount; IX++)
                {
                    row.Add(this + matrix[IY, IX]);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;
        }

        /// <summary>
        /// Scalar - Matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public QsMatrix SubtractMatrix(QsMatrix matrix)
        {
            var Total = new QsMatrix();
            for (var IY = 0; IY < matrix.RowsCount; IY++)
            {
                List<QsScalar> row = new(matrix.ColumnsCount);

                for (var IX = 0; IX < matrix.ColumnsCount; IX++)
                {
                    row.Add(this - matrix[IY, IX]);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;
        }

        /// <summary>
        /// Scalar * Matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public QsMatrix MultiplyMatrix(QsMatrix matrix)
        {
            var Total = new QsMatrix();
            for (var IY = 0; IY < matrix.RowsCount; IY++)
            {
                List<QsScalar> row = new(matrix.ColumnsCount);

                for (var IX = 0; IX < matrix.ColumnsCount; IX++)
                {
                    row.Add(this * matrix[IY, IX]);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;
        }

        #endregion

        #endregion

        #region operators redifintion for scalar explicitly
        public static QsScalar operator +(QsScalar a, QsScalar b)
        {
            return a.AddScalar(b);
        }

        public static QsScalar operator -(QsScalar a, QsScalar b)
        {
            return a.SubtractScalar(b);
        }

        public static QsScalar operator *(QsScalar a, QsScalar b)
        {
            return a.MultiplyScalar(b);
        }

        public static QsScalar operator /(QsScalar a, QsScalar b)
        {
            return a.DivideScalar(b);
        }

        public static QsScalar operator %(QsScalar a, QsScalar b)
        {
            return a.ModuloScalar(b);
        }
        #endregion

        #region Special Values
        private static QsScalar _one = "1".ToScalar();
        private static QsScalar _zero = "0".ToScalar();
        private static QsScalar _minusOne = "-1".ToScalar();

        /// <summary>
        /// Returns -1 as dimensionless quantity scalar.
        /// </summary>
        public static QsScalar NegativeOne => _minusOne;

        /// <summary>
        /// return 1 as dimensionless quantity scalar.
        /// </summary>
        public static QsScalar One => _one;

        /// <summary>
        /// Returns zero as dimensionless quantity.
        /// </summary>
        public static QsScalar Zero => _zero;

        public static QsScalar RandomNumber => new(ScalarTypes.NumericalQuantity) { NumericalQuantity = (new Random().NextDouble()).ToQuantity() };

        #endregion

        #region QsValue Operations

        public override QsValue Identity => One;

        public override QsValue AddOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;

            if (value is QsScalar)
            {
                return AddScalar((QsScalar)value);
            }

            if (value is QsVector)
            {
                var b = value as QsVector;

                return AddVector(b);

            }
            if (value is QsText)
            {
                var text = value as QsText;
                var qt = QsEvaluator.CurrentEvaluator.SilentEvaluate(text.Text) as QsValue;
                return AddOperation(qt);
            }
            if (value is QsFunction)
            {
                if (ScalarType == ScalarTypes.SymbolicQuantity)
                {
                    var fn = value as QsFunction;
                    return AddScalar(fn.ToSymbolicScalar());
                }

                throw new NotImplementedException("Adding QsScalar[" + ScalarType + "] from QsFunction is not implemented yet");
            }
            throw new NotImplementedException("Adding QsScalar to " + value.GetType().Name + " is not implemented yet");
        }

        public override QsValue SubtractOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                return SubtractScalar((QsScalar)value);
            }

            if (value is QsVector)
            {
                var b = value as QsVector;

                return SubtractVector(b);

            }
            if (value is QsMatrix)
            {
                var m = value as QsMatrix;
                return SubtractMatrix(m);
            }
            if (value is QsText)
            {
                var text = value as QsText;
                var qt = QsEvaluator.CurrentEvaluator.SilentEvaluate(text.Text) as QsValue;
                return SubtractOperation(qt);
            }
            if (value is QsFunction)
            {
                if (ScalarType == ScalarTypes.SymbolicQuantity)
                {
                    var fn = value as QsFunction;
                    return SubtractScalar(fn.ToSymbolicScalar());
                }

                throw new NotImplementedException("Subtracting QsScalar[" + ScalarType + "] from QsFunction is not implemented yet");
            }
            throw new NotImplementedException("Subtracting QsScalar from " + value.GetType().Name + " is not implemented yet");
        }


        public override QsValue MultiplyOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                if (_ScalarType == ScalarTypes.QsOperation)
                    return Operation.MultiplyOperation(value);
                return MultiplyScalar((QsScalar)value);
            }

            if (value is QsVector)
            {
                if (ScalarType == ScalarTypes.QsOperation)
                {
                    // because the operation may include Del operator which behave like vector.
                    return Operation.MultiplyOperation(value);
                }

                var b = value as QsVector;

                return MultiplyVector(b);
            }
            if (value is QsMatrix)
            {
                var m = value as QsMatrix;
                return MultiplyMatrix(m);
            }
            if (value is QsText)
            {
                var text = value as QsText;
                var qt = QsEvaluator.CurrentEvaluator.SilentEvaluate(text.Text) as QsValue;
                return MultiplyOperation(qt);
            }
            if (value is QsFunction)
            {
                if (ScalarType == ScalarTypes.SymbolicQuantity)
                {
                    var fn = value as QsFunction;
                    return MultiplyScalar(fn.ToSymbolicScalar());
                }

                throw new NotImplementedException("Multiplying QsScalar[" + ScalarType + "] from QsFunction is not implemented yet");
            }
            throw new NotImplementedException("Multiplying QsScalar with " + value.GetType().Name + " is not implemented yet");
        }

        public override QsValue DotProductOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (_ScalarType == ScalarTypes.QsOperation)
            {
                return Operation.DotProductOperation(value);
            }

            if (_ScalarType == ScalarTypes.SymbolicQuantity && value is QsScalar sc && sc.ScalarType == ScalarTypes.SymbolicQuantity)
            {


                var fbody = "(" + SymbolicQuantity.Value + ")" + "." + "(" + sc.SymbolicQuantity.Value + ")";

                var fb = SymbolicVariable.Parse(fbody).ToQuantity().ToScalar();


                return fb;
            }
            if (_ScalarType == ScalarTypes.FunctionQuantity && value is QsScalar scalar && scalar.ScalarType == ScalarTypes.SymbolicQuantity)
            {
                return ((QsFunction)FunctionQuantity.Value.DotProductOperation(scalar)).ToQuantity().ToScalar();
            }
            return MultiplyOperation(value);
        }

        public override QsValue CrossProductOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;

            if (_ScalarType == ScalarTypes.QsOperation)
            {
                return Operation.CrossProductOperation(value);
            }

            return MultiplyOperation(value);
        }

        public override QsValue DivideOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                return DivideScalar((QsScalar)value);
            }

            if (value is QsText)
            {
                var text = value as QsText;
                var qt = QsEvaluator.CurrentEvaluator.SilentEvaluate(text.Text) as QsValue;
                return DivideOperation(qt);
            }
            if (value is QsFunction)
            {
                if (ScalarType == ScalarTypes.SymbolicQuantity)
                {
                    var fn = value as QsFunction;
                    return DivideScalar(fn.ToSymbolicScalar());
                }

                throw new NotImplementedException("Dividing QsScalar[" + ScalarType + "] from QsFunction is not implemented yet");
            }
            throw new NotImplementedException("Dividing QsScalar over " + value.GetType().Name + " is not implemented yet");
        }

        public override QsValue PowerOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                if (ScalarType == ScalarTypes.QsOperation) return Operation.PowerOperation(vl);
                return PowerScalar((QsScalar)value);
            }

            throw new NotImplementedException("Raising QsScalar to power of " + value.GetType().Name + " is not implemented yet");
        }


        /// <summary>
        /// ||Scalar||
        /// </summary>
        /// <returns></returns>
        public override QsValue NormOperation()
        {
            return AbsOperation();
        }

        /// <summary>
        /// |Scalar|
        /// </summary>
        /// <returns></returns>
        public override QsValue AbsOperation()
        {
            var q = NumericalQuantity;

            if (q.Value < 0)
            {
                return new QsScalar { NumericalQuantity = q * "-1".ToQuantity() };
            }

            return new QsScalar { NumericalQuantity = q * "1".ToQuantity() };

        }


        /// <summary>
        /// Calculate the modulo of
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override QsValue ModuloOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            // this is tricky because if you divide 5<m>/2<s> you got Speed 2.5<m/s>
            //   but the modulus will be 5<m> / 2<s> = 2<m/s> + 1<m>

            if (value is QsScalar)
            {
                return ModuloScalar((QsScalar)value);
            }

            throw new NotImplementedException("Modulo of QsScalar over " + value.GetType().Name + " is not implemented yet");
        }

        public override QsValue TensorProductOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            throw new NotImplementedException("Tensor product of QsScalar and " + value.GetType().Name + " is not implemented yet");
        }

        public override QsValue LeftShiftOperation(QsValue times)
        {
            throw new NotImplementedException();
        }

        public override QsValue RightShiftOperation(QsValue times)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region Relational Operation

        public override bool LessThan(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                var scalar = (QsScalar)value;
                return NumericalQuantity < scalar.NumericalQuantity;
            }

            if (value is QsVector)
            {
                var vector = (QsVector)value;

                //compare with the magnitude of the vector
                var mag = vector.Magnitude();

                return ((QsScalar)AbsOperation()).NumericalQuantity < mag.NumericalQuantity;

            }
            if (value is QsMatrix)
            {
                var matrix = (QsMatrix)value;
                throw new NotSupportedException();
            }
            throw new NotSupportedException();
        }

        public override bool GreaterThan(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                var scalar = (QsScalar)value;
                return NumericalQuantity > scalar.NumericalQuantity;
            }

            if (value is QsVector)
            {
                var vector = (QsVector)value;

                //compare with the magnitude of the vector
                var mag = vector.Magnitude();

                return ((QsScalar)AbsOperation()).NumericalQuantity > mag.NumericalQuantity;
            }
            if (value is QsMatrix)
            {
                var matrix = (QsMatrix)value;
                throw new NotSupportedException();
            }
            throw new NotSupportedException();
        }

        public override bool LessThanOrEqual(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                var scalar = (QsScalar)value;
                return NumericalQuantity <= scalar.NumericalQuantity;
            }

            if (value is QsVector)
            {
                var vector = (QsVector)value;
                //compare with the magnitude of the vector
                var mag = vector.Magnitude();

                return ((QsScalar)AbsOperation()).NumericalQuantity <= mag.NumericalQuantity;
            }
            if (value is QsMatrix)
            {
                var matrix = (QsMatrix)value;
                throw new NotSupportedException();
            }
            throw new NotSupportedException();
        }

        public override bool GreaterThanOrEqual(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                var scalar = (QsScalar)value;
                return NumericalQuantity >= scalar.NumericalQuantity;
            }

            if (value is QsVector)
            {
                var vector = (QsVector)value;
                //compare with the magnitude of the vector
                var mag = vector.Magnitude();

                return ((QsScalar)AbsOperation()).NumericalQuantity >= mag.NumericalQuantity;
            }
            if (value is QsMatrix)
            {
                var matrix = (QsMatrix)value;
                throw new NotSupportedException();
            }
            throw new NotSupportedException();
        }

        public override bool Equality(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if ((object)value == null) return false;

            if (value is QsScalar)
            {
                var scalar = (QsScalar)value;
                if (ScalarType == scalar.ScalarType)
                {
                    switch(ScalarType)
                    {
                        case ScalarTypes.NumericalQuantity:
                            return NumericalQuantity == scalar.NumericalQuantity;
                        case ScalarTypes.ComplexNumberQuantity:
                            return ComplexQuantity == scalar.ComplexQuantity;
                        case  ScalarTypes.QuaternionNumberQuantity:
                            return QuaternionQuantity == scalar.QuaternionQuantity;
                        case ScalarTypes.SymbolicQuantity:
                            return SymbolicQuantity == scalar.SymbolicQuantity;
                        case ScalarTypes.FunctionQuantity:
                            return FunctionQuantity == scalar.FunctionQuantity;
                        case ScalarTypes.QsOperation:
                            return Operation.Equality(scalar.Operation);
                        default:
                            throw new QsException("N/A");
                    }
                }

                return false;
            }

            if (value is QsVector)
            {
                var vector = (QsVector)value;
                //compare with the magnitude of the vector
                var mag = vector.Magnitude();

                return ((QsScalar)AbsOperation()).NumericalQuantity == mag.NumericalQuantity;
            }
            if (value is QsMatrix)
            {
                var matrix = (QsMatrix)value;
                throw new NotSupportedException();
            }
            throw new NotSupportedException();
        }

        public override bool Inequality(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                var scalar = (QsScalar)value;
                return NumericalQuantity != scalar.NumericalQuantity;
            }

            if (value is QsVector)
            {
                var vector = (QsVector)value;
                //compare with the magnitude of the vector
                var mag = vector.Magnitude();

                return ((QsScalar)AbsOperation()).NumericalQuantity != mag.NumericalQuantity;
            }
            if (value is QsMatrix)
            {
                var matrix = (QsMatrix)value;
                throw new NotSupportedException();
            }
            throw new NotSupportedException();
        }

        #endregion

        public override QsValue GetIndexedItem(QsParameter[] indices)
        {
            // I am adding new feature here that
            //  I can execute the function if it was as a scalar value of function type.
            //  which means if f(x) = x^2
            //  you can call the function   as      f(3)
            //  or better you can call it  as  @f[3]
            // -------------
            //  why I am doing this  ??
            //   because I was  want to be able to call the function directly after differentiating it
            //   @f|$x[3]
            //  why I didn't use  normal brackets??
            //   because it is reserved to know the function itself by parameters (as I have overloaded functions by parameter names) not types
            //   @f(x,y)  !=  @f(u,v)    etc.


            if (_ScalarType == ScalarTypes.FunctionQuantity)
            {
                return FunctionQuantity.Value.InvokeByQsParameters(indices);
            }

            throw new QsException(string.Format("Indexer is not implemented for Scalar type {0}", _ScalarType.ToString()));
        }

        public override void SetIndexedItem(QsParameter[] indices, QsValue value)
        {
            throw new NotImplementedException();
        }

        #region ICloneable Members

        public object Clone()
        {
            var n = new QsScalar(_ScalarType);

            switch (_ScalarType)
            {
                case  ScalarTypes.NumericalQuantity:
                    n.NumericalQuantity = (AnyQuantity<double>)NumericalQuantity.Clone();
                    break;

                case ScalarTypes.SymbolicQuantity:
                    {
                        var svalue = (SymbolicVariable)SymbolicQuantity.Value.Clone();
                        var sq = SymbolicQuantity.Unit.GetThisUnitQuantity(svalue);
                        n.SymbolicQuantity = sq;
                    }
                    break;

                case ScalarTypes.ComplexNumberQuantity:
                    n.ComplexQuantity = (AnyQuantity<Complex>)ComplexQuantity.Clone();
                    break;

                case ScalarTypes.QuaternionNumberQuantity:
                    n.QuaternionQuantity = (AnyQuantity<Quaternion>)QuaternionQuantity.Clone();
                    break;

                case ScalarTypes.FunctionQuantity:
                    {
                        var fvalue = (QsFunction)FunctionQuantity.Value.Clone();
                        var fq = FunctionQuantity.Unit.GetThisUnitQuantity<QsFunction>(fvalue);
                        n.FunctionQuantity = fq;
                    }
                    break;

                case ScalarTypes.QsOperation:
                    n.Operation = (QsOperation)Operation.Clone();
                    break;


                case ScalarTypes.RationalNumberQuantity:
                    n.RationalQuantity = (AnyQuantity<Rational>)RationalQuantity.Clone();
                    break;

            }

            return n;
        }

        #endregion

        /// <summary>
        /// differentiate the current scalar  by overriding the method
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override QsValue DifferentiateOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
                return DifferentiateScalar((QsScalar)value);
            return base.DifferentiateOperation(value);
        }

        /// <summary>
        /// make a range from  this scalar to the input parameter scalar.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override QsValue RangeOperation(QsValue vl)
        {
            var value = vl is QsReference reference
                ? reference.ContentValue
                : vl;

            if (value is QsScalar to)
            {
                if (_ScalarType == ScalarTypes.NumericalQuantity && to._ScalarType == ScalarTypes.NumericalQuantity)
                {
                    var start = NumericalQuantity.Value;
                    var end = to.NumericalQuantity.Value;

                    QsVector v = new();
                    if (end >= start)
                        for (var id = start; id <= end; id++) v.AddComponent(id.ToQuantity().ToScalar());
                    else
                        for (var id = start; id >= end; id--) v.AddComponent(id.ToQuantity().ToScalar());

                    return v;
                }

                throw new NotImplementedException("Range from " + _ScalarType + " to " + to._ScalarType + " is not supported");
            }

            throw new NotImplementedException("Range to " + value.GetType().Name + " is not supported");
        }


        #region IConvertible
        public TypeCode GetTypeCode()
        {
            throw new NotImplementedException();
        }

        public bool ToBoolean(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public byte ToByte(IFormatProvider? provider)
        {
            return (byte)NumericalQuantity.Value;
        }

        public char ToChar(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public DateTime ToDateTime(IFormatProvider? provider)
        {
            throw new NotImplementedException();
        }

        public decimal ToDecimal(IFormatProvider? provider)
        {
            return (decimal)NumericalQuantity.Value;
        }

        public double ToDouble(IFormatProvider? provider)
        {
            return NumericalQuantity.Value;
        }

        public short ToInt16(IFormatProvider? provider)
        {
            return (short)NumericalQuantity.Value;
        }

        public int ToInt32(IFormatProvider? provider)
        {
            return (int)NumericalQuantity.Value;
        }

        public long ToInt64(IFormatProvider? provider)
        {
            return (long)NumericalQuantity.Value;
        }

        public sbyte ToSByte(IFormatProvider? provider)
        {
            return (sbyte)NumericalQuantity.Value;
        }

        public float ToSingle(IFormatProvider? provider)
        {
            return (float)NumericalQuantity.Value;
        }

        public string ToString(IFormatProvider? provider)
        {
            return NumericalQuantity.ToShortString();
        }

        public object ToType(Type conversionType, IFormatProvider? provider)
        {
            if (conversionType == typeof(QsFunction))
                return FunctionQuantity.Value;
            if (conversionType == typeof(Complex))
                return ComplexQuantity.Value;
            if (conversionType == typeof(Quaternion))
                return QuaternionQuantity.Value;
            if (conversionType == typeof(Rational))
                return RationalQuantity.Value;
            if (conversionType == typeof(SymbolicVariable))
                return SymbolicQuantity.Value;

            switch (ScalarType)
            {
                case ScalarTypes.ComplexNumberQuantity:
                    return ComplexQuantity;
                case ScalarTypes.FunctionQuantity:
                    return FunctionQuantity;
                case ScalarTypes.NumericalQuantity:
                    return NumericalQuantity;
                case ScalarTypes.QuaternionNumberQuantity:
                    return QuaternionQuantity;
                case ScalarTypes.RationalNumberQuantity:
                    return RationalQuantity;
                case ScalarTypes.SymbolicQuantity:
                    return SymbolicQuantity;

                case ScalarTypes.QsOperation:
                default:
                    return new object();
            }
        }

        public ushort ToUInt16(IFormatProvider? provider)
        {
            return (ushort)NumericalQuantity.Value;
        }

        public uint ToUInt32(IFormatProvider? provider)
        {
            return (uint)NumericalQuantity.Value;
        }

        public ulong ToUInt64(IFormatProvider? provider)
        {
            return (ulong)NumericalQuantity.Value;
        }
        #endregion

        public override QsValue Execute(ParticleLexer.Token expression)
        {
            if (_ScalarType == ScalarTypes.QuaternionNumberQuantity && expression.TokenValue.Equals("RotationMatrix", StringComparison.OrdinalIgnoreCase))
            {
                return QuaternionQuantity.Value.To_3x3_RotationMatrix();
            }

            return base.Execute(expression);
        }


        /// <summary>
        /// returns constant value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static QsValue Constant(string name)
        {
            if(name.Equals("i", StringComparison.OrdinalIgnoreCase))
            {
                return new QsScalar(ScalarTypes.ComplexNumberQuantity)
                {
                    ComplexQuantity = Unit.Parse("1").GetThisUnitQuantity( Complex.ImaginaryOne)
                };
            }

            if (name.Equals("pi", StringComparison.OrdinalIgnoreCase))
            {
                return Math.PI.ToQuantity().ToScalarValue();
            }

            if (name.Equals("phi", StringComparison.OrdinalIgnoreCase))
            {
                return ((1 + Math.Sqrt(5)) / 2).ToQuantity().ToScalarValue();
            }

            if (name.Equals("e", StringComparison.OrdinalIgnoreCase))
            {
                return Math.E.ToQuantity().ToScalarValue();
            }

            return _zero;
        }
    }
}
