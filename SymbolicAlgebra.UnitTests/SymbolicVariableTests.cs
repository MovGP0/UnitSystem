using static SymbolicAlgebra.UnitTests.TestingVariables;

namespace SymbolicAlgebra.UnitTests;

[TestOf(typeof(SymbolicVariable))]
public static class SymbolicVariableTests
{
    [TestClass]
    [TestOf(typeof(SymbolicVariable), ILObjectNames.Constructor)]
    public sealed class ConstructorTests
    {
        [TestMethod]
        [DataRow("log(1)", "0")]
        [DataRow("log(exp(1))", "1")]
        [DataRow("log(exp(x^2/4))", "0.25*x^2")]
        [DataRow("log(x*y)", "log(x)+log(y)")]
        [DataRow("log(2*x*y)", "log(2)+log(x)+log(y)")]
        [DataRow("log(2*x)", "log(2)+log(x)")]
        [DataRow("log(y^x)", "x*log(y)")]
        [DataRow("log(y^(2*x+3))", "2*x*log(y)+3*log(y)")]
        [DataRow("log(3^y*i^5)", "y*log(3)+5*log(i)")]
        [DataRow("log(3^y/i^5)", "y*log(3)-5*log(i)")]
        [DataRow("log(3^y-4^u)", "log(3^y-4^u)")]
        public void LogSimplificationTest(string input, string expectedOutput)
        {
            // act
            var v = new SymbolicVariable(input);

            // assert
            v.ToString().ShouldBe(expectedOutput);
        }
    }

    [TestClass]
    [TestOf(typeof(SymbolicVariable), nameof(SymbolicVariable.FactorWithCommonFactor))]
    public sealed class FactorWithCommonFactorTests
    {
        [TestMethod]
        [Ignore("Test is not working in original version either")]
        public void FactorTest()
        {
            // arrange
            var variable = SymbolicVariable.Parse("x^2+x");

            // act
            var ooo = SymbolicVariable.FactorWithCommonFactor(variable!);

            // assert
            ooo.ToString().ShouldBe("x*(x+1)");
        }
    }

    [TestClass]
    [TestOf(typeof(SymbolicVariable), nameof(SymbolicVariable.Integrate))]
    public sealed class IntegrateTests
    {
        [TestMethod]
        [DataRow("2*x", "x", "x^2")]
        [DataRow("2*x*y", "x", "x^2*y")]
        [DataRow("2*x*y", "y", "x*y^2")]
        [DataRow("2*y*x", "x", "y*x^2")]
        [DataRow("2*y*x", "y", "y^2*x")]
        public void SimpleIntegrationTest(string expression, string variable, string expectedResult)
        {
            // arrange
            var symbolicExpression = SymbolicVariable.Parse(expression);

            // act
            var integratedExpression = symbolicExpression.Integrate(variable);

            // assert
            integratedExpression.ToString().ShouldBe(expectedResult);
        }

        [TestMethod]
        [DataRow("log(x)", "x", "log(x)*x-x")]
        [DataRow("log(x+1)", "x", "(x+1)*log(x+1)-x-1", true)]
        [DataRow("log(x^2)", "x", "2*x*log(x)-2*x", true)]
        [DataRow("log(x^2+x)", "x", "-2x+x*log(x*(x+1))+log(x+1)", true)]
        [DataRow("log(x^2)", "y", "2*log(x)*y")]
        public void LogIntegrationTest(string expression, string variableToIntegrate, string expectedResult, bool isDisabled = false)
        {
            if (isDisabled)
            {
                Assert.Inconclusive("Test is disabled");
                return;
            }

            // arrange
            var symbolicExpression = SymbolicVariable.Parse(expression);

            // act
            var integratedExpression = symbolicExpression!.Integrate(variableToIntegrate);

            // assert
            integratedExpression.ToString().ShouldBe(expectedResult);
        }
    }

    [TestClass]
    [TestOf(typeof(SymbolicVariable), nameof(SymbolicVariable.Parse))]
    public sealed class ParseTests
    {
        [TestMethod]
        [Issue("#11")]
        public void Issues11Testing()
        {
            // act
            var v = SymbolicVariable.Parse("z*x/(x^2+y^2+z^2)^(1.5)");

            // assert
            v.ToString().ShouldBe("z*x/(x^2+y^2+z^2)^(1.5)");
        }

        /// <summary>
        /// Entering $-sin(x)^2$ produces $sin(x)^2$, which is wrong
        /// </summary>
        [TestMethod]
        public void Issues17Test()
        {
            // act
            var result = SymbolicVariable.Parse("-Sin(x)^2");

            // assert
            result.ToString().ShouldBe("-Sin(x)^2");
        }

        [TestMethod]
        [DataRow("4*x^-(h^6-4*x^2)", "4*x^(-h^6+4*x^2)")]
        [DataRow("4^-(3*x-8*y^6)*u^---+-+(x^-(h^6-4*x^2))", "4^(-3*x+8*y^6)*u^(x^(-h^6+4*x^2))")]
        public void ParsingAndSimplificationTest(string expression, string expectedResult)
        {
            // arrange
            var symbolicExpression = SymbolicVariable.Parse(expression);

            // assert
            symbolicExpression.ToString().ShouldBe(expectedResult);
        }

        [TestMethod]
        [DataRow("y^2/x^4/(y^4/x^4+2*y^2/x^2+1)", "y^2/x^4/(y^4/x^4+2*y^2/x^2+1)")]
        public void AdditionalDivisionTest(string expression, string expectedResult)
        {
            // act
            var result = SymbolicVariable.Parse(expression);

            // assert
            result.ToString().ShouldBe(expectedResult);
        }

        [TestMethod]
        [Ignore("Test is not working in original version either")]
        [DataRow(
            "x^2/sqrt(x^2+y^2+z^2)^2+z^2*x^2/(x^2+y^2+z^2)^3/sqrt(1-z^2/(x^2+y^2+z^2))^2+y^2/x^4/(y^4/x^4+2*y^2/x^2+1)",
            "x^2/(x^2+y^2+z^2)+z^2*x^2/(x^6+3*y^2*x^4+3*z^2*x^4+3*y^4*x^2+6*z^2*y^2*x^2+3*z^4*x^2+y^6+3*z^2*y^4+3*z^4*y^2+z^6+(-x^6*z^2-3*y^2*x^4*z^2-3*z^4*x^4-3*y^4*x^2*z^2-6*z^4*y^2*x^2-3*z^6*x^2-y^6*z^2-3*z^4*y^4-3*z^6*y^2-z^8)/(x^2+y^2+z^2)+y^2/x^4/(y^4/x^4+2*y^2/x^2+1)")]
        public void ComprehensiveTest(string expression, string expectedResult)
        {
            // act
            var result = SymbolicVariable.Parse(expression);

            // assert
            result.ToString().ShouldBe(expectedResult);
        }
    }

    [TestClass]
    [TestOf(typeof(SymbolicVariable), nameof(SymbolicVariable.Solve))]
    public sealed class SolveTests
    {
        [TestMethod]
        [DataRow("3*x-4", 4.0 / 3)]
        [DataRow("5-3*x", 5.0 / 3.0)]
        public void SolveTest(string expression, double expected)
        {
            // arrange
            var variable = SymbolicVariable.Parse(expression);

            // act
            var result = variable.Solve();

            // assert
            result.ShouldBe(expected);
        }

        [TestMethod]
        [DataRow("2*x+4*y-4", "x", "-2*y+2")]
        [DataRow("5*u-4/r+3*h-2*x", "x", "2.5*u-2/r+1.5*h")]
        [DataRow("5*u-4/r+3*h-2*x", "h", "-1.6666666666666667*u+1.3333333333333333/r+0.6666666666666666*x")]
        public void SolveVariableTest(
            string expression,
            string variableToSolveFor,
            string expectedResult)
        {
            // arrange
            var symbolicExpression = SymbolicVariable.Parse(expression);

            // act
            var solvedExpression = symbolicExpression.Solve(variableToSolveFor);

            // assert
            solvedExpression.ToString().ShouldBe(expectedResult);
        }
    }

    [TestClass]
    [TestOf(typeof(SymbolicVariable), nameof(SymbolicVariable.ReOrderNegativeSymbols))]
    public sealed class ReOrderNegativeSymbolsTests
    {
        [TestMethod]
        [DataRow("5/a", "a")]
        [DataRow("4*x^-2*y^6*u^-4", "x^2*u^4")]
        [DataRow("4*x^-(h^6-4*x^2)", "1")]
        public void ReOrderNegativeSymbolsTest(string inputExpression, string expectedResult)
        {
            // arrange
            var symbolicExpression = SymbolicVariable.Parse(inputExpression);

            // act
            var result = SymbolicVariable.ReOrderNegativeSymbols(symbolicExpression, out _);

            // assert
            result.ToString().ShouldBe(expectedResult);
        }
    }

    [TestClass]
    [TestOf(typeof(SymbolicVariable), nameof(SymbolicVariable.UnifyDenominators))]
    public sealed class UnifyDenominatorsTests
    {
        [TestMethod]
        [DataRow(
            "5/a+6/b",
            "(5*b+6*a)/(a*b)")]
        [DataRow(
            "a/(x-y)+b/(x+y)",
            "(x*a+y*a+x*b-y*b)/(x^2-y^2)")]
        [DataRow(
            "z^2*x^2/(x^2+y^2+z^2)^3/sqrt(1-z^2/(x^2+y^2+z^2))^2",
            "(x^2*z^2)/((x^2+y^2)*(x^2+y^2+z^2)^2)", true)]
        public void UnifyDenominatorsTest(string inputExpression, string expectedResult, bool disabled = false)
        {
            if (disabled)
            {
                Assert.Inconclusive("Test is disabled");
                return;
            }

            // arrange
            var symbolicExpression = SymbolicVariable.Parse(inputExpression);

            // act
            var result = SymbolicVariable.UnifyDenominators(symbolicExpression!);

            // assert
            result.ToString().ShouldBe(expectedResult);
        }
    }

    [TestClass]
    [TestOf(typeof(SymbolicVariable), nameof(SymbolicVariable.TrigSimplify))]
    public sealed class TrigSimplifyTests
    {
        [TestMethod]
        [DataRow("(cos(x)^2+sin(x)^2)", "1")]
        [DataRow("(cos(x)^2+sin(x)^2)^5", "1", true)]
        public void AdvancedFactorizationTest(string expression, string expectedResult, bool isDisabled = false)
        {
            if (isDisabled)
            {
                Assert.Inconclusive("Test is disabled");
            }

            // arrange
            var symbolicExpression = SymbolicVariable.Parse(expression);

            // act
            var simplifiedExpression = SymbolicVariable.TrigSimplify(symbolicExpression!);

            // assert
            simplifiedExpression.ToString().ShouldBe(expectedResult);
        }

        [TestMethod]
        [DataRow("cos(x)^2+sin(x)^2", "1")]
        [DataRow("a^2*alpha^2*sin(alpha*t)^2+a^2*alpha^2*cos(alpha*t)^2+b^2", "a^2*alpha^2+b^2")]
        [DataRow("(sin(phi)^2+cos(phi)^2)*sin(theta)^2+cos(theta)^2", "1")]
        [DataRow("cos(phi)^2*cos(theta)^2 + sin(phi)^2*cos(theta)^2 + sin(theta)^2", "1")]
        [DataRow("0", "0")]
        [DataRow("r^2*cos(phi)^2*cos(theta)^2+r^2*sin(phi)^2*cos(theta)^2+r^2*sin(theta)^2", "r^2")]
        public void TrigSimplifyTest(string input, string expectedOutput)
        {
            // arrange
            var test = SymbolicVariable.Parse(input);

            // act
            var simplified = SymbolicVariable.TrigSimplify(test);

            // assert
            simplified.ToString().ShouldBe(expectedOutput);
        }
    }

    [TestOf(typeof(SymbolicVariable), ILObjectNames.Multiply)]
    public sealed class MultiplyTests
    {
        [TestMethod]
        public void MultiplyTermByOneOnRightSide()
        {
            var term = _x - _y + _z;
            var result = term * _one;
            result.ToString().ShouldBe("x-y+z");
        }

        [TestMethod]
        public void MultiplyTermByOneOnLeftSide()
        {
            var term = _x - _y + _z;
            var result = _one * term;
            result.ToString().ShouldBe("x-y+z");
        }
    }

    [TestClass]
    [TestOf(typeof(SymbolicVariable), nameof(SymbolicVariable.Execute))]
    public sealed class ExecuteTests
    {
        [TestMethod]
        public void TestIntFunction()
        {
            // arrange
            var ii = SymbolicVariable.Parse("Int(x)");

            // act
            var actual = ii.Execute(3.0/4.0);

            // assert
            actual.ShouldBe(0);
        }

        /// <summary>
        /// Tests the handling of negative signs in expressions, particularly when the expression begins with a negative sign.
        /// $-27*x^3$ has two issues:
        /// <nl>
        /// <li>The negative sign is incorrectly taken as part of the coefficient (-27) in the InvolvedSymbols.</li>
        /// <li>The negative sign is dropped in the evaluation of the parameter expression.</li>
        /// </nl>
        /// </summary>
        [TestMethod]
        [DataRow("IIF(x>45,---27*x^3,-50)", 50, -3375000)]
        [DataRow("IIF(x>45,---27*x^3,-50)", 40, -50.0)]
        public void Issue18Test(string expression, double inputValue, double expectedOutput)
        {
            // arrange
            var iif = SymbolicVariable.Parse(expression);

            // act
            var result = iif.Execute(inputValue);

            // assert
            result.ShouldBe(expectedOutput);
        }
    }

    [TestClass]
    [TestOf(typeof(SymbolicVariable), ILObjectNames.Addition)]
    [TestOf(typeof(SymbolicVariable), ILObjectNames.Subtraction)]
    [TestOf(typeof(SymbolicVariable), ILObjectNames.Multiply)]
    [TestOf(typeof(SymbolicVariable), ILObjectNames.Division)]
    public sealed class OperationsTests
    {
        [TestMethod]
        public void ShouldSimplifyOperations()
        {
            // Act
            var a = (_x + _y) * (_x + _y); // x^2 + 2*y*x + y^2
            var a2 = (_x + _y) * (_x - _y); // x^2 - y^2
            var r = _two * (_y * _x * _y); // 2*y^2*x
            var pr = (_four / _two) * _y * _y * _y / _x; // 2*y^3/x
            var ll = _three / _y; // 3/y
            var h = (3 * _x + 2 * _y) / (2 * _x); // 1.5 + y/x

            var pr2 = pr - (_three * _y * _y / _x); // 2*y^3/x - 3*y^2/x
            var rr = pr2 / (2 * _y); // y^2/x - 1.5*y/x
            var tr = rr * (_x * _x); // y^2*x - 1.5*y*x
            var nx = tr - (_y * _x); // y^2*x - 2.5*y*x
            var nx2 = nx - r; // -1*y^2*x - 2.5*y*x
            var mx = tr + (_x * _y); // y^2*x - 0.5*y*x
            var mx2 = mx + (_y * _y * _x); // 2*y^2*x - 0.5*y*x
            var tx = mx2 + nx2; // y^2*x-3*y*x
            var ll2 = ll * _x; // 3/y*x

            // Assert
            "result".ShouldSatisfyAllConditions(
                () => a.ToString().ShouldBe("x^2+2*y*x+y^2"),
                () => a2.ToString().ShouldBe("x^2-y^2"),
                () => r.ToString().ShouldBe("2*y^2*x"),
                () => mx2.ToString().ShouldBe("2*y^2*x-0.5*y*x"),
                () => pr.ToString().ShouldBe("2*y^3/x"),
                () => pr2.ToString().ShouldBe("2*y^3/x-3*y^2/x"),
                () => rr.ToString().ShouldBe("y^2/x-1.5*y/x"),
                () => tr.ToString().ShouldBe("y^2*x-1.5*y*x"),
                () => nx.ToString().ShouldBe("y^2*x-2.5*y*x"),
                () => nx2.ToString().ShouldBe("-y^2*x-2.5*y*x"),
                () => tx.ToString().ShouldBe("y^2*x-3*y*x"),
                () => ll2.ToString().ShouldBe("3/y*x"),
                () => ll.ToString().ShouldBe("3/y"),
                () => h.ToString().ShouldBe("1.5+y/x"),
                () => mx.ToString().ShouldBe("y^2*x-0.5*y*x"));
        }
    }

    [TestClass]
    [TestOf(typeof(SymbolicVariable), nameof(SymbolicVariable.Differentiate))]
    public sealed class DifferentiateTests
    {
        [TestMethod]
        [Issue("#11")]
        public void DifferentiateAcosTest()
        {
            // arrange
            var v = SymbolicVariable.Parse("acos(x^2+y^2)");

            // act
            var result = v.Differentiate("x");

            // assert
            result.ToString().ShouldBe("-2*x/sqrt(1-x^4-2*y^2*x^2-y^4)");
        }

        [TestMethod]
        [Issue("#11")]
        [Ignore("Test is not working in original version either")]
        public void DifferentiateAcosWithSqrtTest()
        {
            // arrange
            var v = SymbolicVariable.Parse("acos(z/sqrt(x^2+y^2+z^2))");

            // act
            var result = v.Differentiate("x");

            // assert
            result.ToString().ShouldBe("z*x/(x^2+y^2+z^2)^(1.5)/sqrt(1-z^2/(x^2+y^2+z^2))");
        }
    }

    [TestClass]
    [TestOf(typeof(SymbolicVariable), nameof(SymbolicVariable.InvolvedSymbols))]
    public sealed class InvolvedSymbolsTests
    {
        [TestMethod]
        [DataRow("2^(2*f*t)*t^(8*i*u)*g^(7^(h*l)*s*c)", 9, "c,f,g,h,i,l,s,t,u")]
        [DataRow("2*sin(x)*sin(cos(y))*log(z)+ g(f(t(u)))", 4, "u,x,y,z")]
        [DataRow("2*x*sqrt(u,G(F(v,c,x)))*l", 5, "c,l,u,v,x")]
        [DataRow("5*sin(x)", 1, "x")]
        [DataRow("sin(3*x)", 1, "x")]
        [DataRow("sum(x,y)", 2, "x,y")]
        public void InvolvedSymbolsTest(string expression, int expectedCount, string expectedSymbols)
        {
            var symbols = expectedSymbols.Split(",");

            var p = SymbolicVariable.Parse(expression);

            p.InvolvedSymbols.ShouldSatisfyAllConditions(
                () => p.InvolvedSymbols.Length.ShouldBe(expectedCount),
                () => p.InvolvedSymbols.ShouldBe(symbols)
            );
        }

        [TestMethod]
        public void TestInvolvedSymbolsExtraction()
        {
            var b = 2 * _u * _w + _v + _w;
            var tv2 = (_x + _y + _z).RaiseToSymbolicPower(b);
            var tv4 = tv2.RaiseToSymbolicPower(3 * _x);

            string[] gg = tv4.InvolvedSymbols;

            gg.ShouldSatisfyAllConditions(
                () => gg[0].ShouldBe("u"),
                () => gg[1].ShouldBe("v"),
                () => gg[2].ShouldBe("w"),
                () => gg[3].ShouldBe("x"),
                () => gg[4].ShouldBe("y"),
                () => gg[5].ShouldBe("z"));
        }

        [TestMethod]
        public void TestInvolvedSymbols_SingleVariable()
        {
            // arrange
            var o = new SymbolicVariable("o");
            var pp = o.RaiseToSymbolicPower(_x);

            // act
            string[] ppP = pp.InvolvedSymbols;

            // assert
            ppP.ShouldSatisfyAllConditions(
                () => ppP[0].ShouldBe("o"),
                () => ppP[1].ShouldBe("x"));
        }

        [TestMethod]
        public void TestInvolvedSymbols_Constant()
        {
            // act & assert
            _two.InvolvedSymbols.Length.ShouldBe(0);
        }
    }

    [TestClass]
    [TestOf(typeof(SymbolicVariable), nameof(SymbolicVariable.RaiseToSymbolicPower))]
    public sealed class RaiseToSymbolicPowerTests
    {
        [TestMethod]
        public void TestRaiseToSymbolicPower_SingleVariable()
        {
            // arrange
            var b = 2 * _u * _w + _v + _w;

            // act
            var tv1 = _x.RaiseToSymbolicPower(b);

            // assert
            tv1.ToString().ShouldBe("x^(2*u*w+v+w)");
        }

        [TestMethod]
        public void TestRaiseToSymbolicPower_SumOfVariables()
        {
            // arrange
            var b = 2 * _u * _w + _v + _w;

            // act
            var tv2 = (_x + _y + _z).RaiseToSymbolicPower(b);

            // assert
            tv2.ToString().ShouldBe("(x+y+z)^(2*u*w+v+w)");
        }

        [TestMethod]
        public void TestRaiseToSymbolicPower_WithVariableExponent()
        {
            // arrange
            var b = 2 * _u * _w + _v + _w;
            var tv2 = (_x + _y + _z).RaiseToSymbolicPower(b);

            // act
            var tv4 = tv2.RaiseToSymbolicPower(3 * _x);

            // assert
            tv4.ToString().ShouldBe("(x+y+z)^(6*u*w*x+3*v*x+3*w*x)");
        }
    }

    [TestClass]
    [TestOf(typeof(SymbolicVariable), nameof(SymbolicVariable.Power))]
    public sealed class PowerTests
    {
        [TestMethod]
        public void TestPowerOperation()
        {
            // arrange
            var b = 2 * _u * _w + _v + _w;
            var tv2 = (_x + _y + _z).RaiseToSymbolicPower(b);

            // act
            var tv3 = tv2.Power(2);

            // assert
            tv3.ToString().ShouldBe("(x+y+z)^(4*u*w+2*v+2*w)");
        }
    }

    [TestClass]
    [TestOf(typeof(SymbolicVariable), ILObjectNames.Addition)]
    public sealed class AdditionTests
    {
        [TestMethod]
        [Issue("#5")]
        public void Issue5Testing0()
        {
            // act
            var p = _x + _x.Power(5);

            // assert
            p.ToString().ShouldBe("x+x^5");
        }

        [TestMethod]
        [Issue("#5")]
        public void Issue5Testing1()
        {
            // arrange
            SymbolicVariable d = new("d");
            SymbolicVariable r = new("r");

            var a = d.RaiseToSymbolicPower(r);
            var b = d.RaiseToSymbolicPower(2*r);

            // act
            var c = a + b;

            // assert
            c.ToString().ShouldBe("d^r+d^(2*r)");
        }
    }
}