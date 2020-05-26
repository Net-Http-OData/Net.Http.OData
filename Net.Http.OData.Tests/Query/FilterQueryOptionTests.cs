using System;
using System.Net;
using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public class FilterQueryOptionTests
    {
        [Fact]
        public void Constructor_Throws_ArgumentNullException_ForNullModel()
            => Assert.Throws<ArgumentNullException>(() => new FilterQueryOption("$filter=CompanyName eq 'Alfreds Futterkiste'", null));

        [Fact]
        public void Constructor_Throws_ODataException_For_EmptyQueryOption()
        {
            TestHelper.EnsureEDM();

            ODataException odataException = Assert.Throws<ODataException>(() => new FilterQueryOption("$filter=", EntityDataModel.Current.EntitySets["Customers"].EdmType));

            Assert.Equal(ExceptionMessage.QueryOptionValueCannotBeEmpty(ODataUriNames.FilterQueryOption), odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            Assert.Equal(ODataUriNames.FilterQueryOption, odataException.Target);
        }

        public class WhenConstructedWithAValidValue
        {
            private readonly FilterQueryOption _option;
            private readonly string _rawValue;

            public WhenConstructedWithAValidValue()
            {
                TestHelper.EnsureEDM();

                EdmComplexType model = EntityDataModel.Current.EntitySets["Customers"].EdmType;

                _rawValue = "$filter=CompanyName eq 'Alfreds Futterkiste'";
                _option = new FilterQueryOption(_rawValue, model);
            }

            [Fact]
            public void TheExpressionShouldBeSet()
            {
                Assert.NotNull(_option.Expression);
            }

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
            {
                Assert.Equal(_rawValue, _option.RawValue);
            }
        }
    }
}
