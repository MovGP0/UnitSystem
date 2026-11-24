namespace UnitSystem.Orientations;

/// <summary>A right-handed spatial rotation represented by a double-precision unit quaternion.</summary>
public sealed class Rotation3D
{
    private readonly double _x;

    private readonly double _y;

    private readonly double _z;

    private readonly double _w;

    private Rotation3D(double x, double y, double z, double w)
    {
        _x = x;
        _y = y;
        _z = z;
        _w = w;
    }

    /// <summary>Gets the rotation that leaves all directions unchanged.</summary>
    public static Rotation3D Identity { get; } = new(0, 0, 0, 1);

    /// <summary>Creates a rotation by normalizing a finite, nonzero quaternion.</summary>
    public static Rotation3D FromQuaternion(double x, double y, double z, double w)
    {
        var (nx, ny, nz, nw) = Normalize(x, y, z, w);
        return new Rotation3D(nx, ny, nz, nw);
    }

    /// <summary>Creates a rotation from a System.Numerics quaternion.</summary>
    public static Rotation3D FromQuaternion(Quaternion quaternion)
        => FromQuaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

    /// <summary>
    /// Creates a right-handed rotation around a finite, nonzero axis by the specified angle in radians.
    /// </summary>
    public static Rotation3D FromAxisAngle(double axisX, double axisY, double axisZ, double angleRadians)
    {
        if (!double.IsFinite(angleRadians))
        {
            throw new ArgumentOutOfRangeException(nameof(angleRadians), "A rotation requires a finite angle.");
        }

        var (x, y, z, _) = Normalize(axisX, axisY, axisZ, 0);
        var (sin, cos) = Math.SinCos(angleRadians / 2);
        return new Rotation3D(x * sin, y * sin, z * sin, cos);
    }

    internal (double X, double Y, double Z) Apply(double x, double y, double z)
    {
        var xx = 2 * _x * _x;
        var yy = 2 * _y * _y;
        var zz = 2 * _z * _z;
        var xy = 2 * _x * _y;
        var xz = 2 * _x * _z;
        var yz = 2 * _y * _z;
        var wx = 2 * _w * _x;
        var wy = 2 * _w * _y;
        var wz = 2 * _w * _z;

        return (
            ((1 - yy - zz) * x) + ((xy - wz) * y) + ((xz + wy) * z),
            ((xy + wz) * x) + ((1 - xx - zz) * y) + ((yz - wx) * z),
            ((xz - wy) * x) + ((yz + wx) * y) + ((1 - xx - yy) * z));
    }

    private static (double X, double Y, double Z, double W) Normalize(double x, double y, double z, double w)
    {
        var scale = Math.Max(Math.Max(Math.Abs(x), Math.Abs(y)), Math.Max(Math.Abs(z), Math.Abs(w)));
        if (scale == 0 || !double.IsFinite(scale))
        {
            throw new ArgumentException("A rotation requires finite components and a nonzero axis or quaternion.");
        }

        x /= scale;
        y /= scale;
        z /= scale;
        w /= scale;

        var norm = Math.Sqrt((x * x) + (y * y) + (z * z) + (w * w));
        return (x / norm, y / norm, z / norm, w / norm);
    }
}
