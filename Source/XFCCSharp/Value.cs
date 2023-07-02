#pragma warning disable CA1051

namespace XFCCSharp;

/// <summary>
/// Represents an X-Forwarded-Client-Cert Header Value
/// </summary>
public class Value
{
    /// <summary>
    /// Contains the X-Forwarded-Client-Cert Elements contained in this X-Forwarded-Client-Cert Header Value
    /// </summary>
    public List<Element> Elements = new();
}
