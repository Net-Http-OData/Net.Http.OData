using System.Net;
using Net.Http.OData.Query.Expressions;
using Net.Http.OData.Query.Parsers;
using Xunit;

namespace Net.Http.OData.Tests.Query.Parsers
{
    public class LambdaOperatorKindParserTests
    {
        [Fact]
        public void ToLambdaOperatorKind_Throws_ODataException_For_UnsupportedOperatorKind()
        {
            ODataException odataException = Assert.Throws<ODataException>(() => "wibble".ToLambdaOperatorKind());

            Assert.Equal(ExceptionMessage.InvalidOperator("wibble"), odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            Assert.Equal(ODataUriNames.FilterQueryOption, odataException.Target);
        }

        [Fact]
        public void ToLambdaOperatorKindReturnsAllForAll() => Assert.Equal(LambdaOperatorKind.All, "all".ToLambdaOperatorKind());

        [Fact]
        public void ToLambdaOperatorKindReturnsAnyForAny() => Assert.Equal(LambdaOperatorKind.Any, "any".ToLambdaOperatorKind());
    }
}
