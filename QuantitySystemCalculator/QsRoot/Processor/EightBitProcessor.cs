using Qs.Types;

namespace QsRoot.Processor;

public class EightBitProcessor
{
    public QsValue Af { get; set; } = QsScalar.Zero;
    public QsValue Bc { get; set; } = QsScalar.Zero;
    public QsValue De { get; set; } = QsScalar.Zero;
    public QsValue Hl { get; set; } = QsScalar.Zero;

    public QsValue Ix { get; set; } = QsScalar.Zero;
    public QsValue Iy { get; set; } = QsScalar.Zero;

    public QsValue Sp { get; set; } = QsScalar.Zero;
    public QsValue Pc { get; set; } = QsScalar.Zero;

    /// <summary>
    /// this function takes the object of z80 and return its PC  (Pointer Counter)
    /// </summary>
    /// <param name="z80"></param>
    /// <returns></returns>
    public static QsValue GetPc(EightBitProcessor epc)
    {
        return epc.Pc;
    }
}