using System.Net;
using Net.Http.OData.Query.Expressions;
using Net.Http.OData.Query.Parsers;
using Xunit;

namespace Net.Http.OData.Tests.Query.Parsers
{
    public class UnaryOperatorKindParserTests
    {
        [Fact]
        public void ToUnaryOperatorKindReturnsNotForNot()
        {
            Assert.Equal(UnaryOperatorKind.Not, "not".ToUnaryOperatorKind());
        }

        [Fact]
        public void ToUnaryOperatorKindThrowsArgumentExceptionForUnsupportedOperatorKind()
        {
            ODataException exception = Assert.Throws<ODataException>(() => "wibble".ToUnaryOperatorKind());

            Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
            Assert.Equal("The operator 'wibble' is not a valid OData operator.", exception.Message);
        }
    }
}
