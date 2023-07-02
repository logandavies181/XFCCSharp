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
    }

    private static class Keys
    {
        public static readonly string By = "By";
        public static readonly string Hash = "Hash";
        public static readonly string Cert = "Cert";
        public static readonly string Chain = "Chain";
        public static readonly string Subject = "Subject";
        public static readonly string URI = "URI";
        public static readonly string DNS = "DNS";
    }

    ///
    public Parser(string input)
    {
        buf = Encoding.ASCII.GetBytes(input);
        marker = 0;
    }

    ///
    public Value Parse()
    {
        while (true)
        {
            int next = this.read();
            switch (next)
            {
                case Tokens.eof:
                    break;
            }
        }
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
public class Value
{
    ///
    public List<Element> elements = new List<Element>();
}

///
public class Element
{
    ///
    public readonly string? by;
    ///
    public readonly string? hash;
    ///
    public readonly string? cert;
    ///
    public readonly string? chain;
    ///
    public readonly string? subject;
    ///
    public readonly string? uri;
    ///
    public readonly string? dns;
}
