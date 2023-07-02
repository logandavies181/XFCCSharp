namespace XFCC;
using System.Text;

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
        public static readonly char Eof = (char)0; // technically nul char, but good enough
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
