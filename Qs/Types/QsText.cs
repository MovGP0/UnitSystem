using System.Text;

namespace Qs.Types
{
    public partial class QsText : QsValue
    {
        public string Text { get; set; }

        public QsText(string text)
        {
            Text = text.Replace("\\\"", "\"");
        }


        public override string ToString()
        {

            return  Text ;
        }

        public override string ToShortString()
        {
            return "\"" + Text + "\"";
        }


        #region QsValue Operations
        public override QsValue Identity
        {
            get { throw new NotImplementedException(); }
        }

        public override QsValue AddOperation(QsValue value)
        {
            
            var t = new QsText(Text + value);
            return t;
        }

        public override QsValue SubtractOperation(QsValue value)
        {
            throw new NotImplementedException();
        }

        public override QsValue MultiplyOperation(QsValue value)
        {
            throw new NotImplementedException();
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
            var itimes = Qs.IntegerFromQsValue((QsScalar)times);

            if (itimes > Text.Length) itimes = itimes % Text.Length;


            var vec = new StringBuilder(Text.Length);

            for (var i = itimes; i < Text.Length; i++)
            {
                vec.Append(Text[i]);
            }

            for (var i = 0; i < itimes; i++)
            {
                vec.Append(Text[i]);
            }


            return new QsText(vec.ToString());

        }

        public override QsValue RightShiftOperation(QsValue times)
        {
            var itimes = Qs.IntegerFromQsValue((QsScalar)times);

            if (itimes > Text.Length) itimes = itimes % Text.Length;

            // 1 2 3 4 5 >> 2  == 4 5 1 2 3
             
            var vec = new StringBuilder(Text.Length);

            for (var i = Text.Length - itimes; i < Text.Length; i++)
            {
                vec.Append(Text[i]);
            }

            for (var i = 0; i < (Text.Length - itimes); i++)
            {
                vec.Append(Text[i]);
            }


            return new QsText(vec.ToString());

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
            var text = value as QsText;
            if (text==null)
                return false;
            if (text.Text.Equals(Text, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;

        }

        public override bool Inequality(QsValue value)
        {
            return !Equality(value);
        }

        public override QsValue DotProductOperation(QsValue value)
        {
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

        public override QsValue GetIndexedItem(QsParameter[] indices)
        {
            var i = (int)((QsScalar)indices[0].QsNativeValue).NumericalQuantity.Value;

            if (i < 0) i = Text.Length + i;
#if WINRT
            return Char.GetNumericValue(Text, i).ToQuantity().ToScalar();
#else
            return Char.ConvertToUtf32(Text, i).ToQuantity().ToScalar();
#endif
        }

        public override void SetIndexedItem(QsParameter[] indices, QsValue value)
        {
            throw new NotImplementedException();
        }
        #endregion


        public override QsValue ColonOperator(QsValue value)
        {
            string[] lines = Text.Split('\n');
            var l = (int)((QsScalar)value).NumericalQuantity.Value;

            return new QsText(lines[l]);
        }
    }
}
