#pragma warning disable CA1051

namespace XFCC;

/// <Summary>
/// Represents an X-Forwarded-Client-Cert Header Value
/// </Summary>
public class Value
{
    /// <Summary>
    /// Contains the X-Forwarded-Client-Cert Elements contained in this X-Forwarded-Client-Cert Header Value
    /// </Summary>
    public List<Element> Elements = new();
}
