namespace XFCC;

///<Summary>
/// Represents an X-Forwarded-Client-Cert Element
///</Summary>
public class Element
{
    ///<Summary>
    /// The Subject Alternative Name (URI type) of the current proxy’s certificate. The current proxy’s certificate may
    /// contain multiple URI type Subject Alternative Names, each will be a separate key-value pair.
    ///</Summary>
    public readonly string? By;

    ///<Summary>
    /// The SHA 256 digest of the current client certificate.
    ///</Summary>
    public readonly string? Hash;

    ///<Summary>
    /// The entire client certificate in URL encoded PEM format.
    ///</Summary>
    public readonly string? Cert;

    ///<Summary>
    /// The entire client certificate chain (including the leaf certificate) in URL encoded PEM format.
    ///</Summary>
    public readonly string? Chain;

    ///<Summary>
    /// The Subject field of the current client certificate. The value is always double-quoted.
    ///</Summary>
    public readonly string? Subject;

    ///<Summary>
    /// The URI type Subject Alternative Name field of the current client certificate. A client certificate may contain
    /// multiple URI type Subject Alternative Names, each will be a separate key-value pair.
    ///</Summary>
    public readonly string? URI;

    ///<Summary>
    /// The DNS type Subject Alternative Name field of the current client certificate. A client certificate may contain
    /// multiple DNS type Subject Alternative Names, each will be a separate key-value pair.
    ///</Summary>
    public readonly string? DNS;

    ///<Summary>
    /// Constructs an X-Forwarded-Client-Cert Element
    ///</Summary>
    public Element(string? by, string? hash, string? cert, string? chain, string? subject, string? uRI, string? dNS)
    {
        this.By = by;
        this.Hash = hash;
        this.Cert = cert;
        this.Chain = chain;
        this.Subject = subject;
        this.URI = uRI;
    }
}
