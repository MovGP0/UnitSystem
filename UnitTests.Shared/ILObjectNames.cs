// ReSharper disable InconsistentNaming
namespace UnitTests.Shared;

/// <summary>
/// Provides constants for special IL (Intermediate Language) names used in .NET for various operations and constructs.
/// </summary>
public static class ILObjectNames
{
    /// <summary>Represents the name of a class constructor in IL.</summary>
    public const string Constructor = ".ctor";

    /// <summary>Represents the name of a class destructor in IL.</summary>
    public const string Destructor = ".dtor";

    /// <summary>Represents the IL operation name for an Indexer.</summary>
    public const string Indexer = "Item";

    /// <summary>Represents the IL operation name for addition.</summary>
    public const string Addition = "op_Addition";

    /// <summary>Represents the IL operation name for subtraction.</summary>
    public const string Subtraction = "op_Subtraction";

    /// <summary>Represents the IL operation name for multiplication.</summary>
    public const string Multiply = "op_Multiply";

    /// <summary>Represents the IL operation name for division.</summary>
    public const string Division = "op_Division";

    /// <summary>Represents the IL operation name for modulus.</summary>
    public const string Modulus = "op_Modulus";

    /// <summary>Represents the IL operation name for bitwise AND.</summary>
    public const string BitwiseAnd = "op_BitwiseAnd";

    /// <summary>Represents the IL operation name for bitwise OR.</summary>
    public const string BitwiseOr = "op_BitwiseOr";

    /// <summary>Represents the IL operation name for exclusive OR.</summary>
    public const string ExclusiveOr = "op_ExclusiveOr";

    /// <summary>Represents the IL operation name for power.</summary>
    /// <remarks>
    /// Since there is no dedicated power operator in .NET, this defaults to XOR.
    /// </remarks>
    public const string Power = "op_ExclusiveOr";

    /// <summary>Represents the IL operation name for left shift.</summary>
    public const string LeftShift = "op_LeftShift";

    /// <summary>Represents the IL operation name for right shift.</summary>
    public const string RightShift = "op_RightShift";

    /// <summary>Represents the IL operation name for unary negation.</summary>
    public const string UnaryNegation = "op_UnaryNegation";

    /// <summary>Represents the IL operation name for unary plus.</summary>
    public const string UnaryPlus = "op_UnaryPlus";

    /// <summary>Represents the IL operation name for increment.</summary>
    public const string Increment = "op_Increment";

    /// <summary>Represents the IL operation name for decrement.</summary>
    public const string Decrement = "op_Decrement";

    /// <summary>Represents the IL operation name for one's complement (bitwise NOT).</summary>
    public const string OnesComplement = "op_OnesComplement";

    /// <summary>Represents the IL operation name for logical true.</summary>
    public const string True = "op_True";

    /// <summary>Represents the IL operation name for logical false.</summary>
    public const string False = "op_False";

    /// <summary>Represents the IL operation name for equality.</summary>
    public const string Equality = "op_Equality";

    /// <summary>Represents the IL operation name for inequality.</summary>
    public const string Inequality = "op_Inequality";

    /// <summary>Represents the IL operation name for greater than.</summary>
    public const string GreaterThan = "op_GreaterThan";

    /// <summary>Represents the IL operation name for less than.</summary>
    public const string LessThan = "op_LessThan";

    /// <summary>Represents the IL operation name for greater than or equal to.</summary>
    public const string GreaterThanOrEqual = "op_GreaterThanOrEqual";

    /// <summary>Represents the IL operation name for less than or equal to.</summary>
    public const string LessThanOrEqual = "op_LessThanOrEqual";

    /// <summary>Represents the IL operation name for implicit conversion.</summary>
    public const string Implicit = "op_Implicit";

    /// <summary>Represents the IL operation name for explicit conversion.</summary>
    public const string Explicit = "op_Explicit";

    /// <summary>Represents the IL operation name for addition assignment (e.g., +=).</summary>
    public const string AdditionAssignment = "op_AdditionAssignment";

    /// <summary>Represents the IL operation name for subtraction assignment (e.g., -=).</summary>
    public const string SubtractionAssignment = "op_SubtractionAssignment";

    /// <summary>Represents the IL operation name for multiplication assignment (e.g., *=).</summary>
    public const string MultiplicationAssignment = "op_MultiplicationAssignment";

    /// <summary>Represents the IL operation name for division assignment (e.g., /=).</summary>
    public const string DivisionAssignment = "op_DivisionAssignment";

    /// <summary>Represents the IL operation name for modulus assignment (e.g., %=).</summary>
    public const string ModulusAssignment = "op_ModulusAssignment";

    /// <summary>Represents the IL operation name for bitwise AND assignment (e.g., &=).</summary>
    public const string BitwiseAndAssignment = "op_BitwiseAndAssignment";

    /// <summary>Represents the IL operation name for bitwise OR assignment (e.g., |=).</summary>
    public const string BitwiseOrAssignment = "op_BitwiseOrAssignment";

    /// <summary>Represents the IL operation name for exclusive OR assignment (e.g., ^=).</summary>
    public const string ExclusiveOrAssignment = "op_ExclusiveOrAssignment";

    /// <summary>Represents the IL operation name for left shift assignment (e.g., <<=).</summary>
    public const string LeftShiftAssignment = "op_LeftShiftAssignment";

    /// <summary>Represents the IL operation name for right shift assignment (e.g., >>=).</summary>
    public const string RightShiftAssignment = "op_RightShiftAssignment";

    /// <summary>Represents the IL operation name for unary negation assignment.</summary>
    public const string UnaryNegationAssignment = "op_UnaryNegationAssignment";

    /// <summary>Represents the IL operation name for unary plus assignment.</summary>
    public const string UnaryPlusAssignment = "op_UnaryPlusAssignment";

    /// <summary>Represents the IL operation name for increment assignment (e.g., ++).</summary>
    public const string IncrementAssignment = "op_IncrementAssignment";

    /// <summary>Represents the IL operation name for decrement assignment (e.g., --).</summary>
    public const string DecrementAssignment = "op_DecrementAssignment";

    /// <summary>Represents the IL operation name for one's complement assignment (bitwise NOT).</summary>
    public const string OnesComplementAssignment = "op_OnesComplementAssignment";

    /// <summary>Represents the IL operation name for implicit conversion assignment.</summary>
    public const string ImplicitAssignment = "op_ImplicitAssignment";

    /// <summary>Represents the IL operation name for explicit conversion assignment.</summary>
    public const string ExplicitAssignment = "op_ExplicitAssignment";

    /// <summary>Represents the IL operation name for true assignment.</summary>
    public const string TrueAssignment = "op_TrueAssignment";

    /// <summary>Represents the IL operation name for false assignment.</summary>
    public const string FalseAssignment = "op_FalseAssignment";

    /// <summary>Represents the IL operation name for equality assignment.</summary>
    public const string EqualityAssignment = "op_EqualityAssignment";

    /// <summary>Represents the IL operation name for inequality assignment.</summary>
    public const string InequalityAssignment = "op_InequalityAssignment";

    /// <summary>Represents the IL operation name for greater than assignment.</summary>
    public const string GreaterThanAssignment = "op_GreaterThanAssignment";

    /// <summary>Represents the IL operation name for less than assignment.</summary>
    public const string LessThanAssignment = "op_LessThanAssignment";

    /// <summary>Represents the IL operation name for greater than or equal to assignment.</summary>
    public const string GreaterThanOrEqualAssignment = "op_GreaterThanOrEqualAssignment";

    /// <summary>Represents the IL operation name for less than or equal to assignment.</summary>
    public const string LessThanOrEqualAssignment = "op_LessThanOrEqualAssignment";
}