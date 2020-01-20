using System;
using Xunit;

namespace Net.Http.OData.Tests
{
    public class UriExtensionsTests
    {
        [Fact]
        public void IsODataUri_False()
        {
            Assert.False(new Uri("http://services.odata.org/").IsODataUri());
        }

        [Fact]
        public void IsODataUri_True()
        {
            Assert.True(new Uri("http://services.odata.org/OData").IsODataUri());
            Assert.True(new Uri("http://services.odata.org/OData/Products").IsODataUri());
        }

        [Fact]
        public void ResolveODataEntitySetName()
        {
            var requestUri = new Uri("http://services.odata.org/OData/Products");

            Assert.Equal("Products", requestUri.ResolveODataEntitySetName());
        }

        [Fact]
        public void ResolveODataEntitySetName_ReturnsNull_IfNotODataUri()
        {
            var requestUri = new Uri("http://services.odata.org/api/Products");

            Assert.Null(requestUri.ResolveODataEntitySetName());
        }

        [Fact]
        public void ResolveODataEntitySetName_ReturnsNull_IfODataUriWithoutEntitySet()
        {
            var requestUri = new Uri("http://services.odata.org/OData");

            Assert.Null(requestUri.ResolveODataEntitySetName());
        }

        [Fact]
        public void ResolveODataEntitySetName_WithEntityKey()
        {
            var requestUri = new Uri("http://services.odata.org/OData/Products(1)");

            Assert.Equal("Products", requestUri.ResolveODataEntitySetName());
        }

        [Fact]
        public void ResolveODataEntitySetName_WithEntityKeyAndPropertyPath()
        {
            var requestUri = new Uri("http://services.odata.org/OData/Products(1)/Name/$value");

            Assert.Equal("Products", requestUri.ResolveODataEntitySetName());
        }

        [Fact]
        public void ResolveODataEntitySetName_WithEntityKeyUriEncoded()
        {
            var requestUri = new Uri("http://services.odata.org/OData/Products%281%29");

            Assert.Equal("Products", requestUri.ResolveODataEntitySetName());
        }

        [Fact]
        public void ResolveODataEntitySetName_WithODataQuery()
        {
            var requestUri = new Uri("http://services.odata.org/OData/Products$select=Name");

            Assert.Equal("Products", requestUri.ResolveODataEntitySetName());
        }

        [Fact]
        public void ResolveODataEntitySetName_WithTrailingSlash()
        {
            var requestUri = new Uri("http://services.odata.org/OData/Products/");

            Assert.Equal("Products", requestUri.ResolveODataEntitySetName());
        }

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
