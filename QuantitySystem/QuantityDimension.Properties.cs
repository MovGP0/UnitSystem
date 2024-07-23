using System.Globalization;
using QuantitySystem.DimensionDescriptors;

namespace QuantitySystem;

public sealed partial class QuantityDimension
{
    #region Dimension Physical Properties
    public MassDescriptor Mass { get; set; }
    public LengthDescriptor Length { get; set; }
    public TimeDescriptor Time { get; set; }
    public ElectricCurrentDescriptor ElectricCurrent { get; set; }
    public TemperatureDescriptor Temperature { get; set; }
    public AmountOfSubstanceDescriptor AmountOfSubstance { get; set; }
    public LuminousIntensityDescriptor LuminousIntensity { get; set; }
    #endregion

    #region Dimension Extra Properties
    public CurrencyDescriptor Currency { get; set; }
    public DigitalDescriptor Digital { get; set; }
    #endregion

    /// <summary>
    /// The Quality in MLT form
    /// </summary>
    public string MLT
    {
        get
        {
            var mass = "M" + Mass.Exponent.ToString(CultureInfo.InvariantCulture);
            var length = "L" + Length.Exponent.ToString(CultureInfo.InvariantCulture);
            var time = "T" + Time.Exponent.ToString(CultureInfo.InvariantCulture);
            return mass + length + time;
        }
    }

    /// <summary>
    /// returns the power of Force.
    /// </summary>
    public float ForceExponent
    {
        get
        {
            //M1L1T-2
            //take from MLT untill the M==0
            var TargetM = Mass.Exponent;
            float TargetF = 0;
            while (TargetM > 0)
            {
                TargetM--;
                TargetF++;
            }
            return TargetF;
        }
    }

    /// <summary>
    /// FLT powers based on Force Length, and Time.
    /// </summary>
    public string FLT
    {
        get
        {
            //M1L1T-2
            //take from MLT untill the M==0
            var TargetM = Mass.Exponent;
            var TargetL = Length.Exponent;
            var TargetT = Time.Exponent;
            float TargetF = 0;
            while (TargetM > 0)
            {
                TargetM--;
                TargetF++;
                TargetL -= 1;
                TargetT += 2;
            }

            var force = "F" + TargetF.ToString(CultureInfo.InvariantCulture);
            var length = "L" + TargetL.ToString(CultureInfo.InvariantCulture);
            var time = "T" + TargetT.ToString(CultureInfo.InvariantCulture);
            return force + length + time;
        }
    }

    public bool IsDimensionless =>
        Mass.Exponent == 0
        && Length.Exponent == 0
        && Time.Exponent == 0
        && ElectricCurrent.Exponent == 0
        && Temperature.Exponent == 0
        && AmountOfSubstance.Exponent == 0
        && LuminousIntensity.Exponent == 0
        && Currency.Exponent == 0
        && Digital.Exponent == 0;
}