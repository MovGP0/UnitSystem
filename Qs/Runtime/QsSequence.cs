﻿using System.Globalization;
using Qs.Types;

namespace Qs.Runtime;

/// <summary>
/// Single indexed sequence.
/// </summary>
#if WINRT
    public partial class QsSequence : Dictionary<int, QsSequenceElement>, IEnumerable<QsValue>
#else
public partial class QsSequence : SortedList<int, QsSequenceElement> , IEnumerable<QsValue>
#endif
{

    /// <summary>
    /// if the function has a namespace then the value of it is here.
    /// </summary>
    public string SequenceNamespace { get; set; }

    /// <summary>
    /// Minimum decalred index in the sequence.
    /// </summary>
    public int MinimumIndex { get; set; }

    /// <summary>
    /// Maximum declared index in the sequence.
    /// </summary>
    public int MaximumIndex { get; set; }

    /// <summary>
    /// The starting index when sequence operate in range.
    /// </summary>
    public int StartIndex { get; set; }

    /// <summary>
    /// The ending index when sequence operate in range.
    /// </summary>
    public int EndIndex { get; set; }


    /// <summary>
    /// When true indicates that the sequence is executing a range function.
    /// </summary>
    public bool RangeMode { get; set; }


    /// <summary>
    /// Correspones To: S[i]
    /// </summary>
    /// <param name="index"></param>
    /// <returns><see cref="QsSequenceElement"/></returns>
    public new QsSequenceElement this[int index]
    {
        get
        {
            QsSequenceElement el;
            if (!TryGetValue(index, out el))
            {
                //failed to get the value
                // here the magic begins

                if (index > 0)
                {
                    var IX = index;
                    while (!ContainsKey(IX))
                    {
                        IX--;  //decrease the index.
                    }
                    //Bingo we found it
                    TryGetValue(IX, out el);
                }
                else if (index < 0)
                {
                    var IX = index;
                    while (!ContainsKey(IX))
                    {
                        IX++;  //decrease the index.
                    }
                    //Bingo we found it
                    TryGetValue(IX, out el);
                }
                else
                {
                    //zero and not found the in logical error or some one altered the zero element.
                    throw new QsException("Damn it. Where is the zero element");
                }
            }
            return el;
        }
        set
        {
            if (index < MinimumIndex) MinimumIndex = index;
            if (index > MaximumIndex) MaximumIndex = index;

            value.ParentSequence = this;
            value.IndexInParentSequence = index;
            base[index] = value;

            //clear the cache
            CachedValues.Clear();
        }
    }

    /// <summary>
    /// The same function as indexer.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public QsSequenceElement GetElement(int index)
    {
        if (!RangeMode)
        {
            StartIndex = index;
            EndIndex = index;
        }

        return this[index];
    }

    /// <summary>
    /// Internal function to tell the sequence to start in range mode.
    /// </summary>
    /// <param name="startingIndex"></param>
    /// <param name="endingIndex"></param>
    private void BeginRangeOperation(int startingIndex, int endingIndex)
    {
        RangeMode = true;
        StartIndex = startingIndex;
        EndIndex = endingIndex;
    }

    /// <summary>
    /// Internal function to tell the sequence to terminate the range mode.
    /// </summary>
    private void EndRangeOperation()
    {
        RangeMode = false;
    }



    private bool _CachingEnabled;
    /// <summary>
    /// If true the sequence ability to to be cached is available.
    /// </summary>
    public bool CachingEnabled
    {
        internal set
        {
            _CachingEnabled = value;

            if (value == false)
                CachedValues.Clear(); //clear the previous cached values.
        }
        get => _CachingEnabled;
    }

    /// <summary>
    /// Keeps the the values that were calculated before.
    /// When modifieng an item the index of this item and all after items should be deleted.
    /// </summary>
    private Dictionary<int, QsValue> CachedValues = new();


    /// <summary>
    /// Gets the element quantity and employ cach
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public QsValue GetElementValue(int index)
    {

        QsValue val;
        if (CachingEnabled)
        {
            if (CachedValues.TryGetValue(index, out val))
            {
                return val;
            }
        }


        if (Parameters.Length > 0)
        {
            // this is a call to form symbolic element
            // like g[n](x) ..> x^n
            // and calling g[2]
            //  the output should be x^2
            //  and be parsed into function  (QsFunction)
            var e = GetElement(index);

            var se_text = e.ElementDeclaration.Replace("$" + SequenceIndexName, "`");
            se_text = se_text.Replace(SequenceIndexName, index.ToString(CultureInfo.InvariantCulture));
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

            var FunctionBody = se_text;

            var porma = string.Empty;  // the parameters separated by comma ','
            foreach (var prm in Parameters)
            {
                porma += prm.Name + ", ";
            }
            porma = porma.TrimEnd(',', ' ');

            var FunctionDeclaration = "_(" + porma + ") = " + FunctionBody;

            var qs = QsFunction.ParseFunction(QsEvaluator.CurrentEvaluator, FunctionDeclaration);

            return new QsScalar(ScalarTypes.FunctionQuantity) { FunctionQuantity = qs.ToQuantity() };
        }

        val = GetElement(index).Execute(index);

        if (CachingEnabled) CachedValues[index] = val;

        return val;

    }

    #region Get Element Quantity Functions
    public QsValue GetElementValue(int index, QsParameter arg0)
    {
        var val = GetElement(index).Execute(index, arg0);
        return val;
    }

    public QsValue GetElementValue(int index, QsParameter arg0, QsParameter arg1)
    {
        var val = GetElement(index).Execute(index, arg0, arg1);
        return val;
    }

    public QsValue GetElementValue(int index, QsParameter arg0, QsParameter arg1, QsParameter arg2)
    {
        var val = GetElement(index).Execute(index, arg0, arg1, arg2);
        return val;
    }

    public QsValue GetElementValue(int index, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3)
    {
        var val = GetElement(index).Execute(index, arg0, arg1, arg2, arg3);
        return val;
    }

    public QsValue GetElementValue(int index, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3, QsParameter arg4)
    {
        var val = GetElement(index).Execute(index, arg0, arg1, arg2, arg3, arg4);
        return val;
    }

    public QsValue GetElementValue(int index, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3, QsParameter arg4, QsParameter arg5)
    {
        var val = GetElement(index).Execute(index, arg0, arg1, arg2, arg3, arg4, arg5);
        return val;
    }

    public QsValue GetElementValue(int index, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3, QsParameter arg4, QsParameter arg5, QsParameter arg6)
    {
        var val = GetElement(index).Execute(index, arg1, arg1, arg2, arg3, arg4, arg5, arg6);
        return val;
    }

    #endregion


    #region Range Operation Execution

    public QsValue RangeOperation(string operation, int fromIndex, int toIndex)
    {
        BeginRangeOperation(fromIndex, toIndex);
        var result = default(QsValue);
        try
        {
            if (operation.Equals("SumElements", StringComparison.OrdinalIgnoreCase))
                result = SumElements(fromIndex, toIndex);
            else if (operation.Equals("MulElements", StringComparison.OrdinalIgnoreCase))
                result = MulElements(fromIndex, toIndex);
            else if (operation.Equals("Average", StringComparison.OrdinalIgnoreCase))
                result = Average(fromIndex, toIndex);
            else if (operation.Equals("StdDeviation", StringComparison.OrdinalIgnoreCase))
                result = StdDeviation(fromIndex, toIndex);
            else if (operation.Equals("QsValueElements", StringComparison.OrdinalIgnoreCase))
                result = QsValueElements(fromIndex, toIndex);
            else
            {
                EndRangeOperation();
                throw new QsException(operation + " range operation is not known");
            }

            EndRangeOperation();
            return result;
        }
        catch (Exception ex)
        {
            EndRangeOperation();
            throw new QsException("Un Handled Exception", ex);
        }
    }

    public QsValue RangeOperation(string operation, int fromIndex, int toIndex, QsParameter arg0)
    {
        BeginRangeOperation(fromIndex, toIndex);
        var result = default(QsValue);
        try
        {
            if (operation.Equals("SumElements", StringComparison.OrdinalIgnoreCase))
                result = SumElements(fromIndex, toIndex, arg0);
            else if (operation.Equals("MulElements", StringComparison.OrdinalIgnoreCase))
                result = MulElements(fromIndex, toIndex, arg0);
            else if (operation.Equals("Average", StringComparison.OrdinalIgnoreCase))
                result = Average(fromIndex, toIndex, arg0);
            else if (operation.Equals("StdDeviation", StringComparison.OrdinalIgnoreCase))
                result = StdDeviation(fromIndex, toIndex, arg0);
            else if (operation.Equals("QsValueElements", StringComparison.OrdinalIgnoreCase))
                result = QsValueElements(fromIndex, toIndex, arg0);
            else
            {
                EndRangeOperation();
                throw new QsException(operation + " range operation is not known");
            }

            EndRangeOperation();
            return result;
        }
        catch (Exception ex)
        {
            EndRangeOperation();
            throw new QsException("Un Handled Exception", ex);
        }

    }


    public QsValue RangeOperation(string operation, int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1)
    {
        BeginRangeOperation(fromIndex, toIndex);
        var result = default(QsValue);
        try
        {
            if (operation.Equals("SumElements", StringComparison.OrdinalIgnoreCase))
                result = SumElements(fromIndex, toIndex, arg0, arg1);
            else if (operation.Equals("MulElements", StringComparison.OrdinalIgnoreCase))
                result = MulElements(fromIndex, toIndex, arg0, arg1);
            else if (operation.Equals("Average", StringComparison.OrdinalIgnoreCase))
                result = Average(fromIndex, toIndex, arg0, arg1);
            else if (operation.Equals("StdDeviation", StringComparison.OrdinalIgnoreCase))
                result = StdDeviation(fromIndex, toIndex, arg0, arg1);
            else if (operation.Equals("QsValueElements", StringComparison.OrdinalIgnoreCase))
                result = QsValueElements(fromIndex, toIndex, arg0, arg1);
            else
            {
                EndRangeOperation();
                throw new QsException(operation + " range operation is not known");
            }

            EndRangeOperation();
            return result;
        }
        catch (Exception ex)
        {
            EndRangeOperation();
            throw new QsException("Un Handled Exception", ex);
        }

    }
    public QsValue RangeOperation(string operation, int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2)
    {
        BeginRangeOperation(fromIndex, toIndex);
        var result = default(QsValue);
        try
        {
            if (operation.Equals("SumElements", StringComparison.OrdinalIgnoreCase))
                result = SumElements(fromIndex, toIndex, arg0, arg1, arg2);
            else if (operation.Equals("MulElements", StringComparison.OrdinalIgnoreCase))
                result = MulElements(fromIndex, toIndex, arg0, arg1, arg2);
            else if (operation.Equals("Average", StringComparison.OrdinalIgnoreCase))
                result = Average(fromIndex, toIndex, arg0, arg1, arg2);
            else if (operation.Equals("StdDeviation", StringComparison.OrdinalIgnoreCase))
                result = StdDeviation(fromIndex, toIndex, arg0, arg1, arg2);
            else if (operation.Equals("QsValueElements", StringComparison.OrdinalIgnoreCase))
                result = QsValueElements(fromIndex, toIndex, arg0, arg1, arg2);
            else
            {
                EndRangeOperation();
                throw new QsException(operation + " range operation is not known");
            }

            EndRangeOperation();
            return result;
        }
        catch (Exception ex)
        {
            EndRangeOperation();
            throw new QsException("Un Handled Exception", ex);
        }

    }
    public QsValue RangeOperation(string operation, int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3)
    {
        BeginRangeOperation(fromIndex, toIndex);
        var result = default(QsValue);
        try
        {
            if (operation.Equals("SumElements", StringComparison.OrdinalIgnoreCase))
                result = SumElements(fromIndex, toIndex, arg0, arg1, arg2, arg3);
            else if (operation.Equals("MulElements", StringComparison.OrdinalIgnoreCase))
                result = MulElements(fromIndex, toIndex, arg0, arg1, arg2, arg3);
            else if (operation.Equals("Average", StringComparison.OrdinalIgnoreCase))
                result = Average(fromIndex, toIndex, arg0, arg1, arg2, arg3);
            else if (operation.Equals("StdDeviation", StringComparison.OrdinalIgnoreCase))
                result = StdDeviation(fromIndex, toIndex, arg0, arg1, arg2, arg3);
            else if (operation.Equals("QsValueElements", StringComparison.OrdinalIgnoreCase))
                result = QsValueElements(fromIndex, toIndex, arg0, arg1, arg2, arg3);
            else
            {
                EndRangeOperation();
                throw new QsException(operation + " range operation is not known");
            }

            EndRangeOperation();
            return result;
        }
        catch (Exception ex)
        {
            EndRangeOperation();
            throw new QsException("Un Handled Exception", ex);
        }

    }
    public QsValue RangeOperation(string operation, int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3, QsParameter arg4)
    {
        BeginRangeOperation(fromIndex, toIndex);
        var result = default(QsValue);
        try
        {
            if (operation.Equals("SumElements", StringComparison.OrdinalIgnoreCase))
                result = SumElements(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4);
            else if (operation.Equals("MulElements", StringComparison.OrdinalIgnoreCase))
                result = MulElements(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4);
            else if (operation.Equals("Average", StringComparison.OrdinalIgnoreCase))
                result = Average(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4);
            else if (operation.Equals("StdDeviation", StringComparison.OrdinalIgnoreCase))
                result = StdDeviation(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4);
            else if (operation.Equals("QsValueElements", StringComparison.OrdinalIgnoreCase))
                result = QsValueElements(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4);
            else
            {
                EndRangeOperation();
                throw new QsException(operation + " range operation is not known");
            }

            EndRangeOperation();
            return result;
        }
        catch (Exception ex)
        {
            EndRangeOperation();
            throw new QsException("Un Handled Exception", ex);
        }

    }
    public QsValue RangeOperation(string operation, int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3, QsParameter arg4, QsParameter arg5)
    {
        BeginRangeOperation(fromIndex, toIndex);
        var result = default(QsValue);
        try
        {
            if (operation.Equals("SumElements", StringComparison.OrdinalIgnoreCase))
                result = SumElements(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4, arg5);
            else if (operation.Equals("MulElements", StringComparison.OrdinalIgnoreCase))
                result = MulElements(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4, arg5);
            else if (operation.Equals("Average", StringComparison.OrdinalIgnoreCase))
                result = Average(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4, arg5);
            else if (operation.Equals("StdDeviation", StringComparison.OrdinalIgnoreCase))
                result = StdDeviation(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4, arg5);
            else if (operation.Equals("QsValueElements", StringComparison.OrdinalIgnoreCase))
                result = QsValueElements(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4, arg5);
            else
            {
                EndRangeOperation();
                throw new QsException(operation + " range operation is not known");
            }

            EndRangeOperation();
            return result;
        }
        catch (Exception ex)
        {
            EndRangeOperation();
            throw new QsException("Un Handled Exception", ex);
        }

    }

    public QsValue RangeOperation(string operation, int fromIndex, int toIndex, QsParameter arg0, QsParameter arg1, QsParameter arg2, QsParameter arg3, QsParameter arg4, QsParameter arg5, QsParameter arg6)
    {
        BeginRangeOperation(fromIndex, toIndex);
        var result = default(QsValue);
        try
        {
            if (operation.Equals("SumElements", StringComparison.OrdinalIgnoreCase))
                result = SumElements(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            else if (operation.Equals("MulElements", StringComparison.OrdinalIgnoreCase))
                result = MulElements(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            else if (operation.Equals("Average", StringComparison.OrdinalIgnoreCase))
                result = Average(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            else if (operation.Equals("StdDeviation", StringComparison.OrdinalIgnoreCase))
                result = StdDeviation(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            else if (operation.Equals("QsValueElements", StringComparison.OrdinalIgnoreCase))
                result = QsValueElements(fromIndex, toIndex, arg0, arg1, arg2, arg3, arg4, arg5, arg6);
            else
            {
                EndRangeOperation();
                throw new QsException(operation + " range operation is not known");
            }

            EndRangeOperation();
            return result;
        }
        catch(Exception ex)
        {
            EndRangeOperation();
            throw new QsException("Un Handled Exception", ex);
        }

    }

    #endregion




    #region IEnumerable<QsValue> Members


    /// <summary>
    /// Sequence begin from minimum declared index till the maximum number that can stored in double.
    /// </summary>
    /// <returns></returns>
    public new IEnumerator<QsValue> GetEnumerator()
    {
        var i = MinimumIndex;
        var q = GetElementValue(i);
        while (((QsScalar)q).NumericalQuantity.Value < double.MaxValue)
        {

            yield return (QsValue)q;

            i++;
            q = GetElementValue(i);

        }
    }

    #endregion
}