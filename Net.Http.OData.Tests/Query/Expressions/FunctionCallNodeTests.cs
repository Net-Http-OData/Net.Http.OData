using System;
using Net.Http.OData.Query.Expressions;
using Xunit;

namespace Net.Http.OData.Tests.Query.Expressions
{
    public class FunctionCallNodeTests
    {
        [Fact]
        public void Constructor_Throws_ArgumentException_ForNullName()
            => Assert.Throws<ArgumentException>(() => new FunctionCallNode(null));

        public class WhenAddingAParameter
        {
            private readonly FunctionCallNode _node;
            private readonly QueryNode _parameter = ConstantNode.String("Hi", "Hi");

            public WhenAddingAParameter()
            {
                _node = new FunctionCallNode("contains");
                _node.AddParameter(_parameter);
            }

            [Fact]
            public void TheParameterExistsInTheParameterCollection()
            {
                Assert.Contains(_parameter, _node.Parameters);
            }
        }

        public class WhenConstructed
        {
            private readonly string _functionName = "contains";
            private readonly FunctionCallNode _node;

            public WhenConstructed()
            {
                _node = new FunctionCallNode(_functionName);
            }

            [Fact]
            public void TheKindIsQueryNodeKindSingleValueFunctionCall()
            {
                Assert.Equal(QueryNodeKind.FunctionCall, _node.Kind);
            }

            [Fact]
            public void TheParametersCollectionIsEmpty()
            {
                Assert.Empty(_node.Parameters);
            }

            [Fact]
            public void TheParametersCollectionIsNotNull()
            {
                Assert.NotNull(_node.Parameters);
            }

            [Fact]
            public void ThePropertyNamePropertyIsSet()
            {
                Assert.Equal(_functionName, _node.Name);
            }
        }
    }
}
