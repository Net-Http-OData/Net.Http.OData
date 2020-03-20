using Net.Http.OData.Model;
using Net.Http.OData.Query.Expressions;
using Xunit;

namespace Net.Http.OData.Tests.Query.Expressions
{
    public class ConstantNodeTests
    {
        [Fact]
        public void FalseValueIsSingleton()
            => Assert.Same(ConstantNode.False, ConstantNode.False);

        [Fact]
        public void Int32ZeroValueIsSingleton()
            => Assert.Same(ConstantNode.Int32Zero, ConstantNode.Int32Zero);

        [Fact]
        public void Int64ZeroValueIsSingleton()
            => Assert.Same(ConstantNode.Int64Zero, ConstantNode.Int64Zero);

        [Fact]
        public void NullValueIsSingleton()
            => Assert.Same(ConstantNode.Null, ConstantNode.Null);

        [Fact]
        public void TrueValueIsSingleton()
            => Assert.Same(ConstantNode.True, ConstantNode.True);

        public class ByteValue
        {
            private readonly ConstantNode<byte> _node;

            public ByteValue() => _node = ConstantNode.Byte("", 254);

            [Fact]
            public void TheEdmPrimitiveTypeIsSet() => Assert.Equal(EdmPrimitiveType.Byte, _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant() => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet() => Assert.Equal("", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet() => Assert.Equal((byte)254, _node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue() => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }

        public class FalseValue
        {
            private readonly ConstantNode<bool> _node;

            public FalseValue() => _node = ConstantNode.False;

            [Fact]
            public void TheEdmPrimitiveTypeIsSet() => Assert.Equal(EdmPrimitiveType.Boolean, _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant() => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet() => Assert.Equal("false", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet() => Assert.False(_node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue() => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }

        public class Int16Value
        {
            private readonly ConstantNode<short> _node;

            public Int16Value() => _node = ConstantNode.Int16("16", 16);

            [Fact]
            public void TheEdmPrimitiveTypeIsSet() => Assert.Equal(EdmPrimitiveType.Int16, _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant() => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet() => Assert.Equal("16", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet() => Assert.Equal((short)16, _node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue() => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }

        public class Int32ZeroValue
        {
            private readonly ConstantNode<int> _node;

            public Int32ZeroValue() => _node = ConstantNode.Int32Zero;

            [Fact]
            public void TheEdmPrimitiveTypeIsSet() => Assert.Equal(EdmPrimitiveType.Int32, _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant() => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet() => Assert.Equal("0", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet() => Assert.Equal(0, _node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue() => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }

        public class Int64ZeroValue
        {
            private readonly ConstantNode<long> _node;

            public Int64ZeroValue() => _node = ConstantNode.Int64Zero;

            [Fact]
            public void TheEdmPrimitiveTypeIsSet() => Assert.Equal(EdmPrimitiveType.Int64, _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant() => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet() => Assert.Equal("0L", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet() => Assert.Equal(0L, _node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue() => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }

        public class NullValue
        {
            private readonly ConstantNode<object> _node;

            public NullValue() => _node = ConstantNode.Null;

            [Fact]
            public void TheEdmPrimitiveTypeIsSet() => Assert.Null(_node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant() => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet() => Assert.Equal("null", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet() => Assert.Null(_node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue() => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }

        public class SByteValue
        {
            private readonly ConstantNode<sbyte> _node;

            public SByteValue() => _node = ConstantNode.SByte("", -24);

            [Fact]
            public void TheEdmPrimitiveTypeIsSet() => Assert.Equal(EdmPrimitiveType.SByte, _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant() => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet() => Assert.Equal("", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet() => Assert.Equal((sbyte)-24, _node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue() => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }

        public class TrueValue
        {
            private readonly ConstantNode<bool> _node;

            public TrueValue() => _node = ConstantNode.True;

            [Fact]
            public void TheEdmPrimitiveTypeIsSet() => Assert.Equal(EdmPrimitiveType.Boolean, _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant() => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet() => Assert.Equal("true", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet() => Assert.True(_node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue() => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }
    }
}
