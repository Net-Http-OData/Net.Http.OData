using System.Net;
using Net.Http.OData.Query.Expressions;
using Net.Http.OData.Query.Parsers;
using Xunit;

namespace Net.Http.OData.Tests.Query.Parsers
{
    public class UnaryOperatorKindParserTests
    {
        [Fact]
        public void ToUnaryOperatorKind_Throws_ODataException_For_UnsupportedOperatorKind()
        {
            ODataException odataException = Assert.Throws<ODataException>(() => "wibble".ToUnaryOperatorKind());

            Assert.Equal(ExceptionMessage.InvalidOperator("wibble"), odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
        }

        [Fact]
        public void ToUnaryOperatorKindReturnsNotForNot()
        {
            Assert.Equal(UnaryOperatorKind.Not, "not".ToUnaryOperatorKind());
        }
    }
}
