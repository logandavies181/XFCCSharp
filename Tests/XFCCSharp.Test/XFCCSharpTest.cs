namespace XFCCSharp.Test;

using XFCCSharp;
using Xunit;

public class XFCCSharpTest
{
    // Test cases are examples from Envoy docs
    // https://www.envoyproxy.io/docs/envoy/latest/configuration/http/http_conn_man/headers#x-forwarded-client-cert

    [Fact]
    public void Parse_Case1()
    {
        var input = "By=http://frontend.lyft.com;Hash=468ed33be74eee6556d90c0149c1309e9ba61d6425303443c0748a02dd8de688;Subject=\"/C=US/ST=CA/L=San Francisco/OU=Lyft/CN=Test Client\";URI=http://testclient.lyft.com";
        var parser = new Parser(input);

        Assert.NotNull(parser);

        var value = parser.Parse();
        var elements = value.Elements;
        Assert.Single(elements);
        Assert.Equal("http://frontend.lyft.com", elements[0].By);
        Assert.Equal("468ed33be74eee6556d90c0149c1309e9ba61d6425303443c0748a02dd8de688", elements[0].Hash);
        Assert.Null(elements[0].Cert);
        Assert.Null(elements[0].Chain);
        Assert.Equal("/C=US/ST=CA/L=San Francisco/OU=Lyft/CN=Test Client", elements[0].Subject);
        Assert.Equal("http://testclient.lyft.com", elements[0].URI);
        Assert.Null(elements[0].DNS);
    }

    [Fact]
    public void Parse_Case2()
    {
        var input = "By=http://frontend.lyft.com;Hash=468ed33be74eee6556d90c0149c1309e9ba61d6425303443c0748a02dd8de688;URI=http://testclient.lyft.com,By=http://backend.lyft.com;Hash=9ba61d6425303443c0748a02dd8de688468ed33be74eee6556d90c0149c1309e;URI=http://frontend.lyft.com";
        var parser = new Parser(input);

        Assert.NotNull(parser);

        var value = parser.Parse();
        var elements = value.Elements;

        Assert.Equal(2, elements.Count);

        // element 1
        Assert.Equal("http://frontend.lyft.com", elements[0].By);
        Assert.Equal("468ed33be74eee6556d90c0149c1309e9ba61d6425303443c0748a02dd8de688", elements[0].Hash);
        Assert.Null(elements[0].Cert);
        Assert.Null(elements[0].Chain);
        Assert.Null(elements[0].Subject);
        Assert.Equal("http://testclient.lyft.com", elements[0].URI);
        Assert.Null(elements[0].DNS);

        // element 2
        Assert.Equal("http://backend.lyft.com", elements[1].By);
        Assert.Equal("9ba61d6425303443c0748a02dd8de688468ed33be74eee6556d90c0149c1309e", elements[1].Hash);
        Assert.Null(elements[1].Cert);
        Assert.Null(elements[1].Chain);
        Assert.Null(elements[1].Subject);
        Assert.Equal("http://frontend.lyft.com", elements[1].URI);
        Assert.Null(elements[1].DNS);
    }

    [Fact]
    public void Parse_CustomCase_QuotedDelimiters()
    {
        // by = `,;=\"`
        var input = "By=\",;=\\\"\"";
        var parser = new Parser(input);

        Assert.NotNull(parser);

        var value = parser.Parse();
        var elements = value.Elements;

        Assert.Single(elements);

        // element 1
        Assert.Equal(",;=\"", elements[0].By);
    }

    [Fact]
    public void Parse_CustomCase_EmptyValues()
    {
        var input = ",";
        var parser = new Parser(input);

        Assert.NotNull(parser);

        var value = parser.Parse();
        var elements = value.Elements;

        Assert.Equal(2, elements.Count);

        // element 1
        Assert.Null(elements[0].By);
        Assert.Null(elements[0].Hash);
        Assert.Null(elements[0].Cert);
        Assert.Null(elements[0].Chain);
        Assert.Null(elements[0].Subject);
        Assert.Null(elements[0].URI);
        Assert.Null(elements[0].DNS);
        //
        // element 1
        Assert.Null(elements[1].By);
        Assert.Null(elements[1].Hash);
        Assert.Null(elements[1].Cert);
        Assert.Null(elements[1].Chain);
        Assert.Null(elements[1].Subject);
        Assert.Null(elements[1].URI);
        Assert.Null(elements[1].DNS);
    }
}
