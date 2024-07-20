using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Shared;

public sealed class IssueAttribute : TestCategoryBaseAttribute
{
    public override IList<string> TestCategories { get; } = new List<string>();

    public IssueAttribute(string issueId) => TestCategories.Add("Issue " + issueId);
    public IssueAttribute(long issueId) => TestCategories.Add("Issue " + issueId.ToString(CultureInfo.InvariantCulture));
}