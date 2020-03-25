using System;
using Net.Http.OData.Model;
using Net.Http.OData.Query.Expressions;
using Xunit;

namespace Net.Http.OData.Tests.Query.Expressions
{
    public class UnaryOperatorNodeTests
    {
        [Fact]
        public void Constructor_Throws_ArgumentNullException_ForNullPropertyPath()
            => Assert.Throws<ArgumentNullException>(() => new UnaryOperatorNode(null, UnaryOperatorKind.Not));

        public class WhenConstructed
        {
            private readonly UnaryOperatorNode _node;
            private readonly QueryNode _operand;
            private readonly UnaryOperatorKind _unaryOperatorKind = UnaryOperatorKind.Not;

            public WhenConstructed()
            {
                TestHelper.EnsureEDM();

                EdmComplexType model = EntityDataModel.Current.EntitySets["Customers"].EdmType;

                _operand = new PropertyAccessNode(PropertyPath.For(model.GetProperty("CompanyName")));
                _node = new UnaryOperatorNode(_operand, _unaryOperatorKind);
            }

            [Fact]
            public void TheKindIsQueryNodeKindUnaryOperator()
            {
                Assert.Equal(QueryNodeKind.UnaryOperator, _node.Kind);
            }

            [Fact]
            public void TheOperandPropertyIsSet()
            {
                Assert.Equal(_operand, _node.Operand);
            }

            [Fact]
            public void TheOperatorKindIsSet()
            {
                Assert.Equal(_unaryOperatorKind, _node.OperatorKind);
            }
        }
    }
}
