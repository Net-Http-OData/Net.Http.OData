using System;
using Moq;
using Net.Http.OData.Model;
using Net.Http.OData.Query.Expressions;
using Xunit;

namespace Net.Http.OData.Tests.Query.Expressions
{
    public class LambdaOperatorNodeTests
    {
        [Fact]
        public void Constructor_Throws_ArgumentNullException_ForNull_Alias()
            => Assert.Throws<ArgumentNullException>(() => new LambdaOperatorNode(Mock.Of<QueryNode>(), LambdaOperatorKind.All, null, Mock.Of<QueryNode>()));

        [Fact]
        public void Constructor_Throws_ArgumentNullException_ForNull_Body()
            => Assert.Throws<ArgumentNullException>(() => new LambdaOperatorNode(Mock.Of<QueryNode>(), LambdaOperatorKind.All, "d:d", null));

        [Fact]
        public void Constructor_Throws_ArgumentNullException_ForNull_Parameter()
            => Assert.Throws<ArgumentNullException>(() => new LambdaOperatorNode(null, LambdaOperatorKind.All, "d:d", Mock.Of<QueryNode>()));

        public class WhenConstructed
        {
            private readonly string _alias = "d:d";
            private readonly QueryNode _body;
            private readonly LambdaOperatorKind _lambdaOperatorKind = LambdaOperatorKind.Any;
            private readonly LambdaOperatorNode _node;
            private readonly QueryNode _parameter;

            public WhenConstructed()
            {
                TestHelper.EnsureEDM();

                EdmComplexType model = EntityDataModel.Current.EntitySets["Orders"].EdmType;

                _parameter = new PropertyAccessNode(PropertyPath.For(model.GetProperty("OrderDetails")));

                var edmComplexType = (EdmComplexType)((EdmCollectionType)model.GetProperty("OrderDetails").PropertyType).ContainedType;
                _body = new BinaryOperatorNode(new PropertyAccessNode(PropertyPath.For(edmComplexType.GetProperty("Quantity"))), BinaryOperatorKind.GreaterThan, ConstantNode.Int32("5", 5));

                _node = new LambdaOperatorNode(_parameter, _lambdaOperatorKind, _alias, _body);
            }

            [Fact]
            public void Alias_IsSet() => Assert.Equal(_alias, _node.Alias);

            [Fact]
            public void Body_IsSet() => Assert.Equal(_body, _node.Body);

            [Fact]
            public void Kind_Is_QueryNodeKind_LambdaOperator() => Assert.Equal(QueryNodeKind.LambdaOperator, _node.Kind);

            [Fact]
            public void OperatorKind_IsSet() => Assert.Equal(_lambdaOperatorKind, _node.OperatorKind);

            [Fact]
            public void Parameter_IsSet() => Assert.Equal(_parameter, _node.Parameter);
        }
    }
}
