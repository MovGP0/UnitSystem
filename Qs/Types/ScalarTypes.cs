﻿namespace Qs.Types;

/// <summary>
/// Enum of various types
/// </summary>
public enum ScalarTypes
{
    /// <summary>
    /// Default behaviour of scalar values
    /// </summary>
    NumericalQuantity = 0,

    /// <summary>
    /// Complex number storage type.
    /// </summary>
    ComplexNumberQuantity = 10,

    /// <summary>
    /// Quanternion number storage type.
    /// </summary>
    QuaternionNumberQuantity = 20,

    /// <summary>
    /// Symbolic variable
    /// </summary>
    SymbolicQuantity = 100,

    /// <summary>
    /// Function as a variable.
    /// </summary>
    FunctionQuantity = 300,

    /// <summary>
    /// @
    /// </summary>
    QsOperation = 400,

    /// <summary>
    /// Rational Number Storage type.
    /// </summary>
    /// <remarks>
    /// equal 5 because it precedes the complex number in evolution
    /// and written at the end here because I implemented it in that order.
    /// </remarks>
    RationalNumberQuantity = 5
}