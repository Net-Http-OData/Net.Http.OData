using System;
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

        public class DateTimeOffsetValue
        {
            private readonly ConstantNode _node;

            public DateTimeOffsetValue()
            {
                _node = ConstantNode.DateTimeOffset("2002-10-15T17:34:23Z", new DateTimeOffset(2002, 10, 15, 17, 34, 23, TimeSpan.Zero));
            }

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
            {
                Assert.Equal(EdmPrimitiveType.DateTimeOffset, _node.EdmType);
            }

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
            {
                Assert.Equal(QueryNodeKind.Constant, _node.Kind);
            }

            [Fact]
            public void TheLiteralTextPropertyIsSet()
            {
                Assert.Equal("2002-10-15T17:34:23Z", _node.LiteralText);
            }

            [Fact]
            public void TheValuePropertyIsSet()
            {
                Assert.IsType<DateTimeOffset>(_node.Value);
                Assert.Equal(new DateTimeOffset(2002, 10, 15, 17, 34, 23, TimeSpan.Zero), _node.Value);
            }
        }

        public class DateValue
        {
            private readonly ConstantNode _node;

            public DateValue()
            {
                _node = ConstantNode.Date("2000-12-18", new DateTime(2000, 12, 18));
            }

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
            {
                Assert.Equal(EdmPrimitiveType.Date, _node.EdmType);
            }

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
            {
                Assert.Equal(QueryNodeKind.Constant, _node.Kind);
            }

            [Fact]
            public void TheLiteralTextPropertyIsSet()
            {
                Assert.Equal("2000-12-18", _node.LiteralText);
            }

            [Fact]
            public void TheValuePropertyIsSet()
            {
                Assert.IsType<DateTime>(_node.Value);
                Assert.Equal(new DateTime(2000, 12, 18), _node.Value);
            }
        }

        public class DecimalValue
        {
            private readonly ConstantNode _node;

            public DecimalValue()
            {
                _node = ConstantNode.Decimal("2.345M", 2.345M);
            }

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
            {
                Assert.Equal(EdmPrimitiveType.Decimal, _node.EdmType);
            }

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
            {
                Assert.Equal(QueryNodeKind.Constant, _node.Kind);
            }

            [Fact]
            public void TheLiteralTextPropertyIsSet()
            {
                Assert.Equal("2.345M", _node.LiteralText);
            }

            [Fact]
            public void TheValuePropertyIsSet()
            {
                Assert.IsType<decimal>(_node.Value);
                Assert.Equal(2.345M, _node.Value);
            }
        }

        public class DoubleValue
        {
            private readonly ConstantNode _node;

            public DoubleValue()
            {
                _node = ConstantNode.Double("2.029d", 2.029d);
            }

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
            {
                Assert.Equal(EdmPrimitiveType.Double, _node.EdmType);
            }

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
            {
                Assert.Equal(QueryNodeKind.Constant, _node.Kind);
            }

            [Fact]
            public void TheLiteralTextPropertyIsSet()
            {
                Assert.Equal("2.029d", _node.LiteralText);
            }

            [Fact]
            public void TheValuePropertyIsSet()
            {
                Assert.IsType<double>(_node.Value);
                Assert.Equal(2.029d, _node.Value);
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

        public class GuidValue
        {
            private readonly ConstantNode _node;

            public GuidValue()
            {
                _node = ConstantNode.Guid("12345678-aaaa-bbbb-cccc-ddddeeeeffff", new Guid("12345678-aaaa-bbbb-cccc-ddddeeeeffff"));
            }

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
            {
                Assert.Equal(EdmPrimitiveType.Guid, _node.EdmType);
            }

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
            {
                Assert.Equal(QueryNodeKind.Constant, _node.Kind);
            }

            [Fact]
            public void TheLiteralTextPropertyIsSet()
            {
                Assert.Equal("12345678-aaaa-bbbb-cccc-ddddeeeeffff", _node.LiteralText);
            }

            [Fact]
            public void TheValuePropertyIsSet()
            {
                Assert.IsType<Guid>(_node.Value);
                Assert.Equal(new Guid("12345678-aaaa-bbbb-cccc-ddddeeeeffff"), _node.Value);
            }
        }

        public class Int32Value
        {
            private readonly ConstantNode _node;

            public Int32Value()
            {
                _node = ConstantNode.Int32("32", 32);
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
                Assert.Equal("32", _node.LiteralText);
            }

            [Fact]
            public void TheValuePropertyIsSet()
            {
                Assert.IsType<int>(_node.Value);
                Assert.Equal(32, _node.Value);
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

        public class Int64Value
        {
            private readonly ConstantNode _node;

            public Int64Value()
            {
                _node = ConstantNode.Int64("64L", 64L);
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
                Assert.Equal("64L", _node.LiteralText);
            }

            [Fact]
            public void TheValuePropertyIsSet()
            {
                Assert.IsType<long>(_node.Value);
                Assert.Equal(64L, _node.Value);
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

        public class SingleValue
        {
            private readonly ConstantNode _node;

            public SingleValue()
            {
                _node = ConstantNode.Single("2.0f", 2.0f);
            }

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
            {
                Assert.Equal(EdmPrimitiveType.Single, _node.EdmType);
            }

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
            {
                Assert.Equal(QueryNodeKind.Constant, _node.Kind);
            }

            [Fact]
            public void TheLiteralTextPropertyIsSet()
            {
                Assert.Equal("2.0f", _node.LiteralText);
            }

            [Fact]
            public void TheValuePropertyIsSet()
            {
                Assert.IsType<float>(_node.Value);
                Assert.Equal(2.0f, _node.Value);
            }
        }

        public class StringValue
        {
            private readonly ConstantNode _node;

            public StringValue()
            {
                _node = ConstantNode.String("'Hello OData'", "Hello OData");
            }

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
            {
                Assert.Equal(EdmPrimitiveType.String, _node.EdmType);
            }

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
            {
                Assert.Equal(QueryNodeKind.Constant, _node.Kind);
            }

            [Fact]
            public void TheLiteralTextPropertyIsSet()
            {
                Assert.Equal("'Hello OData'", _node.LiteralText);
            }

            [Fact]
            public void TheValuePropertyIsSet()
            {
                Assert.IsType<string>(_node.Value);
                Assert.Equal("Hello OData", _node.Value);
            }
        }

        public class TimeOfDayValue
        {
            private readonly ConstantNode _node;

            public TimeOfDayValue()
            {
                _node = ConstantNode.Time("13:20:00", new TimeSpan(13, 20, 0));
            }

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
            {
                Assert.Equal(EdmPrimitiveType.TimeOfDay, _node.EdmType);
            }

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
            {
                Assert.Equal(QueryNodeKind.Constant, _node.Kind);
            }

            [Fact]
            public void TheLiteralTextPropertyIsSet()
            {
                Assert.Equal("13:20:00", _node.LiteralText);
            }

            [Fact]
            public void TheValuePropertyIsSet()
            {
                Assert.IsType<TimeSpan>(_node.Value);
                Assert.Equal(new TimeSpan(13, 20, 0), _node.Value);
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
