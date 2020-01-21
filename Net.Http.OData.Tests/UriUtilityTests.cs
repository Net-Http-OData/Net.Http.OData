using Xunit;

namespace Net.Http.OData.Tests
{
    public class UriUtilityTests
    {
        [Theory]
        [InlineData("/odata/Products")]
        [InlineData("/OData/Products")]
        [InlineData("/OData/Products/")]
        [InlineData("/OData/Products(1)")]
        [InlineData("/OData/Products(1)/Name/$value")]
        [InlineData("/OData/Products%281%29")]
        [InlineData("/OData/Products$select=Name")]
        public void ResolveEntitySetName_ReturnsEntitySetName(string path)
        {
            string entitySetName = UriUtility.ResolveEntitySetName(path);

            Assert.Equal("Products", entitySetName);
        }

        [Theory]
        [InlineData("/odata")]
        [InlineData("/api/Products")]
        public void ResolveEntitySetName_ReturnsNull(string path)
        {
            string entitySetName = UriUtility.ResolveEntitySetName(path);

            Assert.Null(entitySetName);
        }
    }
}
