using System.Collections.Generic;
using System.Text;

namespace XFCC;


///
public class Parser
{
    private int marker;
    private byte[] buf;

    private static class Tokens
    {
        public static readonly int equals = (int)"="[0];
        public static readonly int comma = (int)","[0];
        public static readonly int semicolon = (int)";"[0];
        public static readonly int backslash = (int)"\\"[0];
        public static readonly int doublequote = (int)"\""[0];
        public static readonly int eof = -1;
        public static readonly int a = (int)"a"[0];
        public static readonly int z = (int)"z"[0];
        public static readonly int A = (int)"A"[0];
        public static readonly int Z = (int)"Z"[0];
    }

    ///
    public Parser(string input)
    {
        buf = Encoding.ASCII.GetBytes(input);
        marker = 0;
    }

    ///
    public Value parse()
    {
        while (true)
        {
            int next = this.read();
            if (next == Tokens.eof)
            {
                break;
            }
            else if (isIdent(next))
            {
                unread();
                var ident = this.readIdent();
            }
        }

        return new Value();
    }

    private string readIdent()
    {
        var sb = new StringBuilder()

        while (true)
        {
            int next = this.read();
            if (next == Tokens.eof)
            {
                break;
            }
            else if (isIdent(next))
            {
                unread();
                var ident = this.readIdent();
            }
        }

        return sb.ToString();
    }

    private bool isIdent(int c)
    {
        return (c >= Tokens.a && c <= Tokens.z) || (c >= Tokens.A && c <= Tokens.Z);
    }

    private int read()
    {
        if (marker == buf.Length)
        {
            return Tokens.eof;
        }

        int ret = buf[marker];
        marker++;

        return ret;
    }

    private void unread()
    {
        marker--;
    }

    private int peek()
    {
        int ret = this.read();
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
