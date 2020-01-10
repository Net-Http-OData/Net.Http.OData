using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Net.Http.OData.Query.Expressions;
using Xunit;

namespace Net.Http.OData.Tests.Query.Expressions
{
    public class PropertyAccessNodeTests
    {
        public class WhenConstructed
        {
            private readonly PropertyAccessNode _node;
            private readonly PropertyPathSegment _propertyPathSegment;

            public WhenConstructed()
            {
                TestHelper.EnsureEDM();

                EdmComplexType model = EntityDataModel.Current.EntitySets["Customers"].EdmType;

                _propertyPathSegment = new PropertyPathSegment(model.GetProperty("CompanyName"));
                _node = new PropertyAccessNode(_propertyPathSegment);
            }

            [Fact]
            public void TheKindIsQueryNodeKindPropertyAccess()
            {
                Assert.Equal(QueryNodeKind.PropertyAccess, _node.Kind);
            }

            [Fact]
            public void ThePropertyPathIsSet()
            {
                Assert.Equal(_propertyPathSegment, _node.PropertyPath);
            }
        }
    }
}
