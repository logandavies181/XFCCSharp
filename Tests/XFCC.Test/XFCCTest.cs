namespace XFCC.Test;

using XFCC;
using Xunit;

public class XFCCTest
{
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
}
