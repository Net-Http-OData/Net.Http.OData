using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Moq;
using Net.Http.OData.Linq;
using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Sample.Model;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public partial class QueryableExtensionsTests
    {
        [Fact]
        public void Apply_Filter_All()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=OrderDetails/all(d:d/Quantity gt 100)",
                EntityDataModel.Current.EntitySets["Orders"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _orders.AsQueryable().Apply(queryOptions).ToList();

            var filteredOrders = _orders.Where(x => x.OrderDetails.All(od => od.Quantity > 100));

            Assert.Equal(filteredOrders.Count(), results.Count);

            Assert.All(results, x => Assert.Contains(filteredOrders, o => o.OrderId == ((dynamic)x).OrderId));
        }

        [Fact]
        public void Apply_Filter_Any()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=OrderDetails/any(d:d/Quantity gt 100)",
                EntityDataModel.Current.EntitySets["Orders"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _orders.AsQueryable().Apply(queryOptions).ToList();

            var filteredOrders = _orders.Where(x => x.OrderDetails.Any(od => od.Quantity > 100));

            Assert.Equal(filteredOrders.Count(), results.Count);

            Assert.All(results, x => Assert.Contains(filteredOrders, o => o.OrderId == ((dynamic)x).OrderId));
        }
    }
}
