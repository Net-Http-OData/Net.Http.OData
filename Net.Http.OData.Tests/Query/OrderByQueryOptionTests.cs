﻿using System;
using System.Net;
using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public class OrderByQueryOptionTests
    {
        [Fact]
        public void Constructor_Throws_ArgumentNullException_For_NullModel()
            => Assert.Throws<ArgumentNullException>(() => new OrderByQueryOption("$orderby=Name", null));

        [Fact]
        public void Constructor_Throws_ODataException_For_EmptyQueryOption()
        {
            TestHelper.EnsureEDM();

            ODataException odataException = Assert.Throws<ODataException>(() => new OrderByQueryOption("$orderby=", EntityDataModel.Current.EntitySets["Products"].EdmType));

            Assert.Equal(ExceptionMessage.QueryOptionValueCannotBeEmpty(ODataUriNames.OrderByQueryOption), odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            Assert.Equal(ODataUriNames.OrderByQueryOption, odataException.Target);
        }

        public class WhenConstructedWithASingleValue
        {
            private readonly OrderByQueryOption _option;
            private readonly string _rawValue;

            public WhenConstructedWithASingleValue()
            {
                TestHelper.EnsureEDM();

                EdmComplexType model = EntityDataModel.Current.EntitySets["Products"].EdmType;

                _rawValue = "$orderby=Name";
                _option = new OrderByQueryOption(_rawValue, model);
            }

            [Fact]
            public void ThePropertiesShouldContainTheCorrectItems()
            {
                Assert.Equal(1, _option.Properties.Count);

                Assert.Equal("Name", _option.Properties[0].PropertyPath.Property.Name);
                Assert.Equal(OrderByDirection.Ascending, _option.Properties[0].Direction);
            }

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
            {
                Assert.Equal(_rawValue, _option.RawValue);
            }
        }

        public class WhenConstructedWithMultipleValues
        {
            private readonly OrderByQueryOption _option;
            private readonly string _rawValue;

            public WhenConstructedWithMultipleValues()
            {
                TestHelper.EnsureEDM();

                EdmComplexType model = EntityDataModel.Current.EntitySets["Products"].EdmType;

                _rawValue = "$orderby=Category/Name,Name,Price desc,Rating asc";
                _option = new OrderByQueryOption(_rawValue, model);
            }

            [Fact]
            public void ThePropertiesShouldContainTheCorrectItems()
            {
                Assert.Equal(4, _option.Properties.Count);

                Assert.Equal("Category", _option.Properties[0].PropertyPath.Property.Name);
                Assert.Equal("Name", _option.Properties[0].PropertyPath.Next.Property.Name);
                Assert.Equal(OrderByDirection.Ascending, _option.Properties[0].Direction);

                Assert.Equal("Name", _option.Properties[1].PropertyPath.Property.Name);
                Assert.Equal(OrderByDirection.Ascending, _option.Properties[1].Direction);

                Assert.Equal("Price", _option.Properties[2].PropertyPath.Property.Name);
                Assert.Equal(OrderByDirection.Descending, _option.Properties[2].Direction);

                Assert.Equal("Rating", _option.Properties[3].PropertyPath.Property.Name);
                Assert.Equal(OrderByDirection.Ascending, _option.Properties[3].Direction);
            }

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
            {
                Assert.Equal(_rawValue, _option.RawValue);
            }
        }
    }
}
