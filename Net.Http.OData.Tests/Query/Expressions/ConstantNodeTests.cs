using Net.Http.OData.Model;
using Net.Http.OData.Query.Expressions;
using Xunit;

namespace Net.Http.OData.Tests.Query.Expressions
{
    public class ConstantNodeTests
    {
        [Fact]
        public void FalseValueIsSingleton()
        {
            ConstantNode node1 = ConstantNode.False;
            ConstantNode node2 = ConstantNode.False;

            Assert.Same(node1, node2);
        }

        [Fact]
        public void Int32ZeroValueIsSingleton()
        {
            ConstantNode node1 = ConstantNode.Int32Zero;
            ConstantNode node2 = ConstantNode.Int32Zero;

            Assert.Same(node1, node2);
        }

        [Fact]
        public void Int64ZeroValueIsSingleton()
        {
            ConstantNode node1 = ConstantNode.Int64Zero;
            ConstantNode node2 = ConstantNode.Int64Zero;

            Assert.Same(node1, node2);
        }

        [Fact]
        public void NullValueIsSingleton()
        {
            ConstantNode node1 = ConstantNode.Null;
            ConstantNode node2 = ConstantNode.Null;

            Assert.Same(node1, node2);
        }

        [Fact]
        public void TrueValueIsSingleton()
        {
            ConstantNode node1 = ConstantNode.True;
            ConstantNode node2 = ConstantNode.True;

            Assert.Same(node1, node2);
        }

        public class ByteValue
        {
            private readonly ConstantNode _node;

            public ByteValue()
            {
                _node = ConstantNode.Byte("", 254);
            }

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
            {
                Assert.Equal(EdmPrimitiveType.Byte, _node.EdmType);
            }

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
            {
                Assert.Equal(QueryNodeKind.Constant, _node.Kind);
            }

            [Fact]
            public void TheLiteralTextPropertyIsSet()
            {
                Assert.Equal("", _node.LiteralText);
            }

            [Fact]
            public void TheValuePropertyIsSet()
            {
                Assert.IsType<byte>(_node.Value);
                Assert.Equal((byte)254, _node.Value);
            }
        }

        public class FalseValue
        {
            private readonly ConstantNode _node;

            public FalseValue()
            {
                _node = ConstantNode.False;
            }

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
            {
                Assert.Equal(EdmPrimitiveType.Boolean, _node.EdmType);
            }

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
            {
                Assert.Equal(QueryNodeKind.Constant, _node.Kind);
            }

            [Fact]
            public void TheLiteralTextPropertyIsSet()
            {
                Assert.Equal("false", _node.LiteralText);
            }

            [Fact]
            public void TheValuePropertyIsSet()
            {
                Assert.IsType<bool>(_node.Value);
                Assert.False((bool)_node.Value);
            }
        }

        public class Int16Value
        {
            private readonly ConstantNode _node;

            public Int16Value()
            {
                _node = ConstantNode.Int16("16", 16);
            }

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
            {
                Assert.Equal(EdmPrimitiveType.Int16, _node.EdmType);
            }

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
            {
                Assert.Equal(QueryNodeKind.Constant, _node.Kind);
            }

            [Fact]
            public void TheLiteralTextPropertyIsSet()
            {
                Assert.Equal("16", _node.LiteralText);
            }

            [Fact]
            public void TheValuePropertyIsSet()
            {
                Assert.IsType<short>(_node.Value);
                Assert.Equal((short)16, _node.Value);
            }
        }

        public class Int32ZeroValue
        {
            private readonly ConstantNode _node;

            public Int32ZeroValue()
            {
                _node = ConstantNode.Int32Zero;
            }

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
            {
                Assert.Equal(EdmPrimitiveType.Int32, _node.EdmType);
            }

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
            {
                Assert.Equal(QueryNodeKind.Constant, _node.Kind);
            }

            [Fact]
            public void TheLiteralTextPropertyIsSet()
            {
                Assert.Equal("0", _node.LiteralText);
            }

            [Fact]
            public void TheValuePropertyIsSet()
            {
                Assert.Equal(0, _node.Value);
            }
        }

        public class Int64ZeroValue
        {
            private readonly ConstantNode _node;

            public Int64ZeroValue()
            {
                _node = ConstantNode.Int64Zero;
            }

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
            {
                Assert.Equal(EdmPrimitiveType.Int64, _node.EdmType);
            }

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
            {
                Assert.Equal(QueryNodeKind.Constant, _node.Kind);
            }

            [Fact]
            public void TheLiteralTextPropertyIsSet()
            {
                Assert.Equal("0L", _node.LiteralText);
            }

            [Fact]
            public void TheValuePropertyIsSet()
            {
                Assert.Equal(0L, _node.Value);
            }
        }

        public class NullValue
        {
            private readonly ConstantNode _node;

            public NullValue()
            {
                _node = ConstantNode.Null;
            }

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
            {
                Assert.Null(_node.EdmType);
            }

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
            {
                Assert.Equal(QueryNodeKind.Constant, _node.Kind);
            }

            [Fact]
            public void TheLiteralTextPropertyIsSet()
            {
                Assert.Equal("null", _node.LiteralText);
            }

            [Fact]
            public void TheValuePropertyIsSet()
            {
                Assert.Null(_node.Value);
            }
        }

        public class SByteValue
        {
            private readonly ConstantNode _node;

            public SByteValue()
            {
                _node = ConstantNode.SByte("", -24);
            }

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
            {
                Assert.Equal(EdmPrimitiveType.SByte, _node.EdmType);
            }

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
            {
                Assert.Equal(QueryNodeKind.Constant, _node.Kind);
            }

            [Fact]
            public void TheLiteralTextPropertyIsSet()
            {
                Assert.Equal("", _node.LiteralText);
            }

            [Fact]
            public void TheValuePropertyIsSet()
            {
                Assert.IsType<sbyte>(_node.Value);
                Assert.Equal((sbyte)-24, _node.Value);
            }
        }

        public class TrueValue
        {
            private readonly ConstantNode _node;

            public TrueValue()
            {
                _node = ConstantNode.True;
            }

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
            {
                Assert.Equal(EdmPrimitiveType.Boolean, _node.EdmType);
            }

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
            {
                Assert.Equal(QueryNodeKind.Constant, _node.Kind);
            }

            [Fact]
            public void TheLiteralTextPropertyIsSet()
            {
                Assert.Equal("true", _node.LiteralText);
            }

            [Fact]
            public void TheValuePropertyIsSet()
            {
                Assert.IsType<bool>(_node.Value);
                Assert.True((bool)_node.Value);
            }
        }
    }
}
