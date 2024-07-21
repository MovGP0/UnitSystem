using Qs.Types;

namespace QsRoot.Processor;

public class EightBitProcessor
{
    public QsValue Af { get; set; }
    public QsValue Bc { get; set; }
    public QsValue De { get; set; }
    public QsValue Hl { get; set; }

    public QsValue Ix { get; set; }
    public QsValue Iy { get; set; }

    public QsValue Sp { get; set; }
    public QsValue Pc { get; set; }

    public EightBitProcessor()
    {
        Af = QsScalar.Zero;
        Bc = QsScalar.Zero;
        De = QsScalar.Zero;
        Hl = QsScalar.Zero;
        Ix = QsScalar.Zero;
        Iy = QsScalar.Zero;
        Sp = QsScalar.Zero;
        Pc = QsScalar.Zero;
    }

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