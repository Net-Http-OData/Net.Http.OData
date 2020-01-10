using System;
using System.Collections.Generic;
using Net.Http.WebApi.OData;
using Xunit;

namespace Net.Http.OData.Tests
{
    public class ODataResponseContentTests
    {
        public class WhenConstructed
        {
            private readonly Uri _context = new Uri("http://services.odata.org/OData/$metadata#Products");
            private readonly int _count = 5;
            private readonly Uri _nextLink = new Uri("http://services.odata.org/OData/Products?$skip=5");
            private readonly ODataResponseContent _responseContent;
            private readonly List<int> _value = new List<int> { 1, 2, 3 };

            public WhenConstructed()
            {
                _responseContent = new ODataResponseContent(_context, _value, _count, _nextLink);
            }

            [Fact]
            public void TheContextIsSet()
            {
                Assert.Equal(_context, _responseContent.Context);
            }

            [Fact]
            public void TheCountIsSet()
            {
                Assert.Equal(_count, _responseContent.Count);
            }

            [Fact]
            public void TheNextLinkIsSet()
            {
                Assert.Equal(_nextLink, _responseContent.NextLink);
            }

            [Fact]
            public void TheValueIsSet()
            {
                Assert.Same(_value, _responseContent.Value);
            }
        }
    }
}
