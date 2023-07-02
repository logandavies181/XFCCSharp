namespace XFCCSharp;

/// <summary>
/// Valid Keys for an XFCC Element.
/// </summary>
internal static class Keys
{
    /// <summary>
    /// The Subject Alternative Name (URI type) of the current proxy’s certificate. The current proxy’s certificate may
    /// contain multiple URI type Subject Alternative Names, each will be a separate key-value pair.
    /// </summary>
    public static readonly string By = "By";

    /// <summary>
    /// The SHA 256 digest of the current client certificate.
    /// </summary>
    public static readonly string Hash = "Hash";

    /// <summary>
    /// The entire client certificate in URL encoded PEM format.
    /// </summary>
    public static readonly string Cert = "Cert";

    /// <summary>
    /// The entire client certificate chain (including the leaf certificate) in URL encoded PEM format.
    /// </summary>
    public static readonly string Chain = "Chain";

    /// <summary>
    /// The Subject field of the current client certificate. The value is always double-quoted.
    /// </summary>
    public static readonly string Subject = "Subject";

    /// <summary>
    /// The URI type Subject Alternative Name field of the current client certificate. A client certificate may contain
    /// multiple URI type Subject Alternative Names, each will be a separate key-value pair.
    /// </summary>
    public static readonly string URI = "URI";

    /// <summary>
    /// The DNS type Subject Alternative Name field of the current client certificate. A client certificate may contain
    /// multiple DNS type Subject Alternative Names, each will be a separate key-value pair.
    /// </summary>
    public static readonly string DNS = "DNS";
}
