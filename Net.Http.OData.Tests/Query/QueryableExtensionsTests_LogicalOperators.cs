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
        public void Apply_Filter_Single_Property_Equals_Enum()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Colour eq Sample.Model.Colour'Blue'",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Colour == Colour.Blue).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(Colour.Blue, ((dynamic)x).Colour));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Equals_Int()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=ProductId eq 4",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Single(results);

            Assert.Equal(4, ((dynamic)results[0]).ProductId);
        }

        [Fact]
        public void Apply_Filter_Single_Property_Equals_Null()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Description eq null",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Empty(results);
        }

        [Fact]
        public void Apply_Filter_Single_Property_Equals_String()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Description eq 'iPhone SE 64GB Blue'",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Single(results);

            Assert.Equal(_products.Single(x => x.Description == "iPhone SE 64GB Blue").ProductId, ((dynamic)results[0]).ProductId);
        }

        [Fact]
        public void Apply_Filter_Single_Property_GreaterThan_And_Single_Property_Equals()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price gt 469.00M and Colour eq Sample.Model.Colour'Blue'",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price > 469.00M && x.Colour == Colour.Blue).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Price > 469.00M && ((dynamic)x).Colour == Colour.Blue));
        }

        [Fact]
        public void Apply_Filter_Single_Property_GreaterThan_Decimal()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price gt 469.00M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price > 469.00M).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Price > 469.00M));
        }

        [Fact]
        public void Apply_Filter_Single_Property_GreaterThan_Or_Single_Property_Equals()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price gt 469.00M or Colour eq Sample.Model.Colour'Blue'",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price > 469.00M || x.Colour == Colour.Blue).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Price > 469.00M || ((dynamic)x).Colour == Colour.Blue));
        }

        [Fact]
        public void Apply_Filter_Single_Property_GreaterThanOrEqual_Decimal()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price ge 469.00M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price >= 469.00M).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Price >= 469.00M));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Has_Enum()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=AccessLevel has Sample.Model.AccessLevel'Read,Write'",
                EntityDataModel.Current.EntitySets["Employees"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _employees.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_employees.Where(x => x.AccessLevel.HasFlag(AccessLevel.Read | AccessLevel.Write)).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).AccessLevel.HasFlag(AccessLevel.Read | AccessLevel.Write)));
        }

        [Fact]
        public void Apply_Filter_Single_Property_LessThan_Decimal()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price lt 469.00M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price < 469.00M).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Price < 469.00M));
        }

        [Fact]
        public void Apply_Filter_Single_Property_LessThanOrEqual_Decimal()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Price le 469.00M",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Price <= 469.00M).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Price <= 469.00M));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Not_Equals_String()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=not Description eq 'iPhone SE 64GB Blue'",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Description != "iPhone SE 64GB Blue").Count(), results.Count);

            Assert.All(results, x => Assert.NotEqual("iPhone SE 64GB Blue", ((dynamic)x).Description));
        }

        [Fact]
        public void Apply_Filter_Single_Property_NotEquals_Enum()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Colour ne Sample.Model.Colour'Blue'",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Colour != Colour.Blue).Count(), results.Count);

            Assert.All(results, x => Assert.NotEqual(Colour.Blue, ((dynamic)x).Colour));
        }

        [Fact]
        public void Apply_Filter_Single_Property_NotEquals_Int()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=ProductId ne 4",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Count - 1, results.Count);
        }

        [Fact]
        public void Apply_Filter_Single_Property_NotEquals_Null()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Description ne null",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Count, results.Count);
        }

        [Fact]
        public void Apply_Filter_Single_Property_NotEquals_String()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Description ne 'iPhone SE 64GB Blue'",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Description != "iPhone SE 64GB Blue").Count(), results.Count);

            Assert.All(results, x => Assert.NotEqual("iPhone SE 64GB Blue", ((dynamic)x).Description));
        }

        [Fact]
        public void Apply_Filter_Single_PropertyPath_Equals_String()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Category/Name eq 'Accessories'",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Category.Name == "Accessories").Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Description.Contains("Case")));
        }

        [Fact]
        public void Apply_Filter_Single_PropertyPath_NotEquals_String()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=Category/Name ne 'Accessories'",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Category.Name != "Accessories").Count(), results.Count);

            Assert.All(results, x => Assert.False(((dynamic)x).Description.Contains("Case")));
        }
    }
}
