using Qs.Types;
using Qs;

namespace QsRoot;

public static class Matrix
{
    public static QsMatrix Identity(int size) => QsMatrix.MakeIdentity(size);

    public static QsMatrix Random(int size) => QsMatrix.Random(size);

    public static QsMatrix Random(int n, int m) => QsMatrix.Random(n, m);

    public static QsValue Transpose(QsParameter matrix)
    {
        if (matrix.QsNativeValue is QsMatrix qsMatrix)
        {
            return qsMatrix.Transpose();
        }

        throw new QsInvalidInputException("Expected matrix input");
    }

    public static QsValue Determinant(QsParameter matrix)
    {
        if (matrix.QsNativeValue is QsMatrix qsMatrix)
        {
            return QsMatrix.Determinant(qsMatrix);
        }

        throw new QsInvalidInputException("Expected matrix input");
    }
}