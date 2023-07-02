namespace XFCC;
using System.Text;

public static class Keys
{
    public static readonly string By = "By";

    public static readonly string Hash = "Hash";

    public static readonly string Cert = "Cert";

    public static readonly string Chain = "Chain";

    public static readonly string Subject = "Subject";

    public static readonly string URI = "URI";

    public static readonly string DNS = "DNS";
}

public class Parser
{
    private readonly char[] buf;
    private int marker;

    public Parser(string input)
    {
        this.buf = input.ToCharArray();
        this.marker = 0;
    }

    public Value Parse()
    {
        var thisValue = new Value();
        var elementBuilder = new ElementBuilder();

        while (true)
        {
            var next = this.Read();
            if (next == Tokens.Eof)
            {
                break;
            }
            else if (IsIdent(next))
            {
                this.Unread();
                var ident = this.ReadIdent();

                // TODO: throw if not an equals
                this.Read();

                var value = this.ReadValue();

                elementBuilder.Add(ident, value);
            }
            else if (next == Tokens.Semicolon)
            {
                // empty value?? maybe throw here
            }
            else if (next == Tokens.Comma)
            {
                thisValue.Elements.Append(elementBuilder.Build());
                elementBuilder.Reset();
            }
        }

        return new Value();
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

    private void Unread() => this.marker--;
}

public class Value
{
    public List<Element> Elements = new();
}

public class Element
{
    public readonly string? By;

    public readonly string? Hash;

    public readonly string? Cert;

    public readonly string? Chain;

    public readonly string? Subject;

    public readonly string? URI;

    public readonly string? DNS;

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

public class ElementBuilder
{
    private Dictionary<string, string> element = new();

    public void Add(string key, string value) =>

        // TODO: raise exception if unknown key
        this.element.Add(key, value);

    public Element Build() => new(
                this.element.GetValueOrDefault(Keys.By),
                this.element.GetValueOrDefault(Keys.Hash),
                this.element.GetValueOrDefault(Keys.Cert),
                this.element.GetValueOrDefault(Keys.Chain),
                this.element.GetValueOrDefault(Keys.Subject),
                this.element.GetValueOrDefault(Keys.URI),
                this.element.GetValueOrDefault(Keys.DNS));

    public void Reset() => this.element = new Dictionary<string, string>();
}
