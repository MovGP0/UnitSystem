using System.Reflection;
using System.Runtime.CompilerServices;

namespace Qs;

internal static class ReflectionExtensions
{
    internal static T RequireMember<T>(this T? member, [CallerArgumentExpression(nameof(member))] string? lookup = null)
        where T : MemberInfo
        => member ?? throw new MissingMemberException($"Could not resolve {lookup}.");
}
