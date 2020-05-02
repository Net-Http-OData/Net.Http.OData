using System;
using System.Net;
using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public class OrderByPropertyTests
    {
        [Fact]
        public void Constructor_Throws_ArgumentNullException_For_NullModel()
        {
            TestHelper.EnsureEDM();

            Assert.Throws<ArgumentNullException>(() => new OrderByProperty("CompanyName", null));
        }

        [Fact]
        public void Constructor_Throws_ArgumentNullException_For_NullRawValue()
        {
            TestHelper.EnsureEDM();

            Assert.Throws<ArgumentNullException>(() => new OrderByProperty(null, EntityDataModel.Current.EntitySets["Customers"].EdmType));
        }

        public class WhenConstructedWithAnIncorrectlyCasedValue
        {
            [Fact]
            public void AnODataExceptionShouldBeThrown()
            {
                TestHelper.EnsureEDM();

                EdmComplexType model = EntityDataModel.Current.EntitySets["Customers"].EdmType;

                ODataException odataException = Assert.Throws<ODataException>(() => new OrderByProperty("CompanyName ASC", model));

                Assert.Equal(ExceptionMessage.InvalidOrderByDirection("ASC", "CompanyName"), odataException.Message);
                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
                Assert.Equal("$orderby", odataException.Target);
            }
        }

        public class WhenConstructedWithAnInvalidValue
        {
            [Fact]
            public void AnArgumentOutOfRangeExceptionShouldBeThrown()
            {
                TestHelper.EnsureEDM();

                EdmComplexType model = EntityDataModel.Current.EntitySets["Customers"].EdmType;

                ODataException odataException = Assert.Throws<ODataException>(() => new OrderByProperty("CompanyName wibble", model));

                Assert.Equal(ExceptionMessage.InvalidOrderByDirection("wibble", "CompanyName"), odataException.Message);
                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
                Assert.Equal("$orderby", odataException.Target);
            }
        }

        public class WhenConstructedWithAsc
        {
            private readonly OrderByProperty _property;
            private readonly string _rawValue;

            public WhenConstructedWithAsc()
            {
                TestHelper.EnsureEDM();

                EdmComplexType model = EntityDataModel.Current.EntitySets["Customers"].EdmType;

                _rawValue = "CompanyName asc";
                _property = new OrderByProperty(_rawValue, model);
            }

            [Fact]
            public void TheDirectionShouldBeSetToAscending()
            {
                Assert.Equal(OrderByDirection.Ascending, _property.Direction);
            }

            [Fact]
            public void ThePropertyNameShouldBeSetToTheNameOfThePropertyPassedToTheConstructor()
            {
                Assert.Equal("CompanyName", _property.PropertyPath.Property.Name);
            }

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
            {
                Assert.Equal(_rawValue, _property.RawValue);
            }
        }

        public class WhenConstructedWithDesc
        {
            private readonly OrderByProperty _property;
            private readonly string _rawValue;

            public WhenConstructedWithDesc()
            {
                TestHelper.EnsureEDM();

                EdmComplexType model = EntityDataModel.Current.EntitySets["Customers"].EdmType;

                _rawValue = "CompanyName desc";
                _property = new OrderByProperty(_rawValue, model);
            }

            [Fact]
            public void TheDirectionShouldBeSetToDescending()
            {
                Assert.Equal(OrderByDirection.Descending, _property.Direction);
            }

            [Fact]
            public void ThePropertyNameShouldBeSetToTheNameOfThePropertyPassedToTheConstructor()
            {
                Assert.Equal("CompanyName", _property.PropertyPath.Property.Name);
            }

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
            {
                Assert.Equal(_rawValue, _property.RawValue);
            }
        }

        public class WhenConstructedWithoutADirection
        {
            private readonly OrderByProperty _property;
            private readonly string _rawValue;

            public WhenConstructedWithoutADirection()
            {
                TestHelper.EnsureEDM();

                EdmComplexType model = EntityDataModel.Current.EntitySets["Customers"].EdmType;

                _rawValue = "CompanyName";
                _property = new OrderByProperty(_rawValue, model);
            }

            [Fact]
            public void TheDirectionShouldDefaultToAscending()
            {
                Assert.Equal(OrderByDirection.Ascending, _property.Direction);
            }

            [Fact]
            public void ThePropertyNameShouldBeSetToTheNameOfThePropertyPassedToTheConstructor()
            {
                Assert.Equal("CompanyName", _property.PropertyPath.Property.Name);
            }

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
            {
                Assert.Equal(_rawValue, _property.RawValue);
            }
        }
    }
}
