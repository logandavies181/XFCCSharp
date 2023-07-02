using System.Collections.Generic;
using System.Text;

namespace XFCC;


///
public class Parser
{
    private int marker;
    private char[] buf;

    private static class Tokens
    {
        public static readonly char equals = "=".ToCharArray()[0];
        public static readonly char comma = ",".ToCharArray()[0];
        public static readonly char semicolon = ";".ToCharArray()[0];
        public static readonly char backslash = "\\".ToCharArray()[0];
        public static readonly char doublequote = "\"".ToCharArray()[0];
        public static readonly char eof = (char)0; // technically nul char, but good enough
        public static readonly char a = "a".ToCharArray()[0];
        public static readonly char z = "z".ToCharArray()[0];
        public static readonly char A = "A".ToCharArray()[0];
        public static readonly char Z = "Z".ToCharArray()[0];
    }

    ///
    public Parser(string input)
    {
        buf = input.ToCharArray();
        marker = 0;
    }

    ///
    public Value parse()
    {
        var thisValue = new Value();
        var elementBuilder = new ElementBuilder();

        while (true)
        {
            char next = read();
            if (next == Tokens.eof)
            {
                break;
            }
            else if (isIdent(next))
            {
                unread();
                var ident = readIdent();

                // TODO: throw if not an equals
                read();

                var value = readValue();

                elementBuilder.add(ident, value);
            }
            else if (next == Tokens.semicolon)
            {
                // empty value?? maybe throw here
            }
            else if (next == Tokens.comma)
            {
                thisValue.elements.Append(elementBuilder.build());
                elementBuilder.reset();
            }
        }

        return new Value();
    }

    private string readIdent()
    {
        var sb = new StringBuilder();

        while (true)
        {
            char next = read();
            if (isIdent(next))
            {
                sb.Append(next.ToString());
            }
            else
            {
                unread();
                break;
            }
        }

        return sb.ToString();
    }

    private bool isIdent(char c)
    {
        return (c >= Tokens.a && c <= Tokens.z) || (c >= Tokens.A && c <= Tokens.Z);
    }

    private string readValue()
    {
        var sb = new StringBuilder();
        bool inQuotes = false;

        while (true)
        {
            char next = read();
            if (next == Tokens.eof)
            {
                this.unread();
                break;
            }
            else if (next == Tokens.doublequote)
            {
                // Assume this is the end of the value
                // TODO: check next value and throw if not semicolon or comma
                if (inQuotes)
                {
                    break;
                }

                inQuotes = true;
            }
            else if (next == Tokens.backslash)
            {
                // Assume only doublequotes can be escaped - spec is unclear here
                // TODO: always discarding the backslash is probably not correct but
                // there is no formal spec.

                char nextNext = read();
                if (nextNext == Tokens.doublequote) {
                    sb.Append(nextNext);
                }
            }
            else if (isDelimiter(next))
            {
                if (!inQuotes)
                {
                    unread();
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

    private bool isDelimiter(char c)
    {
        return (c == Tokens.semicolon || c == Tokens.comma || c == Tokens.equals);
    }

    private char read()
    {
        if (marker == buf.Length)
        {
            return Tokens.eof;
        }

        char ret = buf[marker];
        marker++;

        return ret;
    }

    private void unread()
    {
        marker--;
    }

    private char peek()
    {
        char ret = this.read();
        this.unread();
        return ret;
    }
}

///
public class Value {
    ///
    public List<Element> elements = new List<Element>();
}

///
public class Element {
    ///
    public readonly string? By;
    ///
    public readonly string? Hash;
    ///
    public readonly string? Cert;
    ///
    public readonly string? Chain;
    ///
    public readonly string? Subject;
    ///
    public readonly string? URI;
    ///
    public readonly string? DNS;

    ///
    public Element(string? By, string? Hash, string? Cert, string? Chain, string? Subject, string? URI, string? DNS)
    {
        this.By = By;
        this.Hash = Hash;
        this.Cert = Cert;
        this.Chain = Chain;
        this.Subject = Subject;
        this.URI = URI;
    }
}

///
public class ElementBuilder
{
    ///
    private Dictionary<string, string> element = new Dictionary<string, string>();

    ///
    public void add(string key, string value)
    {
        // TODO: raise exception if unknown key
        element.Add(key, value);
    }

    ///
    public Element build()
    {
        return new Element(
                element.GetValueOrDefault(Keys.By),
                element.GetValueOrDefault(Keys.Hash),
                element.GetValueOrDefault(Keys.Cert),
                element.GetValueOrDefault(Keys.Chain),
                element.GetValueOrDefault(Keys.Subject),
                element.GetValueOrDefault(Keys.URI),
                element.GetValueOrDefault(Keys.DNS)
                );
    }

    ///
    public void reset()
    {
        element = new Dictionary<string, string>();
    }
}

///
public static class Keys {
    ///
    public static readonly string By = "By";
    ///
    public static readonly string Hash = "Hash";
    ///
    public static readonly string Cert = "Cert";
    ///
    public static readonly string Chain = "Chain";
    ///
    public static readonly string Subject = "Subject";
    ///
    public static readonly string URI = "URI";
    ///
    public static readonly string DNS = "DNS";
}
