﻿using System.Text;
using System.Globalization;

namespace Qs.Types;

/// <summary>
/// Matrix that hold quantities
/// and the basic matrix calculations.
/// </summary>
public partial class QsMatrix : QsValue, IEnumerable<QsVector>
{
    public List<QsVector> Rows = [];

    public QsMatrix()
    {
    }

    /// <summary>
    /// Initiate matrix with a set of vectors
    /// </summary>
    /// <param name="vectors"></param>
    public QsMatrix(params QsVector[] vectors)
    {
        List<QsVector> copiedVectors = [];
        foreach (var v in vectors) copiedVectors.Add(QsVector.CopyVector(v));
        Rows.AddRange(copiedVectors);
    }

    /// <summary>
    /// Increase the matrix with a vector values.
    /// </summary>
    /// <param name="vector"></param>
    public void AddVector(QsVector vector)
    {
        Rows.Add(QsVector.CopyVector(vector));
    }

    /// <summary>
    /// Increase the matrix with a column vector values.
    /// </summary>
    /// <param name="vector"></param>
    public void AddColumnVector(QsVector vector)
    {
        AddColumn(vector.ToArray());
    }

    /// <summary>
    /// Increase the matrix with a set of vectors.
    /// </summary>
    /// <param name="vectors"></param>
    public void AddVectors(params QsVector[] vectors)
    {
        Rows.AddRange(vectors);
    }

    /// <summary>
    /// Add a row of quantities to the matrix.
    /// </summary>
    /// <param name="row"></param>
    public void AddRow(params QsScalar[] row)
    {
        if (Rows.Count > 0) //test if the matrix initialized.
        {
            if (row.Length != ColumnsCount)
            {
                throw new QsMatrixException("Added row columns [" + row.Length.ToString(CultureInfo.InvariantCulture) + "] is different than the matrix column count [" + ColumnsCount.ToString(CultureInfo.InvariantCulture) + "]");
            }
        }

        var vec = new QsVector();

        vec.AddComponents(row);

        Rows.Add(vec);
    }

    /// <summary>
    /// Add a column of quantities to the matrix.
    /// </summary>
    /// <param name="column"></param>
    public void AddColumn(params QsScalar[] column)
    {

        if (Rows.Count > 0) //test if the matrix initialized.
        {
            if (column.Length != RowsCount)
            {
                throw new QsMatrixException("Added column rows [" + column.Length.ToString(CultureInfo.InvariantCulture) + "] is different than the matrix rows count [" + RowsCount.ToString(CultureInfo.InvariantCulture) + "]");
            }

            // vertically we will increase all columns of the inner array of every row.
            // this will be slow I think :)

            var newIndex = ColumnsCount+1;
            for (var IY = 0; IY < RowsCount; IY++)
            {
                Rows[IY].AddComponent(column[IY]);
            }
        }
        else
        {

            foreach (var q in column)
            {
                AddRow(q);
            }
        }
    }

    /// <summary>
    /// Count of the matrix rows {m}
    /// </summary>
    public int RowsCount => Rows.Count;


    /// <summary>
    /// Count of the matrix columns {n}
    /// </summary>
    public int ColumnsCount => Rows[0].Count;



    /// <summary>
    /// Returns columns as vector array.
    /// </summary>
    public QsVector[] Columns
    {
        get
        {
            QsVector[] vs = new QsVector[ColumnsCount];

            for (var i = 0; i < ColumnsCount; i++)
            {
                vs[i] = GetColumnVector(i);
            }
            return vs;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="row">i index</param>
    /// <returns></returns>
    public QsVector this[int row]
    {
        get
        {
            if (row < 0) row = Rows.Count + row;
            return Rows[row];
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="row">i index</param>
    /// <param name="column">j index</param>
    /// <returns></returns>
    public QsScalar this[int row, int column]
    {
        get
        {
            if (row < 0) row = Rows.Count + row;
            var required_row = Rows[row];
            if (column < 0) column = required_row.Count + column;

            return required_row[column];
        }
        set
        {
            if (row < 0) row = Rows.Count + row;
            var required_row = Rows[row];
            if (column < 0) column = required_row.Count + column;


            required_row[column] = value;
        }
    }


    #region Matrix Operations

    /// <summary>
    /// Matrix + Matrix
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public QsMatrix AddMatrix(QsMatrix matrix)
    {
        if (DimensionEquals(matrix))
        {
            var Total = new QsMatrix();
            for (var IY = 0; IY < RowsCount; IY++)
            {
                List<QsScalar> row = new(ColumnsCount);

                for (var IX = 0; IX < ColumnsCount; IX++)
                {
                    row.Add(this[IY, IX] + matrix[IY, IX]);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;
        }

        throw new QsMatrixException("Matrix 1 [" + RowsCount.ToString(CultureInfo.InvariantCulture)
                                                 + "x" + ColumnsCount.ToString(CultureInfo.InvariantCulture) +
                                                 "] is not dimensional equal with Materix 2 [" + matrix.RowsCount.ToString(CultureInfo.InvariantCulture)
                                                 + "x" + matrix.ColumnsCount.ToString(CultureInfo.InvariantCulture) + "]");
    }


    /// <summary>
    /// Element wise multiplcation of the matrix.
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public QsMatrix MultiplyMatrixByElements(QsMatrix matrix)
    {
        if (DimensionEquals(matrix))
        {
            var Total = new QsMatrix();
            for (var IY = 0; IY < RowsCount; IY++)
            {
                List<QsScalar> row = new(ColumnsCount);

                for (var IX = 0; IX < ColumnsCount; IX++)
                {
                    row.Add(this[IY, IX] * matrix[IY, IX]);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;
        }

        throw new QsMatrixException("Matrix 1 [" + RowsCount.ToString(CultureInfo.InvariantCulture)
                                                 + "x" + ColumnsCount.ToString(CultureInfo.InvariantCulture) +
                                                 "] is not dimensional equal with Materix 2 [" + matrix.RowsCount.ToString(CultureInfo.InvariantCulture)
                                                 + "x" + matrix.ColumnsCount.ToString(CultureInfo.InvariantCulture) + "]");
    }

    public QsMatrix DivideMatrixByElements(QsMatrix matrix)
    {
        if (DimensionEquals(matrix))
        {
            var Total = new QsMatrix();
            for (var IY = 0; IY < RowsCount; IY++)
            {
                List<QsScalar> row = new(ColumnsCount);

                for (var IX = 0; IX < ColumnsCount; IX++)
                {
                    row.Add(this[IY, IX] / matrix[IY, IX]);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;
        }

        throw new QsMatrixException("Matrix 1 [" + RowsCount.ToString(CultureInfo.InvariantCulture)
                                                 + "x" + ColumnsCount.ToString(CultureInfo.InvariantCulture) +
                                                 "] is not dimensional equal with Materix 2 [" + matrix.RowsCount.ToString(CultureInfo.InvariantCulture)
                                                 + "x" + matrix.ColumnsCount.ToString(CultureInfo.InvariantCulture) + "]");
    }


    public QsValue ModuloMatrixByElements(QsMatrix matrix)
    {
        if (DimensionEquals(matrix))
        {
            var Total = new QsMatrix();
            for (var IY = 0; IY < RowsCount; IY++)
            {
                List<QsScalar> row = new(ColumnsCount);

                for (var IX = 0; IX < ColumnsCount; IX++)
                {
                    row.Add(this[IY, IX] % matrix[IY, IX]);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;
        }

        throw new QsMatrixException("Matrix 1 [" + RowsCount.ToString(CultureInfo.InvariantCulture)
                                                 + "x" + ColumnsCount.ToString(CultureInfo.InvariantCulture) +
                                                 "] is not dimensional equal with Materix 2 [" + matrix.RowsCount.ToString(CultureInfo.InvariantCulture)
                                                 + "x" + matrix.ColumnsCount.ToString(CultureInfo.InvariantCulture) + "]");
    }




    /// <summary>
    /// Matrix - Matrix
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    private QsMatrix SubtractMatrix(QsMatrix matrix)
    {
        if (DimensionEquals(matrix))
        {
            var Total = new QsMatrix();
            for (var IY = 0; IY < RowsCount; IY++)
            {
                List<QsScalar> row = new(ColumnsCount);

                for (var IX = 0; IX < ColumnsCount; IX++)
                {
                    row.Add(this[IY, IX] - matrix[IY, IX]);
                }

                Total.AddRow(row.ToArray());
            }
            return Total;
        }

        throw new QsMatrixException("Matrix 1 [" + RowsCount.ToString(CultureInfo.InvariantCulture)
                                                 + "x" + ColumnsCount.ToString(CultureInfo.InvariantCulture) +
                                                 "] is not dimensional equal with Matrix 2 [" + matrix.RowsCount.ToString(CultureInfo.InvariantCulture)
                                                 + "x" + matrix.ColumnsCount.ToString(CultureInfo.InvariantCulture) + "]");
    }


    /// <summary>
    /// Ordinary multiplicatinon of the matrix.
    /// Naive implementation :D  and I am proud :P
    /// 
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public QsMatrix MultiplyMatrix(QsMatrix matrix)
    {
        if (ColumnsCount == matrix.RowsCount)
        {
            var Total = new QsMatrix();

            //loop through my rows
            for (var iy = 0; iy < RowsCount; iy++)
            {
                var vec = Rows[iy];
                QsScalar[] tvec = new QsScalar[matrix.ColumnsCount]; //the target row in the Total Matrix.

                //loop through all co vectors in the target matrix.
                for (var tix = 0; tix < matrix.ColumnsCount; tix++)
                {
                    var covec = matrix.GetColumnVectorMatrix(tix).ToArray();

                    //multiply vec*covec and store it at the Total matrix at iy,ix

                    QsScalar[] snum = new QsScalar[vec.Count];
                    for (var i = 0; i < vec.Count; i++)
                    {
                        snum[i] = vec[i].MultiplyScalar(covec[i]);
                    }

                    var tnum = snum[0];

                    for (var i = 1; i < snum.Length; i++)
                    {
                        tnum = tnum + snum[i];
                    }

                    tvec[tix] = tnum;

                }

                Total.AddRow(tvec);

            }

            return Total;
        }

        throw new QsMatrixException("Width of the first matrix [" + ColumnsCount + "] not equal to the height of the second matrix [" + matrix.RowsCount + "]");
    }

    /// <summary>
    /// Transfer the columns of the matrix into rows.
    /// </summary>
    /// <returns></returns>
    public QsMatrix Transpose()
    {
        var m = new QsMatrix();
        for (var IX = 0; IX < ColumnsCount; IX++)
        {
            var vec = GetColumnVectorMatrix(IX);

            m.AddRow(vec.ToArray());
        }
        return m;
    }


    /// <summary>
    /// Append the target matrix right to the matrix.
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public QsMatrix AppendRightMatrix(QsMatrix matrix)
    {
        var m = CopyMatrix(this);
        foreach (var column in matrix.Columns)
            m.AddColumnVector(column);
        return m;
    }

    public QsMatrix AppendLowerMatrix(QsMatrix matrix)
    {
        var m = CopyMatrix(this);
        foreach (var vector in matrix)
            m.AddVector(vector);
        return m;
    }

    #endregion



    #region Manipulation

    /// <summary>
    /// Gets a specific row vector
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <returns></returns>
    public QsVector GetVector(int rowIndex)
    {
        if (rowIndex > RowsCount) throw new QsMatrixException("Index '" + rowIndex + "' Exceeds the rows limits '" + RowsCount + "'");

        return QsVector.CopyVector(Rows[rowIndex]);
    }


    public QsMatrix GetVectorMatrix(int rowIndex)
    {
        if (rowIndex > RowsCount) throw new QsMatrixException("Index '" + rowIndex + "' Exceeds the rows limits '" + RowsCount + "'");

        var mat = new QsMatrix();

        mat.AddVector(QsVector.CopyVector(Rows[rowIndex]));

        return mat;
    }

    /// <summary>
    ///  gets a specific covector as a vector object
    /// </summary>
    /// <param name="columnIndex"></param>
    /// <returns></returns>
    public QsVector GetColumnVector(int columnIndex)
    {
        if (columnIndex > ColumnsCount) throw new QsMatrixException("Index '" + columnIndex + "' Exceeds the columns limits '" + ColumnsCount + "'");


        QsScalar[] col = new QsScalar[RowsCount];
        var nc = 0;
        for (var IY = 0; IY < RowsCount; IY++)
        {
            col[nc] = this[IY, columnIndex];
            nc++;
        }
        var vec = new QsVector(col);


        return vec;

    }

    /// <summary>
    /// Gets a specific covector as a matrix object.
    /// </summary>
    /// <param name="columnIndex"></param>
    /// <returns></returns>
    public QsMatrix GetColumnVectorMatrix(int columnIndex)
    {
        var mat = new QsMatrix();
        mat.AddColumnVector(GetColumnVector(columnIndex));
        return mat;
    }

    /// <summary>
    /// returns all quantities in the matrix in <see cref="AnyQuantity<double>"/> array.
    /// starting from left to right then by going down.
    /// </summary>
    /// <returns></returns>
    public QsScalar[] ToArray()
    {
        QsScalar[] values = new QsScalar[ColumnsCount * RowsCount];

        var valIndex=0;
        for (var IX = 0; IX < ColumnsCount; IX++)
        {
            for (var IY = 0; IY < RowsCount; IY++)
            {
                values[valIndex] = this[IY, IX];
                valIndex++;
            }
        }

        return values;

    }

    #endregion

    internal string MatrixText
    {
        get
        {

            var maxCharachtersCount = 0;
            string[,] cells = new string[RowsCount, ColumnsCount];

            for (var IY = 0; IY < RowsCount; IY++)
            {
                for (var IX = 0; IX < ColumnsCount; IX++)
                {
                    var cell = this[IY, IX].ToShortString();

                    cells[IY, IX] = cell;

                    maxCharachtersCount = Math.Max(maxCharachtersCount, cell.Length);

                }
            }

            var sb = new StringBuilder();
            for (var IY = 0; IY < RowsCount; IY++)
            {
                for (var IX = 0; IX < ColumnsCount; IX++)
                {
                    var cell = cells[IY, IX];

                    sb.Append(cell.PadLeft(maxCharachtersCount + 2));
                }

                sb.AppendLine();
            }
            return sb.ToString();
        }
    }

    public override string ToString()
    {

        var sb = new StringBuilder();
        sb.Append("QsMatrix:");
        sb.AppendLine();

        sb.Append(MatrixText);

        return sb.ToString();
    }

    public override string ToShortString()
    {
        return "Matrix";
        /*
        StringBuilder sb = new StringBuilder();

        for (int IY = 0; IY < this.RowsCount; IY++)
        {
            for (int IX = 0; IX < this.ColumnsCount; IX++)
            {
                string cell = this[IY, IX].ToShortString();

                sb.Append(cell);
                if (IX < this.ColumnsCount - 1) sb.Append(", ");
            }

            if (IY < this.RowsCount - 1) sb.Append(" ; ");
        }

        sb.Insert(0, "[");
        sb.Append("]");

        return sb.ToString();
        */
    }

    #region Matrix Attributes
    public bool IsVector
    {
        get
        {
            if (RowsCount == 1) return true;
            return false;
        }
    }

    public bool IsCoVector
    {
        get
        {
            if (ColumnsCount == 1) return true;
            return false;
        }
    }

    public bool IsScalar
    {
        get
        {
            if (RowsCount == 1 && ColumnsCount == 1) return true;
            return false;
        }
    }

    /// <summary>
    /// Check if the matrix is n x n dimensions
    /// </summary>
    public bool IsSquared
    {
        get
        {
            if (RowsCount == ColumnsCount) return true;
            return false;
        }
    }



    /// <summary>
    /// Calculates the determinant and return it as a vector.
    /// </summary>
    /// <returns></returns>
    public QsVector Determinant()
    {
        if (IsSquared)
        {
            if(IsScalar)
            {
                return new QsVector(this[0,0]);
            }

            if (RowsCount == 2)
            {
                var vec = new QsVector(2);
                vec.AddComponent(this[0, 0] * this[1, 1]);
                vec.AddComponent(
                    QsScalar.NegativeOne * (this[0, 1] * this[1, 0])
                );

                return vec;
            }
            if (RowsCount == 3)
            {
                var vec = new QsVector(3);

                vec.AddComponent((this[1, 1] * this[2, 2]) - (this[1, 2] * this[2, 1]));

                vec.AddComponent((this[1, 2] * this[2, 0]) - (this[1, 0] * this[2, 2]));

                vec.AddComponent((this[1, 0] * this[2, 1]) - (this[1, 1] * this[2, 0]));

                vec[0] = this[0, 0] * vec[0];

                vec[1] = this[0, 1] * vec[1];

                vec[2] = this[0, 2] * vec[2];

                return vec;
            }
            throw new NotImplementedException();
        }

        throw new QsMatrixException("This matrix is not a square matrix.");
    }


    #endregion

    /// <summary>
    /// used to make sure that the two matrices are dimensionally equal
    /// have the same count of rows and columnss
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool DimensionEquals(QsMatrix matrix)
    {
        if (matrix== null) return false;
        if (RowsCount == matrix.RowsCount)
        {
            if (ColumnsCount == matrix.ColumnsCount)
            {

                return true;   //just for now.
            }
        }
        //dimensions not equal.
        return false;
    }




    /// <summary>
    /// Make identity matrix bases on dimension
    /// </summary>
    /// <param name="n">dimension or n * n matrix</param>
    /// <returns></returns>
    public static QsMatrix MakeIdentity(int n)
    {
        var m = new QsMatrix();

        for (var i = 0; i < n; i++)
        {
            var v = new QsVector(n);
            for (var j = 0; j < n; j++)
            {
                if (j == i)
                    v.AddComponent(QsScalar.One);
                else
                    v.AddComponent(QsScalar.Zero);
            }
            m.AddVector(v);
        }

        return m;
    }


    public static QsMatrix Random(int rows, int columns)
    {
        var m = new QsMatrix();

        var rr = new Random(Environment.TickCount);

        for (var i = 0; i < rows; i++)
        {
            var v = new QsVector(columns);
            for (var j = 0; j < columns; j++)
            {
                var sr = new QsScalar(ScalarTypes.NumericalQuantity) { NumericalQuantity = (rr.NextDouble()).ToQuantity() };
                v.AddComponent(sr);

            }
            m.AddVector(v);
        }

        return m;
    }

    public static QsMatrix Random(int n)
    {
        return Random(n, n);
    }



    #region IEnumerable<QsVector> Members

    public IEnumerator<QsVector> GetEnumerator()
    {
        return Rows.GetEnumerator();
    }

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return Rows.GetEnumerator();
    }

    #endregion



    /// <summary>
    /// Copy the matrix into new matrix instance.
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static QsMatrix CopyMatrix(QsMatrix matrix)
    {
        var m = new QsMatrix();
        foreach (var v in matrix)
        {
            m.AddVector(QsVector.CopyVector(v));
        }

        return m;
    }


    /// <summary>
    /// Makes n x n matrix with zeros.
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static QsMatrix MakeEmptySquareMatrix(int n)
    {
        var m = new QsMatrix();
        for (var i = 0; i < n; i++)
        {
            var v = new QsVector(n);
            for (var j = 0; j < n; j++)
            {
                v.AddComponent(QsScalar.Zero);
            }
            m.AddVector(v);
        }
        return m;
    }


    #region ICloneable Members

    public object Clone()
    {
        return CopyMatrix(this);
    }

    #endregion
}