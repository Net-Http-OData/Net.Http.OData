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
        public void Apply_Filter_Single_Property_Add_Equals()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price add 11.00M eq 50M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price + 11.00M == 50M).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(39.00M, ((dynamic)x).Price));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Add_Equals_Implicit_Cast()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price add 11 eq 50M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price + 11 == 50M).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(39.00M, ((dynamic)x).Price));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Divide_Equals()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price div 2M eq 19.5M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price / 2 == 19.5M).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(39.00M, ((dynamic)x).Price));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Divide_Equals_Implicit_Cast()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price div 2 eq 19.5M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price / 2 == 19.5M).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(39.00M, ((dynamic)x).Price));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Modulo_Equals()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Rating mod 2f eq 0.68f",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Rating % 2 == 0.68f).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(39.00M, ((dynamic)x).Price));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Modulo_Equals_Implicit_Cast()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Rating mod 2 eq 0.68f",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Rating % 2 == 0.68f).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(39.00M, ((dynamic)x).Price));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Multiply_Equals()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price mul 2M eq 78M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price * 2 == 78M).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(39.00M, ((dynamic)x).Price));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Multiply_Equals_Implicit_Cast()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price mul 2 eq 78M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price * 2 == 78M).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(39.00M, ((dynamic)x).Price));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Subtract_Equals()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price sub 11M eq 28M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price - 11M == 28M).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(39M, ((dynamic)x).Price));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Subtract_Equals_Implicit_Cast()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price sub 11 eq 28M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price - 11 == 28M).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(39M, ((dynamic)x).Price));
        }
    }
}
