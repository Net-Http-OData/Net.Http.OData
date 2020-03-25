using System;
using Net.Http.OData.Model;
using Net.Http.OData.Query.Expressions;
using Net.Http.OData.Query.Parsers;
using NorthwindModel;
using Xunit;

namespace Net.Http.OData.Tests.Query.Parsers
{
    public partial class FilterExpressionParserTests
    {
        public class SingleValuePropertyValueTests
        {
            public SingleValuePropertyValueTests()
                => TestHelper.EnsureEDM();

            [Fact]
            public void ParseIgnoresTrailingWhitespace()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Deleted eq true ", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                var nodeLeft = (PropertyAccessNode)node.Left;
                Assert.Equal("Deleted", nodeLeft.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.Same(ConstantNode.True, node.Right);
            }

            [Fact]
            public void ParsePropertyAddValueEqValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Price add 2.45M eq 5.00M", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<BinaryOperatorNode>(node.Left);
                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeft.Left);
                Assert.Equal("Price", ((PropertyAccessNode)nodeLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Add, nodeLeft.OperatorKind);
                Assert.IsType<ConstantNode<decimal>>(nodeLeft.Right);
                var nodeLeftRight = (ConstantNode<decimal>)nodeLeft.Right;
                Assert.Equal("2.45M", nodeLeftRight.LiteralText);
                Assert.Equal(2.45M, nodeLeftRight.Value);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<decimal>>(node.Right);
                var nodeRight = (ConstantNode<decimal>)node.Right;
                Assert.Equal("5.00M", nodeRight.LiteralText);
                Assert.Equal(5.00M, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyDivValueEqValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Price div 2.55M eq 1M", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<BinaryOperatorNode>(node.Left);
                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeft.Left);
                Assert.Equal("Price", ((PropertyAccessNode)nodeLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Divide, nodeLeft.OperatorKind);
                Assert.IsType<ConstantNode<decimal>>(nodeLeft.Right);
                var nodeLeftRight = (ConstantNode<decimal>)nodeLeft.Right;
                Assert.Equal("2.55M", nodeLeftRight.LiteralText);
                Assert.Equal(2.55M, nodeLeftRight.Value);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<decimal>>(node.Right);
                var nodeRight = (ConstantNode<decimal>)node.Right;
                Assert.Equal("1M", nodeRight.LiteralText);
                Assert.Equal(1M, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqDateTimeOffset_DateHourMinute_ValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("ReleaseDate eq 2013-06-18T09:30", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("ReleaseDate", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<DateTimeOffset>>(node.Right);
                var nodeRight = (ConstantNode<DateTimeOffset>)node.Right;
                Assert.Equal("2013-06-18T09:30", nodeRight.LiteralText);
                Assert.Equal(DateTimeOffset.Parse("2013-06-18T09:30", ParserSettings.CultureInfo, ParserSettings.DateTimeStyles), nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqDateTimeOffset_DateHourMinuteSecond_ValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("ReleaseDate eq 2013-06-18T09:30:54", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("ReleaseDate", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<DateTimeOffset>>(node.Right);
                var nodeRight = (ConstantNode<DateTimeOffset>)node.Right;
                Assert.Equal("2013-06-18T09:30:54", nodeRight.LiteralText);
                Assert.Equal(DateTimeOffset.Parse("2013-06-18T09:30:54", ParserSettings.CultureInfo, ParserSettings.DateTimeStyles), nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqDateTimeOffset_MinusOffset_ValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("ReleaseDate eq 2002-10-15T17:34:23-02:00", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("ReleaseDate", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<DateTimeOffset>>(node.Right);
                var nodeRight = (ConstantNode<DateTimeOffset>)node.Right;
                Assert.Equal("2002-10-15T17:34:23-02:00", nodeRight.LiteralText);
                Assert.Equal(new DateTimeOffset(2002, 10, 15, 17, 34, 23, TimeSpan.FromHours(-2)), nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqDateTimeOffset_MomentJsIsoStringFormat_ValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("ReleaseDate eq 2013-02-04T22:44:30.652Z", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("ReleaseDate", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<DateTimeOffset>>(node.Right);
                var nodeRight = (ConstantNode<DateTimeOffset>)node.Right;
                Assert.Equal("2013-02-04T22:44:30.652Z", nodeRight.LiteralText);
                Assert.Equal(new DateTimeOffset(2013, 2, 4, 22, 44, 30, 652, TimeSpan.Zero), nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqDateTimeOffset_PlusOffset_ValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("ReleaseDate eq 2002-10-15T17:34:23+02:00", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("ReleaseDate", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<DateTimeOffset>>(node.Right);
                var nodeRight = (ConstantNode<DateTimeOffset>)node.Right;
                Assert.Equal("2002-10-15T17:34:23+02:00", nodeRight.LiteralText);
                Assert.Equal(new DateTimeOffset(2002, 10, 15, 17, 34, 23, TimeSpan.FromHours(2)), nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqDateTimeOffset_ToStringSFormat_ValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("ReleaseDate eq 2013-06-18T09:30:20", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("ReleaseDate", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<DateTimeOffset>>(node.Right);
                var nodeRight = (ConstantNode<DateTimeOffset>)node.Right;
                Assert.Equal("2013-06-18T09:30:20", nodeRight.LiteralText);
                Assert.Equal(DateTimeOffset.Parse("2013-06-18T09:30:20", ParserSettings.CultureInfo, ParserSettings.DateTimeStyles), nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqDateTimeOffset_Z_ValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("ReleaseDate eq 2002-10-15T17:34:23Z", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("ReleaseDate", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<DateTimeOffset>>(node.Right);
                var nodeRight = (ConstantNode<DateTimeOffset>)node.Right;
                Assert.Equal("2002-10-15T17:34:23Z", nodeRight.LiteralText);
                Assert.Equal(new DateTimeOffset(2002, 10, 15, 17, 34, 23, TimeSpan.Zero), nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqDateTimeOffset_ZeroOffset_ValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("ReleaseDate eq 2017-02-28T16:34:18", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("ReleaseDate", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<DateTimeOffset>>(node.Right);
                var nodeRight = (ConstantNode<DateTimeOffset>)node.Right;
                Assert.Equal("2017-02-28T16:34:18", nodeRight.LiteralText);
                Assert.Equal(DateTimeOffset.Parse("2017-02-28T16:34:18", ParserSettings.CultureInfo, ParserSettings.DateTimeStyles), nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqDateValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("ReleaseDate eq 2013-06-18", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("ReleaseDate", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<DateTime>>(node.Right);
                var nodeRight = (ConstantNode<DateTime>)node.Right;
                Assert.Equal("2013-06-18", nodeRight.LiteralText);
                Assert.Equal(new DateTime(2013, 6, 18, 0, 0, 0, DateTimeKind.Local), nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqEnumFlagsValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("AccessLevel has NorthwindModel.AccessLevel'Read,Write'", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("AccessLevel", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Has, node.OperatorKind);

                Assert.IsType<ConstantNode<AccessLevel>>(node.Right);
                var nodeRight = (ConstantNode<AccessLevel>)node.Right;
                Assert.Equal("NorthwindModel.AccessLevel'Read,Write'", nodeRight.LiteralText);
                Assert.Equal(AccessLevel.Read | AccessLevel.Write, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqEnumValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Colour eq NorthwindModel.Colour'Blue'", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Colour", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<Colour>>(node.Right);
                var nodeRight = (ConstantNode<Colour>)node.Right;
                Assert.Equal("NorthwindModel.Colour'Blue'", nodeRight.LiteralText);
                Assert.Equal(Colour.Blue, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqFalseValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Deleted eq false", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Deleted", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<bool>>(node.Right);
                Assert.Same(ConstantNode.False, node.Right);
            }

            [Fact]
            public void ParsePropertyEqGuidValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("TransactionId eq 0D01B09B-38CD-4C53-AA04-181371087A00", EntityDataModel.Current.EntitySets["Orders"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("TransactionId", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<Guid>>(node.Right);
                var nodeRight = (ConstantNode<Guid>)node.Right;
                Assert.Equal("0D01B09B-38CD-4C53-AA04-181371087A00", nodeRight.LiteralText);
                Assert.Equal(new Guid("0D01B09B-38CD-4C53-AA04-181371087A00"), nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqInt32ZeroValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Rating eq 0", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                Assert.Same(ConstantNode.Int32Zero, node.Right);
            }

            [Fact]
            public void ParsePropertyEqInt64ZeroValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Rating eq 0L", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<long>>(node.Right);
                Assert.Same(ConstantNode.Int64Zero, node.Right);
            }

            [Fact]
            public void ParsePropertyEqNegativeDecimalValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Price eq -1234.567M", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Price", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<decimal>>(node.Right);
                var nodeRight = (ConstantNode<decimal>)node.Right;
                Assert.Equal("-1234.567M", nodeRight.LiteralText);
                Assert.Equal(-1234.567M, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqNegativeDecimalWithNoDigitBeforeDecimalPointValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Price eq -.1M", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Price", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<decimal>>(node.Right);
                var nodeRight = (ConstantNode<decimal>)node.Right;
                Assert.Equal("-.1M", nodeRight.LiteralText);
                Assert.Equal(-.1M, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqNegativeDoubleValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Rating eq -1234.567D", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<double>>(node.Right);
                var nodeRight = (ConstantNode<double>)node.Right;
                Assert.Equal("-1234.567D", nodeRight.LiteralText);
                Assert.Equal(-1234.567D, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqNegativeDoubleWithExponentValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Rating eq -0.314e1", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<double>>(node.Right);
                var nodeRight = (ConstantNode<double>)node.Right;
                Assert.Equal("-0.314e1", nodeRight.LiteralText);
                Assert.Equal(-0.314e1D, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqNegativeDurationValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("ReleaseDate eq duration'-P6DT23H59M59.9999S'", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("ReleaseDate", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<TimeSpan>>(node.Right);
                var nodeRight = (ConstantNode<TimeSpan>)node.Right;
                Assert.Equal("duration'-P6DT23H59M59.9999S'", nodeRight.LiteralText);
                Assert.Equal(TimeSpan.Parse("-6.23:59:59.9999"), nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqNegativeFloatValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Rating eq -1234.567F", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<float>>(node.Right);
                var nodeRight = (ConstantNode<float>)node.Right;
                Assert.Equal("-1234.567F", nodeRight.LiteralText);
                Assert.Equal(-1234.567F, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqNegativeInt32MinValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Rating eq -2147483648", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("-2147483648", nodeRight.LiteralText);
                Assert.Equal(int.MinValue, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqNegativeInt32ValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Rating eq -1234", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("-1234", nodeRight.LiteralText);
                Assert.Equal(-1234, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqNegativeInt64MinValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Rating eq -9223372036854775808L", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<long>>(node.Right);
                var nodeRight = (ConstantNode<long>)node.Right;
                Assert.Equal("-9223372036854775808L", nodeRight.LiteralText);
                Assert.Equal(long.MinValue, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqNegativeInt64ValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Rating eq -1234L", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<long>>(node.Right);
                var nodeRight = (ConstantNode<long>)node.Right;
                Assert.Equal("-1234L", nodeRight.LiteralText);
                Assert.Equal(-1234L, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqNegativeInt64ValueWithoutSuffixExpression()
            {
                // -2147483649 is 1 less than int.MinValue so must be a long
                QueryNode queryNode = FilterExpressionParser.Parse("Rating eq -2147483649", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<long>>(node.Right);
                var nodeRight = (ConstantNode<long>)node.Right;
                Assert.Equal("-2147483649", nodeRight.LiteralText);
                Assert.Equal(-2147483649L, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqNullValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Description eq null", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Description", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<object>>(node.Right);
                Assert.Same(node.Right, ConstantNode.Null);
            }

            [Fact]
            public void ParsePropertyEqPositiveDecimalSignedValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Price eq +1234.567M", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Price", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<decimal>>(node.Right);
                var nodeRight = (ConstantNode<decimal>)node.Right;
                Assert.Equal("+1234.567M", nodeRight.LiteralText);
                Assert.Equal(1234.567M, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqPositiveDecimalValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Price eq 1234.567M", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Price", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<decimal>>(node.Right);
                var nodeRight = (ConstantNode<decimal>)node.Right;
                Assert.Equal("1234.567M", nodeRight.LiteralText);
                Assert.Equal(1234.567M, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqPositiveDecimalWithNoDigitBeforeDecimalPointValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Price eq .1M", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Price", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<decimal>>(node.Right);
                var nodeRight = (ConstantNode<decimal>)node.Right;
                Assert.Equal(".1M", nodeRight.LiteralText);
                Assert.Equal(.1M, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqPositiveDoubleSignedValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Price eq +1234.567D", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Price", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<double>>(node.Right);
                var nodeRight = (ConstantNode<double>)node.Right;
                Assert.Equal("+1234.567D", nodeRight.LiteralText);
                Assert.Equal(1234.567D, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqPositiveDoubleValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Price eq 1234.567D", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Price", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<double>>(node.Right);
                var nodeRight = (ConstantNode<double>)node.Right;
                Assert.Equal("1234.567D", nodeRight.LiteralText);
                Assert.Equal(1234.567D, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqPositiveDoubleWithExponentValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Rating eq 0.314e1", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<double>>(node.Right);
                var nodeRight = (ConstantNode<double>)node.Right;
                Assert.Equal("0.314e1", nodeRight.LiteralText);
                Assert.Equal(0.314e1D, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqPositiveDurationValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("ReleaseDate eq duration'P6DT23H59M59.9999S'", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("ReleaseDate", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<TimeSpan>>(node.Right);
                var nodeRight = (ConstantNode<TimeSpan>)node.Right;
                Assert.Equal("duration'P6DT23H59M59.9999S'", nodeRight.LiteralText);
                Assert.Equal(TimeSpan.Parse("6.23:59:59.9999"), nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqPositiveFloatSignedValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Rating eq +1234.567F", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<float>>(node.Right);
                var nodeRight = (ConstantNode<float>)node.Right;
                Assert.Equal("+1234.567F", nodeRight.LiteralText);
                Assert.Equal(1234.567F, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqPositiveFloatValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Rating eq 1234.567F", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<float>>(node.Right);
                var nodeRight = (ConstantNode<float>)node.Right;
                Assert.Equal("1234.567F", nodeRight.LiteralText);
                Assert.Equal(1234.567F, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqPositiveInt32MaxValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Rating eq 2147483647", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("2147483647", nodeRight.LiteralText);
                Assert.Equal(int.MaxValue, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqPositiveInt32SignedValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Rating eq +1234", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("+1234", nodeRight.LiteralText);
                Assert.Equal(1234, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqPositiveInt32ValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Rating eq 1234", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("1234", nodeRight.LiteralText);
                Assert.Equal(1234, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqPositiveInt64MaxValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Rating eq 9223372036854775807L", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<long>>(node.Right);
                var nodeRight = (ConstantNode<long>)node.Right;
                Assert.Equal("9223372036854775807L", nodeRight.LiteralText);
                Assert.Equal(long.MaxValue, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqPositiveInt64SignedValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Rating eq +1234L", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<long>>(node.Right);
                var nodeRight = (ConstantNode<long>)node.Right;
                Assert.Equal("+1234L", nodeRight.LiteralText);
                Assert.Equal(1234L, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqPositiveInt64ValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Rating eq 1234L", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<long>>(node.Right);
                var nodeRight = (ConstantNode<long>)node.Right;
                Assert.Equal("1234L", nodeRight.LiteralText);
                Assert.Equal(1234L, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqPositiveInt64ValueWithoutSuffixExpression()
            {
                // 2147483648 is 1 more than int.MaxValue so must be a long
                QueryNode queryNode = FilterExpressionParser.Parse("Rating eq 2147483648", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<long>>(node.Right);
                var nodeRight = (ConstantNode<long>)node.Right;
                Assert.Equal("2147483648", nodeRight.LiteralText);
                Assert.Equal(2147483648L, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqPropertyExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Name eq Description", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Name", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<PropertyAccessNode>(node.Right);
                Assert.Equal("Description", ((PropertyAccessNode)node.Right).PropertyPath.Property.Name);
            }

            [Fact]
            public void ParsePropertyEqStringFullCharacterSetValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse(@"Name eq 'ABCDEFGHIHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~!$&('')*+,;=@\/'", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Name", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<string>>(node.Right);
                var nodeRight = (ConstantNode<string>)node.Right;
                Assert.Equal(@"'ABCDEFGHIHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~!$&('')*+,;=@\/'", nodeRight.LiteralText);
                Assert.Equal(@"ABCDEFGHIHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~!$&(')*+,;=@\/", nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqStringValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Name eq 'Milk'", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Name", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<string>>(node.Right);
                var nodeRight = (ConstantNode<string>)node.Right;
                Assert.Equal("'Milk'", nodeRight.LiteralText);
                Assert.Equal("Milk", nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqStringWithQuoteCharacterValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("CompanyName eq 'O''Neil'", EntityDataModel.Current.EntitySets["Customers"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("CompanyName", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<string>>(node.Right);
                var nodeRight = (ConstantNode<string>)node.Right;
                Assert.Equal("'O''Neil'", nodeRight.LiteralText);
                Assert.Equal("O'Neil", nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqTimeOfDayHourMinuteSecondFractionalSecondValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("ReleaseDate eq 13:20:45.352", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("ReleaseDate", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<TimeSpan>>(node.Right);
                var nodeRight = (ConstantNode<TimeSpan>)node.Right;
                Assert.Equal("13:20:45.352", nodeRight.LiteralText);
                Assert.Equal(new TimeSpan(0, 13, 20, 45, 352), nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqTimeOfDayHourMinuteSecondValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("ReleaseDate eq 13:20:45", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("ReleaseDate", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<TimeSpan>>(node.Right);
                var nodeRight = (ConstantNode<TimeSpan>)node.Right;
                Assert.Equal("13:20:45", nodeRight.LiteralText);
                Assert.Equal(new TimeSpan(13, 20, 45), nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqTimeOfDayHourMinuteValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("ReleaseDate eq 13:20", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("ReleaseDate", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<TimeSpan>>(node.Right);
                var nodeRight = (ConstantNode<TimeSpan>)node.Right;
                Assert.Equal("13:20", nodeRight.LiteralText);
                Assert.Equal(new TimeSpan(13, 20, 0), nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyEqTrueValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Deleted eq true", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Deleted", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<bool>>(node.Right);
                Assert.Same(ConstantNode.True, node.Right);
            }

            [Fact]
            public void ParsePropertyGeThanIntValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Price ge 10", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Price", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.GreaterThanOrEqual, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("10", nodeRight.LiteralText);
                Assert.Equal(10, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyGtThanIntValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Price gt 20", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Price", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.GreaterThan, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("20", nodeRight.LiteralText);
                Assert.Equal(20, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyLeThanIntValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Price le 100", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Price", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.LessThanOrEqual, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("100", nodeRight.LiteralText);
                Assert.Equal(100, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyLtThanIntValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Price lt 20", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Price", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.LessThan, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("20", nodeRight.LiteralText);
                Assert.Equal(20, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyModValueEqPropertyExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Price mod 2 eq 0", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<BinaryOperatorNode>(node.Left);
                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeft.Left);
                Assert.Equal("Price", ((PropertyAccessNode)nodeLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Modulo, nodeLeft.OperatorKind);
                Assert.IsType<ConstantNode<int>>(nodeLeft.Right);
                var nodeLeftRight = (ConstantNode<int>)nodeLeft.Right;
                Assert.Equal("2", nodeLeftRight.LiteralText);
                Assert.Equal(2, nodeLeftRight.Value);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("0", nodeRight.LiteralText);
                Assert.Equal(0, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyMulValueEqValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Price mul 2.0M eq 5.10M", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<BinaryOperatorNode>(node.Left);
                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeft.Left);
                Assert.Equal("Price", ((PropertyAccessNode)nodeLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Multiply, nodeLeft.OperatorKind);
                Assert.IsType<ConstantNode<decimal>>(nodeLeft.Right);
                var nodeLeftRight = (ConstantNode<decimal>)nodeLeft.Right;
                Assert.Equal("2.0M", nodeLeftRight.LiteralText);
                Assert.Equal(2.0M, nodeLeftRight.Value);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<decimal>>(node.Right);
                var nodeRight = (ConstantNode<decimal>)node.Right;
                Assert.Equal("5.10M", nodeRight.LiteralText);
                Assert.Equal(5.10M, nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyNeStringValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Name ne 'Milk'", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                Assert.Equal("Name", ((PropertyAccessNode)node.Left).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.NotEqual, node.OperatorKind);

                Assert.IsType<ConstantNode<string>>(node.Right);
                var nodeRight = (ConstantNode<string>)node.Right;
                Assert.Equal("'Milk'", nodeRight.LiteralText);
                Assert.Equal("Milk", nodeRight.Value);
            }

            [Fact]
            public void ParsePropertyPathEqValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Category/Name eq 'Dairy'", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                var nodeLeft = (PropertyAccessNode)node.Left;
                Assert.Equal("Category", nodeLeft.PropertyPath.Property.Name);
                Assert.NotNull(nodeLeft.PropertyPath.Next);
                Assert.Equal("Name", nodeLeft.PropertyPath.Next.Property.Name);
                Assert.Null(nodeLeft.PropertyPath.Next.Next);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<string>>(node.Right);
                var nodeRight = (ConstantNode<string>)node.Right;
                Assert.Equal("'Dairy'", nodeRight.LiteralText);
                Assert.Equal("Dairy", nodeRight.Value);
            }

            [Fact]
            public void ParsePropertySubValueEqValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Price sub 0.55M eq 2.00M", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<BinaryOperatorNode>(node.Left);
                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeft.Left);
                Assert.Equal("Price", ((PropertyAccessNode)nodeLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Subtract, nodeLeft.OperatorKind);
                Assert.IsType<ConstantNode<decimal>>(nodeLeft.Right);
                var nodeLeftRight = (ConstantNode<decimal>)nodeLeft.Right;
                Assert.Equal("0.55M", nodeLeftRight.LiteralText);
                Assert.Equal(0.55M, nodeLeftRight.Value);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<decimal>>(node.Right);
                var nodeRight = (ConstantNode<decimal>)node.Right;
                Assert.Equal("2.00M", nodeRight.LiteralText);
                Assert.Equal(2.00M, nodeRight.Value);
            }
        }
    }
}
