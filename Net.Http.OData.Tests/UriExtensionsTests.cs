using System;
using Xunit;

namespace Net.Http.OData.Tests
{
    public class UriExtensionsTests
    {
        [Fact]
        public void ResolveODataServiceUri()
        {
            var requestUri = new Uri("http://services.odata.org/OData/Products");

            Assert.Equal("http://services.odata.org/OData/", requestUri.ResolveODataServiceUri().ToString());
        }

        [Fact]
        public void ResolveODataServiceUri_WithEntityKey()
        {
            var requestUri = new Uri("http://services.odata.org/OData/Products(1)");

            Assert.Equal("http://services.odata.org/OData/", requestUri.ResolveODataServiceUri().ToString());
        }

        [Fact]
        public void ResolveODataServiceUri_WithTrailingSlash()
        {
            var requestUri = new Uri("http://services.odata.org/OData/Products/");

            Assert.Equal("http://services.odata.org/OData/", requestUri.ResolveODataServiceUri().ToString());
        }
    }
}
