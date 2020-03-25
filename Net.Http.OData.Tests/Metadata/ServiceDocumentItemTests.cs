using System;
using Net.Http.OData.Metadata;
using Xunit;

namespace Net.Http.OData.Tests.Metadata
{
    public class ServiceDocumentItemTests
    {
        [Fact]
        public void Constructor_Throws_ArgumentException_ForEmpty_Name()
            => Assert.Throws<ArgumentException>(() => ServiceDocumentItem.EntitySet(" ", new Uri("Products", UriKind.Relative)));

        [Fact]
        public void Constructor_Throws_ArgumentException_ForNull_Name()
            => Assert.Throws<ArgumentException>(() => ServiceDocumentItem.EntitySet(null, new Uri("Products", UriKind.Relative)));

        [Fact]
        public void Constructor_Throws_ArgumentNullException_ForNull_Uri()
            => Assert.Throws<ArgumentNullException>(() => ServiceDocumentItem.EntitySet("Products", null));

        [Fact]
        public void EntitySet()
        {
            var uri = new Uri("Products", UriKind.Relative);
            var serviceDocumentItem = ServiceDocumentItem.EntitySet("Products", uri);

            Assert.Equal("EntitySet", serviceDocumentItem.Kind);
            Assert.Equal("Products", serviceDocumentItem.Name);
            Assert.Equal(uri, serviceDocumentItem.Url);
        }
    }
}
