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
        public void Constructor_ThrowsArgumentNullException_ForNullModel()
        {
            TestHelper.EnsureEDM();

            Assert.Throws<ArgumentNullException>(
                () => new OrderByProperty("CompanyName", null));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_ForNullRawValue()
        {
            TestHelper.EnsureEDM();

            Assert.Throws<ArgumentNullException>(
                () => new OrderByProperty(null, EntityDataModel.Current.EntitySets["Customers"].EdmType));
        }

        public class WhenConstructedWithAnIncorrectlyCasedValue
        {
            [Fact]
            public void AnArgumentOutOfRangeExceptionShouldBeThrown()
            {
                TestHelper.EnsureEDM();

                EdmComplexType model = EntityDataModel.Current.EntitySets["Customers"].EdmType;

                ODataException exception = Assert.Throws<ODataException>(() => new OrderByProperty("CompanyName ASC", model));

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("The supplied order value for CompanyName is invalid, valid options are 'asc' and 'desc'", exception.Message);
            }
        }

        public class WhenConstructedWithAnInvalidValue
        {
            [Fact]
            public void AnArgumentOutOfRangeExceptionShouldBeThrown()
            {
                TestHelper.EnsureEDM();

                EdmComplexType model = EntityDataModel.Current.EntitySets["Customers"].EdmType;

                ODataException exception = Assert.Throws<ODataException>(() => new OrderByProperty("CompanyName wibble", model));

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("The supplied order value for CompanyName is invalid, valid options are 'asc' and 'desc'", exception.Message);
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
