﻿using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Qs.Types;
using Qs.Types.Attributes;
using QsRoot;

namespace Qs.Runtime;

/// <summary>
/// Employing the namespace concept in the Qs
/// Namespace is in the form of xml namespaces.
/// Namespace are dual functionality:
///    1) normal namespace declared in qs context while execution.
///    2) custom namespace from built-in or external classes during the program load.
/// For example Math namespace is built-in namespace in the program but you can add
///     another variables and functions for it during running.
///     
/// This class is responsible to act as a provider for the two functionalities.
/// </summary>
public class QsNamespace(string name)
{



    public string NameSpaceRoot { get; set; }


    public string Name { get; } = name;

    private Type _NamespaceType = null;
    public Type NamespaceType => _NamespaceType;


    private Dictionary<string, object> Values = new(StringComparer.OrdinalIgnoreCase);

    public QsReference AddReference(string newVariable, string targetVariable)
    {
        var qsr = new QsReference(targetVariable);

        Values.Add(newVariable, qsr);

        return qsr;

    }

    public void SetValue(string name, object value)
    {
        if (char.IsLetter(name[0]))
        {
            if (_NamespaceType != null)
            {
                var prop = _NamespaceType.GetProperty(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
                if (prop != null)
                {
                    prop.SetValue(null, value, null);
                }
                else
                {
                    Values[name] = value;
                }
            }
            else
            {
                object o;
                Values.TryGetValue(name, out o);
                var r = o as QsReference;
                if (r == null)
                    Values[name] = value;
                else
                    r.ContentValue = (QsValue)value;

            }
        }
        else
        {
            throw new QsSyntaxErrorException("Variable name must start with a letter.");
        }
    }


    /// <summary>
    /// It tries to get the value of the given key.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public object GetValue(string name)
    {
        if (_NamespaceType != null)
        {
            if (QsFunction.IsItFunctionSymbolicName(name))
            {
                if (HardCodedMethods == null) HardCodedMethods = GetQsNamespaceMethods();

                if (HardCodedMethods.ContainsKey(name))
                {
                    return HardCodedMethods[name];
                }
            }
            else
            {
                var prop = _NamespaceType.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
                if (prop != null)
                {
                    return Root.NativeToQsConvert(prop.GetValue(null, null));
                }

            }
        }

        // try in values hash

        object o;
        Values.TryGetValue(name, out o);

        if (o == null) throw new QsVariableNotFoundException("Variable '" + name + "' not found in '" + Name + "' namespace.");
        return o;
    }


    public object GetValueOrNull(string name)
    {
        if (_NamespaceType != null)
        {
            if (QsFunction.IsItFunctionSymbolicName(name))
            {
                if (HardCodedMethods == null) HardCodedMethods = GetQsNamespaceMethods();

                if (HardCodedMethods.ContainsKey(name))
                {
                    return HardCodedMethods[name];
                }
            }
            else
            {
                var prop = _NamespaceType.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
                if (prop != null)
                {
                    return Root.NativeToQsConvert(prop.GetValue(null, null));
                }
            }
        }

        // try in values hash

        object o;
        Values.TryGetValue(name, out o);

        return o;
    }


    /// <summary>
    /// Decorate a native C# function with QsFunction
    /// </summary>
    /// <param name="methodInfo"></param>
    /// <param name="funcAttribute"></param>
    /// <returns></returns>
    public QsFunction GetQsFunctionFromTypeMethod(MethodInfo methodInfo, QsFunctionAttribute funcAttribute)
    {
        var qsNamespace = _NamespaceType.Name;

        var qsFuncName = funcAttribute == null ? methodInfo.Name : funcAttribute.FunctionName;

        var miParameters = methodInfo.GetParameters();
        var qsFuncParamCount = miParameters.Length;


        var QsModFunc = new QsFunction("[" +qsNamespace + "." + qsFuncName + "]", true);
        QsModFunc.FunctionNamespace = qsNamespace;
        QsModFunc.FunctionName = qsFuncName;

        List<QsParamInfo> prms = new(miParameters.Length);

        foreach (var miParam in miParameters)
        {
            var prm = new QsParamInfo();

            //parameter name
            prm.Name = miParam.Name;

            var pis = miParam.GetCustomAttributes(typeof(QsParamInfoAttribute), true);

            if (pis.Length > 0)
                prm.Type = ((QsParamInfoAttribute)pis[0]).ParameterType;
            else
                prm.Type = QsParamType.Value;

            prms.Add(prm);
        }

        QsModFunc.Parameters = prms.ToArray();
        QsModFunc.FunctionDeclaration += "(";
        var sb = new StringBuilder();
        foreach (var p in prms) sb.Append(", " + p.Name);
        if (sb.Length > 0) QsModFunc.FunctionDeclaration += sb.ToString().TrimStart(',', ' ');
        QsModFunc.FunctionDeclaration += ")";



        QsModFunc.InternalFunctionDelegate = FormNativeFunctionDelegate(methodInfo);
        return QsModFunc;

    }



    #region discover of items

    /// <summary>
    /// Cache the discovered QsNamespace inner class type members.
    /// </summary>
    private Dictionary<string, object> HardCodedMethods;

    /// <summary>
    /// Gets the members of the QsNamespace or (static class ) that hold the hard coded functions and properties.
    /// </summary>
    /// <param name="namespaceType"></param>
    /// <returns></returns>
    private Dictionary<string, object> GetQsNamespaceMethods()
    {

        // The namespace is a static class with public visibility to its static members.
        var methods = _NamespaceType.GetMethods(
            BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public
            /*| BindingFlags.InvokeMethod */
        );

        Dictionary<string, object> CodedMembers = new(StringComparer.OrdinalIgnoreCase);

        /*
        var FilteredMethods = from m in methods
                              where
                              m.IsSpecialName == false

                              && m.ReturnType.IsArray == false &&
                              (m.ReturnType.BaseType == typeof(QsValue) || m.ReturnType == typeof(QsValue) ||
                              m.ReturnType == typeof(string) || m.ReturnType.IsValueType == true)

                              select m;
        */
        var FilteredMethods = from m in methods
            where
                m.IsSpecialName == false
            select m;

        foreach (var member in FilteredMethods)
        {
            var method = ((MethodInfo)member);
            if (!method.IsSpecialName) //because properies getters are methods also :S
            {
                ParameterInfo[] mps = method.GetParameters();


                var paramCount = mps.Length;


                var fAttributes = method.GetCustomAttributes(typeof(QsFunctionAttribute), false);

                QsFunctionAttribute funcAttribute = null;
                var method_name = string.Empty;
                var methodTrueName = string.Empty;

                if (fAttributes.Length > 0)
                {

                    funcAttribute = (QsFunctionAttribute)fAttributes[0];

                    // if the attribute were found treat the function as namedargument function that is not default function.
                    method_name = funcAttribute.FunctionName;

                    if (funcAttribute.DefaultScopeFunction == false)
                    {
                        // alter the name so it includes the parameters also
                        string[] ptext = (from p in mps select p.Name).ToArray();
                        methodTrueName = QsFunction.FormFunctionSymbolicName(method_name, ptext);
                    }
                    else
                    {
                        // don't use special parameter symbolic naming.
                        methodTrueName = QsFunction.FormFunctionSymbolicName(method_name, paramCount);
                    }

                    CodedMembers.Add(methodTrueName, GetQsFunctionFromTypeMethod(method, funcAttribute));

                }
                else
                {
                    method_name = method.Name;
                    methodTrueName = QsFunction.FormFunctionSymbolicName(method_name, paramCount);

                    CodedMembers.Add(methodTrueName, GetQsFunctionFromTypeMethod(method, null));

                }
            }
        }

        return CodedMembers;
    }


    private IEnumerable<KeyValuePair<string, object>> GetQsNamespaceProperties()
    {

        var properties = _NamespaceType.GetProperties(
            BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);

        var FilteredProperties = from m in properties
            // m.IsSpecialName == false && m.ReturnType.IsArray == false &&
            //(m.ReturnType.BaseType == typeof(QsValue) || m.ReturnType == typeof(QsValue) ||
            //m.ReturnType == typeof(string) || m.ReturnType.IsValueType == true)
            where m.IsSpecialName == false && m.PropertyType.IsArray == false
                                           && m.PropertyType != typeof(bool) &&
                                           (
                                               m.PropertyType.BaseType == typeof(QsValue)
                                               || m.PropertyType == typeof(QsValue)
                                               || m.PropertyType == typeof(string)
                                               || m.PropertyType.IsValueType == true
                                           )
            select m;

        List<KeyValuePair<string, object>> CodedMembers = [];

        foreach (var prop in FilteredProperties)
        {
            CodedMembers.Add(new KeyValuePair<string, object>
                (prop.Name, prop.GetValue(null,null)));

        }

        return CodedMembers;

    }

    public IEnumerable<KeyValuePair<string, object>> GetItems()
    {
        List<KeyValuePair<string, object>> items = [];

        items.AddRange(Values);

        //also include functions of NamespaceType
        if (_NamespaceType != null)
        {
            // add the properties
            items.AddRange(GetQsNamespaceProperties());

            if (HardCodedMethods == null)
            {
                HardCodedMethods = GetQsNamespaceMethods();
            }
            items.AddRange(HardCodedMethods);
        }

        return items;
    }


    public IEnumerable<string> GetVariablesKeys()
    {
        var varo = from item in GetItems()
            select item.Key;
        return varo;

    }

    #endregion

    #region Helpers for getting qsnampespaces


    /// <summary>
    /// The namespace is a static C# class under QsRoot.*
    /// </summary>
    /// <param name="nameSpace"></param>
    /// <returns></returns>
    private static Type GetQsNamespaceType(string qsNamespace)
    {

        qsNamespace = qsNamespace.Replace(':', '.');


        var cls = qsNamespace;

        if (cls.StartsWith(".."))
            cls = cls.Replace("..", ""); // indicates that we used root operator in namespace  ::
        else
            cls = "QsRoot." + cls;



        //try the current assembly
#if WINRT
            // in Windows RT get only the types under the QsRoot

            System.Type ns = Root.GetInternalType(cls);
#else

        var ns = Type.GetType(cls, false, true);
#endif

        if (ns == null)
        {

            /*
            // try  another search in the Qs*.dll dlls

            DirectoryInfo di = new DirectoryInfo(System.Environment.CurrentDirectory + "/Modules");
            var files = di.GetFiles("Qs*.dll");

            foreach (var file in files)
            {
                var a = Assembly.LoadFrom(file.FullName);
                ns = a.GetType(cls, false, true);//+ ", " + file.Name.TrimEnd('.', 'd', 'l', 'l'));
                if (ns != null) break;  //found the break and pop out from the loop.
            }
             * */

            ns = Root.GetExternalType(cls);
        }

        return ns;
    }



    /// <summary>
    /// try to get the name
    /// </summary>
    /// <param name="scope"></param>
    /// <param name="moduleNamespace"></param>
    /// <returns></returns>
    public static QsNamespace GetNamespace(QsScope scope, string moduleNamespace)
    {
        return GetNamespace(scope, moduleNamespace, false);
    }


    /// <summary>
    /// The primary  function that gets the namespace from the scope.
    /// </summary>
    /// <param name="scope"></param>
    /// <param name="moduleNamespace"></param>
    /// <param name="forceCreation">force creation of namespace in scope if dosen't exist before.</param>
    /// <returns></returns>
    public static QsNamespace GetNamespace(QsScope scope, string moduleNamespace, bool forceCreation)
    {
        QsNamespace NameSpace = null;

        // try to get the namespace object from the scope
        //  then see if it represent a custom namespace also or not.
        // then return the object that hold the whole thing.
        if (scope != null)
        {

            if (scope.HasValue(moduleNamespace))
            {
                NameSpace = (QsNamespace)scope.GetValue(moduleNamespace);
            }

            if (NameSpace == null)
            {
                // no namespace in this scope
                NameSpace = new QsNamespace(moduleNamespace);

                // search if this namespace represent hardcoded namespace
                var nst = GetQsNamespaceType(moduleNamespace);
                NameSpace._NamespaceType = nst;

                if (forceCreation | (nst != null))
                {
                    scope.SetValue(moduleNamespace, NameSpace);
                }
            }
        }


        return NameSpace;
    }

    /// <summary>
    /// Gets a strongly typed namespace that were defined with static class type and has static methods
    /// that can be used to provide qs compatible methods.
    /// </summary>
    /// <param name="moduleNamespace"></param>
    /// <returns></returns>
    public static QsNamespace GetTypedNamespace(string moduleNamespace)
    {
        var NameSpace = new QsNamespace(moduleNamespace);

        // search if this namespace represent hardcoded namespace
        var nst = GetQsNamespaceType(moduleNamespace);
        NameSpace._NamespaceType = nst;

        return NameSpace;
    }

    #endregion





    /// <summary>
    /// Returns a delegate to native c# function.
    /// </summary>
    /// <param name="method"></param>
    internal static Delegate FormNativeFunctionDelegate(MethodInfo method)
    {
        var DecorateNativeFunction = false;

        if (method.ReturnType != typeof(QsValue)) DecorateNativeFunction = true;
        //construct the lambda

        var lb = SimpleLambdaBuilder.Create(typeof(QsValue), method.Name);


        //prepare parameters with the same name of native function but with qsparameter type
        var parameters = method.GetParameters();

        foreach (var prm in parameters)
        {
            if (prm.ParameterType != typeof(QsParameter)) DecorateNativeFunction = true;
            lb.Parameter(typeof(QsParameter), prm.Name);
        }

        if (!DecorateNativeFunction)
        {

            #region Delegate creation section
            switch (parameters.Length)
            {
                case 0:
                    return  Delegate.CreateDelegate(
                        typeof(Func<QsValue>),
                        method);

                case 1:
                    return Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsValue>),
                        method);

                case 2:
                    return Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsValue>),
                        method);

                case 3:
                    return Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsParameter, QsValue>),
                        method);
                case 4:
                    return Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                        method);
                case 5:
                    return Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                        method);
                case 6:
                    return Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                        method);
                case 7:
                    return Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                        method);
                case 8:
                    return Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                        method);

                case 9:
                    return Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                        method);

                case 10:
                    return Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                        method);

                case 11:
                    return Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                        method);

                case 12:
                    return Delegate.CreateDelegate(
                        typeof(Func<QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsParameter, QsValue>),
                        method);

            }
            #endregion

        }

        // we will form a function body that
        //    1- check the parameter
        //    2- convert the parameter into native c# during runtime so it can be passed to the desired function

        List<Expression> statements = [];

        var qns = typeof(QsNamespace);
        var qsys = typeof(Root);

        var convparMethod = qsys.GetMethod("QsParametersToNativeValues", BindingFlags.Static | BindingFlags.Public);
        var NTOQ = qsys.GetMethod("NativeToQsConvert", BindingFlags.Static | BindingFlags.Public);
        var iv = qns.GetMethod("IndirectInvoke", BindingFlags.Static | BindingFlags.NonPublic);
        Expression methodExpress = Expression.Constant(method);

        if (parameters.Length > 0)
        {
            // Take the function parameter values
            //  to convert it to native values if required.

            // convert to array of QsParameter
            var parms = Expression.NewArrayInit(typeof(QsParameter), lb.Parameters.ToArray());


            // Convert to array of Object
            Expression ConvertedParametersExpression = Expression.Call(convparMethod, methodExpress ,parms);

            var rr = Expression.Call(iv, methodExpress, ConvertedParametersExpression);

            var vv = Expression.Call(NTOQ, rr);
            statements.Add(vv);
        }
        else
        {
            var parms = Expression.NewArrayInit(typeof(object));

            var rr = Expression.Call(iv, methodExpress, parms);


            //var rr = Expression.Call(null, method);
            var vv = Expression.Call(NTOQ, rr);
            statements.Add(vv);
        }


        lb.Body = Expression.Block(statements);

        var lbe = lb.MakeLambda();

        return lbe.Compile();

    }




    internal static object IndirectInvoke(MethodInfo method, params object[] parameters)
    {

        return  method.Invoke(null, parameters);
    }



}