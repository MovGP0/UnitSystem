using System.Diagnostics;
using static SymbolicAlgebra.UnitTests.TestingVariables;

namespace SymbolicAlgebra.UnitTests;

/// <summary>
///This is a test class for SymbolicVariableTest and is intended
///to contain all SymbolicVariableTest Unit Tests
///</summary>
[TestClass]
[TestOf(typeof(SymbolicVariable))]
public class SymbolicVariableTest
{
    const string op_LnText = "log";

    [TestMethod]
    public void DivisionTest()
    {
        var a = _x + _y;
        var b = _x - _y;

        // dividing by two terms
        var r = a / b;

        r.ToString().ShouldBe("(x+y)/(x-y)");

        // dividing by one term
        var rx = a / _x;
        rx.ToString().ShouldBe("1+y/x");

        var ry = a / _y;
        ry.ToString().ShouldBe("x/y+1");

        // dividing one term by three terms

        var r3 = _x * _y / (_x - _y + _z);

        r3.ToString().ShouldBe("x*y/(x-y+z)");
    }

    [TestMethod]
    public void ZeroTest()
    {
        var r = _x - _x;

        r.IsZero.ShouldBeTrue();

        r = _x * _y - (_y * _x) + _one;
        r.IsZero.ShouldBeFalse();

        r = r - _one;
        r.IsZero.ShouldBeTrue();
    }

    [TestMethod]
    public void PowerTest()
    {
        var x5 = (_x + _y).Power(-5);
        x5.ToString().ShouldBe("1/(x^5+5*y*x^4+10*y^2*x^3+10*y^3*x^2+5*y^4*x+y^5)");

        var x0 = _x.Power(0);
        x0.ToString().ShouldBe("1");

        var r = _x.Power(5);
        r.ToString().ShouldBe("x^5");

        var co = _x - _y;
        var ee = co.Power(3);
        ee.ToString().ShouldBe("x^3-3*y*x^2+3*y^2*x-y^3");

        var re = ee / _x.Power(3);

        re.ToString().ShouldBe("1-3*y/x+3*y^2/x^2-y^3/x^3");
    }

    [TestMethod]
    public void SymbolicPowerTest()
    {
        var x0 = _x.RaiseToSymbolicPower(_zero);
        Assert.AreEqual(expected: "1", actual: x0.ToString());

        var x3 = _x.RaiseToSymbolicPower(_three);
        Assert.AreEqual(actual: x3.ToString(), expected: "x^3");

        var r = _three.RaiseToSymbolicPower(_x);
        Assert.AreEqual(actual: r.ToString(), expected:"3^x");

        var xx = _x * _x;
        var xx3 = _three * xx;

        var xx3Y = xx3.RaiseToSymbolicPower(_y);

        Assert.AreEqual(actual: xx3Y.ToString(), expected: "3^y*x^(2*y)");

        var cplx = _x * _y * _z * _four * _y * _z * _z;

        Assert.AreEqual("4*x*y^2*z^3", cplx.ToString());

        var vpls = cplx.RaiseToSymbolicPower(_two);

        Assert.AreEqual("16*x^2*y^4*z^6", vpls.ToString());

        // testing coeffecient part with   symbol part

        var cx = _x * _seven;
        Assert.AreEqual("7*x", cx.ToString());

        var cx2Y = cx.RaiseToSymbolicPower((_two * _y));
        Assert.AreEqual("7^(2*y)*x^(2*y)", cx2Y.ToString());
    }

    [TestMethod]
    public void MultiTermSymbolicPowerTest()
    {
        var xpy = _x.RaiseToSymbolicPower(_y);

        var xp2 = _x.RaiseToSymbolicPower(_two);

        var xp2Y = xp2 * xpy;  // x^2 * x^y == x^(2+y)

        Assert.AreEqual("x^(2+y)", xp2Y.ToString());

        var xp2_y = SymbolicVariable.Divide( xp2 , xpy);    // x^2 / x^y == x^(2-y)

        Assert.AreEqual("x^(2-y)", xp2_y.ToString());

        var xy = _x * _y;

        var xy2 = xy.RaiseToSymbolicPower(_two);

        Assert.AreEqual("x^2*y^2", xy2.ToString());

        var t = new SymbolicVariable("t");
        var xyt = xy.RaiseToSymbolicPower(t);

        Assert.AreEqual("x^t*y^t", xyt.ToString());

        var xy2T = xy2.RaiseToSymbolicPower(t);
        Assert.AreEqual("x^(2*t)*y^(2*t)", xy2T.ToString());

        var xxy2T = xy2T * _x;
        Assert.AreEqual("x^(2*t+1)*y^(2*t)", xxy2T.ToString());


        var xxy2T2 = xxy2T.Power(2);
        Assert.AreEqual("x^(4*t+2)*y^(4*t)", xxy2T2.ToString());


        var xe = xxy2T + (3 * t * _z);
        Assert.AreEqual("x^(2*t+1)*y^(2*t)+3*t*z", xe.ToString());


        var xe2 = xe.Power(2);
        Assert.AreEqual("x^(4*t+2)*y^(4*t)+6*t*z*x^(2*t+1)*y^(2*t)+9*t^2*z^2", xe2.ToString());

    }

    /// <summary>
    /// Test the multiplication of two powered termed
    /// </summary>
    [TestMethod]
    public void SymbolicPowerMulDivTest()
    {

        var lx = _x.RaiseToSymbolicPower(_y);

        var hx = _x.RaiseToSymbolicPower(_three);

        var hlx = hx * lx;

        Assert.AreEqual("x^(3+y)", hlx.ToString());
    }

    [TestMethod]
    public void IssuesTesting()
    {
        // Issue 1 when multiplying x * u^2-2*v*u+v^2  the middle term was -2*x*v because only 'v' from middle term attached to fused variables
        //   fixed with adding all fused variables from middle term.

        var a = _x;

        var b = new SymbolicVariable("u") - new SymbolicVariable("v");

        var c = b.Power(2);

        var r = a * c;

        Assert.AreEqual(r.ToString(), "x*u^2-2*x*v*u+x*v^2");

        // Issue 2 when dividing x / u^2-2*v*u+v^2  the number changed.
        r = 1 / c;

        Assert.AreEqual(r.ToString(), "1/(u^2-2*v*u+v^2)");

    }

    [TestMethod]
    public void MiscTesting()
    {

        var t = new SymbolicVariable("t");
        var po = _two * t + _one;

        Assert.AreEqual("2*t+1", po.ToString());

        var po2 = po + po;
        Assert.AreEqual("4*t+2", po2.ToString());

        var xp = _x.RaiseToSymbolicPower(po);
        var xp2 = xp * xp;
        Assert.AreEqual("x^(4*t+2)", xp2.ToString());


        var xp2V2 = xp.Power(2);
        Assert.AreEqual("x^(4*t+2)", xp2V2.ToString());



    }

    [TestMethod]
    [Issue(2)]
    public void Issues2Testing()
    {
        #region main issue in division
        var a = -1 * _eight * _x.Power(2) * _y.Power(2);
        var b = _four * _x.Power(3) * _y;

        var c = b.Power(2) / a;

        Assert.AreEqual("-2*x^4", c.ToString());
        #endregion

        #region same issue in multiplication
        var u = _two * _x * _y;                        //2*x*y
        var v = _four * _x.Power(2) * _y.Power(3);     //4*x^2*y^3

        var uv = u * v;
        Assert.AreEqual("8*x^3*y^4", uv.ToString());

        #region testing with different orders of symbols with the same value terms
        u = _two * _y * _x;                        // 2*y*x
        v = _four * _x.Power(2) * _y.Power(3);     // 4*x^2*y^3

        uv = u * v;
        Assert.AreEqual("8*y^4*x^3", uv.ToString());


        u = _three * _y * _x * _z;
        v = _five * _y.Power(2) * _x.Power(3) * _z.Power(7);
        uv = u * v;

        Assert.AreEqual("15*y^3*x^4*z^8", uv.ToString());

        u = _three * _z * _x * _y;
        v = _five * _y.Power(2) * _z.Power(3) * _x.Power(7);
        uv = u * v;

        Assert.AreEqual("15*z^4*x^8*y^3", uv.ToString());
        #endregion
        #endregion


        #region division to the same value term with different orders
        var l = _eight * _z.Power(3) * _x.Power(2) * _y.Power(5);
        var m = _two * _x * _y.Power(3) * _z.Power(7);
        var lm = l / m;
        Assert.AreEqual("4/z^4*x*y^2", lm.ToString());

        #endregion

    }

    [TestMethod]
    public void IssuesRaisedFromIssues2Testing()
    {
        var a = _eight * _x.Power(2);
        var b = _four * _x.Power(4);
        var ab = a / b;
        Assert.AreEqual("2/x^2", ab.ToString());

        var rx = (_x + _y) / _x.Power(2);   // x+y/x^2  ==  x/x^2 + y/x^2 == 1/x + y/x^2

        Assert.AreEqual(actual: rx.ToString(), expected: "1/x+y/x^2");
    }

    [TestMethod]
    [Issue(3)]
    public void Issues3Testing()
    {
        // x^(2*t+1)*y^(2*t)+3*t*z   in power three :S  not the same as maxima my reference program.

        var aa = _x.RaiseToSymbolicPower(2 * _t + _one) * _y.RaiseToSymbolicPower(2 * _t) + 3 * _t * _z;

        var aa2 = aa.Power(2);

        var ex2 = "x^(4*t+2)*y^(4*t)+6*t*z*x^(2*t+1)*y^(2*t)+9*t^2*z^2";

        Assert.AreEqual(ex2, aa2.ToString());

        var aaa = aa * aa2;

        var maxima =
        "x^(6*t+3)*y^(6*t)+9*t*z*x^(4*t+2)*y^(4*t)+27*t^2*z^2*x^(2*t+1)*y^(2*t)+27*t^3*z^3";
        Assert.AreEqual(maxima, aaa.ToString());

        var aal = aa2 * aa;
        Assert.AreEqual(maxima, aal.ToString());


        var aa3 = aa.Power(3);
        Assert.AreEqual(maxima, aa3.ToString());
    }

    [TestMethod]
    [Issue(4)]
    public void Issues4Testing()
    {
        var a = new SymbolicVariable("-10") * _y + new SymbolicVariable("50");
        var b = new SymbolicVariable("-1") * a;

        var big = _x * _y + _x;

        var err1 = big + b;

        foreach (var at in err1.AddedTerms)
            if (at.Value.AddedTerms.Count > 0) throw new Exception("No more than sub terms required");

        var err2 = big - b;

        foreach (var at in err2.AddedTerms)
            if (at.Value.AddedTerms.Count > 0) throw new Exception("No more than sub terms required");

        Assert.AreEqual(1, 1);


    }



    [TestMethod]
    public void RemovingZeroTerms()
    {
        var a = _x + _y + _z;
        var b = -1 * _x + _y + _z;

        var tot = a + b;

        Assert.AreEqual(tot.ToString(), "2*y+2*z");

    }

    [TestMethod]
    public void ExtraPowerTesting()
    {
        var a = (_x + _y + _z).RaiseToSymbolicPower(3 * _x);

        var b = (_x + _y + _z).RaiseToSymbolicPower(_y);

        var aB = a + b;

        Assert.AreEqual("(x+y+z)^(3*x)+(x+y+z)^y", aB.ToString());

        var rs = _y + 3 * _x;
        var ls = 3 * _x + _y;

        Assert.AreEqual(rs.Equals(ls), true);

        var ot = _x.RaiseToSymbolicPower(_v) + _x.RaiseToSymbolicPower(_z) + _x.RaiseToSymbolicPower(_y);

        Assert.AreEqual("x^v+x^z+x^y", ot.ToString());

        var c = SymbolicVariable.Multiply(aB, aB);

        Assert.AreEqual("(x+y+z)^(6*x)+2*(x+y+z)^(y+3*x)+(x+y+z)^(2*y)", c.ToString());

    }


    /// <summary>
    ///A test for Diff
    ///</summary>
    [TestMethod]
    public void DiffTest()
    {
        var a = _x.Power(2).Differentiate("x");
        Assert.AreEqual("2*x", a.ToString());

        var b = (_x.Power(2) + _x.Power(3) + _x.Power(4)).Differentiate("x");
        Assert.AreEqual("2*x+3*x^2+4*x^3", b.ToString());

        var c = (_x.Power(2) * _y.Power(3) * _z.Power(4));

        var dcZ = c.Differentiate("z");

        Assert.AreEqual("4*x^2*y^3*z^3", dcZ.ToString());

        var d = (_x.Power(3) * _y.Power(-40))+(_y.Power(3)*_z.Power(-1));
        var ddY = d.Differentiate("y");

        Assert.AreEqual("-40*x^3/y^41+3/z*y^2", ddY.ToString());

        var e = _x.RaiseToSymbolicPower(2*_y);
        var deX = e.Differentiate("x");

        Assert.AreEqual("2*x^(2*y-1)*y", deX.ToString());

        var f = _two * _x.RaiseToSymbolicPower(3 * _z) * _y.RaiseToSymbolicPower(2 * _z);
        var dfX = f.Differentiate("y");

        Assert.AreEqual("4*x^(3*z)*y^(2*z-1)*z", dfX.ToString());

        var g = _two * _y + 3 * _x.RaiseToSymbolicPower(_z) - 5 * _x.RaiseToSymbolicPower(_y - _one);
        var dgX = g.Differentiate("x");

        Assert.AreEqual("3*x^(z-1)*z-5*x^(y-2)*y+5*x^(y-2)", dgX.ToString());

        var dfiveX = _five.Differentiate("u");
        Assert.AreEqual("0", dfiveX.ToString());

        var h = _x.Power(3)+ _x.RaiseToSymbolicPower(_two) + _x; //x^2+x
        var dhX = h.Differentiate("x");

        Assert.AreEqual("3*x^2+2*x+1", dhX.ToString());

        var i = 18 * _x * _y.Power(3);
        var diX = i.Differentiate("x");
        Assert.AreEqual("18*y^3", diX.ToString());

        var j = 18 * _x * _y * _z.Power(2);
        var djY = j.Differentiate("y");
        Assert.AreEqual("18*x*z^2", djY.ToString());

        var k = 18 * _x.Power(2) + _y.Power(3) * _x + 2 * _x * _y;
        var dkY = k.Differentiate("y");

        Assert.AreEqual("3*x*y^2+2*x", dkY.ToString());
    }


    [TestMethod]
    public void ParseTest()
    {
        var s = SymbolicVariable.Parse("2*x+3*x");

        Assert.AreEqual(5.0, s.Coeffecient);

        var c = SymbolicVariable.Parse("2*x*y^3-3*x^2*y^2+6*x-3+5*x+6*x-4*y^x");

        Assert.AreEqual("2*x*y^3-3*x^2*y^2+17*x-3-4*y^x", c.ToString());

        var h = SymbolicVariable.Parse("3*(x+3*x)-4*y");
        Assert.AreEqual("12*x-4*y", h.ToString());

        var v = SymbolicVariable.Parse("sin(x)^2+cos(x)^2");

        var g = SymbolicVariable.Parse("sin(x^2)+cos(x^3)");

        var ns = SymbolicVariable.Parse("-6^w");
        Assert.AreEqual("-6^w", ns.ToString());

    }



    [TestMethod]
    public void FuncDiscoveryTest()
    {
        Assert.AreEqual(true, _cosFunction.IsFunction);
        Assert.AreEqual(false, _x.IsFunction);
        Assert.AreEqual(true, _uxyFunction.IsFunction);

        Assert.AreEqual(true, _vFuntion.IsFunction);

        Assert.AreEqual(true, _emptyFuntion.IsFunction);


        var cf = SymbolicVariable.Parse("sum(o, sum(a,b))");
        var g = cf.FunctionParameters;
        Assert.AreEqual("o", g[0].ToString());
        Assert.AreEqual("sum(a,b)", g[1].ToString());



    }

    [TestMethod]
    public void SpecialFunctionsDifftest()
    {
        var sin = new SymbolicVariable("sin(x)");
        var cos = new SymbolicVariable("cos(x)");
        var tan = new SymbolicVariable("tan(x)");
        var sec = new SymbolicVariable("sec(x)");
        var csc = new SymbolicVariable("csc(x)");
        var cot = new SymbolicVariable("cot(x)");

        Assert.AreEqual("cos(x)", sin.Differentiate("x").ToString());
        Assert.AreEqual("-sin(x)", cos.Differentiate("x").ToString());
        Assert.AreEqual("sec(x)^2", tan.Differentiate("x").ToString());
        Assert.AreEqual("sec(x)*tan(x)", sec.Differentiate("x").ToString());
        Assert.AreEqual("-csc(x)*cot(x)", csc.Differentiate("x").ToString());
        Assert.AreEqual("-csc(x)^2", cot.Differentiate("x").ToString());

        var sinh = new SymbolicVariable("sinh(x)");
        var cosh = new SymbolicVariable("cosh(x)");
        var tanh = new SymbolicVariable("tanh(x)");
        var sech = new SymbolicVariable("sech(x)");
        var csch = new SymbolicVariable("csch(x)");
        var coth = new SymbolicVariable("coth(x)");

        Assert.AreEqual("cosh(x)", sinh.Differentiate("x").ToString());
        Assert.AreEqual("sinh(x)", cosh.Differentiate("x").ToString());
        Assert.AreEqual("sech(x)^2", tanh.Differentiate("x").ToString());
        Assert.AreEqual("-sech(x)*tanh(x)", sech.Differentiate("x").ToString());
        Assert.AreEqual("-csch(x)*coth(x)", csch.Differentiate("x").ToString());
        Assert.AreEqual("-csch(x)^2", coth.Differentiate("x").ToString());


        var complexsin = SymbolicVariable.Parse("cos(x^2+x^2+x^2)");
        Assert.AreEqual("cos(3*x^2)", complexsin.ToString());

        Assert.AreEqual("-6*x*sin(3*x^2)", complexsin.Differentiate("x").ToString());


        var log = new SymbolicVariable(op_LnText + "(x^6*y^3)");

        Assert.AreEqual("6/x", log.Differentiate("x").ToString());

        Assert.AreEqual("3/y", log.Differentiate("y").ToString());
    }

    [TestMethod]
    public void SpecialFunctionHigherPowerTest()
    {
        var sin = new SymbolicVariable("sin(x)");
        var sin2Y = sin.RaiseToSymbolicPower(SymbolicVariable.Parse("2-y"));
        Assert.AreEqual("sin(x)^(2-y)", sin2Y.ToString());
        Assert.AreEqual("2*cos(x)*sin(x)^(1-y)-cos(x)*sin(x)^(1-y)*y", sin2Y.Differentiate("x").ToString());

        sin = SymbolicVariable.Parse("2*sin(x)");
        Assert.AreEqual("2*cos(x)", sin.Differentiate("x").ToString());
    }

    [TestMethod]
    [Issue(5)]
    public void Issues5Testing()
    {
        var p = SymbolicVariable.Multiply( _one, SymbolicVariable.Parse("2^x"));

        Assert.AreEqual("2^x", p.ToString());


        var p2 = SymbolicVariable.Multiply(SymbolicVariable.Parse("2^x"), _one);
        Assert.AreEqual("2^x", p2.ToString());


        var xx = SymbolicVariable.Parse("x^2");
        var roro = SymbolicVariable.Multiply(p, xx);
        Assert.AreEqual("2^x*x^2", roro.ToString());

        var g = SymbolicVariable.Multiply(new SymbolicVariable(op_LnText + "(2)"), SymbolicVariable.Parse("2^x"));
        Assert.AreEqual(op_LnText + "(2)*2^x", g.ToString());

        g = SymbolicVariable.Multiply( g , SymbolicVariable.Parse("2^y"));
        Assert.AreEqual(op_LnText + "(2)*2^(x+y)", g.ToString());

        g = SymbolicVariable.Multiply(g, SymbolicVariable.Parse("3^y"));
        Assert.AreEqual(op_LnText + "(2)*2^(x+y)*3^y", g.ToString());

        g = SymbolicVariable.Multiply(g, SymbolicVariable.Parse("3^z"));
        Assert.AreEqual(op_LnText + "(2)*2^(x+y)*3^(y+z)", g.ToString());
    }

    [TestMethod]
    public void BaseMultiplicationTest()
    {
        var fp = SymbolicVariable.Parse("2^x");
        var sp = SymbolicVariable.Parse("2^y");

        var g = SymbolicVariable.Multiply(fp, sp);

        Assert.AreEqual("2^(x+y)", g.ToString());


        var fl = SymbolicVariable.Parse("3^x");
        g = SymbolicVariable.Multiply(fp, fl);
        Assert.AreEqual("2^x*3^x", g.ToString());

        g = SymbolicVariable.Multiply(g, SymbolicVariable.Parse("3^y"));
        Assert.AreEqual("2^x*3^(x+y)", g.ToString());

        g = SymbolicVariable.Multiply(g, SymbolicVariable.Parse("2^z"));
        Assert.AreEqual("2^(x+z)*3^(x+y)", g.ToString());


        g = SymbolicVariable.Multiply(g, SymbolicVariable.Parse("2^v"));
        Assert.AreEqual("2^(x+z+v)*3^(x+y)", g.ToString());

        g = SymbolicVariable.Multiply(g, SymbolicVariable.Parse("3^u"));
        Assert.AreEqual("2^(x+z+v)*3^(x+y+u)", g.ToString());


        var ns = SymbolicVariable.Parse("-6.6^w");
        Assert.AreEqual("-6.6^w", ns.ToString());

        g = SymbolicVariable.Multiply(g, ns);
        Assert.AreEqual("2^(x+z+v)*3^(x+y+u)*-6.6^w", g.ToString());

    }

    [TestMethod]
    [Issue(6)]
    public void Issues6Testing()
    {
        var px = SymbolicVariable.Parse("3^x");
        var tw7 = SymbolicVariable.Parse("27");

        var g = SymbolicVariable.Multiply(px, tw7);

        Assert.AreEqual("3^(x+3)", g.ToString());

        g = SymbolicVariable.Multiply(g, new SymbolicVariable("30"));
        Assert.AreEqual("3^(x+3)*30", g.ToString());

    }


    [TestMethod]
    public void BaseVariablePowerVariableDiffTest()
    {
        var p = SymbolicVariable.Parse("u^(x^2)");
        var g= p.Differentiate("x");
        Assert.AreEqual("2*"+ op_LnText + "(u)*x*u^(x^2)", g.ToString());
    }

    [TestMethod]
    [Issue(7)]
    public void Issues7Testing()
    {
        var s = new SymbolicVariable("---  4 5");
        Assert.AreEqual("-45", s.ToString());

        s = new SymbolicVariable("--- + sin (3- 4 + t * 9) + 4 - 5");
        Assert.AreEqual("-sin(3-4+t*9)+4-5", s.ToString());
    }

    [TestMethod]
    [Issue(8)]
    public void Issues8Testing()
    {
        //3^(2*x)*3
        var t2X = SymbolicVariable.Parse("3^(2*x)");
        var g = SymbolicVariable.Multiply(t2X, _three);
        Assert.AreEqual("3^(2*x+1)", g.ToString());

        var tx5 = SymbolicVariable.Parse("3^(x-5)");
        g = SymbolicVariable.Multiply(_eleven, tx5);
        Assert.AreEqual("11*3^(x-5)", g.ToString());

        g = SymbolicVariable.Multiply(new SymbolicVariable("27"), tx5); // 3^3*3^(x-5)
        Assert.AreEqual("3^(-2+x)", g.ToString());

        var fvx = SymbolicVariable.Parse("5^x");
        g = SymbolicVariable.Multiply(_five, fvx);

        Assert.AreEqual("5^(1+x)", g.ToString());

        var cst = new SymbolicVariable("-cos(t)");
        g = SymbolicVariable.Multiply(_t, cst);
        Assert.AreEqual("-t*cos(t)", g.ToString());

    }


    [TestMethod]
    public void DifferentiateMultpliedSymbols()
    {
        var p = SymbolicVariable.Parse("x^2*y^(2*x)");
        var g = p.Differentiate("x");

        Assert.AreEqual("2*y^(2*x)*x+2*x^2*"+ op_LnText + "(y)*y^(2*x)", g.ToString());

        p = SymbolicVariable.Parse("2^(x^2)*x^3");
        g = p.Differentiate("x");
        Assert.AreEqual("x^4*log(2)*2^(1+x^2)+2^x^2*x^2*3", g.ToString());

        p = SymbolicVariable.Parse("sin(x)*cos(x)");
        g = p.Differentiate("x");

        Assert.AreEqual("cos(x)^2-sin(x)^2", g.ToString());


        p = SymbolicVariable.Parse("sin(2*x*y)*cos(x)");
        g = p.Differentiate("x");

        Assert.AreEqual("2*cos(x)*y*cos(2*x*y)-sin(2*x*y)*sin(x)", g.ToString());

        g = p.Differentiate("y");
        Assert.AreEqual("2*cos(x)*x*cos(2*x*y)", g.ToString());

        p = SymbolicVariable.Parse(op_LnText + "(x^(2*f*t)*y^(5*t))");
        Assert.AreEqual("2*f*t*log(x)+5*t*log(y)", p.ToString());

        g = p.Differentiate("t");
        Assert.AreEqual("2*f*log(x)+5*log(y)", g.ToString());

        g = p.Differentiate("x");
        Assert.AreEqual("2*f*t/x", g.ToString());

        g = p.Differentiate("y");
        Assert.AreEqual("5*t/y", g.ToString());

    }


    /// <summary>
    ///A test for ParseLambda
    ///</summary>
    [TestMethod]
    public void ParseLambdaTest()
    {
        var twoX = SymbolicVariable.Parse("x*x");

        Assert.AreEqual(16, twoX.Execute(4));

        var pp = SymbolicVariable.Parse("2*x^60*x*y+z-3^u");

        List<Tuple<string, double>> foo2 = new(4);
        var x = new Tuple<string, double>("x", 0);
        var y= new Tuple<string, double>("y", 9);
        var z =new Tuple<string, double>("z", 8);
        var u =new Tuple<string, double>("u", 7);

        Assert.AreEqual(-2179.0, pp.Execute(y, x, z, u));

        var v = _cosFunction.Execute(3.14159265358979);

        Assert.AreEqual(-1.0, v);

        v = SymbolicVariable.Parse("asin(x)").Execute(0.03);
        Assert.AreEqual(Math.Asin(0.03), v);

        v = SymbolicVariable.Parse("exp(x)*sin(x)").Execute(2);
        Assert.AreEqual(Math.Exp(2) * Math.Sin(2), v);

        v = SymbolicVariable.Parse("5*sin(x)").Execute(5);
        Assert.AreEqual(5 * Math.Sin(5), v);

        v = SymbolicVariable.Parse("20*cos(x)*sin(x)^3+x").Execute(3);
        Assert.AreEqual(20 * Math.Cos(3) * Math.Pow(Math.Sin(3), 3) + 3, v);

        v = SymbolicVariable.Parse("20*cos(x)*sin(x)^3-x").Execute(3);
        Assert.AreEqual(20 * Math.Cos(3) * Math.Pow(Math.Sin(3), 3) - 3, v);

        //v = SymbolicVariable.Parse("7*sin(x)*log(x)").Execute(3);
        //Assert.AreEqual(7 * Math.Sin(3) * Math.Log(3), v);

        v = SymbolicVariable.Parse("-1*x^2").Execute(8);
        Assert.AreEqual(-64, v);

        v = SymbolicVariable.Parse("sin(3*x)").Execute(2);
        Assert.AreEqual(Math.Sin(3 * 2), v);
    }

    [TestMethod]
    public void ParseLambdaPerformanceTest()
    {
        var r = SymbolicVariable.Parse("3*sin(x)*x^3");
        r.Execute(0);

        Dictionary<string, double> xdic = new(1);

        xdic.Add("x", 0);

        var times = 100000;

        // Test using dictionary
        var t0 = Environment.TickCount;
        for (var i = 0; i < times; i++)
        {
            r.Execute(xdic);
            xdic["x"] = xdic["x"]++;
        }

        var tElapsed = Environment.TickCount - t0;

        Trace.WriteLine(string.Format("Dictionary One Parameter Elapsed Time {0}", tElapsed));

        // test without dictionary
        double pizo = 0;
        t0 = Environment.TickCount;
        for (var i = 0; i < times; i++)
        {
            r.Execute(pizo);
            pizo++;
        }

        tElapsed = Environment.TickCount - t0;
        Trace.WriteLine(string.Format("Native Offset One Parameter Elapsed Time {0}", tElapsed));
    }

    [TestMethod]
    [Issue(9)]
    public void Issues9Testing()
    {
        // the following issues has been fixed by using ExtraTerms in Symbolic Variable
        // Extra Term include the terms that is not divided by one

        var v = SymbolicVariable.Parse("x+2/(x+8)");
        Assert.AreEqual("x+2/(x+8)", v.ToString());

        v = SymbolicVariable.Parse("x/(x+8)+x");
        Assert.AreEqual("x/(x+8)+x", v.ToString());

        v = SymbolicVariable.Parse("0 - 1/(1+x)");
        Assert.AreEqual("-1/(1+x)", v.ToString());

        v = SymbolicVariable.Parse("+6/(z*x*y)-(2+u+v)/(x+y)");
        Assert.AreEqual("6/z/x/y-(2+u+v)/(x+y)", v.ToString());


        v = SymbolicVariable.Parse("2/(x+2) + y/(x+2)");
        Assert.AreEqual("(2+y)/(x+2)", v.ToString());

        v = SymbolicVariable.Parse("2/(x+2) - y/(x+2)");
        Assert.AreEqual("(2-y)/(x+2)", v.ToString());

        v = SymbolicVariable.Parse("(1/(y+x)+2/y)*8");
        Assert.AreEqual("8/(y+x)+16/y", v.ToString());

        v = SymbolicVariable.Divide(v, _eight);
        Assert.AreEqual("1/(y+x)+2/y", v.ToString());

        v = SymbolicVariable.Parse("(2/(x+y)^2)|x");
        Assert.AreEqual("(-4*x-4*y)/(x^4+4*y*x^3+6*y^2*x^2+4*y^3*x+y^4)", v.ToString());

        v = SymbolicVariable.Parse("(2+3/x+5/(x-1))|x");
        Assert.AreEqual("-3/x^2-5/(x^2-2*x+1)", v.ToString());
    }

    [TestMethod]
    [Issue(10)]
    public void Issues10Testing()
    {
        var v = SymbolicVariable.Parse("4+5*t");
        v = v.Power(0.5);

        Assert.AreEqual("Sqrt(4+5*t)", v.ToString());


        v = v.Power(2);
        Assert.AreEqual("4+5*t", v.ToString());

        v = SymbolicVariable.Parse("sqrt(8+x)*sqrt(8+x)");
        Assert.AreEqual("8+x", v.ToString());

        v = SymbolicVariable.Parse("sqrt(5*x^2)*6*sqrt(x^2*5)");
        Assert.AreEqual("30*x^2", v.ToString());

        v = SymbolicVariable.Parse("(6*sqrt(x*2)+4*sqrt(x+x))*sqrt(2*x)");
        Assert.AreEqual("20*x", v.ToString());
    }

    [TestMethod]
    [Issue(12)]
    public void Issues12Test()
    {
        // it is about addition and subtraction of coefficients with different or the same powers.

        var v = SymbolicVariable.Parse("3^y-4^u");
        Assert.AreEqual("3^y-4^u", v.ToString());

        v = SymbolicVariable.Parse("3^y-4^y");
        Assert.AreEqual("3^y-4^y", v.ToString());

        v = SymbolicVariable.Parse("3^y+4^y");
        Assert.AreEqual("3^y+4^y", v.ToString());

        v = SymbolicVariable.Parse("3^y+4^u");
        Assert.AreEqual("3^y+4^u", v.ToString());

        v = SymbolicVariable.Parse("3^u-5^x-3^u");
        Assert.AreEqual("-5^x", v.ToString());

        v = SymbolicVariable.Parse("3^u-5^x-3^u+6^x+2^x+5^y");
        Assert.AreEqual("-5^x+6^x+2^x+5^y", v.ToString());

        var pp = SymbolicVariable.Parse("2*(1-3*x)^cos(x)");   //issue is that parser consider the (1-3*x) is a whole parameter which is wrong

        Assert.AreEqual(pp.InvolvedSymbols.Length, 1);
        Assert.AreEqual(pp.InvolvedSymbols[0], "x");
    }

    [TestMethod]
    public void LogarithmicDifferentiationTest()
    {
        //LOGARITHMIC DIFFERENTIATION  was not correct

        var v = SymbolicVariable.Parse("x^x");
        v = v.Differentiate("x");
        Assert.AreEqual("x^x*log(x)+x^x", v.ToString());

        v = SymbolicVariable.Parse("2*x^(3*x^2)");
        v = v.Differentiate("x");
        Assert.AreEqual("12*x^(3*x^2+1)*log(x)+6*x^(3*x^2+1)", v.ToString());

        v = SymbolicVariable.Parse("x^exp(x)");
        v = v.Differentiate("x");
        Assert.AreEqual("x^(exp(x))*log(x)*exp(x)+x^(exp(x)-1)*exp(x)", v.ToString());

        v = SymbolicVariable.Parse("(1-3*x)^cos(x)");
        v = v.Differentiate("x");
        Assert.AreEqual("-(1-3*x)^(cos(x))*log((1-3*x))*sin(x)-3*(1-3*x)^(cos(x))*cos(x)/(1-3*x)", v.ToString());


        v = SymbolicVariable.Parse("log(2*x^(3*x^2))");
        v = v.Differentiate("x");
        Assert.AreEqual("6*log(x)*x+3*x", v.ToString());
    }

    [TestMethod]
    [Issue(13)]
    public void Issues13Test()
    {
        var v = new SymbolicVariable("log(sin(x)^x)");

        Assert.AreEqual("x*log(sin(x))", v.ToString());
        var dv = v.Differentiate("x");

        Assert.AreEqual("log(sin(x))+x*cos(x)/sin(x)", dv.ToString());
        v = SymbolicVariable.Parse("sin(x)^x");

        dv = v.Differentiate("x");
        Assert.AreEqual("sin(x)^x*log(sin(x))+sin(x)^(x-1)*x*cos(x)", dv.ToString());

    }

    [TestMethod]
    public void PowerRightAssociativity()
    {
        var d = SymbolicVariable.Parse("a^b^c");
        Assert.AreEqual("a^(b^c)", d.ToString());

        var r = d.Execute(3, 2, 4);
        Assert.AreEqual(43046721.0, r);
    }

    [TestMethod]
    [Issue(14)]
    public void Issues14Test()
    {
        // operations on zero when muliplied or divided should return zero
        var bb = SymbolicVariable.Parse("0/(x+3)");
        Assert.AreEqual("0", bb.ToString());

        bb = SymbolicVariable.Parse("(2*cos(theta)^2*r^2*sin(theta)^2+sin(theta)^4*r^2+cos(theta)^4*r^2)/(2*cos(theta)^2*r^2*sin(theta)^2+sin(theta)^4*r^2+cos(theta)^4*r^2)");
        Assert.AreEqual("1", bb.ToString());

        var gg = SymbolicVariable.Parse("(cos(theta)^2+sin(theta)^2)/(2*cos(theta)^2*r^2*sin(theta)^2+sin(theta)^4*r^2+cos(theta)^4*r^2)");
        var cc = _zero * gg;
        Assert.AreEqual("0", cc.ToString());

        var kk = SymbolicVariable.Parse("(cos(theta)^2+sin(theta)^2)/(2*cos(theta)^2*r^2*sin(theta)^2+sin(theta)^4*r^2+cos(theta)^4*r^2)");
        var ll = _one * kk;
        Assert.AreEqual(kk.ToString(), ll.ToString());

        var we = SymbolicVariable.Parse("5*sin(x)^4^x");
        var rs = we.Differentiate("x");
        Assert.AreEqual("5*sin(x)^(4^x)*log(sin(x))*log(4)*4^x+5*sin(x)^(4^x-1)*cos(x)*4^x", rs.ToString());
        Assert.AreEqual(1, rs.InvolvedSymbols.Length);
    }

    [TestMethod]
    [Issue(15)]
    public void Issues15Test()
    {
        // When parsing 4e-2 the parsing go wrong because it consider 4e a unit and -2 another unit

        var bb = SymbolicVariable.Parse("4e-2");

        Assert.AreEqual("0.04", bb.ToString());

        bb = SymbolicVariable.Parse("4.45e+2");
        Assert.AreEqual("445", bb.ToString());

        bb = SymbolicVariable.Parse("-120*t^3+180*t^2+1.06581410364015E-14*t+-30");
        Assert.AreEqual(bb.InvolvedSymbols.Length, 1);

        var r = bb.Execute(2);
    }

    [TestMethod]
    [Issue(16)]
    public void Issues16Test()
    {
        var v1 = SymbolicVariable.Parse("5*t1");
        var v2 = SymbolicVariable.Parse("3/t1");
        var vv = SymbolicVariable.Add(v1, v2);

        Assert.AreEqual("5*t1+3/t1", vv.ToString());

        var vm = SymbolicVariable.Subtract(v1, v2);
        Assert.AreEqual("5*t1-3/t1", vm.ToString());

        var g = SymbolicVariable.Add(vv, vm);
        Assert.AreEqual("10*t1", g.ToString());
    }

    [TestMethod]
    public void TestingInfinity()
    {
        var inf = new SymbolicVariable("inf");
        Assert.AreEqual(double.PositiveInfinity, inf.Coeffecient);

        inf = new SymbolicVariable("infinity");
        Assert.AreEqual(double.PositiveInfinity, inf.Coeffecient);

        inf = new SymbolicVariable("infinity") * SymbolicVariable.NegativeOne;
        Assert.AreEqual(double.NegativeInfinity, inf.Coeffecient);
    }

    [TestMethod]
    public void IIF_ExecuteTest()
    {
        var iif = SymbolicVariable.Parse("IIF(x <= 5, x^2, x^3)");

        Assert.AreEqual<string>(iif.InvolvedSymbols[0], "x");

        Assert.AreEqual(iif.Execute(3), 3 * 3);
        Assert.AreEqual(iif.Execute(5), 5 * 5);
        Assert.AreEqual(iif.Execute(6), 6 * 6 * 6);

        var complexIIf = SymbolicVariable.Parse("IIF(x*y <= 20, 0, IIF(x<5, 1, 2))");

        Assert.AreEqual(complexIIf.Execute(4, 5), 0);
        Assert.AreEqual(complexIIf.Execute(4, 6), 1);
        Assert.AreEqual(complexIIf.Execute(6, 4), 2);

        var iif2 = SymbolicVariable.Parse("IIF(x+y<10, x*y, x/y)");

        Assert.AreEqual(iif2.Execute(2, 3), 2.0 * 3.0);
        Assert.AreEqual(iif2.Execute(8, 9), 8.0 / 9.0);
    }

    [TestMethod]
    public void PiConstant()
    {
        var pi = new SymbolicVariable("%pi");

        Assert.AreEqual(pi.InvolvedSymbols.Length, 0);
        Assert.AreEqual(Math.PI, pi.Execute());

        var sinpi2 = new SymbolicVariable("sin(%pi/2)");

        Assert.AreEqual(pi.InvolvedSymbols.Length, 0);

        Assert.AreEqual(Math.Sin(Math.PI / 2), sinpi2.Execute());

        var e = new SymbolicVariable("%e");
        Assert.AreEqual(Math.E, e.Execute());
    }

    [TestMethod]
    public void TestGetCommonFactorsMap()
    {
        var ex = SymbolicVariable.Parse("a*cos(x)^2+8*x+a*sin(x)^2+c");

        var map = ex.GetCommonFactorsMap();

        var a = new SymbolicVariable("a");

        var aExistence = map[a];

        aExistence[0].ShouldBe(0);
        aExistence[1].ShouldBe(2);

        var ex2 = SymbolicVariable.Parse("a*c*v*5*x + c*v + d*v*u + u*h*s + v*d*h*s + v*a*c*Q");
        var map2 = ex2.GetCommonFactorsMap();

        var c = new SymbolicVariable("c");
        var d = new SymbolicVariable("d");
        var v = new SymbolicVariable("v");
        var u = new SymbolicVariable("u");
        var h = new SymbolicVariable("h");
        var s = new SymbolicVariable("s");

        map2[a][0].ShouldBe(0);
        map2[a][1].ShouldBe(5);

        map2[c][0].ShouldBe(0);
        map2[c][1].ShouldBe(1);
        map2[c][2].ShouldBe(5);

        map2[v][0].ShouldBe(0);
        map2[v][1].ShouldBe(1);
        map2[v][2].ShouldBe(2);
        map2[v][3].ShouldBe(4);
        map2[v][4].ShouldBe(5);

        map2[d][0].ShouldBe(2);
        map2[d][1].ShouldBe(4);

        map2[h][0].ShouldBe(3);
        map2[h][1].ShouldBe(4);

        map2[s][0].ShouldBe(3);
        map2[s][1].ShouldBe(4);

        var test = SymbolicVariable.Parse("a^2*alpha^2*sin(alpha*t)^2+a^2*alpha^2*cos(alpha*t)^2+b^2");
        var tmap = test.GetCommonFactorsMap();

        // the following expression has a divded term that needs to be taken into account
        var cc = SymbolicVariable.Parse("(sin(x)^4-cos(x)^4)/(sin(x)^2-cos(x)^2)");
        var ccmap = cc.GetCommonFactorsMap();

        var zmapp = SymbolicVariable.Parse("0").GetCommonFactorsMap();
    }

    [TestMethod]
    public void FactorCommonFactorTest()
    {
        var sv = SymbolicVariable.Parse("a*x+a*y+z");
        var result = SymbolicVariable.FactorWithCommonFactor(sv);

        Assert.AreEqual("a*(x+y)+z", result.ToString());

        var rr = SymbolicVariable.Parse("a*b*c*f + a*b*s + a*b*c*h");
        var rrResult = SymbolicVariable.FactorWithCommonFactor(rr);
        Assert.AreEqual("a*b*(c*f+s+c*h)", rrResult.ToString());

        var trig = SymbolicVariable.Parse("a^2*alpha^2*sin(alpha*t)^2+a^2*alpha^2*cos(alpha*t)^2+b^2");
        var trigResult = SymbolicVariable.FactorWithCommonFactor(trig);

        Assert.AreEqual("a^2*alpha^2*(sin(alpha*t)^2+cos(alpha*t)^2)+b^2", trigResult.ToString());

        var test = SymbolicVariable.Parse("(sin(phi)^2+cos(phi)^2)*sin(theta)^2+cos(theta)^2");
        var factored = SymbolicVariable.FactorWithCommonFactor(test);

        Assert.AreEqual("sin(theta)^2*(sin(phi)^2+cos(phi)^2)+cos(theta)^2", factored.ToString());

        var spz = SymbolicVariable.FactorWithCommonFactor(SymbolicVariable.Parse("0"));
    }

    [TestMethod]
    [DataRow("sqrt(1-z^2/(x^2+y^2+z^2))^2", "z^2/(x^2+y^2+z^2)")]
    public void SquareRootSimplificationTest_Subtract(string expression, string expectedResult)
    {
        var g = SymbolicVariable.Parse(expression);
        var gp = SymbolicVariable.Parse(g.ToString());

        // act
        var result = SymbolicVariable.Subtract(SymbolicVariable.One, gp);

        Assert.AreEqual(expectedResult, result.ToString());
    }

    [TestMethod]
    [DataRow("sqrt(1-z^2/(x^2+y^2+z^2))^2", "2-z^2/(x^2+y^2+z^2)")]
    public void SquareRootSimplificationTest(string expression, string expectedResult)
    {
        var g = SymbolicVariable.Parse(expression);
        var gp = SymbolicVariable.Parse(g.ToString());

        // act
        var result = SymbolicVariable.Add(SymbolicVariable.One, gp);

        Assert.AreEqual(expectedResult, result.ToString());
    }

    [TestMethod]
    [DataRow("(x^2)", "(1-z^2/(x^2+y^2))", "x^2-x^2*z^2/(x^2+y^2)")]
    [DataRow("(x^2+y^2)", "1-z^2/(x^2+y^2)", "x^2+y^2+(-x^2*z^2-y^2*z^2)/(x^2+y^2)")]
    public void MultiplicationTest(string leftExpression, string rightExpression, string expectedResult)
    {
        var pl = SymbolicVariable.Parse(leftExpression);
        var pr = SymbolicVariable.Parse(rightExpression);
        var result = SymbolicVariable.Multiply(pl, pr);
        Assert.AreEqual(expectedResult, result.ToString());
    }

    [TestMethod]
    [DataRow("(x^2+y^2+z^2)^3*sqrt(1-z^2/(x^2+y^2+z^2))^2", "x^6+3*y^2*x^4+3*z^2*x^4+3*y^4*x^2+6*z^2*y^2*x^2+3*z^4*x^2+y^6+3*z^2*y^4+3*z^4*y^2+z^6+(-x^6*z^2-3*y^2*x^4*z^2-3*z^4*x^4-3*y^4*x^2*z^2-6*z^4*y^2*x^2-3*z^6*x^2-y^6*z^2-3*z^4*y^4-3*z^6*y^2-z^8)/(x^2+y^2+z^2)")]
    public void ComplexExpressionTest(string expression, string expectedResult)
    {
        var result = SymbolicVariable.Parse(expression);
        Assert.AreEqual(expectedResult, result.ToString());
    }

    [TestMethod]
    [DataRow("z^2*x^2", "(x^2+y^2+z^2)^3", "z^2*x^2/(x^6+3*y^2*x^4+3*z^2*x^4+3*y^4*x^2+6*z^2*y^2*x^2+3*z^4*x^2+y^6+3*z^2*y^4+3*z^4*y^2+z^6)")]
    [DataRow("z^2*x^2/(x^2+y^2+z^2)^3", "sqrt(1-z^2/(x^2+y^2+z^2))^2", "z^2*x^2/(x^6+3*y^2*x^4+3*z^2*x^4+3*y^4*x^2+6*z^2*y^2*x^2+3*z^4*x^2+y^6+3*z^2*y^4+3*z^4*y^2+z^6+(-x^6*z^2-3*y^2*x^4*z^2-3*z^4*x^4-3*y^4*x^2*z^2-6*z^4*y^2*x^2-3*z^6*x^2-y^6*z^2-3*z^4*y^4-3*z^6*y^2-z^8)/(x^2+y^2+z^2)")]
    public void DivisionTest(string numerator, string denominator, string expectedResult)
    {
        var t1 = SymbolicVariable.Parse(numerator);
        var t2 = SymbolicVariable.Parse(denominator);
        var result = SymbolicVariable.Divide(t1, t2);
        Assert.AreEqual(expectedResult, result.ToString());
    }
}