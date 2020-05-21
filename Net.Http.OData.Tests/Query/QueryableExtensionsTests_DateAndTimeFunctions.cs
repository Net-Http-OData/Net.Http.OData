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
        public void Apply_Filter_Single_Property_Date()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=date(Date) eq 2020-04-25",
                EntityDataModel.Current.EntitySets["Orders"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _orders.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_orders.Where(x => x.Date.Date == new DateTime(2020, 4, 25)).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(new DateTime(2020, 4, 25), ((dynamic)x).Date.Date));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Day_Date()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=day(ReleaseDate) eq 24",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.ReleaseDate.Day == 24).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(24, ((dynamic)x).ReleaseDate.Day));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Day_DateTimeOffset()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=day(Date) eq 25",
                EntityDataModel.Current.EntitySets["Orders"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _orders.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_orders.Where(x => x.Date.Day == 25).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(25, ((dynamic)x).Date.Day));
        }

        [Fact]
        public void Apply_Filter_Single_Property_FractionalSeconds_DateTimeOffset()
        {
        }

        [Fact]
        public void Apply_Filter_Single_Property_FractionalSeconds_TimeOfDay()
        {
        }

        [Fact]
        public void Apply_Filter_Single_Property_Hour_DateTimeOffset()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=hour(Date) eq 9",
                EntityDataModel.Current.EntitySets["Orders"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _orders.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_orders.Where(x => x.Date.Hour == 9).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(9, ((dynamic)x).Date.Hour));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Hour_TimeOfDay()
        {
        }

        [Fact]
        public void Apply_Filter_Single_Property_MaxDateTime()
        {
        }

        [Fact]
        public void Apply_Filter_Single_Property_MinDateTime()
        {
        }

        [Fact]
        public void Apply_Filter_Single_Property_Minute_DateTimeOffset()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=minute(Date) eq 30",
                EntityDataModel.Current.EntitySets["Orders"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _orders.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_orders.Where(x => x.Date.Minute == 30).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(30, ((dynamic)x).Date.Minute));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Minute_TimeOfDay()
        {
        }

        [Fact]
        public void Apply_Filter_Single_Property_Month_Date()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=month(ReleaseDate) eq 4",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.ReleaseDate.Month == 4).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(4, ((dynamic)x).ReleaseDate.Month));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Month_DateTimeOffset()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=month(Date) eq 4",
                EntityDataModel.Current.EntitySets["Orders"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _orders.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_orders.Where(x => x.Date.Month == 4).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(4, ((dynamic)x).Date.Month));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Now()
        {
        }

        [Fact]
        public void Apply_Filter_Single_Property_Second_DateTimeOffset()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=second(Date) eq 53",
                EntityDataModel.Current.EntitySets["Orders"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _orders.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_orders.Where(x => x.Date.Second == 53).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(53, ((dynamic)x).Date.Second));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Second_TimeOfDay()
        {
        }

        [Fact]
        public void Apply_Filter_Single_Property_Time()
        {
        }

        [Fact]
        public void Apply_Filter_Single_Property_TotalOffsetMinutes()
        {
        }

        [Fact]
        public void Apply_Filter_Single_Property_TotalSeconds()
        {
        }

        [Fact]
        public void Apply_Filter_Single_Property_Year_Date()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=year(ReleaseDate) eq 2020",
                EntityDataModel.Current.EntitySets["Products"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _products.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_products.Where(x => x.ReleaseDate.Year == 2020).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(2020, ((dynamic)x).ReleaseDate.Year));
        }

        [Fact]
        public void Apply_Filter_Single_Property_Year_DateTimeOffset()
        {
            TestHelper.EnsureEDM();

            var queryOptions = new ODataQueryOptions(
                "?$filter=year(Date) eq 2020",
                EntityDataModel.Current.EntitySets["Orders"],
                Mock.Of<IODataQueryOptionsValidator>());

            IList<ExpandoObject> results = _orders.AsQueryable().Apply(queryOptions).ToList();

            Assert.Equal(_orders.Where(x => x.Date.Year == 2020).Count(), results.Count);

            Assert.All(results, x => Assert.Equal(2020, ((dynamic)x).Date.Year));
        }
    }
}
