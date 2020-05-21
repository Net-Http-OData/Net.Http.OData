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
        public void Apply_Filter_Single_Property_Concat()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=concat(concat(Forename, ', '), Surname) eq 'Jess, Smith'",
                EntityDataModel.Current.EntitySets["Employees"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _employees.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_employees.Where(x => (x.Forename + ", " + x.Surname) == "Jess, Smith").Count(), results.Count);

            Assert.Single(results);

            Assert.All(results, x => Assert.True(((dynamic)x).Forename == "Jess" && ((dynamic)x).Surname == "Smith"));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Contains()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=contains(Description, 'Case')",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Description.Contains("Case")).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Description.Contains("Case")));
        }

        [Fact]
        public void Apply_Filter_Single_Property_EndsWith()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=endswith(Description, 'Blue')",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Description.EndsWith("Blue")).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Description.EndsWith("Blue")));
        }

        [Fact]
        public void Apply_Filter_Single_Property_IndexOf()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=indexof(Description, '64GB') eq 10",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Description.IndexOf("64GB") == 10).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Description.Contains("64GB")));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Length()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=length(Description) eq 19",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Description.Length == 19).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Description.Length == 19));
        }

        [Fact]
        public void Apply_Filter_Single_Property_StartsWith()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=startswith(Description, 'iPhone SE 64GB')",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Description.StartsWith("iPhone SE 64GB")).Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Description.StartsWith("iPhone SE 64GB")));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Substring()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=substring(Description, 7) eq 'SE 64GB Blue'",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.Description.Substring(7) == "SE 64GB Blue").Count(), results.Count);

            Assert.All(results, x => Assert.True(((dynamic)x).Description == "iPhone SE 64GB Blue"));
        }

        [Fact]
        public void Apply_Filter_Single_Property_ToLower()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=tolower(Forename) eq 'jess'",
                EntityDataModel.Current.EntitySets["Employees"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _employees.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_employees.Where(x => x.Forename.ToLower() == "jess").Count(), results.Count);

            Assert.Single(results);

            Assert.All(results, x => Assert.True(((dynamic)x).Forename == "Jess"));
        }

        [Fact]
        public void Apply_Filter_Single_Property_ToUpper()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=toupper(Forename) eq 'JESS'",
                EntityDataModel.Current.EntitySets["Employees"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _employees.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_employees.Where(x => x.Forename.ToUpper() == "JESS").Count(), results.Count);

            Assert.Single(results);

            Assert.All(results, x => Assert.True(((dynamic)x).Forename == "Jess"));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Trim()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=trim(Forename) eq 'Jess'",
                EntityDataModel.Current.EntitySets["Employees"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _employees.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_employees.Where(x => x.Forename.Trim() == "Jess").Count(), results.Count);

            Assert.Single(results);

            Assert.All(results, x => Assert.True(((dynamic)x).Forename == "Jess"));
        }
    }
}
