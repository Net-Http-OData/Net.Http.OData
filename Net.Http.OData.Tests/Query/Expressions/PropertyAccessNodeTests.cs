using System;
using Net.Http.OData.Model;
using Net.Http.OData.Query.Expressions;
using Xunit;

namespace Net.Http.OData.Tests.Query.Expressions
{
    public class PropertyAccessNodeTests
    {
        [Fact]
        public void Constructor_Throws_ArgumentNullException_ForNullPropertyPath()
            => Assert.Throws<ArgumentNullException>(() => new PropertyAccessNode(null));

        public class WhenConstructed
        {
            private readonly PropertyAccessNode _node;
            private readonly PropertyPath _propertyPath;

            public WhenConstructed()
            {
                TestHelper.EnsureEDM();

                EdmComplexType model = EntityDataModel.Current.EntitySets["Customers"].EdmType;

                _propertyPath = PropertyPath.For(model.GetProperty("CompanyName"));
                _node = new PropertyAccessNode(_propertyPath);
            }

            [Fact]
            public void TheKindIsQueryNodeKindPropertyAccess()
            {
                Assert.Equal(QueryNodeKind.PropertyAccess, _node.Kind);
            }

            [Fact]
            public void ThePropertyPathIsSet()
            {
                Assert.Equal(_propertyPath, _node.PropertyPath);
            }
        }
    }
}
