using System;
using System.Net;
using Net.Http.OData.Query;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public class ODataRawQueryOptionsTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullException_ForNullHttpReuestMessage()
        {
            Assert.Throws<ArgumentNullException>(() => new ODataRawQueryOptions(null));
        }

        public class WhenCallingConstructorWithAllQueryOptions
        {
            private readonly ODataRawQueryOptions _rawQueryOptions;

            public WhenCallingConstructorWithAllQueryOptions()
            {
                _rawQueryOptions = new ODataRawQueryOptions(
                    "?$count=true&$expand=*&$filter=Name eq 'Fred'&$format=json&$orderby=Name&$search=blue OR green&$select=Name,Id&$skip=10&$skiptoken=5&$top=25");
            }

            [Fact]
            public void CountShouldBeSet()
            {
                Assert.Equal("$count=true", _rawQueryOptions.Count);
            }

            [Fact]
            public void ExpandShouldBeSet()
            {
                Assert.Equal("$expand=*", _rawQueryOptions.Expand);
            }

            [Fact]
            public void FilterShouldBeSet()
            {
                Assert.Equal("$filter=Name eq 'Fred'", _rawQueryOptions.Filter);
            }

            [Fact]
            public void FormatShouldBeSet()
            {
                Assert.Equal("$format=json", _rawQueryOptions.Format);
            }

            [Fact]
            public void OrderByShouldBeSet()
            {
                Assert.Equal("$orderby=Name", _rawQueryOptions.OrderBy);
            }

            [Fact]
            public void SearchShouldBeSet()
            {
                Assert.Equal("$search=blue OR green", _rawQueryOptions.Search);
            }

            [Fact]
            public void SelectShouldBeSet()
            {
                Assert.Equal("$select=Name,Id", _rawQueryOptions.Select);
            }

            [Fact]
            public void SkipShouldBeSet()
            {
                Assert.Equal("$skip=10", _rawQueryOptions.Skip);
            }

            [Fact]
            public void SkipTokenShouldBeSet()
            {
                Assert.Equal("$skiptoken=5", _rawQueryOptions.SkipToken);
            }

            [Fact]
            public void TopShouldBeSet()
            {
                Assert.Equal("$top=25", _rawQueryOptions.Top);
            }

            [Fact]
            public void ToStringShouldReturnTheOriginalQuery()
            {
                Assert.Equal(
                    "?$count=true&$expand=*&$filter=Name eq 'Fred'&$format=json&$orderby=Name&$search=blue OR green&$select=Name,Id&$skip=10&$skiptoken=5&$top=25",
                    _rawQueryOptions.ToString());
            }
        }

        public class WhenCallingConstructorWithAnEmptyCount
        {
            [Fact]
            public void AnHttpResponseExceptionShouldBeThrown()
            {
                ODataException exception = Assert.Throws<ODataException>(() => new ODataRawQueryOptions("?$count="));

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("The OData query option $count cannot be empty", exception.Message);
            }
        }

        public class WhenCallingConstructorWithAnEmptyExpand
        {
            [Fact]
            public void AnHttpResponseExceptionShouldBeThrown()
            {
                ODataException exception = Assert.Throws<ODataException>(() => new ODataRawQueryOptions("?$expand="));

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("The OData query option $expand cannot be empty", exception.Message);
            }
        }

        public class WhenCallingConstructorWithAnEmptyFilter
        {
            [Fact]
            public void AnHttpResponseExceptionShouldBeThrown()
            {
                ODataException exception = Assert.Throws<ODataException>(() => new ODataRawQueryOptions("?$filter="));

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("The OData query option $filter cannot be empty", exception.Message);
            }
        }

        public class WhenCallingConstructorWithAnEmptyFormat
        {
            [Fact]
            public void AnHttpResponseExceptionShouldBeThrown()
            {
                ODataException exception = Assert.Throws<ODataException>(() => new ODataRawQueryOptions("?$format="));

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("The OData query option $format cannot be empty", exception.Message);
            }
        }

        public class WhenCallingConstructorWithAnEmptyOrderBy
        {
            [Fact]
            public void AnHttpResponseExceptionShouldBeThrown()
            {
                ODataException exception = Assert.Throws<ODataException>(() => new ODataRawQueryOptions("?$orderby="));

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("The OData query option $orderby cannot be empty", exception.Message);
            }
        }

        public class WhenCallingConstructorWithAnEmptySearch
        {
            [Fact]
            public void AnHttpResponseExceptionShouldBeThrown()
            {
                ODataException exception = Assert.Throws<ODataException>(() => new ODataRawQueryOptions("?$search="));

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("The OData query option $search cannot be empty", exception.Message);
            }
        }

        public class WhenCallingConstructorWithAnEmptySelect
        {
            [Fact]
            public void AnHttpResponseExceptionShouldBeThrown()
            {
                ODataException exception = Assert.Throws<ODataException>(() => new ODataRawQueryOptions("?$select="));

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("The OData query option $select cannot be empty", exception.Message);
            }
        }

        public class WhenCallingConstructorWithAnEmptySkip
        {
            [Fact]
            public void AnHttpResponseExceptionShouldBeThrown()
            {
                ODataException exception = Assert.Throws<ODataException>(() => new ODataRawQueryOptions("?$skip="));

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("The OData query option $skip cannot be empty", exception.Message);
            }
        }

        public class WhenCallingConstructorWithAnEmptySkipToken
        {
            [Fact]
            public void AnHttpResponseExceptionShouldBeThrown()
            {
                ODataException exception = Assert.Throws<ODataException>(() => new ODataRawQueryOptions("?$skiptoken="));

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("The OData query option $skiptoken cannot be empty", exception.Message);
            }
        }

        public class WhenCallingConstructorWithAnEmptyTop
        {
            [Fact]
            public void AnHttpResponseExceptionShouldBeThrown()
            {
                ODataException exception = Assert.Throws<ODataException>(() => new ODataRawQueryOptions("?$top="));

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("The OData query option $top cannot be empty", exception.Message);
            }
        }

        public class WhenCallingConstructorWithAnUnknownQueryOptionWhichDoesNotStartsWithADollar
        {
            [Fact]
            public void AnExceptionShouldNotBeThrown()
            {
                new ODataRawQueryOptions("wibble=*");
            }
        }

        public class WhenCallingConstructorWithAnUnknownQueryOptionWhichStartsWithADollar
        {
            [Fact]
            public void AnHttpResponseExceptionShouldBeThrown()
            {
                ODataException exception = Assert.Throws<ODataException>(() => new ODataRawQueryOptions("?$wibble=*"));

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("Unknown OData query option $wibble", exception.Message);
            }
        }

        public class WhenCallingConstructorWithNoQueryOptions
        {
            private readonly ODataRawQueryOptions _rawQueryOptions;

            public WhenCallingConstructorWithNoQueryOptions()
            {
                _rawQueryOptions = new ODataRawQueryOptions(string.Empty);
            }

            [Fact]
            public void CountShouldBeNull()
            {
                Assert.Null(_rawQueryOptions.Count);
            }

            [Fact]
            public void ExpandShouldBeNull()
            {
                Assert.Null(_rawQueryOptions.Expand);
            }

            [Fact]
            public void FilterShouldBeNull()
            {
                Assert.Null(_rawQueryOptions.Filter);
            }

            [Fact]
            public void FormatShouldBeNull()
            {
                Assert.Null(_rawQueryOptions.Format);
            }

            [Fact]
            public void OrderByShouldBeNull()
            {
                Assert.Null(_rawQueryOptions.OrderBy);
            }

            [Fact]
            public void SearchShouldBeNull()
            {
                Assert.Null(_rawQueryOptions.Search);
            }

            [Fact]
            public void SelectShouldBeNull()
            {
                Assert.Null(_rawQueryOptions.Select);
            }

            [Fact]
            public void SkipShouldBeNull()
            {
                Assert.Null(_rawQueryOptions.Skip);
            }

            [Fact]
            public void SkipTokenShouldBeNull()
            {
                Assert.Null(_rawQueryOptions.SkipToken);
            }

            [Fact]
            public void TopShouldBeNull()
            {
                Assert.Null(_rawQueryOptions.Top);
            }

            [Fact]
            public void ToStringShouldReturnEmpty()
            {
                Assert.Equal(string.Empty, _rawQueryOptions.ToString());
            }
        }
    }
}
