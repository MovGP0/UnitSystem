using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;
using System.Text;
using Qs.Runtime;

namespace Qs.Scripting;

public class QsScriptCode(SourceUnit sourceUnit) : ScriptCode(sourceUnit)
{
    public override object Run()
    {
        return Run(new Scope());
    }

    public static string LastLine { get; set; }

    public override object Run(Scope scope)
    {
        var code = string.Empty;
        //try
        {
            if (SourceUnit.HasPath)
            {
                if (File.Exists(SourceUnit.Path))
                {
                    code = SourceUnit.GetReader().ReadToEnd();
                }
                else
                    throw new QsException("File Not Found");
            }
            else
            {
                code = SourceUnit.GetReader().ReadToEnd();
            }
        }
        //catch(Exception xx)
        //{
        //    code = LastLine;   //workaround because Host have something weird in SourceTextReader that don't work linux mono
        //}

        var qs = QsEvaluator.CurrentEvaluator;

        //qs.Scope = scope;

        string[] lines = code.Split(Environment.NewLine.ToCharArray());

        object ret=null;

        foreach (var line in lines)
        {
            if (!string.IsNullOrEmpty(line))
            {
                // test for directive like  %module  %unitdef gogo m/s^6
                // hmmm
                //  changed it to #module   .. who cares :P :P
                //   
                if(line.TrimStart().StartsWith("#module"))
                {
                    // this is a directive
                    var dir = line.TrimStart();
                    QsRoot.Root.LoadLibrary(dir.Substring(8).Trim());
                }
                else if (!line.StartsWith("#"))
                {
                    //I want to exclude # if it was between parentthesis.
                    //  oo(ferwe#kd adflk ) #

                    // first pass (from left to right): find the # char which is the comment.
                    var pc = 0; // for ()

                    var qcOpened = false;
                    var ix = 0;

                    var sb = new StringBuilder();
                    while (ix < line.Length)
                    {
                        var c = line[ix];
                        if (c == '(') pc++;

                        if (line[ix] == '"')
                        {
                            if (ix > 0)
                            {
                                if (line[ix - 1] != '\\') // not escape charachter for qoutation mark
                                    qcOpened = !qcOpened;
                            }
                            else
                                qcOpened = !qcOpened;
                        }

                        // is it a comment charachter.
                        if (c == '#')
                        {
                            if (pc == 0 && qcOpened == false)
                            {
                                // found the comment
                                //  break
                                break;
                            }
                        }

                        if (c == ')') pc--;

                        sb.Append(c);

                        ix++;
                    }

                    var l2 = sb.ToString().Trim();  // text without comment.

                    //check the last charachter
                    if (l2.EndsWith(";"))
                    {
                        //trim the ';' and silent evaluate the expression.
                        ret = qs.SilentEvaluate(l2.Trim(';'));
                    }
                    else
                    {
                        ret = qs.Evaluate(l2);
                    }
                }
            }
        }

        return ret;
    }

}