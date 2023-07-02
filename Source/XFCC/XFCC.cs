namespace XFCC;
using System.Text;

///<Summary>
/// Valid Keys for an XFCC Element
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

///<Summary>
/// Parses a X-Forwarded-Client-Cert string into a XFCC.
///</Summary>
public class Parser
{
    private readonly char[] buf;
    private int marker;

    ///<Summary>
    /// Initializes a Parser
    ///</Summary>
    public Parser(string input)
    {
        this.buf = input.ToCharArray();
        this.marker = 0;
    }

    ///<Summary>
    /// Parses the string used to construct this instance into an XFCC.
    ///</Summary>
    public Value Parse()
    {
        var thisValue = new Value();
        var elementBuilder = new ElementBuilder();

        while (true)
        {
            var next = this.Peek();
            if (next == Tokens.Eof)
            {
                break;
            }
            else if (IsIdent(next))
            {
                var ident = this.ReadIdent();

                // TODO: throw if not an equals
                this.Read();

                var value = this.ReadValue();

                elementBuilder.Add(ident, value);
            }
            else if (next == Tokens.Semicolon)
            {
                // empty value?? maybe throw here

                this.Read();
            }
            else if (next == Tokens.Comma)
            {
                this.Read();
                thisValue.Elements.Append(elementBuilder.Build());
                elementBuilder.Reset();
            }
        }

        return thisValue;
    }

    private static class Tokens
    {
        public static new readonly char Equals = "=".ToCharArray()[0];
        public static readonly char Comma = ",".ToCharArray()[0];
        public static readonly char Semicolon = ";".ToCharArray()[0];
        public static readonly char Backslash = "\\".ToCharArray()[0];
        public static readonly char Doublequote = "\"".ToCharArray()[0];
        public static readonly char Eof; // technically nul char, but good enough
        public static readonly char AValue = "a".ToCharArray()[0];
        public static readonly char ZValue = "z".ToCharArray()[0];
        public static readonly char A = "A".ToCharArray()[0];
        public static readonly char Z = "Z".ToCharArray()[0];
    }

    private static bool IsIdent(char c) => (c >= Tokens.AValue && c <= Tokens.ZValue) || (c >= Tokens.A && c <= Tokens.Z);

    private static bool IsDelimiter(char c) => c == Tokens.Semicolon || c == Tokens.Comma || c == Tokens.Equals;

    private string ReadIdent()
    {
        var sb = new StringBuilder();

        while (true)
        {
            var next = this.Read();
            if (IsIdent(next))
            {
                sb.Append(next);
            }
            else
            {
                this.Unread();
                break;
            }
        }

        return sb.ToString();
    }

    private string ReadValue()
    {
        var sb = new StringBuilder();
        var inQuotes = false;

        while (true)
        {
            var next = this.Read();
            if (next == Tokens.Eof)
            {
                this.Unread();
                break;
            }
            else if (next == Tokens.Doublequote)
            {
                // Assume this is the end of the value
                // TODO: check next value and throw if not semicolon or comma
                if (inQuotes)
                {
                    break;
                }

                inQuotes = true;
            }
            else if (next == Tokens.Backslash)
            {
                // Assume only doublequotes can be escaped - spec is unclear here
                // TODO: always discarding the backslash is probably not correct but
                // there is no formal spec.
                var nextNext = this.Read();
                if (nextNext == Tokens.Doublequote)
                {
                    sb.Append(nextNext);
                }
            }
            else if (IsDelimiter(next))
            {
                if (!inQuotes)
                {
                    this.Unread();
                    break;
                }

                sb.Append(next);
            }
            else
            {
                sb.Append(next);
            }
        }

        return sb.ToString();
    }

    private char Read()
    {
        if (this.marker == this.buf.Length)
        {
            return Tokens.Eof;
        }

        var ret = this.buf[this.marker];
        this.marker++;

        return ret;
    }

    private char Peek()
    {
        var ret = this.Read();
        this.Unread();

        return ret;
    }

    private void Unread() => this.marker--;
}

///<Summary>
/// Represents an X-Forwarded-Client-Cert Header Value
///</Summary>
public class Value
{
    ///<Summary>
    /// Contains the X-Forwarded-Client-Cert Elements contained in this X-Forwarded-Client-Cert Header Value
    ///</Summary>
    public List<Element> Elements = new();
}

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

///<Summary>
/// Helps build an Element, ensuring that keys are specified exactly once and no unexpected keys are supplied. TODO:
///</Summary>
public class ElementBuilder
{
    private Dictionary<string, string> element = new();

    ///<Summary>
    /// Add a Key-Value pair
    ///</Summary>
    public void Add(string key, string value) =>

        // TODO: raise exception if unknown key
        this.element.Add(key, value);

    ///<Summary>
    /// Construct an Element with the current keys and values.
    ///</Summary>
    public Element Build() => new(
                this.element.GetValueOrDefault(Keys.By),
                this.element.GetValueOrDefault(Keys.Hash),
                this.element.GetValueOrDefault(Keys.Cert),
                this.element.GetValueOrDefault(Keys.Chain),
                this.element.GetValueOrDefault(Keys.Subject),
                this.element.GetValueOrDefault(Keys.URI),
                this.element.GetValueOrDefault(Keys.DNS));

    ///<Summary>
    /// Reset to start building a new Element.
    ///</Summary>
    public void Reset() => this.element = new Dictionary<string, string>();
}
