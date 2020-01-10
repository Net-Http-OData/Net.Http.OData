using System.Net.Http;
using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public class ODataQueryOptionsExtensionsTests
    {
        [Fact]
        public void NextLink_WithAllQueryOptions()
        {
            TestHelper.EnsureEDM();

            var _queryOptions = new ODataQueryOptions(
                new HttpRequestMessage(HttpMethod.Get, "http://services.odata.org/OData/Products?$count=true&$expand=Category&$filter=Name eq 'Milk'&$format=json&$orderby=Name&$search=blue OR green&$select=Name,Price$top=25"),
                EntityDataModel.Current.EntitySets["Products"]);

            Assert.Equal("http://services.odata.org/OData/Products?$skip=75&$count=true&$expand=Category&$filter=Name eq 'Milk'&$format=json&$orderby=Name&$search=blue OR green&$select=Name,Price$top=25", _queryOptions.NextLink(50, 25).ToString());
        }

        [Fact]
        public void NextLink_WithNoQueryOptions()
        {
            TestHelper.EnsureEDM();

            var _queryOptions = new ODataQueryOptions(
                new HttpRequestMessage(HttpMethod.Get, "http://services.odata.org/OData/Customers"),
                EntityDataModel.Current.EntitySets["Customers"]);

            Assert.Equal("http://services.odata.org/OData/Customers?$skip=75", _queryOptions.NextLink(50, 25).ToString());
        }
    }
}
