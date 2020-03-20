using System;
using Net.Http.OData.Model;
using Net.Http.OData.Query.Expressions;
using Net.Http.OData.Query.Parsers;
using Xunit;

namespace Net.Http.OData.Tests.Query.Parsers
{
    public partial class FilterExpressionParserTests
    {
        public class SingleValueFunctionCallTests
        {
            public SingleValueFunctionCallTests()
                => TestHelper.EnsureEDM();

            [Fact]
            public void ParseCastFunctionWithExpressionAndTypeExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("cast(Rating, Edm.Int64) eq 20", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("cast", nodeLeft.Name);
                Assert.Equal(2, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("Rating", nodeLeftParam0.PropertyPath.Property.Name);
                Assert.IsType<ConstantNode<EdmType>>(nodeLeft.Parameters[1]);
                var constantNode = (ConstantNode<EdmType>)nodeLeft.Parameters[1];
                Assert.Equal("Edm.Int64", constantNode.LiteralText);
                Assert.Equal(EdmPrimitiveType.Int64, constantNode.Value);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("20", nodeRight.LiteralText);
                Assert.Equal(20, nodeRight.Value);
            }

            [Fact]
            public void ParseCastFunctionWithTypeOnlyExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("cast(Edm.Int64)", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<FunctionCallNode>(queryNode);

                var node = (FunctionCallNode)queryNode;

                Assert.Equal("cast", node.Name);
                Assert.Equal(1, node.Parameters.Count);
                Assert.IsType<ConstantNode<EdmType>>(node.Parameters[0]);
                var constantNode = (ConstantNode<EdmType>)node.Parameters[0];
                Assert.Equal("Edm.Int64", constantNode.LiteralText);
                Assert.Equal(EdmPrimitiveType.Int64, constantNode.Value);
            }

            [Fact]
            public void ParseCeilingFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("ceiling(Freight) eq 32", EntityDataModel.Current.EntitySets["Orders"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("ceiling", nodeLeft.Name);
                Assert.Equal(1, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("Freight", nodeLeftParam0.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("32", nodeRight.LiteralText);
                Assert.Equal(32, nodeRight.Value);
            }

            [Fact]
            public void ParseConcatFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("concat(concat(City, ', '), Country) eq 'Berlin, Germany'", EntityDataModel.Current.EntitySets["Customers"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("concat", nodeLeft.Name);
                Assert.Equal(2, nodeLeft.Parameters.Count);
                Assert.IsType<FunctionCallNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (FunctionCallNode)nodeLeft.Parameters[0];
                Assert.Equal("concat", nodeLeftParam0.Name);
                Assert.Equal(2, nodeLeftParam0.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeftParam0.Parameters[0]);
                var nodeLeftParam0Param0 = (PropertyAccessNode)nodeLeftParam0.Parameters[0];
                Assert.Equal("City", nodeLeftParam0Param0.PropertyPath.Property.Name);
                Assert.IsType<ConstantNode<string>>(nodeLeftParam0.Parameters[1]);
                var nodeLeftParam0Param1 = (ConstantNode<string>)nodeLeftParam0.Parameters[1];
                Assert.Equal(", ", nodeLeftParam0Param1.Value);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[1]);
                var nodeLeftParam1 = (PropertyAccessNode)nodeLeft.Parameters[1];
                Assert.Equal("Country", nodeLeftParam1.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<string>>(node.Right);
                var nodeRight = (ConstantNode<string>)node.Right;
                Assert.Equal("'Berlin, Germany'", nodeRight.LiteralText);
                Assert.Equal("Berlin, Germany", nodeRight.Value);
            }

            [Fact]
            public void ParseContainsFunctionEqTrueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("contains(CompanyName,'Alfreds') eq true", EntityDataModel.Current.EntitySets["Customers"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("contains", nodeLeft.Name);
                Assert.Equal(2, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("CompanyName", nodeLeftParam0.PropertyPath.Property.Name);
                Assert.IsType<ConstantNode<string>>(nodeLeft.Parameters[1]);
                var nodeLeftParam1 = (ConstantNode<string>)nodeLeft.Parameters[1];
                Assert.Equal("'Alfreds'", nodeLeftParam1.LiteralText);
                Assert.Equal("Alfreds", nodeLeftParam1.Value);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<bool>>(node.Right);
                var nodeRight = (ConstantNode<bool>)node.Right;
                Assert.Equal("true", nodeRight.LiteralText);
                Assert.True(nodeRight.Value);
            }

            [Fact]
            public void ParseContainsFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("contains(CompanyName,'Alfreds')", EntityDataModel.Current.EntitySets["Customers"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<FunctionCallNode>(queryNode);

                var node = (FunctionCallNode)queryNode;

                Assert.Equal("contains", node.Name);
                Assert.Equal(2, node.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(node.Parameters[0]);
                var nodeParam0 = (PropertyAccessNode)node.Parameters[0];
                Assert.Equal("CompanyName", nodeParam0.PropertyPath.Property.Name);
                Assert.IsType<ConstantNode<string>>(node.Parameters[1]);
                var nodeParam1 = (ConstantNode<string>)node.Parameters[1];
                Assert.Equal("'Alfreds'", nodeParam1.LiteralText);
                Assert.Equal("Alfreds", nodeParam1.Value);
            }

            [Fact]
            public void ParseDateFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("date(Date) eq 2019-04-17", EntityDataModel.Current.EntitySets["Orders"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("date", nodeLeft.Name);
                Assert.Equal(1, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("Date", nodeLeftParam0.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<DateTime>>(node.Right);
                var nodeRight = (ConstantNode<DateTime>)node.Right;
                Assert.Equal("2019-04-17", nodeRight.LiteralText);
                Assert.Equal(new DateTime(2019, 4, 17), nodeRight.Value);
            }

            [Fact]
            public void ParseDayFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("day(BirthDate) eq 8", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("day", nodeLeft.Name);
                Assert.Equal(1, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("BirthDate", nodeLeftParam0.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("8", nodeRight.LiteralText);
                Assert.Equal(8, nodeRight.Value);
            }

            [Fact]
            public void ParseEndswithFunctionEqTrueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("endswith(CompanyName, 'Futterkiste') eq true", EntityDataModel.Current.EntitySets["Customers"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("endswith", nodeLeft.Name);
                Assert.Equal(2, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("CompanyName", nodeLeftParam0.PropertyPath.Property.Name);
                Assert.IsType<ConstantNode<string>>(nodeLeft.Parameters[1]);
                var nodeLeftParam1 = (ConstantNode<string>)nodeLeft.Parameters[1];
                Assert.Equal("'Futterkiste'", nodeLeftParam1.LiteralText);
                Assert.Equal("Futterkiste", nodeLeftParam1.Value);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<bool>>(node.Right);
                Assert.Same(ConstantNode.True, node.Right);
            }

            [Fact]
            public void ParseEndswithFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("endswith(CompanyName, 'Futterkiste')", EntityDataModel.Current.EntitySets["Customers"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<FunctionCallNode>(queryNode);

                var node = (FunctionCallNode)queryNode;

                Assert.Equal("endswith", node.Name);
                Assert.Equal(2, node.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(node.Parameters[0]);
                var nodeParam0 = (PropertyAccessNode)node.Parameters[0];
                Assert.Equal("CompanyName", nodeParam0.PropertyPath.Property.Name);
                Assert.IsType<ConstantNode<string>>(node.Parameters[1]);
                var nodeParam1 = (ConstantNode<string>)node.Parameters[1];
                Assert.Equal("'Futterkiste'", nodeParam1.LiteralText);
                Assert.Equal("Futterkiste", nodeParam1.Value);
            }

            [Fact]
            public void ParseFloorFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("floor(Freight) eq 32", EntityDataModel.Current.EntitySets["Orders"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("floor", nodeLeft.Name);
                Assert.Equal(1, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("Freight", nodeLeftParam0.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("32", nodeRight.LiteralText);
                Assert.Equal(32, nodeRight.Value);
            }

            [Fact]
            public void ParseFractionalSecondsFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("fractionalseconds(BirthDate) lt 0.1m", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("fractionalseconds", nodeLeft.Name);
                Assert.Equal(1, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("BirthDate", nodeLeftParam0.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.LessThan, node.OperatorKind);

                Assert.IsType<ConstantNode<decimal>>(node.Right);
                var nodeRight = (ConstantNode<decimal>)node.Right;
                Assert.Equal("0.1m", nodeRight.LiteralText);
                Assert.Equal(0.1m, nodeRight.Value);
            }

            [Fact]
            public void ParseHourFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("hour(BirthDate) eq 4", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("hour", nodeLeft.Name);
                Assert.Equal(1, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("BirthDate", nodeLeftParam0.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("4", nodeRight.LiteralText);
                Assert.Equal(4, nodeRight.Value);
            }

            [Fact]
            public void ParseIndexOfFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("indexof(CompanyName, 'lfreds') eq 1", EntityDataModel.Current.EntitySets["Customers"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("indexof", nodeLeft.Name);
                Assert.Equal(2, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("CompanyName", nodeLeftParam0.PropertyPath.Property.Name);
                Assert.IsType<ConstantNode<string>>(nodeLeft.Parameters[1]);
                var nodeLeftParam1 = (ConstantNode<string>)nodeLeft.Parameters[1];
                Assert.Equal("'lfreds'", nodeLeftParam1.LiteralText);
                Assert.Equal("lfreds", nodeLeftParam1.Value);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("1", nodeRight.LiteralText);
                Assert.Equal(1, nodeRight.Value);
            }

            [Fact]
            public void ParseIsOfFunctionWithExpressionAndTypeExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("isof(ShipCountry, Edm.String)", EntityDataModel.Current.EntitySets["Orders"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<FunctionCallNode>(queryNode);

                var node = (FunctionCallNode)queryNode;

                Assert.Equal("isof", node.Name);
                Assert.Equal(2, node.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(node.Parameters[0]);
                var nodeParam0 = (PropertyAccessNode)node.Parameters[0];
                Assert.Equal("ShipCountry", nodeParam0.PropertyPath.Property.Name);
                Assert.IsType<ConstantNode<EdmType>>(node.Parameters[1]);
                var nodeParam1 = (ConstantNode<EdmType>)node.Parameters[1];
                Assert.Equal("Edm.String", nodeParam1.LiteralText);
                Assert.Equal(EdmPrimitiveType.String, nodeParam1.Value);
            }

            [Fact]
            public void ParseIsOfFunctionWithTypeOnlyExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("isof(NorthwindModel.Order)", EntityDataModel.Current.EntitySets["Orders"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<FunctionCallNode>(queryNode);

                var node = (FunctionCallNode)queryNode;

                Assert.Equal("isof", node.Name);
                Assert.Equal(1, node.Parameters.Count);
                Assert.IsType<ConstantNode<EdmType>>(node.Parameters[0]);
                var nodeParam0 = (ConstantNode<EdmType>)node.Parameters[0];
                Assert.Equal("NorthwindModel.Order", nodeParam0.LiteralText);
                Assert.Equal(EdmType.GetEdmType(typeof(NorthwindModel.Order)), nodeParam0.Value);
            }

            [Fact]
            public void ParseLengthFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("length(CompanyName) eq 19", EntityDataModel.Current.EntitySets["Customers"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("length", nodeLeft.Name);
                Assert.Equal(1, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("CompanyName", nodeLeftParam0.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("19", nodeRight.LiteralText);
                Assert.Equal(19, nodeRight.Value);
            }

            [Fact]
            public void ParseMaxDateTimeFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("ReleaseDate eq maxdatetime()", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                var nodeLeft = (PropertyAccessNode)node.Left;
                Assert.Equal("ReleaseDate", nodeLeft.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<FunctionCallNode>(node.Right);
                Assert.Equal("maxdatetime", ((FunctionCallNode)node.Right).Name);
                Assert.Equal(0, ((FunctionCallNode)node.Right).Parameters.Count);
            }

            [Fact]
            public void ParseMinDateTimeFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("ReleaseDate eq mindatetime()", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                var nodeLeft = (PropertyAccessNode)node.Left;
                Assert.Equal("ReleaseDate", nodeLeft.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<FunctionCallNode>(node.Right);
                Assert.Equal("mindatetime", ((FunctionCallNode)node.Right).Name);
                Assert.Equal(0, ((FunctionCallNode)node.Right).Parameters.Count);
            }

            [Fact]
            public void ParseMinuteFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("minute(BirthDate) eq 40", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("minute", nodeLeft.Name);
                Assert.Equal(1, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("BirthDate", nodeLeftParam0.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("40", nodeRight.LiteralText);
                Assert.Equal(40, nodeRight.Value);
            }

            [Fact]
            public void ParseMonthFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("month(BirthDate) eq 5", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("month", nodeLeft.Name);
                Assert.Equal(1, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("BirthDate", nodeLeftParam0.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("5", nodeRight.LiteralText);
                Assert.Equal(5, nodeRight.Value);
            }

            [Fact]
            public void ParseNotEndsWithFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("not endswith(Description, 'ilk')", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<UnaryOperatorNode>(queryNode);

                var node = (UnaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Operand);
                var nodeOperand = (FunctionCallNode)node.Operand;
                Assert.Equal("endswith", nodeOperand.Name);
                Assert.Equal(2, nodeOperand.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeOperand.Parameters[0]);
                var nodeOperandParam0 = (PropertyAccessNode)nodeOperand.Parameters[0];
                Assert.Equal("Description", nodeOperandParam0.PropertyPath.Property.Name);
                Assert.IsType<ConstantNode<string>>(nodeOperand.Parameters[1]);
                var nodeOperandParam1 = (ConstantNode<string>)nodeOperand.Parameters[1];
                Assert.Equal("'ilk'", nodeOperandParam1.LiteralText);
                Assert.Equal("ilk", nodeOperandParam1.Value);

                Assert.Equal(UnaryOperatorKind.Not, node.OperatorKind);
            }

            [Fact]
            public void ParseNowFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("ReleaseDate ge now()", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<PropertyAccessNode>(node.Left);
                var nodeLeft = (PropertyAccessNode)node.Left;
                Assert.Equal("ReleaseDate", nodeLeft.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.GreaterThanOrEqual, node.OperatorKind);

                Assert.IsType<FunctionCallNode>(node.Right);
                Assert.Equal("now", ((FunctionCallNode)node.Right).Name);
                Assert.Equal(0, ((FunctionCallNode)node.Right).Parameters.Count);
            }

            [Fact]
            public void ParseRoundFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("round(Freight) eq 32", EntityDataModel.Current.EntitySets["Orders"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("round", nodeLeft.Name);
                Assert.Equal(1, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("Freight", nodeLeftParam0.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("32", nodeRight.LiteralText);
                Assert.Equal(32, nodeRight.Value);
            }

            [Fact]
            public void ParseSecondFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("second(BirthDate) eq 40", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("second", nodeLeft.Name);
                Assert.Equal(1, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("BirthDate", nodeLeftParam0.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("40", nodeRight.LiteralText);
                Assert.Equal(40, nodeRight.Value);
            }

            [Fact]
            public void ParseStartswithFunctionEqTrueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("startswith(CompanyName, 'Alfr') eq true", EntityDataModel.Current.EntitySets["Customers"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("startswith", nodeLeft.Name);
                Assert.Equal(2, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("CompanyName", nodeLeftParam0.PropertyPath.Property.Name);
                Assert.IsType<ConstantNode<string>>(nodeLeft.Parameters[1]);
                var nodeLeftParam1 = (ConstantNode<string>)nodeLeft.Parameters[1];
                Assert.Equal("'Alfr'", nodeLeftParam1.LiteralText);
                Assert.Equal("Alfr", nodeLeftParam1.Value);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<bool>>(node.Right);
                Assert.Same(ConstantNode.True, node.Right);
            }

            [Fact]
            public void ParseStartswithFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("startswith(CompanyName, 'Alfr')", EntityDataModel.Current.EntitySets["Customers"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<FunctionCallNode>(queryNode);

                var node = (FunctionCallNode)queryNode;

                Assert.Equal("startswith", node.Name);
                Assert.Equal(2, node.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(node.Parameters[0]);
                var nodeParam0 = (PropertyAccessNode)node.Parameters[0];
                Assert.Equal("CompanyName", nodeParam0.PropertyPath.Property.Name);
                Assert.IsType<ConstantNode<string>>(node.Parameters[1]);
                var nodeParam1 = (ConstantNode<string>)node.Parameters[1];
                Assert.Equal("'Alfr'", nodeParam1.LiteralText);
                Assert.Equal("Alfr", nodeParam1.Value);
            }

            [Fact]
            public void ParseSubstringFunctionExpressionWithOneArgument()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("substring(CompanyName, 1) eq 'lfreds Futterkiste'", EntityDataModel.Current.EntitySets["Customers"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("substring", nodeLeft.Name);
                Assert.Equal(2, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("CompanyName", nodeLeftParam0.PropertyPath.Property.Name);
                Assert.IsType<ConstantNode<int>>(nodeLeft.Parameters[1]);
                var nodeLeftParam1 = (ConstantNode<int>)nodeLeft.Parameters[1];
                Assert.Equal("1", nodeLeftParam1.LiteralText);
                Assert.Equal(1, nodeLeftParam1.Value);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<string>>(node.Right);
                var nodeRight = (ConstantNode<string>)node.Right;
                Assert.Equal("'lfreds Futterkiste'", nodeRight.LiteralText);
                Assert.Equal("lfreds Futterkiste", nodeRight.Value);
            }

            /// <summary>
            /// https://github.com/TrevorPilley/Net.Http.WebApi.OData/issues/57 - Nested function call parsing error.
            /// </summary>
            [Fact]
            public void ParseSubstringFunctionExpressionWithOneArgumentWhichIsAlsoAFunction()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("substring(tolower(CompanyName), 'alfreds futterkiste')", EntityDataModel.Current.EntitySets["Customers"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<FunctionCallNode>(queryNode);

                var node = (FunctionCallNode)queryNode;
                Assert.Equal("substring", node.Name);
                Assert.Equal(2, node.Parameters.Count);
                Assert.IsType<FunctionCallNode>(node.Parameters[0]);

                var nodeParam0 = (FunctionCallNode)node.Parameters[0];
                Assert.Equal("tolower", nodeParam0.Name);
                Assert.Equal(1, nodeParam0.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeParam0.Parameters[0]);
                var nodeParam0Param0 = (PropertyAccessNode)nodeParam0.Parameters[0];
                Assert.Equal("CompanyName", nodeParam0Param0.PropertyPath.Property.Name);

                Assert.IsType<ConstantNode<string>>(node.Parameters[1]);
                var nodeParam1 = (ConstantNode<string>)node.Parameters[1];
                Assert.Equal("'alfreds futterkiste'", nodeParam1.LiteralText);
                Assert.Equal("alfreds futterkiste", nodeParam1.Value);
            }

            [Fact]
            public void ParseSubstringFunctionExpressionWithTwoArguments()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("substring(CompanyName,1,2) eq 'lf'", EntityDataModel.Current.EntitySets["Customers"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("substring", nodeLeft.Name);
                Assert.Equal(3, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("CompanyName", nodeLeftParam0.PropertyPath.Property.Name);
                Assert.IsType<ConstantNode<int>>(nodeLeft.Parameters[1]);
                var nodeLeftParam1 = (ConstantNode<int>)nodeLeft.Parameters[1];
                Assert.Equal("1", nodeLeftParam1.LiteralText);
                Assert.Equal(1, nodeLeftParam1.Value);
                Assert.IsType<ConstantNode<int>>(nodeLeft.Parameters[2]);
                var nodeLeftParam2 = (ConstantNode<int>)nodeLeft.Parameters[2];
                Assert.Equal("2", nodeLeftParam2.LiteralText);
                Assert.Equal(2, nodeLeftParam2.Value);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<string>>(node.Right);
                var nodeRight = (ConstantNode<string>)node.Right;
                Assert.Equal("'lf'", nodeRight.LiteralText);
                Assert.Equal("lf", nodeRight.Value);
            }

            [Fact]
            public void ParseToLowerFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("tolower(CompanyName) eq 'alfreds futterkiste'", EntityDataModel.Current.EntitySets["Customers"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("tolower", nodeLeft.Name);
                Assert.Equal(1, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("CompanyName", nodeLeftParam0.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<string>>(node.Right);
                var nodeRight = (ConstantNode<string>)node.Right;
                Assert.Equal("'alfreds futterkiste'", nodeRight.LiteralText);
                Assert.Equal("alfreds futterkiste", nodeRight.Value);
            }

            [Fact]
            public void ParseToLowerFunctionPropertyPathExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("tolower(Category/Name) eq 'dairy'", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("tolower", nodeLeft.Name);
                Assert.Equal(1, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("Category", nodeLeftParam0.PropertyPath.Property.Name);
                Assert.Equal("Name", nodeLeftParam0.PropertyPath.Next.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<string>>(node.Right);
                var nodeRight = (ConstantNode<string>)node.Right;
                Assert.Equal("'dairy'", nodeRight.LiteralText);
                Assert.Equal("dairy", nodeRight.Value);
            }

            [Fact]
            public void ParseTotalOffsetMinutesFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("totaloffsetminutes(Date) eq 321541354", EntityDataModel.Current.EntitySets["Orders"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("totaloffsetminutes", nodeLeft.Name);
                Assert.Equal(1, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("Date", nodeLeftParam0.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("321541354", nodeRight.LiteralText);
                Assert.Equal(321541354, nodeRight.Value);
            }

            [Fact]
            public void ParseToUpperFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("toupper(CompanyName) eq 'ALFREDS FUTTERKISTE'", EntityDataModel.Current.EntitySets["Customers"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("toupper", nodeLeft.Name);
                Assert.Equal(1, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("CompanyName", nodeLeftParam0.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<string>>(node.Right);
                var nodeRight = (ConstantNode<string>)node.Right;
                Assert.Equal("'ALFREDS FUTTERKISTE'", nodeRight.LiteralText);
                Assert.Equal("ALFREDS FUTTERKISTE", nodeRight.Value);
            }

            [Fact]
            public void ParseTrimFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("trim(CompanyName) eq CompanyName", EntityDataModel.Current.EntitySets["Customers"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("trim", nodeLeft.Name);
                Assert.Equal(1, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("CompanyName", nodeLeftParam0.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<PropertyAccessNode>(node.Right);
                Assert.Equal("CompanyName", ((PropertyAccessNode)node.Right).PropertyPath.Property.Name);
            }

            [Fact]
            public void ParseYearFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("year(BirthDate) eq 1971", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("year", nodeLeft.Name);
                Assert.Equal(1, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                var nodeLeftParam0 = (PropertyAccessNode)nodeLeft.Parameters[0];
                Assert.Equal("BirthDate", nodeLeftParam0.PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("1971", nodeRight.LiteralText);
                Assert.Equal(1971, nodeRight.Value);
            }
        }
    }
}
