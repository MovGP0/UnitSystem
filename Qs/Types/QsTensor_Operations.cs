namespace Qs.Types;

public partial class QsTensor : QsValue
{

    #region QsValue operations
    public override QsValue Identity
    {
        get { throw new NotImplementedException(); }
    }

    public override QsValue AddOperation(QsValue vl)
    {
        QsValue value;
        if (vl is QsReference) value = ((QsReference)vl).ContentValue;
        else value = vl;


        if (value is QsScalar)
        {
            var scalar = value as QsScalar;

            var NewTensor = new QsTensor();
            if (Order > 3)
            {
                foreach (var iTensor in InnerTensors)
                {
                    NewTensor.AddInnerTensor((QsTensor)iTensor.AddOperation(value));
                }
            }
            else
            {
                for (var il = 0; il < MatrixLayers.Count; il++)
                {
                    var ResultMatrix = (QsMatrix)MatrixLayers[il].AddOperation(scalar);
                    NewTensor.AddMatrix(ResultMatrix);
                }
            }
            return NewTensor;
        }

        if (value is QsTensor)
        {
            var tensor = value as QsTensor;
            if (Order != (tensor.Order)) throw new QsException("Adding two different ranked tensors are not supported");

            if (Order > 3)
            {
                var NewTensor = new QsTensor();
                for (var i=0; i< InnerTensors.Count(); i++)
                {
                    var iTensor = InnerTensors[i];
                    NewTensor.AddInnerTensor((QsTensor)
                        iTensor.AddOperation(tensor.InnerTensors[i]));
                }
                return NewTensor;
            }

            if (tensor.MatrixLayers.Count == MatrixLayers.Count)
            {
                // the operation will succeed
                if (tensor.FaceRowsCount == FaceRowsCount)
                {
                    if (tensor.FaceColumnsCount == FaceColumnsCount)
                    {
                        var NewTensor = new QsTensor();
                        for (var il = 0; il < MatrixLayers.Count; il++)
                        {
                            var ResultMatrix = (QsMatrix)MatrixLayers[il].AddOperation(tensor.MatrixLayers[il]);
                            NewTensor.AddMatrix(ResultMatrix);
                        }
                        return NewTensor;
                    }
                }
            }
        }

        throw new QsException("Summing Operation with " + value.GetType().Name + " Failed");
    }

    public override QsValue SubtractOperation(QsValue vl)
    {
        QsValue value;
        if (vl is QsReference) value = ((QsReference)vl).ContentValue;
        else value = vl;


        if (value is QsScalar)
        {
            var scalar = value as QsScalar;

            var NewTensor = new QsTensor();
            if (Order > 3)
            {
                foreach (var iTensor in InnerTensors)
                {
                    NewTensor.SubtractOperation((QsTensor)iTensor.AddOperation(value));
                }
            }
            else
            {
                for (var il = 0; il < MatrixLayers.Count; il++)
                {
                    var ResultMatrix = (QsMatrix)MatrixLayers[il].SubtractOperation(scalar);
                    NewTensor.AddMatrix(ResultMatrix);
                }
            }
            return NewTensor;
        }

        if (value is QsTensor)
        {
            var tensor = value as QsTensor;
            if (Order != (tensor.Order)) throw new QsException("Adding two different ranked tensors are not supported");

            if (Order > 3)
            {
                var NewTensor = new QsTensor();
                for (var i = 0; i < InnerTensors.Count(); i++)
                {
                    var iTensor = InnerTensors[i];
                    NewTensor.AddInnerTensor((QsTensor)
                        iTensor.SubtractOperation(tensor.InnerTensors[i]));
                }
                return NewTensor;
            }

            if (tensor.MatrixLayers.Count == MatrixLayers.Count)
            {
                // the operation will succeed
                if (tensor.FaceRowsCount == FaceRowsCount)
                {
                    if (tensor.FaceColumnsCount == FaceColumnsCount)
                    {
                        var NewTensor = new QsTensor();
                        for (var il = 0; il < MatrixLayers.Count; il++)
                        {
                            var ResultMatrix = (QsMatrix)MatrixLayers[il].SubtractOperation(tensor.MatrixLayers[il]);
                            NewTensor.AddMatrix(ResultMatrix);
                        }
                        return NewTensor;
                    }
                }
            }
        }

        throw new QsException("Tensor Subtract Operation with " + value.GetType().Name + " Failed");
    }

    public override QsValue MultiplyOperation(QsValue vl)
    {
        QsValue value;
        if (vl is QsReference) value = ((QsReference)vl).ContentValue;
        else value = vl;


        if (value is QsScalar)
        {
            var scalar = (QsScalar)value;

            var NewTensor = new QsTensor();
            if (Order > 3)
            {
                foreach (var iTensor in InnerTensors)
                {
                    NewTensor.AddInnerTensor((QsTensor)iTensor.MultiplyOperation(value));
                }
            }
            else
            {
                for (var il = 0; il < MatrixLayers.Count; il++)
                {
                    var ResultMatrix = (QsMatrix)MatrixLayers[il].MultiplyOperation(scalar);
                    NewTensor.AddMatrix(ResultMatrix);
                }
            }

            return NewTensor;
        }

        if (value is QsTensor)
        {
            var tensor = (QsTensor) value;
            if (Order == 1 && tensor.Order == 1)
            {
                var thisVec = this[0][0];
                var thatVec = tensor[0][0];
                var result = (QsMatrix)thisVec.TensorProductOperation(thatVec);
                return new QsTensor(result);
            }
            if (Order == 2 && tensor.Order <= 2)
            {
                //tenosrial product of two matrices will result in another matrix also.
                var result = (QsMatrix)MatrixLayers[0].TensorProductOperation(tensor.MatrixLayers[0]);

                return new QsTensor(result);
            }

            throw new QsException("", new NotImplementedException());
        }

        throw new QsException("Tensor Multiplication Operation with " + value.GetType().Name + " Failed");
    }

    public override QsValue DivideOperation(QsValue value)
    {
        throw new NotImplementedException();
    }

    public override QsValue PowerOperation(QsValue value)
    {
        throw new NotImplementedException();
    }

    public override QsValue ModuloOperation(QsValue value)
    {
        throw new NotImplementedException();
    }

    public override QsValue LeftShiftOperation(QsValue times)
    {
        throw new NotImplementedException();
    }

    public override QsValue RightShiftOperation(QsValue times)
    {
        throw new NotImplementedException();
    }


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
        if (value is QsTensor ts)
        {
            if (Order != ts.Order) return false;
            if (!ReferenceEquals(ts, this))
            {

                // we need to test equality to

                if (InnerTensors != null)
                {
                    for (var i = 0; i < InnerTensors.Count; i++)
                    {
                        if (!InnerTensors[i].Equality(ts.InnerTensors[i])) return false;
                    }
                }
                else
                {

                    for (var mn = 0; mn < MatrixLayers.Count; mn++)
                    {
                        if (!MatrixLayers[mn].Equality(ts.MatrixLayers[mn])) return false;
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

    public override QsValue DotProductOperation(QsValue vl)
    {
        QsValue value;
        if (vl is QsReference) value = ((QsReference)vl).ContentValue;
        else value = vl;



        QsTensor tensor;

        if (value is QsTensor) tensor = (QsTensor)value;
        else if (value is QsMatrix) tensor = new QsTensor((QsMatrix)value);
        else if (value is QsVector) tensor = new QsTensor(new QsMatrix((QsVector)value));
        else if (value is QsScalar) tensor = new QsTensor(new QsMatrix(new QsVector((QsScalar)value)));
        else throw new QsException("Tensor Operation with non mathematical type is not permitted");

        if (tensor == null) throw new QsException("Must be a tensor for scalar product");

        if (Order > 3)
        {
            throw new NotImplementedException();
        }

        if (Order == 1)
        {
            if (tensor.Order == 1)
            {
                var thisVec = this[0][0];
                var thatVec = tensor[0][0];
                var result = (QsScalar)thisVec.DotProductOperation(thatVec);
                return new QsTensor(new QsMatrix(new QsVector(result)));
            }
        }
        else if (Order == 2)
        {
            // only for tensors that looks like matrix.

            // reference:  http://people.rit.edu/pnveme/EMEM851n/constitutive/tensors_rect.html
            // the scalar product called there as double dot  ':'  and I don't know if this is right or wrong
            // 
            /*

            if (tensor.Order == 2)
            {
                var matrix = tensor.MatrixLayers[0];
                var thisMatrix = this.MatrixLayers[0];
                if (thisMatrix.RowsCount == matrix.RowsCount && thisMatrix.ColumnsCount == matrix.ColumnsCount)
                {
                    QsScalar Total = default(QsScalar);

                    for (int i = 0; i < thisMatrix.RowsCount; i++)
                    {
                        for (int j = 0; j < thisMatrix.ColumnsCount; j++)
                        {
                            if (Total == null) Total = thisMatrix[i, j] * matrix[j, i];
                            else Total = Total + thisMatrix[i, j] * matrix[j, i];
                        }
                    }
                    return Total;
                }
                else
                {
                    throw new QsException("The two matrices should be equal in rows and columns");
                }
            }
            */
        }

        throw new NotImplementedException();

    }

    public override QsValue CrossProductOperation(QsValue value)
    {
        throw new NotImplementedException();
    }

    public override QsValue TensorProductOperation(QsValue value)
    {
        throw new NotImplementedException();
    }

    public override QsValue NormOperation()
    {
        throw new NotImplementedException();
    }

    public override QsValue AbsOperation()
    {
        throw new NotImplementedException();
    }

    public override QsValue GetIndexedItem(QsParameter[] allIndices)
    {
        var indices = new int[allIndices.Length];

        for (var ix = 0; ix < indices.Length; ix++) indices[ix] = (int)((QsScalar)allIndices[ix].QsNativeValue).NumericalQuantity.Value;

        var dr = Order - indices.Count();
        if (dr < 0)
        {
            throw new QsException("Indices count exceed the tensor rank, only specify indices to the same rank to get scalar, or less to get vectors to tensors");
        }

        if (dr == 0)
        {
            return GetScalar(indices);
        }
        if (dr == 1)
        {
            // return vector
            if (Order == 2)
            {
                return this[0][indices[0]];
            }

            if (Order == 3)
            {
                return this[indices[0]][indices[1]];
            }
            var t = this;
            var ix = 0;
            var ic = indices.Count();
            while (ix < ic - 2)
            {
                var idx = indices[ix];
                if (idx < 0) idx = t.InnerTensors.Count + idx;
                t = t.InnerTensors[idx];
                ix++;
            }
            return t[indices[ix]][indices[ix + 1]];  //ix was increased the latest time.
        }
        if (dr == 2)
        {
            // return matrix;
            if (Order == 2)
            {
                return this[indices[0]];
            }

            // specify the tensor
            var t = this;
            var ix = 0;
            var ic = indices.Count();
            while (ix < ic - 1)
            {
                var idx = indices[ix];

                if (idx < 0) idx = t.InnerTensors.Count + idx;

                t = t.InnerTensors[idx];
                ix++;
            }



            // then return the matrix.
            return t[indices[ix]];  //ix was increased the latest time.
        }
        else
        {
            // return tensor
            var t = this;
            var ix = 0;
            while (ix < indices.Count())
            {
                var idx = indices[ix];
                if (idx < 0) idx = t.InnerTensors.Count + idx;
                t = t.InnerTensors[idx];
                ix++;
            }
            return t;
        }
    }

    public override void SetIndexedItem(QsParameter[] indices, QsValue value)
    {
        throw new NotImplementedException();
    }

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
            var t = new QsTensor();
            foreach (var m in t.MatrixLayers)
            {
                t.AddMatrix((QsMatrix)m.DifferentiateOperation(value));
            }
            return t;
        }

        return base.DifferentiateOperation(value);
    }
    #endregion

}