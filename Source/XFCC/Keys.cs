namespace XFCC;

///<Summary>
/// Valid Keys for an XFCC Element.
///</Summary>
public static class Keys
{
    ///<Summary>
    /// The Subject Alternative Name (URI type) of the current proxy’s certificate. The current proxy’s certificate may
    /// contain multiple URI type Subject Alternative Names, each will be a separate key-value pair.
    ///</Summary>
    public static readonly string By = "By";

    ///<Summary>
    /// The SHA 256 digest of the current client certificate.
    ///</Summary>
    public static readonly string Hash = "Hash";

    ///<Summary>
    /// The entire client certificate in URL encoded PEM format.
    ///</Summary>
    public static readonly string Cert = "Cert";

    ///<Summary>
    /// The entire client certificate chain (including the leaf certificate) in URL encoded PEM format.
    ///</Summary>
    public static readonly string Chain = "Chain";

    ///<Summary>
    /// The Subject field of the current client certificate. The value is always double-quoted.
    ///</Summary>
    public static readonly string Subject = "Subject";

    ///<Summary>
    /// The URI type Subject Alternative Name field of the current client certificate. A client certificate may contain
    /// multiple URI type Subject Alternative Names, each will be a separate key-value pair.
    ///</Summary>
    public static readonly string URI = "URI";

    ///<Summary>
    /// The DNS type Subject Alternative Name field of the current client certificate. A client certificate may contain
    /// multiple DNS type Subject Alternative Names, each will be a separate key-value pair.
    ///</Summary>
    public static readonly string DNS = "DNS";
}
