namespace Qs.Types
{
    public partial class QsVector : QsValue, IEnumerable<QsScalar>
    {
        
        #region Scalar Operations

        /// <summary>
        /// Add Scalar to the vector components.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private QsVector AddScalar(QsScalar s)
        {
            var v = new QsVector(Count);

            for (var i = 0; i < Count; i++)
            {
                v.AddComponent(this[i] + s);
            }

            return v;
        }

        /// <summary>
        /// Subtract scalar from the vector components.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private QsVector SubtractScalar(QsScalar s)
        {
            var v = new QsVector(Count);

            for (var i = 0; i < Count; i++)
            {
                v.AddComponent(this[i] - s);
            }

            return v;

        }

        private QsVector MultiplyScalar(QsScalar s)
        {
            var v = new QsVector(Count);

            for (var i = 0; i < Count; i++)
            {
                v.AddComponent(this[i] * s);
            }

            return v;

        }

        public QsVector DivideScalar(QsScalar scalar)
        {
            var v = new QsVector(Count);

            for (var i = 0; i < Count; i++)
            {
                v.AddComponent(this[i] / scalar);
            }

            return v;

        }

        private QsValue ModuloScalar(QsScalar scalar)
        {
            var v = new QsVector(Count);

            for (var i = 0; i < Count; i++)
            {
                v.AddComponent(this[i] % scalar);
            }

            return v;
        }


        public QsVector PowerScalar(QsScalar scalar)
        {
            var v = new QsVector(Count);

            for (var i = 0; i < Count; i++)
            {
                v.AddComponent(this[i].PowerScalar(scalar));
            }
            return v;
        }


        /// <summary>
        /// Differentiate every component in vectpr with the scalar.
        /// </summary>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public QsVector DifferentiateScalar(QsScalar scalar)
        {
            var v = new QsVector(Count);

            for (var i = 0; i < Count; i++)
            {
                v.AddComponent((QsScalar)this[i].DifferentiateScalar(scalar));
            }

            return v;
        }


        #endregion
        #region Vector Operations

        /// <summary>
        /// Adds two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public QsVector AddVector(QsVector vector)
        {
            if (Count != vector.Count) throw new QsException("Vectors are not equal");

            var v = CopyVector(this);

            for (var ix = 0; ix < Count; ix++) v[ix] = v[ix] + vector[ix];

            return v;
        }

        /// <summary>
        /// Subtract two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public QsVector SubtractVector(QsVector vector)
        {
            if (Count != vector.Count) throw new QsException("Vectors are not equal");

            var v = CopyVector(this);

            for (var ix = 0; ix < Count; ix++) v[ix] = v[ix] - vector[ix];

            return v;
        }



        /// <summary>
        /// Multiply vector component by component.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public QsVector MultiplyVector(QsVector vector)
        {
            if (Count != vector.Count) throw new QsException("Vectors are not equal");

            var v = CopyVector(this);

            for (var ix = 0; ix < Count; ix++) v[ix] = v[ix].MultiplyScalar( vector[ix]);

            return v;
        }
        
        
        /// <summary>
        /// Divide vector component by component.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public QsVector DivideVector(QsVector vector)
        {
            if (Count != vector.Count) throw new QsException("Vectors are not equal");

            var v = CopyVector(this);

            for (var ix = 0; ix < Count; ix++) v[ix] = v[ix] / vector[ix];


            return v;
        }

        private QsValue ModuloVector(QsVector vector)
        {
            if (Count != vector.Count) throw new QsException("Vectors are not equal");


            var v = CopyVector(this);

            for (var ix = 0; ix < Count; ix++) v[ix] = v[ix] % vector[ix];

            return v;
        }



        /*
         * Scalar prodcut is commutative that is a{}.b{} = b{}.a{}
         * a{}.b{} = a1b1+a2b2+anbn
         */


        /// <summary>
        /// Dot product of two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public QsScalar ScalarProduct(QsVector vector)
        {
            if (Count != vector.Count) throw new QsException("Vectors are not equal");

            var Total = this[0] * vector[0];

            for (var i = 1; i < Count; i++)
            {
                Total = Total + (this[i] * vector[i]);
            }

            return Total ;
        }


        /// <summary>
        /// Cross product for 3 components vector.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public QsVector VectorProduct(QsVector v2)
        {
            if (Count != v2.Count) throw new QsException("Vectors are not equal");
            if (Count != 3) throw new QsException("Cross product only happens with 3 component vector");

            // cross product as determinant of matrix.

            var mat = new QsMatrix(
                new QsVector(QsScalar.One, QsScalar.One, QsScalar.One)
                , this
                , v2);

            return mat.Determinant();

            // problem now: what if we have more than 3 elements in the vector.
            // there is no cross product for more than 3 elements for vectors.

        }




        #endregion



        #region QsValue operations

        public override QsValue Identity
        {
            get
            {
                var v = new QsVector(Count);
                for (var i = 0; i < Count; i++)
                {
                    v.AddComponent(QsScalar.One);
                }
                return v;
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
                return AddVector((QsVector)value);
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
                return SubtractVector((QsVector)value);
            }
            throw new NotSupportedException();
        }


        /// <summary>
        /// Normal multiplication.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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
                return MultiplyVector((QsVector)value);
            }
            if (value is QsMatrix)
            {
                var mvec =  ToVectorMatrix().MultiplyMatrix((QsMatrix)value);
                // make it vector again.
                return mvec[0];
            }
            throw new NotSupportedException();
        }


        /// <summary>
        /// Dot product
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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
                return ScalarProduct((QsVector)value);
            }
            throw new NotSupportedException();
        }

        public override QsValue CrossProductOperation(QsValue vl)
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
                return VectorProduct((QsVector)value);
            }
            throw new NotSupportedException();
        }

        public override QsValue DivideOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                var s = value as QsScalar;
                return DivideScalar(s);
            }

            if (value is QsVector)
            {
                return DivideVector((QsVector)value);
            }
            throw new NotSupportedException();
        }

        public override QsValue DifferentiateOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {   
                return DifferentiateScalar((QsScalar)value);
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
                return PowerScalar((QsScalar)value);
            }

            throw new NotSupportedException();
        }

        /// <summary>
        /// ||vector||
        /// </summary>
        /// <returns></returns>
        public override QsValue NormOperation()
        {
            return Magnitude();
        }

        /// <summary>
        /// |vector|
        /// </summary>
        /// <returns></returns>
        public override QsValue AbsOperation()
        {
            return Magnitude();  
            
            //this is according to wikipedia that |x| if x is vector is valid but discouraged
            //  the norm of vector should be ||x|| notation.


        }

        public override QsValue ModuloOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                var s = value as QsScalar;
                return ModuloScalar(s);
            }

            if (value is QsVector)
            {
                return ModuloVector((QsVector)value);
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// Form a matrix from two vectors
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override QsValue TensorProductOperation(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                throw new NotSupportedException();
            }

            if (value is QsVector)
            {
                
                var vec = (QsVector)value;

                if (vec.Count != Count) throw new QsException("Not equal vector components");

                List<QsVector> vcs = [];
                foreach (var c in this)
                {
                    var v = (QsVector)(c * vec);
                    vcs.Add(v);

                }

                return (new QsMatrix(vcs.ToArray()));

            }
            throw new NotSupportedException();
        }


        public override QsValue LeftShiftOperation(QsValue vl)
        {
            QsValue times;
            if (vl is QsReference) times = ((QsReference)vl).ContentValue;
            else times = vl;


            var itimes = Qs.IntegerFromQsValue((QsScalar)times);

            if (itimes > Count) itimes = itimes % Count;

            
            var vec = new QsVector(Count);
            
            for (var i = itimes; i < Count; i++)
            {
                vec.AddComponent(this[i]);
            }

            for (var i = 0; i < itimes; i++)
            {
                vec.AddComponent(this[i]);
            }


            return vec;
        }

        public override QsValue RightShiftOperation(QsValue vl)
        {
            QsValue times;
            if (vl is QsReference) times = ((QsReference)vl).ContentValue;
            else times = vl;


            var itimes = Qs.IntegerFromQsValue((QsScalar)times);

            if (itimes > Count) itimes = itimes % Count;

            // 1 2 3 4 5 >> 2  == 4 5 1 2 3

            var vec = new QsVector(Count);

            for (var i = Count - itimes; i < Count; i++)
            {
                vec.AddComponent(this[i]);
            }

            for (var i = 0; i < (Count - itimes); i++)
            {
                vec.AddComponent(this[i]);
            }


            return vec;
        }


        #endregion



        public override bool LessThan(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                var s = (QsScalar)value.AbsOperation();

                return Magnitude().NumericalQuantity < s.NumericalQuantity;
            }

            if (value is QsVector)
            {
                //the comparison will be based on the vector magnitudes.
                var v = (QsVector)value;
                return (Magnitude().NumericalQuantity < v.Magnitude().NumericalQuantity);
            }
            throw new NotSupportedException();
        }

        public override bool GreaterThan(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                var s = (QsScalar)value.AbsOperation();
                return Magnitude().NumericalQuantity > s.NumericalQuantity;

            }

            if (value is QsVector)
            {
                //the comparison will be based on the vector magnitudes.
                var v = (QsVector)value;
                return (Magnitude().NumericalQuantity > v.Magnitude().NumericalQuantity);

            }
            throw new NotSupportedException();
        }

        public override bool LessThanOrEqual(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                var s = (QsScalar)value.AbsOperation();
                return Magnitude().NumericalQuantity <= s.NumericalQuantity;

            }

            if (value is QsVector)
            {
                //the comparison will be based on the vector magnitudes.
                var v = (QsVector)value;
                return (Magnitude().NumericalQuantity <= v.Magnitude().NumericalQuantity);

            }
            throw new NotSupportedException();
        }

        public override bool GreaterThanOrEqual(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                var s = (QsScalar)value.AbsOperation();
                return Magnitude().NumericalQuantity >= s.NumericalQuantity;

            }

            if (value is QsVector)
            {
                //the comparison will be based on the vector magnitudes.
                var v = (QsVector)value;
                return (Magnitude().NumericalQuantity >= v.Magnitude().NumericalQuantity);

            }
            throw new NotSupportedException();
        }

        public override bool Equality(QsValue vl)
        {

            if ((object)vl == null) return false;

            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                var s = (QsScalar)value.AbsOperation();
                return Magnitude().NumericalQuantity == s.NumericalQuantity;

            }

            if (value is QsVector v)
            {
                if (Count != v.Count) return false;
                for(var i = 0; i < ListStorage.Count; i++) 
                {
                    if (!ListStorage[i].Equality(v.ListStorage[i])) return false;
                }

                return true;
            }
            throw new NotSupportedException();
        }

        public override bool Inequality(QsValue vl)
        {
            QsValue value;
            if (vl is QsReference) value = ((QsReference)vl).ContentValue;
            else value = vl;


            if (value is QsScalar)
            {
                var s = (QsScalar)value.AbsOperation();
                return Magnitude().NumericalQuantity != s.NumericalQuantity;

            }

            if (value is QsVector)
            {
                //the comparison will be based on the vector magnitudes.
                var v = (QsVector)value;
                return (Magnitude().NumericalQuantity != v.Magnitude().NumericalQuantity);

            }
            throw new NotSupportedException();
        }

        public override QsValue GetIndexedItem(QsParameter[] allIndices)
        {
            if (allIndices.Count() > 1) throw new QsException("Vector have one index only");
            var indices = new int[allIndices.Length];
            for (var ix = 0; ix < indices.Length; ix++) indices[ix] = (int)((QsScalar)allIndices[ix].QsNativeValue).NumericalQuantity.Value;                
            var index = indices[0];
            
            return this[index];
        }

        public override void SetIndexedItem(QsParameter[] indices, QsValue value)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Some operations on the vector.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public override QsValue Execute(ParticleLexer.Token expression)
        {
            if (expression.TokenValue.Equals("length", StringComparison.OrdinalIgnoreCase))
                return Count.ToScalarValue();

            return base.Execute(expression);

        }
    }
}
