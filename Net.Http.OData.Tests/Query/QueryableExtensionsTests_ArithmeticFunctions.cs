using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Moq;
using Net.Http.OData.Linq;
using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public partial class QueryableExtensionsTests
    {
        [Fact]
        public void Apply_Filter_Single_Property_Ceiling_Decimal()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=ceiling(Freight) eq 13",
                EntityDataModel.Current.EntitySets["Orders"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _orders.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_orders.Where(x => Math.Ceiling(x.Freight) == 13).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(13, Math.Ceiling(((dynamic)x).Freight)));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Ceiling_Double()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=ceiling(ShippingWeight) eq 9",
                EntityDataModel.Current.EntitySets["Orders"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _orders.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_orders.Where(x => Math.Ceiling(x.ShippingWeight) == 9).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(9, Math.Ceiling(((dynamic)x).ShippingWeight)));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Floor_Decimal()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=floor(Freight) eq 12",
                EntityDataModel.Current.EntitySets["Orders"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _orders.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_orders.Where(x => Math.Floor(x.Freight) == 12).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(12, Math.Floor(((dynamic)x).Freight)));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Floor_Double()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=floor(ShippingWeight) eq 8",
                EntityDataModel.Current.EntitySets["Orders"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _orders.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_orders.Where(x => Math.Floor(x.ShippingWeight) == 8).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(8, Math.Floor(((dynamic)x).ShippingWeight)));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Round_Decimal()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=round(Freight) eq 13",
                EntityDataModel.Current.EntitySets["Orders"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _orders.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_orders.Where(x => Math.Round(x.Freight) == 13).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(13, Math.Round(((dynamic)x).Freight)));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Round_Double()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=round(ShippingWeight) eq 9",
                EntityDataModel.Current.EntitySets["Orders"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _orders.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_orders.Where(x => Math.Round(x.ShippingWeight) == 9).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(9, Math.Round(((dynamic)x).ShippingWeight)));
        }
    }
}
