using ParticleLexer;

namespace Qs.Types;

public partial class QsMatrix : QsValue
{

    #region Scalar operations

    /// <summary>
    /// Matrix / scalar
    /// </summary>
    /// <param name="scalarQuantity"></param>
    /// <returns></returns>
    public QsMatrix MultiplyScalar(QsScalar scalar)
    {
        var Total = new QsMatrix();
        for (var IY = 0; IY < RowsCount; IY++)
        {
            List<QsScalar> row = new(ColumnsCount);

            for (var IX = 0; IX < ColumnsCount; IX++)
            {
                row.Add(this[IY, IX] * scalar);
            }

            Total.AddRow(row.ToArray());
        }
        return Total;

    }

    /// <summary>
    /// Matrix / scalar
    /// </summary>
    /// <param name="scalarQuantity"></param>
    /// <returns></returns>
    public QsMatrix DivideScalar(QsScalar scalar)
    {
        var Total = new QsMatrix();
        for (var IY = 0; IY < RowsCount; IY++)
        {
            List<QsScalar> row = new(ColumnsCount);

            for (var IX = 0; IX < ColumnsCount; IX++)
            {
                row.Add(this[IY, IX] / scalar);
            }

            Total.AddRow(row.ToArray());
        }
        return Total;

    }

    /// <summary>
    /// Matrix % Scalar   Remainder of division of matrix
    /// </summary>
    /// <param name="scalar"></param>
    /// <returns></returns>
    private QsValue ModuloScalar(QsScalar scalar)
    {
        var Total = new QsMatrix();
        for (var IY = 0; IY < RowsCount; IY++)
        {
            List<QsScalar> row = new(ColumnsCount);

            for (var IX = 0; IX < ColumnsCount; IX++)
            {
                row.Add(this[IY, IX] % scalar);
            }

            Total.AddRow(row.ToArray());
        }
        return Total;
    }

    /// <summary>
    /// Matrix + scalar
    /// </summary>
    /// <param name="scalar"></param>
    /// <returns></returns>
    private QsMatrix AddScalar(QsScalar scalar)
    {
        var Total = new QsMatrix();
        for (var IY = 0; IY < RowsCount; IY++)
        {
            List<QsScalar> row = new(ColumnsCount);

            for (var IX = 0; IX < ColumnsCount; IX++)
            {
                row.Add(this[IY, IX] + scalar);
            }

            Total.AddRow(row.ToArray());
        }
        return Total;
    }

    /// <summary>
    /// Matrix - scalar
    /// </summary>
    /// <param name="scalarQuantity"></param>
    /// <returns></returns>
    private QsMatrix SubtractScalar(QsScalar scalar)
    {
        var Total = new QsMatrix();
        for (var IY = 0; IY < RowsCount; IY++)
        {
            List<QsScalar> row = new(ColumnsCount);

            for (var IX = 0; IX < ColumnsCount; IX++)
            {
                row.Add(this[IY, IX] - scalar);
            }

            Total.AddRow(row.ToArray());
        }
        return Total;
    }



    /// <summary>
    /// Matrix ^ scalar
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public QsMatrix PowerScalar(QsScalar value)
    {
        var Total = (QsMatrix)Identity; //first get the identity matrix of this matrix.

        var count = Qs.IntegerFromQsValue(value);

        if (count > 0)
        {
            for (var i = 1; i <= count; i++)
            {
                Total = Total.MultiplyMatrix(this);
            }
        }
        else if (count == 0)
        {
            return Total;   //which id identity already
        }
        else
        {
            count = Math.Abs(count);
            for (var i = 1; i <= count; i++)
            {
                Total = Total.MultiplyMatrix(Inverse);    //multiply the inverses many times
            }
        }

        return Total;

    }



    /// <summary>
    /// Matrix elements ^. scalar
    /// </summary>
    /// <param name="scalarQuantity"></param>
    /// <returns></returns>
    public QsMatrix ElementsPowerScalar(QsScalar scalar)
    {
        var Total = new QsMatrix();
        for (var IY = 0; IY < RowsCount; IY++)
        {
            List<QsScalar> row = new(ColumnsCount);

            for (var IX = 0; IX < ColumnsCount; IX++)
            {
                row.Add(this[IY, IX].PowerScalar(scalar));
            }

            Total.AddRow(row.ToArray());
        }
        return Total;
    }

    #endregion



    #region Matrix oeprators
    public static QsMatrix operator *(QsMatrix a, QsScalar b)
    {
        return a.MultiplyScalar(b);
    }

    public static QsMatrix operator *(QsMatrix a, QsMatrix b)
    {
        return a.MultiplyMatrix(b);
    }

    public static QsMatrix operator +(QsMatrix a, QsScalar b)
    {
        return a.AddScalar(b);
    }

    public static QsMatrix operator +(QsMatrix a, QsMatrix b)
    {
        return a.AddMatrix(b);
    }

    public static QsMatrix operator -(QsMatrix a, QsScalar b)
    {
        return a.SubtractScalar(b);
    }

    public static QsMatrix operator -(QsMatrix a, QsMatrix b)
    {
        return a.SubtractMatrix(b);
    }

    #endregion


    /*
    #region Scalar operators
    public static QsMatrix operator +(QsMatrix a, AnyQuantity<double> b)
    {
        return a.AddMatrixToScalar(b);
    }

    public static QsMatrix operator +(AnyQuantity<double> a, QsMatrix b)
    {
        return b.AddScalarToMatrix(a);
    }

    public static QsMatrix operator -(QsMatrix a, AnyQuantity<double> b)
    {
        return a.SubtractMatrixFromScalar(b);
    }

    public static QsMatrix operator -(AnyQuantity<double> a, QsMatrix b)
    {
        return b.SubtractScalarFromMatrix(a);
    }


    public static QsMatrix operator /(QsMatrix a, AnyQuantity<double> b)
    {
        return a.DivideMatrixByScalar(b);
    }

    public static QsMatrix operator /(AnyQuantity<double> a, QsMatrix b)
    {
        return b.DivideScalarByMatrix(a);
    }

    public static QsMatrix operator *(QsMatrix a, AnyQuantity<double> b)
    {
        return a.MultiplyMatrixByScalar(b);
    }

    public static QsMatrix operator *(AnyQuantity<double> a, QsMatrix b)
    {
        return b.MultiplyMatrixByScalar(a);
    }
    #endregion

    */



    #region QsValue operations

    public override QsValue Identity
    {
        get
        {
            if (IsSquared)
            {
                return MakeIdentity(RowsCount);
            }

            throw new QsMatrixException("No Identity matrix for non square matrix");
        }

    }

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
            throw new NotSupportedException();
        }
        if (value is QsMatrix)
        {
            return AddMatrix((QsMatrix)value);
        }
        throw new NotSupportedException();
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
            throw new NotSupportedException();
        }
        if (value is QsMatrix)
        {
            return SubtractMatrix((QsMatrix)value);
        }
        throw new NotSupportedException();
    }

    public override QsValue MultiplyOperation(QsValue vl)
    {
        QsValue value;
        if (vl is QsReference) value = ((QsReference)vl).ContentValue;
        else value = vl;

        if (value is QsScalar)
        {
            return MultiplyScalar((QsScalar)value);
        }

        if (value is QsVector)
        {
            // return value is column vector
            var mvec =  MultiplyMatrix(((QsVector)value).ToCoVectorMatrix());

            // make it ordinary vector again.
            return mvec.Columns[0];
        }
        if (value is QsMatrix)
        {
            return MultiplyMatrix((QsMatrix)value);
        }
        throw new NotSupportedException();
    }

    public override QsValue DotProductOperation(QsValue vl)
    {
        QsValue value;
        if (vl is QsReference) value = ((QsReference)vl).ContentValue;
        else value = vl;


        if (value is QsScalar)
        {
            return MultiplyScalar((QsScalar)value);
        }

        if (value is QsVector)
        {
            throw new NotSupportedException();

        }
        if (value is QsMatrix)
        {

            throw new NotSupportedException();
        }
        throw new NotSupportedException();
    }

    public override QsValue CrossProductOperation(QsValue value)
    {
        throw new NotImplementedException();
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

        if (value is QsVector)
        {
            throw new NotSupportedException();
        }
        if (value is QsMatrix)
        {
            return MultiplyMatrix(((QsMatrix)value).Inverse);
        }
        throw new NotSupportedException();
    }

    public override QsValue ModuloOperation(QsValue vl)
    {
        QsValue value;
        if (vl is QsReference) value = ((QsReference)vl).ContentValue;
        else value = vl;


        if (value is QsScalar)
        {
            return ModuloScalar((QsScalar)value);
        }

        if (value is QsVector)
        {
            throw new NotSupportedException();
        }
        if (value is QsMatrix)
        {
            throw new NotSupportedException();
        }
        throw new NotSupportedException();
    }


    public override QsValue PowerOperation(QsValue vl)
    {
        QsValue value;
        if (vl is QsReference) value = ((QsReference)vl).ContentValue;
        else value = vl;


        if (value is QsScalar)
        {
            var s = value as QsScalar;
            return PowerScalar(s);
        }

        if (value is QsVector)
        {
            throw new NotSupportedException();
        }
        if (value is QsMatrix)
        {
            throw new NotSupportedException();
        }
        throw new NotSupportedException();
    }

    public override QsValue NormOperation()
    {
        throw new NotImplementedException();
    }

    public override QsValue AbsOperation()
    {

        if (RowsCount < 4)
            return Determinant().Sum();
        return Determinant(this);

    }


    public override QsValue PowerDotOperation(QsValue vl)
    {
        QsValue value;
        if (vl is QsReference) value = ((QsReference)vl).ContentValue;
        else value = vl;


        if (value is QsScalar)
        {
            var s = value as QsScalar;
            return ElementsPowerScalar(s);
        }

        if (value is QsVector)
        {
            throw new NotSupportedException();
        }
        if (value is QsMatrix)
        {
            throw new NotSupportedException();
        }
        throw new NotSupportedException();
    }

    /// <summary>
    /// for the tensor product '(*)'  operator
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public override QsValue TensorProductOperation(QsValue vl)
    {
        QsValue value;
        if (vl is QsReference) value = ((QsReference)vl).ContentValue;
        else value = vl;


        if (value is QsMatrix)
        {
            // Kronecker product
            var tm = (QsMatrix)value;

            var result = new QsMatrix();

            List<QsMatrix> lm = [];

            for (var i = 0; i < RowsCount; i++)
            {
                QsMatrix rowM = null;
                for (var j = 0; j < ColumnsCount; j++)
                {
                    var element = this[i, j];
                    var imat =  (QsMatrix)(tm * element);
                    if (rowM == null) rowM = imat;
                    else rowM = rowM.AppendRightMatrix(imat);

                }
                lm.Add(rowM);
            }

            // append vertically all matrices

            foreach (var rm in lm)
            {

                result = result.AppendLowerMatrix(rm);
            }

            return result;
        }
        throw new NotImplementedException();
    }


    /// <summary>
    /// '<<' left shift operator
    /// </summary>
    /// <param name="times"></param>
    /// <returns></returns>
    public override QsValue LeftShiftOperation(QsValue vl)
    {
        QsValue times;
        if (vl is QsReference) times = ((QsReference)vl).ContentValue;
        else times = vl;


        var ShiftedMatrix = new QsMatrix();
        foreach (var vec in this)
        {
            ShiftedMatrix.AddVector((QsVector)vec.LeftShiftOperation(times));
        }
        return ShiftedMatrix;
    }


    /// <summary>
    /// '>>' Right Shift Operator
    /// </summary>
    /// <param name="times"></param>
    /// <returns></returns>
    public override QsValue RightShiftOperation(QsValue vl)
    {
        QsValue times;
        if (vl is QsReference) times = ((QsReference)vl).ContentValue;
        else times = vl;


        var ShiftedMatrix = new QsMatrix();
        foreach (var vec in this)
        {
            ShiftedMatrix.AddVector((QsVector)vec.RightShiftOperation(times));
        }
        return ShiftedMatrix;
    }

    #endregion


    #region Relational Operations
    public override bool LessThan(QsValue value)
    {
        throw new NotImplementedException();
    }

    public override bool GreaterThan(QsValue value)
    {
        throw new NotImplementedException();
    }

    public override bool LessThanOrEqual(QsValue value)
    {
        throw new NotImplementedException();
    }

    public override bool GreaterThanOrEqual(QsValue value)
    {
        throw new NotImplementedException();
    }

    public override bool Equality(QsValue value)
    {
        //if ((object)value == null) return false;
        if(value is QsMatrix m)
        {
            if (RowsCount != m.RowsCount) return false;
            if (ColumnsCount != m.ColumnsCount) return false;

            if (!ReferenceEquals(m, this))
            {

                // if not then we need to compare it element by element

                for (var i = 0; i < RowsCount; i++)
                {
                    for (var j = 0; j < ColumnsCount; j++)
                    {

                        if (!this[i, j].Equality(m[i, j])) return false;
                    }
                }

            }

            return true;
        }

        return false;
    }

    public override bool Inequality(QsValue value)
    {

        throw new NotImplementedException();
    }

    /// <summary>
    /// Return scalar with two indices
    /// or Vector in one index
    /// </summary>
    /// <param name="indices"></param>
    /// <returns></returns>
    public override QsValue GetIndexedItem(QsParameter[] allIndices)
    {
        var indices = new int[allIndices.Length];
        for (var ix = 0; ix < indices.Length; ix++) indices[ix] = (int)((QsScalar)allIndices[ix].QsNativeValue).NumericalQuantity.Value;

        var icount = indices.Count();
        if (icount == 2)
        {
            return this[indices[0], indices[1]];
        }

        if (icount == 1)
        {
            return this[indices[0]];
        }
        throw new QsException("Matrices indices only up to two");
    }

    public override void SetIndexedItem(QsParameter[] indices, QsValue value)
    {
        throw new NotImplementedException();
    }


    #endregion


    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public override QsValue DifferentiateOperation(QsValue vl)
    {
        QsValue value;
        if (vl is QsReference) value = ((QsReference)vl).ContentValue;
        else value = vl;


        if (value is QsScalar)
        {
            var n = new QsMatrix();
            foreach (var row in n.Rows)
            {
                n.AddRow(row.DifferentiateScalar((QsScalar)value).ToArray());
            }
            return n;
        }

        return base.DifferentiateOperation(value);
    }


    /// <summary>
    /// Removes row at index
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <returns></returns>
    public QsMatrix RemoveRow(int rowIndex)
    {
        var m = new QsMatrix();
        for (var iy = 0; iy < RowsCount; iy++)
        {
            if (iy != rowIndex) m.AddVector(this[iy]);
        }
        return m;
    }


    /// <summary>
    /// Removes column at index
    /// </summary>
    /// <param name="columnIndex"></param>
    /// <returns></returns>
    public QsMatrix RemoveColumn(int columnIndex)
    {
        var m = new QsMatrix();
        for (var ix = 0; ix < ColumnsCount; ix++)
        {
            if (ix != columnIndex) m.AddColumnVector(GetColumnVector(ix));
        }
        return m;
    }

    private QsMatrix _Cofactors;
    public QsMatrix Cofactors
    {
        get
        {
            if (ReferenceEquals( _Cofactors , null))
            {
                _Cofactors = new QsMatrix();

                var FirstNegative = false;
                for (var i = 0; i < RowsCount; i++)
                {
                    var v = new QsVector(ColumnsCount);

                    for (var j = 0; j < ColumnsCount; j++)
                    {
                        var minor = RemoveRow(i).RemoveColumn(j);
                        var d = Determinant(minor);
                        if (Math.Pow(-1, i + j) < 0) d = QsScalar.Zero - d;
                        v.AddComponent(d);
                    }

                    FirstNegative = !FirstNegative; //
                    _Cofactors.AddVector(v);
                }
            }
            return _Cofactors;
        }
    }

    /// <summary>
    /// Transpose of Cofactors matrix
    /// </summary>
    public QsMatrix Adjoint
    {
        get
        {
            return Cofactors.Transpose();
        }
    }

    public QsMatrix Inverse
    {
        get
        {
            return Adjoint.DivideScalar( Determinant(this));
        }
    }


    public override QsValue Execute(Token expression)
    {

        var operation = expression.TokenValue;

        if (operation.Equals("Transpose()", StringComparison.OrdinalIgnoreCase))
            return Transpose();

        if (operation.Equals("Identity", StringComparison.OrdinalIgnoreCase))
            return Identity;

        if (operation.Equals("Determinant", StringComparison.OrdinalIgnoreCase))
            return Determinant(this);

        if (operation.Equals("Cofactors", StringComparison.OrdinalIgnoreCase))
            return Cofactors;

        if (operation.Equals("Adjoint", StringComparison.OrdinalIgnoreCase))
            return Adjoint;

        if (operation.Equals("Inverse", StringComparison.OrdinalIgnoreCase))
            return Inverse;


        throw new QsException("Not implemented or Unknow method for the matrix type");
    }
}