using System.Text;

namespace Qs.Types
{
    public partial class QsVector: QsValue, IEnumerable<QsScalar>
    {

        /// <summary>
        /// for storing the quantities.
        /// </summary>
        private List<QsScalar> ListStorage;


        #region constructors

        public QsVector()
        {
            ListStorage = new List<QsScalar>();
        }

        public QsVector(int count)            
        {
            ListStorage = new List<QsScalar>(count);
            
        }

        public QsVector(params QsScalar[] scalars)
        {
            ListStorage = new List<QsScalar>(scalars.Length);
            ListStorage.AddRange(scalars);
        }

        /// <summary>
        /// Concatenate the double value to the current vector.
        /// </summary>
        /// <param name="value"></param>
        public void AddComponent(double value)
        {
            ListStorage.Add(value.ToQuantity().ToScalar());
        }

        /// <summary>
        /// Concatenate the scalar to the current vector.
        /// </summary>
        /// <param name="scalar"></param>
        public void AddComponent(QsScalar scalar)
        {
            ListStorage.Add(scalar);
        }

        /// <summary>
        /// Concatenate the scalars to the current vector
        /// </summary>
        /// <param name="scalars"></param>
        public void AddComponents(params QsScalar[] scalars)
        {
            ListStorage.AddRange(scalars);
        }

        /// <summary>
        /// Concatenate the elements of vectors to the current vector.
        /// </summary>
        /// <param name="vector"></param>
        public void AddComponents(QsVector vector)
        {
            foreach (var s in vector) ListStorage.Add(s);
        }

        #endregion

        /// <summary>
        /// Magnitude of the vector.
        /// </summary>
        /// <returns></returns>
        public QsScalar Magnitude()
        {
            

            var v_dot_v = DotProductOperation(this) as QsScalar;

            var sqrt_v_dot_v = v_dot_v.PowerScalar("0.5".ToScalar());


            return sqrt_v_dot_v;
        }

        /// <summary>
        /// Returns the sum of the vector components
        /// </summary>
        /// <returns></returns>
        public QsScalar Sum()
        {
            var total = this[0];
            for (var i = 1; i < Count; i++) total = total + this[i];
            return total;

        }

        #region Vector behaviour
        public int Count => ListStorage.Count;
        

        public QsScalar this[int i]
        {
            get
            {
                if (i < 0) i = ListStorage.Count + i;
                return ListStorage[i];
            }
            set
            {
                if (i < 0) i = ListStorage.Count + i;
                ListStorage[i] = value;
            }
        }
        #endregion


        /// <summary>
        /// Test if the vector is one element only.
        /// </summary>
        public bool IsScalar
        {
            get
            {
                if (Count == 1) return true;
                return false;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("QsVector: ");
            for (var ix = 0; ix < Count; ix++)
            {
                var cell = this[ix].ToShortString();
                sb.Append(cell);
                sb.Append(" ");
            }

            return sb.ToString();
        }


        /// <summary>
        /// Copy the vector to another instance with the same components.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static QsVector CopyVector(QsVector vector)
        {
            var vec = new QsVector(vector.Count);

            foreach (var q in vector)
            {
                vec.AddComponent((QsScalar)q.Clone());
            }
            return vec;
        }

        public QsScalar[] ToArray()
        {
            return ListStorage.ToArray();
        }



        #region IEnumerable<QsScalar> Members

        public IEnumerator<QsScalar> GetEnumerator()
        {
            return ListStorage.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ListStorage.GetEnumerator();
        }

        #endregion




        #region ICloneable Members

        public object Clone()
        {
            return CopyVector(this);

        }

        #endregion



        public override string ToShortString()
        {
            return "Vector";
        }

        public QsMatrix ToVectorMatrix()
        {
            return new QsMatrix(this);
        }

        public QsMatrix ToCoVectorMatrix()
        {
            return new QsMatrix(this).Transpose();
        }

    }
}
