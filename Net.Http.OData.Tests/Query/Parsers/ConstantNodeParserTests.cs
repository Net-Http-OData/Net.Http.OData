using System;
using Net.Http.OData.Model;
using Net.Http.OData.Query.Expressions;
using Net.Http.OData.Query.Parsers;
using NorthwindModel;
using Xunit;

namespace Net.Http.OData.Tests.Query.Parsers
{
    public class ConstantNodeParserTests
    {
        [Fact]
        public void Parse_False_Returns_ConstantNodeFalse()
            => Assert.Equal(ConstantNode.False, ConstantNodeParser.ParseConstantNode(new Token(TokenType.False, "false", 0)));

        [Fact]
        public void Parse_Int32_Zero_Returns_ConstantNodeInt32Zero()
            => Assert.Equal(ConstantNode.Int32Zero, ConstantNodeParser.ParseConstantNode(new Token(TokenType.Integer, "0", 0)));

        [Fact]
        public void Parse_Int64_Zero_Returns_ConstantNodeInt64Zero()
        {
            Assert.Equal(ConstantNode.Int64Zero, ConstantNodeParser.ParseConstantNode(new Token(TokenType.Integer, "0l", 0)));
            Assert.Equal(ConstantNode.Int64Zero, ConstantNodeParser.ParseConstantNode(new Token(TokenType.Integer, "0L", 0)));
        }

        [Fact]
        public void Parse_Null_Returns_ConstantNodeNull()
            => Assert.Equal(ConstantNode.Null, ConstantNodeParser.ParseConstantNode(new Token(TokenType.Null, "null", 0)));

        [Fact]
        public void Parse_True_Returns_ConstantNodeTrue()
            => Assert.Equal(ConstantNode.True, ConstantNodeParser.ParseConstantNode(new Token(TokenType.True, "true", 0)));

        [Fact]
        public void ParseConstantNode_Throws_NotSupportedException_For_UnsupportedTokenType()
        {
            NotSupportedException exception = Assert.Throws<NotSupportedException>(() => ConstantNodeParser.ParseConstantNode(new Token(TokenType.Comma, ",", 0)));

            Assert.Equal("Comma", exception.Message);
        }

        public class Parse_Base64Binary
        {
            private readonly ConstantNode<byte[]> _node;

            public Parse_Base64Binary()
            {
                // Base64 of Net.Http.OData
                _node = (ConstantNode<byte[]>)ConstantNodeParser.ParseConstantNode(new Token(TokenType.Base64Binary, "TmV0Lkh0dHAuT0RhdGE=", 0));
            }

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
                => Assert.Same(EdmPrimitiveType.Binary, _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
                => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet()
                => Assert.Equal("TmV0Lkh0dHAuT0RhdGE=", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet()
                => Assert.Equal(new byte[] { 78, 101, 116, 46, 72, 116, 116, 112, 46, 79, 68, 97, 116, 97 }, _node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue()
                => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }

        public class Parse_Date
        {
            private readonly ConstantNode<DateTime> _node;

            public Parse_Date()
                => _node = (ConstantNode<DateTime>)ConstantNodeParser.ParseConstantNode(new Token(TokenType.Date, "2000-12-18", 0));

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
                => Assert.Same(EdmPrimitiveType.Date, _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
                => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet()
                => Assert.Equal("2000-12-18", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet()
                => Assert.Equal(new DateTime(2000, 12, 18), _node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue()
                => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }

        public class Parse_DateTimeOffset
        {
            private readonly ConstantNode<DateTimeOffset> _node;

            public Parse_DateTimeOffset()
                => _node = (ConstantNode<DateTimeOffset>)ConstantNodeParser.ParseConstantNode(new Token(TokenType.DateTimeOffset, "2002-10-15T17:34:23Z", 0));

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
                => Assert.Same(EdmPrimitiveType.DateTimeOffset, _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
                => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet()
                => Assert.Equal("2002-10-15T17:34:23Z", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet()
                => Assert.Equal(new DateTimeOffset(2002, 10, 15, 17, 34, 23, TimeSpan.Zero), _node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue()
                => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }

        public class Parse_Decimal
        {
            private readonly ConstantNode<decimal> _node;

            public Parse_Decimal()
                => _node = (ConstantNode<decimal>)ConstantNodeParser.ParseConstantNode(new Token(TokenType.Decimal, "2.345M", 0));

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
                => Assert.Same(EdmPrimitiveType.Decimal, _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
                => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet()
                => Assert.Equal("2.345M", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet()
                => Assert.Equal(2.345M, _node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue()
                => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }

        public class Parse_Double
        {
            private readonly ConstantNode<double> _node;

            public Parse_Double()
                => _node = (ConstantNode<double>)ConstantNodeParser.ParseConstantNode(new Token(TokenType.Double, "2.029d", 0));

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
                => Assert.Same(EdmPrimitiveType.Double, _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
                => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet()
                => Assert.Equal("2.029d", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet()
                => Assert.Equal(2.029d, _node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue()
                => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }

        public class Parse_Duration
        {
            private readonly ConstantNode<TimeSpan> _node;

            public Parse_Duration()
                => _node = (ConstantNode<TimeSpan>)ConstantNodeParser.ParseConstantNode(new Token(TokenType.Duration, "duration'P6DT23H59M59.9999S'", 0));

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
                => Assert.Same(EdmPrimitiveType.Duration, _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
                => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet()
                => Assert.Equal("duration'P6DT23H59M59.9999S'", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet()
                => Assert.Equal(TimeSpan.Parse("6.23:59:59.9999"), _node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue()
                => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }

        public class Parse_EdmType
        {
            private readonly ConstantNode<EdmType> _node;

            public Parse_EdmType()
                => _node = (ConstantNode<EdmType>)ConstantNodeParser.ParseConstantNode(new Token(TokenType.EdmType, "Edm.String", 0));

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
                => Assert.Same(EdmPrimitiveType.String, _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
                => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet()
                => Assert.Equal("Edm.String", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet()
                => Assert.Same(EdmPrimitiveType.String, _node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue()
                => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }

        public class Parse_Enum
        {
            private readonly ConstantNode<AccessLevel> _node;

            public Parse_Enum()
            {
                TestHelper.EnsureEDM();

                _node = (ConstantNode<AccessLevel>)ConstantNodeParser.ParseConstantNode(new Token(TokenType.Enum, "NorthwindModel.AccessLevel'Read'", 0));
            }

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
                => Assert.Equal(EdmType.GetEdmType(typeof(AccessLevel)), _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
                => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet()
                => Assert.Equal("NorthwindModel.AccessLevel'Read'", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet()
                => Assert.Equal(AccessLevel.Read, _node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue()
                => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }

        public class Parse_Guid
        {
            private readonly ConstantNode<Guid> _node;

            public Parse_Guid()
                => _node = (ConstantNode<Guid>)ConstantNodeParser.ParseConstantNode(new Token(TokenType.Guid, "12345678-aaaa-bbbb-cccc-ddddeeeeffff", 0));

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
                => Assert.Same(EdmPrimitiveType.Guid, _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
                => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet()
                => Assert.Equal("12345678-aaaa-bbbb-cccc-ddddeeeeffff", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet()
                => Assert.Equal(new Guid("12345678-aaaa-bbbb-cccc-ddddeeeeffff"), _node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue()
                => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }

        public class Parse_Int32
        {
            private readonly ConstantNode<int> _node;

            public Parse_Int32()
                => _node = (ConstantNode<int>)ConstantNodeParser.ParseConstantNode(new Token(TokenType.Integer, "32", 0));

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
                => Assert.Same(EdmPrimitiveType.Int32, _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
                => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet()
                => Assert.Equal("32", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet()
                => Assert.Equal(32, _node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue()
                => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }

        public class Parse_Int64
        {
            private readonly ConstantNode<long> _node;

            public Parse_Int64()
                => _node = (ConstantNode<long>)ConstantNodeParser.ParseConstantNode(new Token(TokenType.Integer, "64L", 0));

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
                => Assert.Same(EdmPrimitiveType.Int64, _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
                => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet()
                => Assert.Equal("64L", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet()
                => Assert.Equal(64L, _node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue()
                => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }

        public class Parse_Single
        {
            private readonly ConstantNode<float> _node;

            public Parse_Single() =>
                _node = (ConstantNode<float>)ConstantNodeParser.ParseConstantNode(new Token(TokenType.Single, "2.0f", 0));

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
                => Assert.Same(EdmPrimitiveType.Single, _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
                => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet()
                => Assert.Equal("2.0f", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet()
                => Assert.Equal(2.0f, _node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue()
                => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }

        public class Parse_String
        {
            private readonly ConstantNode<string> _node;

            public Parse_String() =>
                _node = (ConstantNode<string>)ConstantNodeParser.ParseConstantNode(new Token(TokenType.String, "'Hello OData'", 0));

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
                => Assert.Same(EdmPrimitiveType.String, _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
                => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet()
                => Assert.Equal("'Hello OData'", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet()
                => Assert.Equal("Hello OData", _node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue()
                => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }

        public class Parse_TimeOfDay
        {
            private readonly ConstantNode<TimeSpan> _node;

            public Parse_TimeOfDay()
                => _node = (ConstantNode<TimeSpan>)ConstantNodeParser.ParseConstantNode(new Token(TokenType.TimeOfDay, "13:20:00", 0));

            [Fact]
            public void TheEdmPrimitiveTypeIsSet()
                => Assert.Same(EdmPrimitiveType.TimeOfDay, _node.EdmType);

            [Fact]
            public void TheKindIsQueryNodeKindConstant()
                => Assert.Equal(QueryNodeKind.Constant, _node.Kind);

            [Fact]
            public void TheLiteralTextPropertyIsSet()
                => Assert.Equal("13:20:00", _node.LiteralText);

            [Fact]
            public void TheValuePropertyIsSet()
                => Assert.Equal(new TimeSpan(13, 20, 0), _node.Value);

            [Fact]
            public void TheValuePropertyReturnsTheSameAsTheBaseValue()
                => Assert.Equal(_node.Value, ((ConstantNode)_node).Value);
        }
    }
}
