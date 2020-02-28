using System;
using Net.Http.OData.Model;
using Net.Http.OData.Query.Expressions;
using Xunit;

namespace Net.Http.OData.Tests.Query.Expressions
{
    public class BinaryOperatorNodeTests
    {
        [Fact]
        public void Constructor_Throws_ArgumentNullException_ForNullLeftNode()
            => Assert.Throws<ArgumentNullException>(() => new BinaryOperatorNode(null, BinaryOperatorKind.Equal, ConstantNode.False));

        public class WhenConstructed
        {
            private readonly BinaryOperatorKind _binaryOperatorKind = BinaryOperatorKind.Equal;
            private readonly QueryNode _left;
            private readonly BinaryOperatorNode _node;
            private readonly QueryNode _right = ConstantNode.String("'Alfreds Futterkiste'", "AlfredsFutterkiste");

            public WhenConstructed()
            {
                TestHelper.EnsureEDM();

                EdmComplexType model = EntityDataModel.Current.EntitySets["Customers"].EdmType;

                _left = new PropertyAccessNode(PropertyPath.For(model.GetProperty("CompanyName")));
                _node = new BinaryOperatorNode(_left, _binaryOperatorKind, _right);
            }

            [Fact]
            public void TheKindIsQueryNodeKindBinaryOperator()
            {
                Assert.Equal(QueryNodeKind.BinaryOperator, _node.Kind);
            }

            [Fact]
            public void TheLeftPropertyIsSet()
            {
                Assert.Equal(_left, _node.Left);
            }

            [Fact]
            public void TheOperatorKindIsSet()
            {
                Assert.Equal(_binaryOperatorKind, _node.OperatorKind);
            }

            [Fact]
            public void TheRightPropertyIsSet()
            {
                Assert.Equal(_right, _node.Right);
            }
        }
    }
}
