using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public class FilterQueryOptionTests
    {
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
