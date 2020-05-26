using System;
using System.Net;
using Net.Http.OData.Query;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public class ODataRawQueryOptionsTests
    {
        [Fact]
        public void Constructor_DoesNotThrow_WithAnUnknownQueryOptionWhichDoesNotStartsWithADollar()
        {
            var odataRawQueryOptions = new ODataRawQueryOptions("wibble=*");
            Assert.NotNull(odataRawQueryOptions);
        }

        [Fact]
        public void Constructor_Throws_ArgumentNullException_For_Null_RawQuery()
        {
            Assert.Throws<ArgumentNullException>(() => new ODataRawQueryOptions(null));
        }

        [Fact]
        public void Constructor_Throws_ODataException_For_UnknownQueryOption()
        {
            ODataException odataException = Assert.Throws<ODataException>(() => new ODataRawQueryOptions("?$wibble=*"));

            Assert.Equal(ExceptionMessage.UnsupportedQueryOption("$wibble"), odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            Assert.Equal("$wibble", odataException.Target);
        }

        public class WhenCallingConstructorWithAllQueryOptions
        {
            private readonly ODataRawQueryOptions _rawQueryOptions;

            public WhenCallingConstructorWithAllQueryOptions()
                => _rawQueryOptions = new ODataRawQueryOptions("?$count=true&$expand=*&$filter=Name eq 'Fred'&$format=json&$orderby=Name&$search=blue OR green&$select=Name,Id&$skip=10&$skiptoken=5&$top=25");

            [Fact]
            public void CountShouldBeSet() => Assert.Equal("$count=true", _rawQueryOptions.Count);

            [Fact]
            public void ExpandShouldBeSet() => Assert.Equal("$expand=*", _rawQueryOptions.Expand);

            [Fact]
            public void FilterShouldBeSet() => Assert.Equal("$filter=Name eq 'Fred'", _rawQueryOptions.Filter);

            [Fact]
            public void FormatShouldBeSet() => Assert.Equal("$format=json", _rawQueryOptions.Format);

            [Fact]
            public void OrderByShouldBeSet() => Assert.Equal("$orderby=Name", _rawQueryOptions.OrderBy);

            [Fact]
            public void SearchShouldBeSet() => Assert.Equal("$search=blue OR green", _rawQueryOptions.Search);

            [Fact]
            public void SelectShouldBeSet() => Assert.Equal("$select=Name,Id", _rawQueryOptions.Select);

            [Fact]
            public void SkipShouldBeSet() => Assert.Equal("$skip=10", _rawQueryOptions.Skip);

            [Fact]
            public void SkipTokenShouldBeSet() => Assert.Equal("$skiptoken=5", _rawQueryOptions.SkipToken);

            [Fact]
            public void TopShouldBeSet() => Assert.Equal("$top=25", _rawQueryOptions.Top);

            [Fact]
            public void ToStringShouldReturnTheOriginalQuery()
            {
                Assert.Equal(
                    "?$count=true&$expand=*&$filter=Name eq 'Fred'&$format=json&$orderby=Name&$search=blue OR green&$select=Name,Id&$skip=10&$skiptoken=5&$top=25",
                    _rawQueryOptions.ToString());
            }
        }

        public class WhenCallingConstructorWithNoQueryOptions
        {
            private readonly ODataRawQueryOptions _rawQueryOptions;

            public WhenCallingConstructorWithNoQueryOptions()
                => _rawQueryOptions = new ODataRawQueryOptions(string.Empty);

            [Fact]
            public void CountShouldBeNull() => Assert.Null(_rawQueryOptions.Count);

            [Fact]
            public void ExpandShouldBeNull() => Assert.Null(_rawQueryOptions.Expand);

            [Fact]
            public void FilterShouldBeNull() => Assert.Null(_rawQueryOptions.Filter);

            [Fact]
            public void FormatShouldBeNull() => Assert.Null(_rawQueryOptions.Format);

            [Fact]
            public void OrderByShouldBeNull() => Assert.Null(_rawQueryOptions.OrderBy);

            [Fact]
            public void SearchShouldBeNull() => Assert.Null(_rawQueryOptions.Search);

            [Fact]
            public void SelectShouldBeNull() => Assert.Null(_rawQueryOptions.Select);

            [Fact]
            public void SkipShouldBeNull() => Assert.Null(_rawQueryOptions.Skip);

            [Fact]
            public void SkipTokenShouldBeNull() => Assert.Null(_rawQueryOptions.SkipToken);

            [Fact]
            public void TopShouldBeNull() => Assert.Null(_rawQueryOptions.Top);

            [Fact]
            public void ToStringShouldReturnEmpty() => Assert.Equal(string.Empty, _rawQueryOptions.ToString());
        }
    }
}
