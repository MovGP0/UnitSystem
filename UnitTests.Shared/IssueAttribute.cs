using System.Globalization;
using NUnit.Framework;

namespace UnitTests.Shared;

public sealed class IssueAttribute : CategoryAttribute
{
    public IssueAttribute(string issueId)
        : base("Issue " + issueId)
    {
    }

    public IssueAttribute(long issueId)
        : this(issueId.ToString(CultureInfo.InvariantCulture))
    {
    }
}