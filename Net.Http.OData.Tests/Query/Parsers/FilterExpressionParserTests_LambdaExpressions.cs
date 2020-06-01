using Net.Http.OData.Model;
using Net.Http.OData.Query.Expressions;
using Net.Http.OData.Query.Parsers;
using Xunit;

namespace Net.Http.OData.Tests.Query.Parsers
{
    public partial class FilterExpressionParserTests
    {
        [Fact]
        public void ParseAllLambda()
        {
            TestHelper.EnsureEDM();

            QueryNode queryNode = FilterExpressionParser.Parse("$filter=OrderDetails/all(d:d/Quantity gt 100)", EntityDataModel.Current.EntitySets["Orders"].EdmType);

            Assert.NotNull(queryNode);
            Assert.IsType<LambdaOperatorNode>(queryNode);

            var node = (LambdaOperatorNode)queryNode;

            Assert.IsType<PropertyAccessNode>(node.Parameter);
            var nodeParameter = (PropertyAccessNode)node.Parameter;
            Assert.Equal("OrderDetails", nodeParameter.PropertyPath.Property.Name);

            Assert.Equal(LambdaOperatorKind.All, node.OperatorKind);
            Assert.Equal("d:d", node.Alias);

            Assert.IsType<BinaryOperatorNode>(node.Body);
            var nodeBody = (BinaryOperatorNode)node.Body;

            Assert.IsType<PropertyAccessNode>(nodeBody.Left);
            var nodeBodyLeft = (PropertyAccessNode)nodeBody.Left;
            Assert.Equal("Quantity", nodeBodyLeft.PropertyPath.Property.Name);

            Assert.Equal(BinaryOperatorKind.GreaterThan, nodeBody.OperatorKind);

            Assert.IsType<ConstantNode>(nodeBody.Right);
            var nodeBodyRight = (ConstantNode)nodeBody.Right;
            Assert.Equal(100, nodeBodyRight.Value);
        }

        [Fact]
        public void ParseAnyLambda()
        {
            TestHelper.EnsureEDM();

            QueryNode queryNode = FilterExpressionParser.Parse("$filter=OrderDetails/any(d:d/Quantity gt 100)", EntityDataModel.Current.EntitySets["Orders"].EdmType);

            Assert.NotNull(queryNode);
            Assert.IsType<LambdaOperatorNode>(queryNode);

            var node = (LambdaOperatorNode)queryNode;

            Assert.IsType<PropertyAccessNode>(node.Parameter);
            var nodeParameter = (PropertyAccessNode)node.Parameter;
            Assert.Equal("OrderDetails", nodeParameter.PropertyPath.Property.Name);

            Assert.Equal(LambdaOperatorKind.Any, node.OperatorKind);
            Assert.Equal("d:d", node.Alias);

            Assert.IsType<BinaryOperatorNode>(node.Body);
            var nodeBody = (BinaryOperatorNode)node.Body;

            Assert.IsType<PropertyAccessNode>(nodeBody.Left);
            var nodeBodyLeft = (PropertyAccessNode)nodeBody.Left;
            Assert.Equal("Quantity", nodeBodyLeft.PropertyPath.Property.Name);

            Assert.Equal(BinaryOperatorKind.GreaterThan, nodeBody.OperatorKind);

            Assert.IsType<ConstantNode>(nodeBody.Right);
            var nodeBodyRight = (ConstantNode)nodeBody.Right;
            Assert.Equal(100, nodeBodyRight.Value);
        }
    }
}
