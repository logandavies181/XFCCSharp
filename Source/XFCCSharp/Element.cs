#pragma warning disable CA1051

namespace XFCCSharp;

/// <summary>
/// Represents an X-Forwarded-Client-Cert Element.
/// </summary>
public class Element
{
    /// <summary>
    /// The Subject Alternative Name (URI type) of the current proxy’s certificate. The current proxy’s certificate may
    /// contain multiple URI type Subject Alternative Names, each will be a separate key-value pair.
    /// </summary>
    public string? By { get; }

    /// <summary>
    /// The SHA 256 digest of the current client certificate.
    /// </summary>
    public string? Hash { get; }

    /// <summary>
    /// The entire client certificate in URL encoded PEM format.
    /// </summary>
    public string? Cert { get; }

    /// <summary>
    /// The entire client certificate chain (including the leaf certificate) in URL encoded PEM format.
    /// </summary>
    public string? Chain { get; }

    /// <summary>
    /// The Subject field of the current client certificate. The value is always double-quoted.
    /// </summary>
    public string? Subject { get; }

    /// <summary>
    /// The URI type Subject Alternative Name field of the current client certificate. A client certificate may contain
    /// multiple URI type Subject Alternative Names, each will be a separate key-value pair.
    /// </summary>
    public string? URI { get; }

    /// <summary>
    /// The DNS type Subject Alternative Name field of the current client certificate. A client certificate may contain
    /// multiple DNS type Subject Alternative Names, each will be a separate key-value pair.
    /// </summary>
    public string? DNS { get; }

    /// <summary>
    /// Constructs an X-Forwarded-Client-Cert Element.
    /// </summary>
    public Element(string? by, string? hash, string? cert, string? chain, string? subject, string? uri, string? dns)
    {
        this.By = by;
        this.Hash = hash;
        this.Cert = cert;
        this.Chain = chain;
        this.Subject = subject;
        this.URI = uri;
        this.DNS = dns;
    }
}
