using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Misc;

/// <summary>
/// Represents a device independent pixel (DIP) unit.
/// A device independent pixel is defined as a 1/96 of inch:
/// 1 inch = 96 DIP = 2.54 cm.
/// It is the primary unit of measurement, instead of hardware pixels, for WPF.
/// </summary>
/// <see href="http://msmvps.com/blogs/odewit/archive/2008/01/07/how-to-exploit-the-wpf-device-independent-pixel.aspx"/>
/// <seealso href="http://msdn.microsoft.com/en-us/library/ms748373.aspx"/>
[Unit("DIP", typeof(Length<>))]
[ReferenceUnit(0.0254, 96.0)]
public sealed class DeviceIndependentPixel : Unit;