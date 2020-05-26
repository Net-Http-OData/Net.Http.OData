using System.Net;
using Net.Http.OData.Query;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public class SearchQueryOptionTests
    {
        [Fact]
        public void Constructor_Throws_ODataException_For_EmptyQueryOption()
        {
            TestHelper.EnsureEDM();

            ODataException odataException = Assert.Throws<ODataException>(() => new SearchQueryOption("$search="));

            Assert.Equal(ExceptionMessage.QueryOptionValueCannotBeEmpty(ODataUriNames.SearchQueryOption), odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            Assert.Equal(ODataUriNames.SearchQueryOption, odataException.Target);
        }

        public class WhenConstructedWithASearchExpression
        {
            private readonly SearchQueryOption _option;
            private readonly string _rawValue;

            public WhenConstructedWithASearchExpression()
            {
                _rawValue = "$search=blue OR green";
                _option = new SearchQueryOption(_rawValue);
            }

            [Fact]
            public void TheExpressionShouldBeSet()
            {
                Assert.Equal("blue OR green", _option.Expression);
            }

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
            {
                Assert.Equal(_rawValue, _option.RawValue);
            }
        }
    }
}
