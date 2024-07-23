using System.Globalization;
using Qs.Types;



namespace Qs.Runtime
{
    public partial class QsSequence
    {


        #region Higher Sequence Manipulation functions (Summation ++, Multiplication **, Range ..).

        /// <summary>
        /// Check if toIndex is greater than fromIndex and swap if not.
        /// </summary>
        /// <param name="fromIndex"></param>
        /// <param name="toIndex"></param>
        private void FixIndices(ref int fromIndex, ref int toIndex)
        {
            if (fromIndex > toIndex)
            {
                var f = fromIndex;
                fromIndex = toIndex;
                toIndex = f;
            }
       }

        /// <summary>
        /// Replace elements with operation and replace index variable with indexing number.
        /// </summary>
        /// <param name="fromIndex"></param>
        /// <param name="toIndex"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        private string JoinElementsWithOperation(int fromIndex, int toIndex, string operation)
        {
            var FunctionBody = string.Empty;
            var counterIndex = fromIndex;
            var still = true;
            while (still)
            {
                var e = GetElement(counterIndex);

                var se_text = e.ElementDeclaration.Replace("$" + SequenceIndexName, "`");
                se_text = se_text.Replace(SequenceIndexName, counterIndex.ToString(CultureInfo.InvariantCulture));
                se_text = se_text.Replace("`", "$" + SequenceIndexName);

                if (!string.IsNullOrEmpty(SequenceRangeStartName))
                {
                    se_text = se_text.Replace("$" + SequenceRangeStartName, "`");
                    se_text = se_text.Replace(SequenceRangeStartName, StartIndex.ToString(CultureInfo.InvariantCulture));
                    se_text = se_text.Replace("`", "$" + SequenceRangeStartName);
                }

                if (!string.IsNullOrEmpty(SequenceRangeEndName))
                {
                    se_text = se_text.Replace("$" + SequenceRangeEndName, "`");
                    se_text = se_text.Replace(SequenceRangeEndName, EndIndex.ToString(CultureInfo.InvariantCulture));
                    se_text = se_text.Replace("`", "$" + SequenceRangeEndName);
                }

                FunctionBody += "(" + se_text + ")";

                if (fromIndex > toIndex)
                {
                    counterIndex--;
                    if (counterIndex < toIndex) still = false;
                }
                else
                {
                    counterIndex++;
                    if (counterIndex > toIndex) still = false;
                }
                if (still)
                    FunctionBody += " " + operation + " ";
            }
            return FunctionBody;
        }


        /// <summary>
        /// returns elements in array
        /// </summary>
        /// <param name="fromIndex"></param>
        /// <param name="toIndex"></param>
        /// <returns></returns>
        public QsSequenceElement[] GetElementsValuesInArray(int fromIndex, int toIndex)
        {
            List<QsSequenceElement> Elements = [];

            var counterIndex = fromIndex;
            var still = true;
            while (still)
            {
                Elements.Add(GetElement(counterIndex));

                if (fromIndex > toIndex)
                {
                    counterIndex--;
                    if (counterIndex < toIndex) still = false;
                }
                else
                {
                    counterIndex++;
                    if (counterIndex > toIndex) still = false;
                }
                if (still)
                {
                }
            }

            return Elements.ToArray();
        }

        /// <summary>
        /// The method sum all elements in the sequence between the supplied indexes.
        /// Correspondes To: S[i++k]
        /// </summary>
        public QsValue SumElements(int fromIndex, int toIndex)
        {


            if (Parameters.Length > 0)
            {
                // this is a call to form symbolic element
                // like g[n](x) ..> x^n
                // and calling g[0++2]
                //  the output should be x^0 + x^1 + x^2
                //  and be parsed into function  (QsFunction)


                var porma = string.Empty;  // the parameters separated by comma ','
                foreach (var prm in Parameters)
                {
                    porma += prm.Name + ", ";
                }
                porma = porma.TrimEnd(',', ' ');

                var FunctionBody = JoinElementsWithOperation(fromIndex, toIndex, "+");

                var FunctionDeclaration = "_(" + porma + ") = " + FunctionBody;


                var qs = QsFunction.ParseFunction(QsEvaluator.CurrentEvaluator, FunctionDeclaration);

                return new QsScalar(ScalarTypes.FunctionQuantity) { FunctionQuantity = qs.ToQuantity() };
            }

            FixIndices(ref fromIndex, ref toIndex);

            var Total = GetElementValue(fromIndex);

            for (var i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i);
            }


            return Total;
        }

        #region SumElements Functions
        public QsValue SumElements(int fromIndex, int toIndex, QsParameter arg0)
        {


            FixIndices(ref fromIndex, ref toIndex);

            var Total = GetElementValue(fromIndex, arg0);
            for (var i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0);
            }

            return Total;
        }


        public QsValue SumElements(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1)
        {


            FixIndices(ref fromIndex, ref toIndex);

            var Total = GetElementValue(fromIndex, arg0, arg1);
            for (var i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0, arg1);
            }

            return Total;
        }
        public QsValue SumElements(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2)
        {


            FixIndices(ref fromIndex, ref toIndex);

            var Total = GetElementValue(fromIndex, arg0, arg1, arg2);
            for (var i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0, arg1, arg2);
            }

            return Total;
        }
        public QsValue SumElements(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3)
        {


            FixIndices(ref fromIndex, ref toIndex);

            var Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3);
            for (var i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0, arg1, arg2, arg3);
            }

            return Total;
        }
        public QsValue SumElements(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3, QsParameter arg4)
        {


            FixIndices(ref fromIndex, ref toIndex);

            var Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4);
            for (var i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0, arg1, arg2, arg3, arg4);
            }

            return Total;
        }
        public QsValue SumElements(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3, QsParameter arg4, QsParameter arg5)
        {


            FixIndices(ref fromIndex, ref toIndex);

            var Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5);
            for (var i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5);
            }

            return Total;
        }
        public QsValue SumElements(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3, QsParameter arg4, QsParameter arg5, QsParameter arg6)
        {


            FixIndices(ref fromIndex, ref toIndex);

            var Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            for (var i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total + GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            }

            return Total;
        }

        #endregion



        /// <summary>
        /// The method multiply all elements in the sequence between the supplied indexes.
        /// Correspondes To: S[i**k]
        /// </summary>
        public QsValue MulElements(int fromIndex, int toIndex)
        {


            if (Parameters.Length > 0)
            {
                // this is a call to form symbolic element
                // like g[n](x) ..> x^n
                // and calling g[0++2]
                //  the output should be x^0 + x^1 + x^2
                //  and be parsed into function  (QsFunction)


                var porma = string.Empty;  // the parameters separated by comma ','
                foreach (var prm in Parameters)
                {
                    porma += prm.Name + ", ";
                }
                porma = porma.TrimEnd(',', ' ');

                var FunctionBody = JoinElementsWithOperation(fromIndex, toIndex, "*");

                var FunctionDeclaration = "_(" + porma + ") = " + FunctionBody;

                var qs = QsFunction.ParseFunction(QsEvaluator.CurrentEvaluator, FunctionDeclaration);

                return new QsScalar(ScalarTypes.FunctionQuantity) { FunctionQuantity = qs.ToQuantity() };

            }

            FixIndices(ref fromIndex, ref toIndex);

            var Total = (QsValue)GetElementValue(fromIndex);

            for (var i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * (QsValue)GetElementValue(i);
            }


            return Total;
        }

        #region MulElements Functions
        public QsValue MulElements(int fromIndex, int toIndex, QsParameter arg0)
        {


            FixIndices(ref fromIndex, ref toIndex);

            var Total = GetElementValue(fromIndex, arg0);
            for (var i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0);
            }

            return Total;
        }

        public QsValue MulElements(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1)
        {


            FixIndices(ref fromIndex, ref toIndex);

            var Total = GetElementValue(fromIndex, arg0, arg1);
            for (var i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0, arg1);
            }

            return Total;
        }
        public QsValue MulElements(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2)
        {


            FixIndices(ref fromIndex, ref toIndex);

            var Total = GetElementValue(fromIndex, arg0, arg1, arg2);
            for (var i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0, arg1, arg2);
            }

            return Total;
        }
        public QsValue MulElements(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3)
        {


            FixIndices(ref fromIndex, ref toIndex);

            var Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3);
            for (var i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0, arg1, arg2, arg3);
            }

            return Total;
        }
        public QsValue MulElements(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3, QsParameter arg4)
        {


            FixIndices(ref fromIndex, ref toIndex);

            var Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4);
            for (var i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0, arg1, arg2, arg3, arg4);
            }

            return Total;
        }
        public QsValue MulElements(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3, QsParameter arg4, QsParameter arg5)
        {


            FixIndices(ref fromIndex, ref toIndex);

            var Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5);
            for (var i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5);
            }

            return Total;
        }
        public QsValue MulElements(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3, QsParameter arg4, QsParameter arg5, QsParameter arg6)
        {


            FixIndices(ref fromIndex, ref toIndex);

            var Total = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            for (var i = fromIndex + 1; i <= toIndex; i++)
            {
                Total = Total * GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            }

            return Total;
        }

        #endregion



        /// <summary>
        /// This is a tricky functions
        /// it returns Vector if components are Scalars.
        /// Matrix if components are Vectors
        /// </summary>
        /// <param name="fromIndex"></param>
        /// <param name="toIndex"></param>
        /// <returns></returns>
        public QsValue QsValueElements(int fromIndex, int toIndex)
        {
            if (Parameters.Length > 0)
            {
                List<string> ProcessedElements = [];

                #region symbolic representation

                if (fromIndex > toIndex)
                {
                    for (var e_ix = fromIndex; e_ix >= toIndex; e_ix--)
                    {
                        var se = GetElement(e_ix);

                        // s[n](x) ..> $x*x^n+$n     # symbolic variables shouldn't be changed ($x, $n) we should take care.

                        // first preserve the symbolic variable with the same index name that we are going to change.
                        var se_text = se.ElementDeclaration.Replace("$" + SequenceIndexName, "`");

                        // replace the index name with the
                        se_text = se_text.Replace(SequenceIndexName, e_ix.ToString(CultureInfo.InvariantCulture));

                        // get back the symbolic
                        se_text = se_text.Replace("`", "$" + SequenceIndexName);

                        if (!string.IsNullOrEmpty(SequenceRangeStartName))
                        {
                            se_text = se_text.Replace("$" + SequenceRangeStartName, "`");
                            se_text = se_text.Replace(SequenceRangeStartName, StartIndex.ToString(CultureInfo.InvariantCulture));
                            se_text = se_text.Replace("`", "$" + SequenceRangeStartName);
                        }

                        if (!string.IsNullOrEmpty(SequenceRangeEndName))
                        {
                            se_text = se_text.Replace("$" + SequenceRangeEndName, "`");
                            se_text = se_text.Replace(SequenceRangeEndName, EndIndex.ToString(CultureInfo.InvariantCulture));
                            se_text = se_text.Replace("`", "$" + SequenceRangeEndName);
                        }


                        // replace the parameters in declaration with the same
                        foreach (var param in Parameters)
                        {
                            se_text = se_text.Replace("$" + param.Name, "`");

                            se_text = se_text.Replace(param.Name, "$" + param.Name);

                            se_text = se_text.Replace("`" , "$" + param.Name);

                        }

                        ProcessedElements.Add(se_text);
                    }
                }
                else
                {
                    for (var e_ix = fromIndex; e_ix <= toIndex; e_ix++)
                    {
                        var se = GetElement(e_ix);

                        var se_text = se.ElementDeclaration.Replace("$" + SequenceIndexName, "`");
                        se_text = se_text.Replace(SequenceIndexName, e_ix.ToString(CultureInfo.InvariantCulture));
                        se_text = se_text.Replace("`", "$" + SequenceIndexName);
                        if (!string.IsNullOrEmpty(SequenceRangeStartName))
                        {
                            se_text = se_text.Replace("$" + SequenceRangeStartName, "`");
                            se_text = se_text.Replace(SequenceRangeStartName, StartIndex.ToString(CultureInfo.InvariantCulture));
                            se_text = se_text.Replace("`", "$" + SequenceRangeStartName);
                        }

                        if (!string.IsNullOrEmpty(SequenceRangeEndName))
                        {
                            se_text = se_text.Replace("$" + SequenceRangeEndName, "`");
                            se_text = se_text.Replace(SequenceRangeEndName, EndIndex.ToString(CultureInfo.InvariantCulture));
                            se_text = se_text.Replace("`", "$" + SequenceRangeEndName);
                        }


                        // replace the parameters with names
                        foreach (var param in Parameters)
                        {
                            se_text = se_text.Replace("$" + param.Name, "`");

                            se_text = se_text.Replace(param.Name, "$" + param.Name);

                            se_text = se_text.Replace("`", "$" + param.Name);

                        }

                        ProcessedElements.Add(se_text);
                    }
                }
                var ee = QsEvaluator.CurrentEvaluator.SilentEvaluate(ProcessedElements[0]);
                QsValue Total;

                if (ee is QsScalar)
                {
                    Total = new QsVector(Math.Abs(toIndex - fromIndex) + 1);
                    foreach (var pel in ProcessedElements)
                        ((QsVector)Total).AddComponent((QsScalar)QsEvaluator.CurrentEvaluator.SilentEvaluate(pel));
                }
                else if (ee is QsVector)
                {
                    Total = new QsMatrix();
                    foreach (var pel in ProcessedElements)
                        ((QsMatrix)Total).AddVector((QsVector)QsEvaluator.CurrentEvaluator.SilentEvaluate(pel));
                }
                else if (ee is QsMatrix)
                {
                    Total = new QsTensor();
                    foreach (var pel in ProcessedElements)
                        ((QsTensor)Total).AddMatrix((QsMatrix)QsEvaluator.CurrentEvaluator.SilentEvaluate(pel));
                }
                else
                {

                    throw new QsException("This is enough, no more than matrix values please.");
                }



                #endregion


                return Total;
            }

            var firstElement = (QsValue)GetElementValue(fromIndex);
            if (firstElement is QsScalar)
            {
                //return vector
                var Total = new QsVector(Math.Abs(toIndex - fromIndex) + 1);


                #region Numerical Representation
                Total.AddComponent((QsScalar)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (var i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i));
                    }
                }
                else
                {
                    for (var i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i));
                    }
                }
                #endregion


                return Total;
            }

            if (firstElement is QsVector)
            {
                //return vector
                var Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (var i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddVector((QsVector)GetElementValue(i));
                    }
                }
                else
                {
                    for (var i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddVector((QsVector)GetElementValue(i));
                    }
                }


                return Total;
            }
            if (firstElement is QsMatrix)
            {

                throw new NotImplementedException();
            }
            throw new NotSupportedException();
        }

        #region QsValueElements Functions
        public QsValue QsValueElements(int fromIndex, int toIndex, QsParameter arg0)
        {


            var firstElement = GetElementValue(fromIndex, arg0);
            if (firstElement is QsScalar)
            {
                //return vector
                var Total = new QsVector(Math.Abs(toIndex - fromIndex) + 1);

                Total.AddComponent((QsScalar)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (var i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0));
                    }
                }
                else
                {
                    for (var i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0));
                    }
                }


                return Total;
            }

            if (firstElement is QsVector)
            {
                //return vector
                var Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (var i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0));
                    }
                }
                else
                {
                    for (var i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0));
                    }
                }


                return Total;
            }
            if (firstElement is QsMatrix)
            {

                throw new NotImplementedException();
            }
            throw new NotSupportedException();
        }

        public QsValue QsValueElements(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1)
        {
              var firstElement = GetElementValue(fromIndex, arg0, arg1);
            if (firstElement is QsScalar)
            {
                //return vector
                var Total = new QsVector(Math.Abs(toIndex - fromIndex) + 1);

                Total.AddComponent((QsScalar)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (var i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1));
                    }
                }
                else
                {
                    for (var i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1));
                    }
                }
                 return Total;
            }

            if (firstElement is QsVector)
            {
                //return vector
                var Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (var i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1));
                    }
                }
                else
                {
                    for (var i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1));
                    }
                }
                 return Total;
            }
            if (firstElement is QsMatrix)
            {
                 throw new NotImplementedException();
            }
            throw new NotSupportedException();
        }

        public QsValue QsValueElements(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2)
        {
              var firstElement = GetElementValue(fromIndex, arg0, arg1, arg2);
            if (firstElement is QsScalar)
            {
                //return vector
                var Total = new QsVector(Math.Abs(toIndex - fromIndex) + 1);

                Total.AddComponent((QsScalar)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (var i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2));
                    }
                }
                else
                {
                    for (var i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2));
                    }
                }
                 return Total;
            }

            if (firstElement is QsVector)
            {
                //return vector
                var Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (var i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2));
                    }
                }
                else
                {
                    for (var i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2));
                    }
                }
                 return Total;
            }
            if (firstElement is QsMatrix)
            {
                 throw new NotImplementedException();
            }
            throw new NotSupportedException();
        }
        public QsValue QsValueElements(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3)
        {
              var firstElement = GetElementValue(fromIndex, arg0, arg1, arg2, arg3);
            if (firstElement is QsScalar)
            {
                //return vector
                var Total = new QsVector(Math.Abs(toIndex - fromIndex) + 1);

                Total.AddComponent((QsScalar)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (var i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2, arg3));
                    }
                }
                else
                {
                    for (var i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2, arg3));
                    }
                }

                 return Total;
            }

            if (firstElement is QsVector)
            {
                //return vector
                var Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (var i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2, arg3));
                    }
                }
                else
                {
                    for (var i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2, arg3));
                    }
                }
                 return Total;
            }
            if (firstElement is QsMatrix)
            {
                 throw new NotImplementedException();
            }
            throw new NotSupportedException();
        }

        public QsValue QsValueElements(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3, QsParameter arg4)
        {
              var firstElement = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4);
            if (firstElement is QsScalar)
            {
                //return vector
                var Total = new QsVector(Math.Abs(toIndex - fromIndex) + 1);

                Total.AddComponent((QsScalar)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (var i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2, arg3, arg4));
                    }
                }
                else
                {
                    for (var i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2, arg3, arg4));
                    }
                }
                 return Total;
            }

            if (firstElement is QsVector)
            {
                //return vector
                var Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (var i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2, arg3, arg4));
                    }
                }
                else
                {
                    for (var i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2, arg3, arg4));
                    }
                }
                 return Total;
            }
            if (firstElement is QsMatrix)
            {
                 throw new NotImplementedException();
            }
            throw new NotSupportedException();
        }

        public QsValue QsValueElements(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3, QsParameter arg4, QsParameter arg5)
        {
              var firstElement = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5);
            if (firstElement is QsScalar)
            {
                //return vector
                var Total = new QsVector(Math.Abs(toIndex - fromIndex) + 1);

                Total.AddComponent((QsScalar)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (var i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5));
                    }
                }
                else
                {
                    for (var i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5));
                    }
                }
                 return Total;
            }

            if (firstElement is QsVector)
            {
                //return vector
                var Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (var i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5));
                    }
                }
                else
                {
                    for (var i = fromIndex - 1; i >= toIndex; i++)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5));
                    }
                }
                 return Total;
            }
            if (firstElement is QsMatrix)
            {
                 throw new NotImplementedException();
            }
            throw new NotSupportedException();
        }
        public QsValue QsValueElements(int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3, QsParameter arg4, QsParameter arg5, QsParameter arg6)
        {
              var firstElement = GetElementValue(fromIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            if (firstElement is QsScalar)
            {
                //return vector
                var Total = new QsVector(Math.Abs(toIndex - fromIndex) + 1);

                Total.AddComponent((QsScalar)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (var i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5, arg6));
                    }
                }
                else
                {
                    for (var i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddComponent((QsScalar)GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5, arg6));
                    }
                }
                 return Total;
            }

            if (firstElement is QsVector)
            {
                //return vector
                var Total = new QsMatrix();
                Total.AddVector((QsVector)firstElement);

                if (toIndex >= fromIndex)
                {
                    for (var i = fromIndex + 1; i <= toIndex; i++)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5, arg6));
                    }
                }
                else
                {
                    for (var i = fromIndex - 1; i >= toIndex; i--)
                    {
                        Total.AddVector((QsVector)GetElementValue(i, arg0, arg1, arg2, arg3, arg4, arg5, arg6));
                    }
                }
                 return Total;
            }
            if (firstElement is QsMatrix)
            {
                 throw new NotImplementedException();
            }
            throw new NotSupportedException();
        }


        #endregion

        #endregion

    }
}
