﻿using System;
using Net.Http.OData.Model;
using Net.Http.OData.Query.Expressions;
using Net.Http.OData.Query.Parsers;
using Xunit;

namespace Net.Http.OData.Tests.Query.Parsers
{
    public partial class FilterExpressionParserTests
    {
        public class ComplexExpressionTests
        {
            public ComplexExpressionTests()
            {
                TestHelper.EnsureEDM();
            }

            /// <summary>
            /// https://github.com/TrevorPilley/Net.Http.WebApi.OData/issues/55 - Grouped parsing error
            /// </summary>
            [Fact]
            public void Parse_GroupedA_AndGroupedBandC_AndGroupedD()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("(Rating eq 5) and (ReleaseDate ge 2015-02-06T00:00:00 and ReleaseDate le 2015-02-06T23:59:59) and (Price eq 150.00M)", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                //                             == Expected Tree Structure ==
                //                         ----------------- and -------------------
                //                        |                                         |
                //              -------- and --------                          --- eq ---
                //             |                     |                        |          |
                //         --- eq ---           --- and ---
                //        |          |         |           |
                //                        --- ge ---   --- le ---
                //                       |          |  |          |

                var node = (BinaryOperatorNode)queryNode;

                // node.Left = (Rating eq 5) and (ReleaseDate ge 2015-02-06T00:00:00 and ReleaseDate le 2015-02-06T23:59:59)
                Assert.IsType<BinaryOperatorNode>(node.Left);
                var nodeLeft = (BinaryOperatorNode)node.Left;

                // node.Left.Left = (Rating eq 5)
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Left);
                var nodeLeftLeft = (BinaryOperatorNode)nodeLeft.Left;

                Assert.IsType<PropertyAccessNode>(nodeLeftLeft.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)nodeLeftLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftLeft.OperatorKind);
                Assert.IsType<ConstantNode<int>>(nodeLeftLeft.Right);
                Assert.Equal(5, ((ConstantNode<int>)nodeLeftLeft.Right).Value);

                // node.Left.Right = (ReleaseDate ge 2015-02-06T00:00:00 and ReleaseDate le 2015-02-06T23:59:59)
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Right);
                var nodeLeftRight = (BinaryOperatorNode)nodeLeft.Right;

                Assert.IsType<BinaryOperatorNode>(nodeLeftRight.Left);
                var nodeLeftRightLeft = (BinaryOperatorNode)nodeLeftRight.Left;

                Assert.IsType<PropertyAccessNode>(nodeLeftRightLeft.Left);
                Assert.Equal("ReleaseDate", ((PropertyAccessNode)nodeLeftRightLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.GreaterThanOrEqual, nodeLeftRightLeft.OperatorKind);
                Assert.IsType<ConstantNode<DateTimeOffset>>(nodeLeftRightLeft.Right);
                Assert.Equal(DateTimeOffset.Parse("2015-02-06T00:00:00", ParserSettings.CultureInfo, ParserSettings.DateTimeStyles), ((ConstantNode<DateTimeOffset>)nodeLeftRightLeft.Right).Value);

                Assert.Equal(BinaryOperatorKind.And, nodeLeftRight.OperatorKind);

                Assert.IsType<BinaryOperatorNode>(nodeLeftRight.Right);
                var nodeLeftRightRight = (BinaryOperatorNode)nodeLeftRight.Right;

                Assert.IsType<PropertyAccessNode>(nodeLeftRightRight.Left);
                Assert.Equal("ReleaseDate", ((PropertyAccessNode)nodeLeftRightRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.LessThanOrEqual, nodeLeftRightRight.OperatorKind);
                Assert.IsType<ConstantNode<DateTimeOffset>>(nodeLeftRightRight.Right);
                Assert.Equal(DateTimeOffset.Parse("2015-02-06T23:59:59", ParserSettings.CultureInfo, ParserSettings.DateTimeStyles), ((ConstantNode<DateTimeOffset>)nodeLeftRightRight.Right).Value);

                Assert.Equal(BinaryOperatorKind.And, node.OperatorKind);

                // node.Right = (Price eq 150.00M)
                Assert.IsType<BinaryOperatorNode>(node.Right);
                var nodeRight = (BinaryOperatorNode)node.Right;

                Assert.IsType<PropertyAccessNode>(nodeRight.Left);
                Assert.Equal("Price", ((PropertyAccessNode)nodeRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRight.OperatorKind);
                Assert.IsType<ConstantNode<decimal>>(nodeRight.Right);
                Assert.Equal(150.00M, ((ConstantNode<decimal>)nodeRight.Right).Value);
            }

            [Fact]
            public void Parse_GroupedAandBandC_And_GroupedDorEorF()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("(ReleaseDate ge 2015-02-06T00:00:00 and ReleaseDate le 2015-02-06T23:59:59 and Price eq 200.00M) and (Rating eq 3 or Rating eq 4 or Rating eq 5)", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                //                             == Expected Tree Structure ==
                //                         ----------------- and -------------------
                //                        |                                         |
                //              -------- and --------                      -------- or ---------
                //             |                     |                    |                     |
                //       ---- and ----           --- eq ---          ---- or ----           --- eq ---
                //      |             |         |          |        |            |         |          |
                //  --- ge ---    --- le ---                    --- eq ---     --- eq ---
                // |          |  |          |                  |          |   |          |

                var node = (BinaryOperatorNode)queryNode;

                // node.Left = (ReleaseDate ge 2015-02-06T00:00:00 and ReleaseDate le 2015-02-06T23:59:59 and Price eq 200.00M)
                Assert.IsType<BinaryOperatorNode>(node.Left);
                var nodeLeft = (BinaryOperatorNode)node.Left;

                // node.Left.Left = ReleaseDate ge 2015-02-06T00:00:00 and ReleaseDate le 2015-02-06T23:59:59
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Left);
                var nodeLeftLeft = (BinaryOperatorNode)nodeLeft.Left;

                // node.Left.Left.Left = ReleaseDate ge 2015-02-06T00:00:00
                Assert.IsType<BinaryOperatorNode>(nodeLeftLeft.Left);
                var nodeLeftLeftLeft = (BinaryOperatorNode)nodeLeftLeft.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeftLeftLeft.Left);
                Assert.Equal("ReleaseDate", ((PropertyAccessNode)nodeLeftLeftLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.GreaterThanOrEqual, nodeLeftLeftLeft.OperatorKind);
                Assert.Equal("2015-02-06T00:00:00", ((ConstantNode)nodeLeftLeftLeft.Right).LiteralText);

                Assert.Equal(BinaryOperatorKind.And, nodeLeftLeft.OperatorKind);

                // node.Right.Left.Right = ReleaseDate le 2015-02-06T23:59:59
                Assert.IsType<BinaryOperatorNode>(nodeLeftLeft.Right);
                var nodeLeftLeftRight = (BinaryOperatorNode)nodeLeftLeft.Right;
                Assert.IsType<PropertyAccessNode>(nodeLeftLeftRight.Left);
                Assert.Equal("ReleaseDate", ((PropertyAccessNode)nodeLeftLeftRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.LessThanOrEqual, nodeLeftLeftRight.OperatorKind);
                Assert.Equal("2015-02-06T23:59:59", ((ConstantNode)nodeLeftLeftRight.Right).LiteralText);

                Assert.Equal(BinaryOperatorKind.And, nodeLeft.OperatorKind);

                // node.Right.Right = Price eq 200.00M
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Right);
                var nodeLeftRight = (BinaryOperatorNode)nodeLeft.Right;
                Assert.IsType<PropertyAccessNode>(nodeLeftRight.Left);
                Assert.Equal("Price", ((PropertyAccessNode)nodeLeftRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftRight.OperatorKind);
                Assert.Equal("200.00M", ((ConstantNode)nodeLeftRight.Right).LiteralText);

                Assert.Equal(BinaryOperatorKind.And, node.OperatorKind);

                // node.Right = Rating eq 3 or Rating eq 4 or Rating eq 5
                Assert.IsType<BinaryOperatorNode>(node.Right);
                var nodeRight = (BinaryOperatorNode)node.Right;

                // node.Right.Left = Rating eq 3 or Rating eq 4
                Assert.IsType<BinaryOperatorNode>(nodeRight.Left);
                var nodeRightLeft = (BinaryOperatorNode)nodeRight.Left;

                // node.Right.Left.Left = Rating eq 3
                Assert.IsType<BinaryOperatorNode>(nodeRightLeft.Left);
                var nodeRightLeftLeft = (BinaryOperatorNode)nodeRightLeft.Left;
                Assert.IsType<PropertyAccessNode>(nodeRightLeftLeft.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)nodeRightLeftLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightLeftLeft.OperatorKind);
                Assert.IsType<ConstantNode<int>>(nodeRightLeftLeft.Right);
                var nodeRightLeftLeftRight = (ConstantNode<int>)nodeRightLeftLeft.Right;
                Assert.Equal("3", nodeRightLeftLeftRight.LiteralText);
                Assert.Equal(3, nodeRightLeftLeftRight.Value);

                Assert.Equal(BinaryOperatorKind.Or, nodeRightLeft.OperatorKind);

                // node.Right.Left.Right = Rating eq 4
                Assert.IsType<BinaryOperatorNode>(nodeRightLeft.Right);
                var nodeRightLeftRight = (BinaryOperatorNode)nodeRightLeft.Right;
                Assert.IsType<PropertyAccessNode>(nodeRightLeftRight.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)nodeRightLeftRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightLeftRight.OperatorKind);
                Assert.IsType<ConstantNode<int>>(nodeRightLeftRight.Right);
                var nodeRightLeftRightRight = (ConstantNode<int>)nodeRightLeftRight.Right;
                Assert.Equal("4", nodeRightLeftRightRight.LiteralText);
                Assert.Equal(4, nodeRightLeftRightRight.Value);

                Assert.Equal(BinaryOperatorKind.Or, nodeRight.OperatorKind);

                // node.Right.Right = Rating eq 5
                Assert.IsType<BinaryOperatorNode>(nodeRight.Right);
                var nodeRightRight = (BinaryOperatorNode)nodeRight.Right;
                Assert.IsType<PropertyAccessNode>(nodeRightRight.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)nodeRightRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightRight.OperatorKind);
                Assert.IsType<ConstantNode<int>>(nodeRightRight.Right);
                var nodeRightRightRight = (ConstantNode<int>)nodeRightRight.Right;
                Assert.Equal("5", nodeRightRightRight.LiteralText);
                Assert.Equal(5, nodeRightRightRight.Value);
            }

            /// <summary>
            /// https://github.com/TrevorPilley/Net.Http.WebApi.OData/issues/40 - Grouping doesn't support (x and y) and (a or b or c)
            /// </summary>
            [Fact]
            public void Parse_GroupedXandY_And_GroupedAorBorC()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("(ReleaseDate ge 2015-02-06T00:00:00 and ReleaseDate le 2015-02-06T23:59:59) and (Rating eq 3 or Rating eq 4 or Rating eq 5)", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                //                  == Expected Tree Structure ==
                //              --------------- and -----------------
                //             |                                     |
                //       ---- and ----                     --------- or ---------
                //      |             |                   |                      |
                // --- ge ---    --- le ---          ---- or ----           --- eq ---
                // |         |   |        |         |            |         |          |
                //                              --- eq ---   --- eq ---
                //                             |         |  |          |

                var node = (BinaryOperatorNode)queryNode;

                // node.Left = (ReleaseDate ge 2015-02-06T00:00:00 and ReleaseDate le 2015-02-06T23:59:59)
                Assert.IsType<BinaryOperatorNode>(node.Left);
                var nodeLeft = (BinaryOperatorNode)node.Left;

                // node.Left.Left = ReleaseDate ge 2015-02-06T00:00:00
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Left);
                var nodeLeftLeft = (BinaryOperatorNode)nodeLeft.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeftLeft.Left);
                Assert.Equal("ReleaseDate", ((PropertyAccessNode)nodeLeftLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.GreaterThanOrEqual, nodeLeftLeft.OperatorKind);
                Assert.Equal("2015-02-06T00:00:00", ((ConstantNode)nodeLeftLeft.Right).LiteralText);

                Assert.Equal(BinaryOperatorKind.And, nodeLeft.OperatorKind);

                // node.Left.Right = ReleaseDate le 2015-02-06T23:59:59
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Right);
                var nodeLeftRight = (BinaryOperatorNode)nodeLeft.Right;
                Assert.IsType<PropertyAccessNode>(nodeLeftRight.Left);
                Assert.Equal("ReleaseDate", ((PropertyAccessNode)nodeLeftRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.LessThanOrEqual, nodeLeftRight.OperatorKind);
                Assert.Equal("2015-02-06T23:59:59", ((ConstantNode)nodeLeftRight.Right).LiteralText);

                Assert.Equal(BinaryOperatorKind.And, node.OperatorKind);

                // node.Right = (Rating eq 3 or Rating eq 4 or Rating eq 5)
                Assert.IsType<BinaryOperatorNode>(node.Right);
                var nodeRight = (BinaryOperatorNode)node.Right;

                // node.Right.Left = Rating eq 3 or Rating eq 4
                Assert.IsType<BinaryOperatorNode>(nodeRight.Left);
                var nodeRightLeft = (BinaryOperatorNode)nodeRight.Left;

                // node.Right.Left.Left = Rating eq 3
                Assert.IsType<BinaryOperatorNode>(nodeRightLeft.Left);
                var nodeRightLeftLeft = (BinaryOperatorNode)nodeRightLeft.Left;
                Assert.IsType<PropertyAccessNode>(nodeRightLeftLeft.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)nodeRightLeftLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightLeftLeft.OperatorKind);
                Assert.IsType<ConstantNode<int>>(nodeRightLeftLeft.Right);
                var nodeRightLeftLeftRight = (ConstantNode<int>)nodeRightLeftLeft.Right;
                Assert.Equal("3", nodeRightLeftLeftRight.LiteralText);
                Assert.Equal(3, nodeRightLeftLeftRight.Value);

                Assert.Equal(BinaryOperatorKind.Or, nodeRightLeft.OperatorKind);

                // node.Right.Left.Right = Rating eq 4
                Assert.IsType<BinaryOperatorNode>(nodeRightLeft.Right);
                var nodeRightLeftRight = (BinaryOperatorNode)nodeRightLeft.Right;
                Assert.IsType<PropertyAccessNode>(nodeRightLeftRight.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)nodeRightLeftRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightLeftRight.OperatorKind);
                Assert.IsType<ConstantNode<int>>(nodeRightLeftRight.Right);
                var nodeRightLeftRightRight = (ConstantNode<int>)nodeRightLeftRight.Right;
                Assert.Equal("4", nodeRightLeftRightRight.LiteralText);
                Assert.Equal(4, nodeRightLeftRightRight.Value);

                Assert.Equal(BinaryOperatorKind.Or, nodeRight.OperatorKind);

                // node.Right.Right = Rating eq 5
                Assert.IsType<BinaryOperatorNode>(nodeRight.Right);
                var nodeRightRight = (BinaryOperatorNode)nodeRight.Right;
                Assert.IsType<PropertyAccessNode>(nodeRightRight.Left);
                Assert.Equal("Rating", ((PropertyAccessNode)nodeRightRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightRight.OperatorKind);
                Assert.IsType<ConstantNode<int>>(nodeRightRight.Right);
                var nodeRightRightRight = (ConstantNode<int>)nodeRightRight.Right;
                Assert.Equal("5", nodeRightRightRight.LiteralText);
                Assert.Equal(5, nodeRightRightRight.Value);
            }

            [Fact]
            public void Parse_XsubY_gt_Z()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("(Price sub 5) gt 10", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                //    == Expected Tree Structure ==
                //          ------ gt ------
                //         |                |
                //    --- sub ---           10
                //   |           |
                // Price         5

                var node = (BinaryOperatorNode)queryNode;

                // node.Left = (Price sub 5)
                Assert.IsType<BinaryOperatorNode>(node.Left);
                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeft.Left);
                Assert.Equal(BinaryOperatorKind.Subtract, nodeLeft.OperatorKind);
                Assert.IsType<ConstantNode<int>>(nodeLeft.Right);
                var nodeLeftRight = (ConstantNode<int>)nodeLeft.Right;
                Assert.Equal(5, nodeLeftRight.Value);

                Assert.Equal(BinaryOperatorKind.GreaterThan, node.OperatorKind);

                // node.Right = 10
                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("10", nodeRight.LiteralText);
                Assert.Equal(10, nodeRight.Value);
            }

            [Fact]
            public void ParseFunctionCallAndGroupedPropertyEqValueOrPropertyEqValue()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("endswith(Forename, 'ohn') and (Surname eq 'Smith' or Surname eq 'Smythe')", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("endswith", nodeLeft.Name);
                Assert.Equal(2, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeLeft.Parameters[0]).PropertyPath.Property.Name);
                Assert.IsType<ConstantNode<string>>(nodeLeft.Parameters[1]);
                var nodeLeftParams1 = (ConstantNode<string>)nodeLeft.Parameters[1];
                Assert.Equal("ohn", nodeLeftParams1.Value);

                Assert.Equal(BinaryOperatorKind.And, node.OperatorKind);

                Assert.IsType<BinaryOperatorNode>(node.Right);
                var nodeRight = (BinaryOperatorNode)node.Right;
                Assert.IsType<BinaryOperatorNode>(nodeRight.Left);
                var nodeRightLeft = (BinaryOperatorNode)nodeRight.Left;
                Assert.IsType<PropertyAccessNode>(nodeRightLeft.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeRightLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightLeft.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeRightLeft.Right);
                Assert.Equal("Smith", ((ConstantNode<string>)nodeRightLeft.Right).Value);
                Assert.Equal(BinaryOperatorKind.Or, nodeRight.OperatorKind);
                var nodeRightRight = (BinaryOperatorNode)nodeRight.Right;
                Assert.IsType<PropertyAccessNode>(nodeRightRight.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeRightRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightRight.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeRightRight.Right);
                Assert.Equal("Smythe", ((ConstantNode<string>)nodeRightRight.Right).Value);
            }

            [Fact]
            public void ParseFunctionCallAndPropertyEqValue()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("endswith(Forename, 'ohn') and Surname eq 'Smith'", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("endswith", nodeLeft.Name);
                Assert.Equal(2, nodeLeft.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeft.Parameters[0]);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeLeft.Parameters[0]).PropertyPath.Property.Name);
                Assert.IsType<ConstantNode<string>>(nodeLeft.Parameters[1]);
                Assert.Equal("ohn", ((ConstantNode<string>)nodeLeft.Parameters[1]).Value);

                Assert.Equal(BinaryOperatorKind.And, node.OperatorKind);

                var nodeRight = (BinaryOperatorNode)node.Right;
                Assert.IsType<PropertyAccessNode>(nodeRight.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRight.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeRight.Right);
                Assert.Equal("Smith", ((ConstantNode<string>)nodeRight.Right).Value);
            }

            [Fact]
            public void ParseGroupedPropertyEqValueAndPropertyEqValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("(Forename eq 'John' and Surname eq 'Smith')", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<BinaryOperatorNode>(node.Left);
                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeft.Left);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeft.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeLeft.Right);
                Assert.Equal("John", ((ConstantNode<string>)nodeLeft.Right).Value);

                Assert.Equal(BinaryOperatorKind.And, node.OperatorKind);

                Assert.IsType<BinaryOperatorNode>(node.Right);
                var nodeRight = (BinaryOperatorNode)node.Right;
                Assert.IsType<PropertyAccessNode>(nodeRight.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRight.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeRight.Right);
                Assert.Equal("Smith", ((ConstantNode<string>)nodeRight.Right).Value);
            }

            [Fact]
            public void ParseGroupedPropertyEqValueAndPropertyEqValueOrPropertyEqValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("(Forename eq 'John' and Surname eq 'Smith') or Title eq 'Mr'", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Left);
                var nodeLeftLeft = (BinaryOperatorNode)nodeLeft.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeftLeft.Left);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeLeftLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftLeft.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeLeftLeft.Right);
                Assert.Equal("John", ((ConstantNode<string>)nodeLeftLeft.Right).Value);
                Assert.Equal(BinaryOperatorKind.And, nodeLeft.OperatorKind);
                var nodeLeftRight = (BinaryOperatorNode)nodeLeft.Right;
                Assert.IsType<PropertyAccessNode>(nodeLeftRight.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeLeftRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftRight.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeLeftRight.Right);
                Assert.Equal("Smith", ((ConstantNode<string>)nodeLeftRight.Right).Value);

                Assert.Equal(BinaryOperatorKind.Or, node.OperatorKind);

                var nodeRight = (BinaryOperatorNode)node.Right;
                var nodeRightLeft = (PropertyAccessNode)nodeRight.Left;
                Assert.Equal("Title", nodeRightLeft.PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRight.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeRight.Right);
                Assert.Equal("Mr", ((ConstantNode<string>)nodeRight.Right).Value);
            }

            [Fact]
            public void ParseGroupedPropertyEqValueOrPropertyEqValueAndFunctionCall()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("(Surname eq 'Smith' or Surname eq 'Smythe') and endswith(Forename, 'ohn')", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<BinaryOperatorNode>(node.Left);
                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Left);
                var nodeLeftLeft = (BinaryOperatorNode)nodeLeft.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeftLeft.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeLeftLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftLeft.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeLeftLeft.Right);
                Assert.Equal("Smith", ((ConstantNode<string>)nodeLeftLeft.Right).Value);
                Assert.Equal(BinaryOperatorKind.Or, nodeLeft.OperatorKind);
                var nodeLeftRight = (BinaryOperatorNode)nodeLeft.Right;
                Assert.IsType<PropertyAccessNode>(nodeLeftRight.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeLeftRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftRight.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeLeftRight.Right);
                Assert.Equal("Smythe", ((ConstantNode<string>)nodeLeftRight.Right).Value);

                Assert.Equal(BinaryOperatorKind.And, node.OperatorKind);

                Assert.IsType<FunctionCallNode>(node.Right);
                var nodeRight = (FunctionCallNode)node.Right;
                Assert.Equal("endswith", nodeRight.Name);
                Assert.Equal(2, nodeRight.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeRight.Parameters[0]);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeRight.Parameters[0]).PropertyPath.Property.Name);
                Assert.IsType<ConstantNode<string>>(nodeRight.Parameters[1]);
                Assert.Equal("ohn", ((ConstantNode<string>)nodeRight.Parameters[1]).Value);
            }

            [Fact]
            public void ParseGroupedPropertyEqValueorPropertyEqValueAndGroupedPropertyEqValueOrPropertyEqValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("(Forename eq 'John' or Forename eq 'Joe') and (Surname eq 'Smith' or Surname eq 'Bloggs')", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<BinaryOperatorNode>(node.Left);
                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Left);
                var nodeLeftLeft = (BinaryOperatorNode)nodeLeft.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeftLeft.Left);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeLeftLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftLeft.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeLeftLeft.Right);
                Assert.Equal("John", ((ConstantNode<string>)nodeLeftLeft.Right).Value);
                Assert.Equal(BinaryOperatorKind.Or, nodeLeft.OperatorKind);
                var nodeLeftRight = (BinaryOperatorNode)nodeLeft.Right;
                Assert.IsType<PropertyAccessNode>(nodeLeftRight.Left);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeLeftRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftRight.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeLeftRight.Right);
                Assert.Equal("Joe", ((ConstantNode<string>)nodeLeftRight.Right).Value);

                Assert.Equal(BinaryOperatorKind.And, node.OperatorKind);

                Assert.IsType<BinaryOperatorNode>(node.Right);
                var nodeRight = (BinaryOperatorNode)node.Right;
                Assert.IsType<BinaryOperatorNode>(nodeRight.Left);
                var nodeRightLeft = (BinaryOperatorNode)nodeRight.Left;
                Assert.IsType<PropertyAccessNode>(nodeRightLeft.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeRightLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightLeft.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeRightLeft.Right);
                Assert.Equal("Smith", ((ConstantNode<string>)nodeRightLeft.Right).Value);
                Assert.Equal(BinaryOperatorKind.Or, nodeRight.OperatorKind);
                var nodeRightRight = (BinaryOperatorNode)nodeRight.Right;
                Assert.IsType<PropertyAccessNode>(nodeRightRight.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeRightRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightRight.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeRightRight.Right);
                Assert.Equal("Bloggs", ((ConstantNode<string>)nodeRightRight.Right).Value);
            }

            [Fact]
            public void ParseGroupedPropertyEqValueOrPropertyEqValueAndPropertyEqValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("(Surname eq 'Smith' or Surname eq 'Smythe') and Forename eq 'John'", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Left);
                var nodeRightLeft = (BinaryOperatorNode)nodeLeft.Left;
                Assert.IsType<PropertyAccessNode>(nodeRightLeft.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeRightLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightLeft.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeRightLeft.Right);
                Assert.Equal("Smith", ((ConstantNode<string>)nodeRightLeft.Right).Value);
                Assert.Equal(BinaryOperatorKind.Or, nodeLeft.OperatorKind);
                var nodeRightRight = (BinaryOperatorNode)nodeLeft.Right;
                Assert.IsType<PropertyAccessNode>(nodeRightRight.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeRightRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightRight.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeRightRight.Right);
                Assert.Equal("Smythe", ((ConstantNode<string>)nodeRightRight.Right).Value);

                Assert.Equal(BinaryOperatorKind.And, node.OperatorKind);

                var nodeRight = (BinaryOperatorNode)node.Right;
                Assert.IsType<PropertyAccessNode>(nodeRight.Left);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRight.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeRight.Right);
                Assert.Equal("John", ((ConstantNode<string>)nodeRight.Right).Value);
            }

            [Fact]
            public void ParseNestedFunctionCallEqFunctionCallExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("length(trim(CompanyName)) eq length(CompanyName)", EntityDataModel.Current.EntitySets["Customers"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("length", nodeLeft.Name);
                Assert.Equal(1, nodeLeft.Parameters.Count);
                Assert.IsType<FunctionCallNode>(nodeLeft.Parameters[0]);
                var nodeLeftArg0 = (FunctionCallNode)nodeLeft.Parameters[0];
                Assert.Equal("trim", nodeLeftArg0.Name);
                Assert.Equal(1, nodeLeftArg0.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeftArg0.Parameters[0]);
                Assert.Equal("CompanyName", ((PropertyAccessNode)nodeLeftArg0.Parameters[0]).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<FunctionCallNode>(node.Right);
                var nodeRight = (FunctionCallNode)node.Right;
                Assert.Equal("length", nodeRight.Name);
                Assert.Equal(1, nodeRight.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeRight.Parameters[0]);
                var nodeRightArg0 = (PropertyAccessNode)nodeRight.Parameters[0];
                Assert.Equal("CompanyName", nodeRightArg0.PropertyPath.Property.Name);
            }

            [Fact]
            public void ParseNestedFunctionCallEqValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("length(trim(CompanyName)) eq 50", EntityDataModel.Current.EntitySets["Customers"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<FunctionCallNode>(node.Left);
                var nodeLeft = (FunctionCallNode)node.Left;
                Assert.Equal("length", nodeLeft.Name);
                Assert.Equal(1, nodeLeft.Parameters.Count);
                Assert.IsType<FunctionCallNode>(nodeLeft.Parameters[0]);
                var nodeLeftArg0 = (FunctionCallNode)nodeLeft.Parameters[0];
                Assert.Equal("trim", nodeLeftArg0.Name);
                Assert.Equal(1, nodeLeftArg0.Parameters.Count);
                Assert.IsType<PropertyAccessNode>(nodeLeftArg0.Parameters[0]);
                Assert.Equal("CompanyName", ((PropertyAccessNode)nodeLeftArg0.Parameters[0]).PropertyPath.Property.Name);

                Assert.Equal(BinaryOperatorKind.Equal, node.OperatorKind);

                Assert.IsType<ConstantNode<int>>(node.Right);
                var nodeRight = (ConstantNode<int>)node.Right;
                Assert.Equal("50", nodeRight.LiteralText);
                Assert.Equal(50, nodeRight.Value);
            }

            /// <summary>
            /// https://github.com/TrevorPilley/Net.Http.WebApi.OData/issues/49 - InvalidOperationException: Stack Empty thrown by FilterExpressionParser with nested grouping.
            /// </summary>
            [Fact]
            public void ParseNestedGrouping()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("(ReleaseDate ge 2015-03-01T00:00:00 and ReleaseDate le 2015-03-31T23:59:59) and ((Discontinued eq false and substringof('ilk', Description) eq true) or (Discontinued eq false and substringof('rrots', Description) eq true))", EntityDataModel.Current.EntitySets["Products"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                //                  == Expected Tree Structure ==
                //              ---------------- and ------------------
                //             |                                       |
                //       ---- and ----                     ----------- or ------------
                //      |             |                   |                           |
                // --- ge ---    --- le ---         ---- and ----                --- and ---
                // |         |   |        |         |            |              |           |
                //                              --- eq ---   --- eq ---    --- eq ---   --- eq ---
                //                             |         |  |          |  |         |  |          |

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<BinaryOperatorNode>(node.Left);
                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Left);
                var nodeLeftLeft = (BinaryOperatorNode)nodeLeft.Left;
                Assert.Equal(BinaryOperatorKind.GreaterThanOrEqual, nodeLeftLeft.OperatorKind);
                Assert.Equal(BinaryOperatorKind.And, nodeLeft.OperatorKind);
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Right);
                var nodeLeftRight = (BinaryOperatorNode)nodeLeft.Right;
                Assert.Equal(BinaryOperatorKind.LessThanOrEqual, nodeLeftRight.OperatorKind);

                Assert.Equal(BinaryOperatorKind.And, node.OperatorKind);

                Assert.IsType<BinaryOperatorNode>(node.Right);
                var nodeRight = (BinaryOperatorNode)node.Right;
                Assert.IsType<BinaryOperatorNode>(nodeRight.Left);
                var nodeRightLeft = (BinaryOperatorNode)nodeRight.Left;
                Assert.IsType<BinaryOperatorNode>(nodeRightLeft.Left);
                var nodeRightLeftLeft = (BinaryOperatorNode)nodeRightLeft.Left;
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightLeftLeft.OperatorKind);
                Assert.Equal(BinaryOperatorKind.And, nodeRightLeft.OperatorKind);
                Assert.IsType<BinaryOperatorNode>(nodeRightLeft.Right);
                var nodeRightLeftRight = (BinaryOperatorNode)nodeRightLeft.Right;
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightLeftRight.OperatorKind);
                Assert.Equal(BinaryOperatorKind.Or, nodeRight.OperatorKind);
                Assert.IsType<BinaryOperatorNode>(nodeRight.Right);
                var nodeRightRight = (BinaryOperatorNode)nodeRight.Right;
                Assert.IsType<BinaryOperatorNode>(nodeRightRight.Left);
                var nodeRightRightLeft = (BinaryOperatorNode)nodeRightLeft.Left;
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightRightLeft.OperatorKind);
                Assert.Equal(BinaryOperatorKind.And, nodeRightRight.OperatorKind);
                Assert.IsType<BinaryOperatorNode>(nodeRightRight.Right);
                var nodeRightRightRight = (BinaryOperatorNode)nodeRightLeft.Right;
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightRightRight.OperatorKind);
            }

            /// <summary>
            /// https://github.com/TrevorPilley/Net.Http.WebApi.OData/issues/36#issuecomment-70567443.
            /// </summary>
            [Fact]
            public void ParseOuterGroupedExample1()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("(((Forename eq 'andrew') and (Surname eq 'davis')) or (Forename eq 'system'))", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Left);
                var nodeLeftLeft = (BinaryOperatorNode)nodeLeft.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeftLeft.Left);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeLeftLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftLeft.OperatorKind);
                Assert.Equal("andrew", ((ConstantNode<string>)nodeLeftLeft.Right).Value);
                Assert.Equal(BinaryOperatorKind.And, nodeLeft.OperatorKind);
                var nodeLeftRight = (BinaryOperatorNode)nodeLeft.Right;
                Assert.IsType<PropertyAccessNode>(nodeLeftRight.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeLeftRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftRight.OperatorKind);
                Assert.Equal("davis", ((ConstantNode<string>)nodeLeftRight.Right).Value);

                Assert.Equal(BinaryOperatorKind.Or, node.OperatorKind);

                var nodeRight = (BinaryOperatorNode)node.Right;
                var nodeRightLeft = (PropertyAccessNode)nodeRight.Left;
                Assert.Equal("Forename", nodeRightLeft.PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRight.OperatorKind);
                Assert.Equal("system", ((ConstantNode<string>)nodeRight.Right).Value);
            }

            [Fact]
            public void ParseOuterGroupedPropertyEqValueAndGroupedPropertyEqValueOrPropertyEqValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("(Forename eq 'John' and (Surname eq 'Smith' or Surname eq 'Smythe'))", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeft.Left);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeft.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeLeft.Right);
                Assert.Equal("John", ((ConstantNode<string>)nodeLeft.Right).Value);

                Assert.Equal(BinaryOperatorKind.And, node.OperatorKind);

                var nodeRight = (BinaryOperatorNode)node.Right;
                Assert.IsType<BinaryOperatorNode>(nodeRight.Left);
                var nodeRightLeft = (BinaryOperatorNode)nodeRight.Left;
                Assert.IsType<PropertyAccessNode>(nodeRightLeft.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeRightLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightLeft.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeRightLeft.Right);
                Assert.Equal("Smith", ((ConstantNode<string>)nodeRightLeft.Right).Value);
                Assert.Equal(BinaryOperatorKind.Or, nodeRight.OperatorKind);
                var nodeRightRight = (BinaryOperatorNode)nodeRight.Right;
                Assert.IsType<PropertyAccessNode>(nodeRightRight.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeRightRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightRight.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeRightRight.Right);
                Assert.Equal("Smythe", ((ConstantNode<string>)nodeRightRight.Right).Value);
            }

            [Fact]
            public void ParseOuterGroupedPropertyEqValueAndPropertyEqValueOrPropertyEqValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("((Forename eq 'John' and Surname eq 'Smith') or Title eq 'Mr')", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Left);
                var nodeLeftLeft = (BinaryOperatorNode)nodeLeft.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeftLeft.Left);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeLeftLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftLeft.OperatorKind);
                Assert.Equal("John", ((ConstantNode<string>)nodeLeftLeft.Right).Value);
                Assert.Equal(BinaryOperatorKind.And, nodeLeft.OperatorKind);
                var nodeLeftRight = (BinaryOperatorNode)nodeLeft.Right;
                Assert.IsType<PropertyAccessNode>(nodeLeftRight.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeLeftRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftRight.OperatorKind);
                Assert.Equal("Smith", ((ConstantNode<string>)nodeLeftRight.Right).Value);

                Assert.Equal(BinaryOperatorKind.Or, node.OperatorKind);

                var nodeRight = (BinaryOperatorNode)node.Right;
                var nodeRightLeft = (PropertyAccessNode)nodeRight.Left;
                Assert.Equal("Title", nodeRightLeft.PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRight.OperatorKind);
                Assert.Equal("Mr", ((ConstantNode<string>)nodeRight.Right).Value);
            }

            [Fact]
            public void ParseOuterGroupedPropertyEqValueorPropertyEqValueAndGroupedPropertyEqValueOrPropertyEqValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("((Forename eq 'John' or Forename eq 'Joe') and (Surname eq 'Smith' or Surname eq 'Bloggs'))", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<BinaryOperatorNode>(node.Left);
                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Left);
                var nodeLeftLeft = (BinaryOperatorNode)nodeLeft.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeftLeft.Left);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeLeftLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftLeft.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeLeftLeft.Right);
                Assert.Equal("John", ((ConstantNode<string>)nodeLeftLeft.Right).Value);
                Assert.Equal(BinaryOperatorKind.Or, nodeLeft.OperatorKind);
                var nodeLeftRight = (BinaryOperatorNode)nodeLeft.Right;
                Assert.IsType<PropertyAccessNode>(nodeLeftRight.Left);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeLeftRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftRight.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeLeftRight.Right);
                Assert.Equal("Joe", ((ConstantNode<string>)nodeLeftRight.Right).Value);

                Assert.Equal(BinaryOperatorKind.And, node.OperatorKind);

                Assert.IsType<BinaryOperatorNode>(node.Right);
                var nodeRight = (BinaryOperatorNode)node.Right;
                Assert.IsType<BinaryOperatorNode>(nodeRight.Left);
                var nodeRightLeft = (BinaryOperatorNode)nodeRight.Left;
                Assert.IsType<PropertyAccessNode>(nodeRightLeft.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeRightLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightLeft.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeRightLeft.Right);
                Assert.Equal("Smith", ((ConstantNode<string>)nodeRightLeft.Right).Value);
                Assert.Equal(BinaryOperatorKind.Or, nodeRight.OperatorKind);
                var nodeRightRight = (BinaryOperatorNode)nodeRight.Right;
                Assert.IsType<PropertyAccessNode>(nodeRightRight.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeRightRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightRight.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeRightRight.Right);
                Assert.Equal("Bloggs", ((ConstantNode<string>)nodeRightRight.Right).Value);
            }

            [Fact]
            public void ParsePropertyEqValueAndGroupedPropertyEqValueOrPropertyEqValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Forename eq 'John' and (Surname eq 'Smith' or Surname eq 'Smythe')", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeft.Left);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeft.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeLeft.Right);
                Assert.Equal("John", ((ConstantNode<string>)nodeLeft.Right).Value);

                Assert.Equal(BinaryOperatorKind.And, node.OperatorKind);

                var nodeRight = (BinaryOperatorNode)node.Right;
                Assert.IsType<BinaryOperatorNode>(nodeRight.Left);
                var nodeRightLeft = (BinaryOperatorNode)nodeRight.Left;
                Assert.IsType<PropertyAccessNode>(nodeRightLeft.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeRightLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightLeft.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeRightLeft.Right);
                Assert.Equal("Smith", ((ConstantNode<string>)nodeRightLeft.Right).Value);
                Assert.Equal(BinaryOperatorKind.Or, nodeRight.OperatorKind);
                var nodeRightRight = (BinaryOperatorNode)nodeRight.Right;
                Assert.IsType<PropertyAccessNode>(nodeRightRight.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeRightRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRightRight.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeRightRight.Right);
                Assert.Equal("Smythe", ((ConstantNode<string>)nodeRightRight.Right).Value);
            }

            [Fact]
            public void ParsePropertyEqValueAndPropertyEqValueAndPropertyEqValueAndPropertyEqValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Forename eq 'John' and Surname eq 'Smith' and Title eq 'Dr' and Title eq 'Mr'", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Left);
                var nodeLeftLeft = (BinaryOperatorNode)nodeLeft.Left;
                Assert.IsType<BinaryOperatorNode>(nodeLeftLeft.Left);
                var nodeLeftLeftLeft = (BinaryOperatorNode)nodeLeftLeft.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeftLeftLeft.Left);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeLeftLeftLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftLeftLeft.OperatorKind);
                Assert.Equal("John", ((ConstantNode<string>)nodeLeftLeftLeft.Right).Value);
                Assert.Equal(BinaryOperatorKind.And, nodeLeftLeft.OperatorKind);
                var nodeLeftLeftRight = (BinaryOperatorNode)nodeLeftLeft.Right;
                Assert.IsType<PropertyAccessNode>(nodeLeftLeftRight.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeLeftLeftRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftLeftRight.OperatorKind);
                Assert.Equal("Smith", ((ConstantNode<string>)nodeLeftLeftRight.Right).Value);
                Assert.Equal(BinaryOperatorKind.And, nodeLeft.OperatorKind);
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Right);
                var nodeLeftRight = (BinaryOperatorNode)nodeLeft.Right;
                Assert.IsType<PropertyAccessNode>(nodeLeftRight.Left);
                Assert.Equal("Title", ((PropertyAccessNode)nodeLeftRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftRight.OperatorKind);
                Assert.Equal("Dr", ((ConstantNode<string>)nodeLeftRight.Right).Value);

                Assert.Equal(BinaryOperatorKind.And, node.OperatorKind);

                var nodeRight = (BinaryOperatorNode)node.Right;
                Assert.IsType<PropertyAccessNode>(nodeRight.Left);
                Assert.Equal("Title", ((PropertyAccessNode)nodeRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRight.OperatorKind);
                Assert.Equal("Mr", ((ConstantNode<string>)nodeRight.Right).Value);
            }

            [Fact]
            public void ParsePropertyEqValueAndPropertyEqValueAndPropertyEqValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Forename eq 'John' and Surname eq 'Smith' and Title eq 'Mr'", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Left);
                var nodeLeftLeft = (BinaryOperatorNode)nodeLeft.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeftLeft.Left);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeLeftLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftLeft.OperatorKind);
                Assert.Equal("John", ((ConstantNode<string>)nodeLeftLeft.Right).Value);
                Assert.Equal(BinaryOperatorKind.And, nodeLeft.OperatorKind);
                var nodeLeftRight = (BinaryOperatorNode)nodeLeft.Right;
                Assert.IsType<PropertyAccessNode>(nodeLeftRight.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeLeftRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftRight.OperatorKind);
                Assert.Equal("Smith", ((ConstantNode<string>)nodeLeftRight.Right).Value);

                Assert.Equal(BinaryOperatorKind.And, node.OperatorKind);

                var nodeRight = (BinaryOperatorNode)node.Right;
                var nodeRightLeft = (PropertyAccessNode)nodeRight.Left;
                Assert.Equal("Title", nodeRightLeft.PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRight.OperatorKind);
                Assert.Equal("Mr", ((ConstantNode<string>)nodeRight.Right).Value);
            }

            [Fact]
            public void ParsePropertyEqValueAndPropertyEqValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Forename eq 'John' and Surname eq 'Smith'", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<BinaryOperatorNode>(node.Left);
                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeft.Left);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeft.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeLeft.Right);
                Assert.Equal("John", ((ConstantNode<string>)nodeLeft.Right).Value);

                Assert.Equal(BinaryOperatorKind.And, node.OperatorKind);

                Assert.IsType<BinaryOperatorNode>(node.Right);
                var nodeRight = (BinaryOperatorNode)node.Right;
                Assert.IsType<PropertyAccessNode>(nodeRight.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRight.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeRight.Right);
                Assert.Equal("Smith", ((ConstantNode<string>)nodeRight.Right).Value);
            }

            [Fact]
            public void ParsePropertyEqValueAndPropertyEqValueOrPropertyEqValueUnGroupedExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Forename eq 'John' and Surname eq 'Smith' or Title eq 'Mr'", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Left);
                var nodeLeftLeft = (BinaryOperatorNode)nodeLeft.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeftLeft.Left);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeLeftLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftLeft.OperatorKind);
                Assert.Equal("John", ((ConstantNode<string>)nodeLeftLeft.Right).Value);
                Assert.Equal(BinaryOperatorKind.And, nodeLeft.OperatorKind);
                var nodeLeftRight = (BinaryOperatorNode)nodeLeft.Right;
                Assert.IsType<PropertyAccessNode>(nodeLeftRight.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeLeftRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftRight.OperatorKind);
                Assert.Equal("Smith", ((ConstantNode<string>)nodeLeftRight.Right).Value);

                Assert.Equal(BinaryOperatorKind.Or, node.OperatorKind);

                var nodeRight = (BinaryOperatorNode)node.Right;
                var nodeRightLeft = (PropertyAccessNode)nodeRight.Left;
                Assert.Equal("Title", nodeRightLeft.PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRight.OperatorKind);
                Assert.Equal("Mr", ((ConstantNode<string>)nodeRight.Right).Value);
            }

            [Fact]
            public void ParsePropertyEqValueAndYearFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Surname eq 'Smith' and year(BirthDate) eq 1971", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<BinaryOperatorNode>(node.Left);
                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeft.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeft.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeLeft.Right);
                Assert.Equal("Smith", ((ConstantNode<string>)nodeLeft.Right).Value);

                Assert.Equal(BinaryOperatorKind.And, node.OperatorKind);

                Assert.IsType<BinaryOperatorNode>(node.Right);
                var nodeRight = (BinaryOperatorNode)node.Right;
                Assert.IsType<FunctionCallNode>(nodeRight.Left);
                Assert.Equal("year", ((FunctionCallNode)nodeRight.Left).Name);
                var rightNodeLeft = (FunctionCallNode)nodeRight.Left;
                Assert.IsType<PropertyAccessNode>(rightNodeLeft.Parameters[0]);
                Assert.Equal("BirthDate", ((PropertyAccessNode)rightNodeLeft.Parameters[0]).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRight.OperatorKind);
                Assert.IsType<ConstantNode<int>>(nodeRight.Right);
                Assert.Equal(1971, ((ConstantNode<int>)nodeRight.Right).Value);
            }

            [Fact]
            public void ParsePropertyEqValueOrPropertyEqValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Forename eq 'John' or Surname eq 'Smith'", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<BinaryOperatorNode>(node.Left);
                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeft.Left);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeft.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeLeft.Right);
                Assert.Equal("John", ((ConstantNode<string>)nodeLeft.Right).Value);

                Assert.Equal(BinaryOperatorKind.Or, node.OperatorKind);

                Assert.IsType<BinaryOperatorNode>(node.Right);
                var nodeRight = (BinaryOperatorNode)node.Right;
                Assert.IsType<PropertyAccessNode>(nodeRight.Left);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRight.OperatorKind);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeRight.Left).PropertyPath.Property.Name);
                Assert.IsType<ConstantNode<string>>(nodeRight.Right);
                Assert.Equal("Smith", ((ConstantNode<string>)nodeRight.Right).Value);
            }

            [Fact]
            public void ParsePropertyEqValueOrPropertyEqValueOrPropertyEqValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Forename eq 'John' or Surname eq 'Smith' or Title eq 'Mr'", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                var nodeLeft = (BinaryOperatorNode)node.Left;

                Assert.IsType<BinaryOperatorNode>(nodeLeft.Left);
                var nodeLeftLeft = (BinaryOperatorNode)nodeLeft.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeftLeft.Left);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeLeftLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftLeft.OperatorKind);
                Assert.Equal("John", ((ConstantNode<string>)nodeLeftLeft.Right).Value);
                Assert.Equal(BinaryOperatorKind.Or, nodeLeft.OperatorKind);
                var nodeLeftRight = (BinaryOperatorNode)nodeLeft.Right;
                Assert.IsType<PropertyAccessNode>(nodeLeftRight.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeLeftRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftRight.OperatorKind);
                Assert.Equal("Smith", ((ConstantNode<string>)nodeLeftRight.Right).Value);

                Assert.Equal(BinaryOperatorKind.Or, node.OperatorKind);

                var nodeRight = (BinaryOperatorNode)node.Right;
                var nodeRightLeft = (PropertyAccessNode)nodeRight.Left;
                Assert.Equal("Title", nodeRightLeft.PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRight.OperatorKind);
                Assert.Equal("Mr", ((ConstantNode<string>)nodeRight.Right).Value);
            }

            [Fact]
            public void ParsePropertyEqValueOrPropertyEqValueOrPropertyEqValueOrPropertyEqValueExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Forename eq 'John' or Surname eq 'Smith' or Title eq 'Dr' or Title eq 'Mr'", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Left);
                var nodeLeftLeft = (BinaryOperatorNode)nodeLeft.Left;
                Assert.IsType<BinaryOperatorNode>(nodeLeftLeft.Left);
                var nodeLeftLeftLeft = (BinaryOperatorNode)nodeLeftLeft.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeftLeftLeft.Left);
                Assert.Equal("Forename", ((PropertyAccessNode)nodeLeftLeftLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftLeftLeft.OperatorKind);
                Assert.Equal("John", ((ConstantNode<string>)nodeLeftLeftLeft.Right).Value);
                Assert.Equal(BinaryOperatorKind.Or, nodeLeftLeft.OperatorKind);
                var nodeLeftLeftRight = (BinaryOperatorNode)nodeLeftLeft.Right;
                Assert.IsType<PropertyAccessNode>(nodeLeftLeftRight.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeLeftLeftRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftLeftRight.OperatorKind);
                Assert.Equal("Smith", ((ConstantNode<string>)nodeLeftLeftRight.Right).Value);
                Assert.Equal(BinaryOperatorKind.Or, nodeLeft.OperatorKind);
                Assert.IsType<BinaryOperatorNode>(nodeLeft.Right);
                var nodeLeftRight = (BinaryOperatorNode)nodeLeft.Right;
                Assert.IsType<PropertyAccessNode>(nodeLeftRight.Left);
                Assert.Equal("Title", ((PropertyAccessNode)nodeLeftRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeftRight.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeLeftRight.Right);
                Assert.Equal("Dr", ((ConstantNode<string>)nodeLeftRight.Right).Value);

                Assert.Equal(BinaryOperatorKind.Or, node.OperatorKind);

                var nodeRight = (BinaryOperatorNode)node.Right;
                Assert.IsType<PropertyAccessNode>(nodeRight.Left);
                Assert.Equal("Title", ((PropertyAccessNode)nodeRight.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRight.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeRight.Right);
                Assert.Equal("Mr", ((ConstantNode<string>)nodeRight.Right).Value);
            }

            [Fact]
            public void ParsePropertyEqValueOrYearFunctionExpression()
            {
                QueryNode queryNode = FilterExpressionParser.Parse("Surname eq 'Smith' or year(BirthDate) eq 1971", EntityDataModel.Current.EntitySets["Employees"].EdmType);

                Assert.NotNull(queryNode);
                Assert.IsType<BinaryOperatorNode>(queryNode);

                var node = (BinaryOperatorNode)queryNode;

                Assert.IsType<BinaryOperatorNode>(node.Left);
                var nodeLeft = (BinaryOperatorNode)node.Left;
                Assert.IsType<PropertyAccessNode>(nodeLeft.Left);
                Assert.Equal("Surname", ((PropertyAccessNode)nodeLeft.Left).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeLeft.OperatorKind);
                Assert.IsType<ConstantNode<string>>(nodeLeft.Right);
                Assert.Equal("Smith", ((ConstantNode<string>)nodeLeft.Right).Value);

                Assert.Equal(BinaryOperatorKind.Or, node.OperatorKind);

                Assert.IsType<BinaryOperatorNode>(node.Right);
                var nodeRight = (BinaryOperatorNode)node.Right;
                Assert.IsType<FunctionCallNode>(nodeRight.Left);
                Assert.Equal("year", ((FunctionCallNode)nodeRight.Left).Name);
                var rightNodeLeft = (FunctionCallNode)nodeRight.Left;
                Assert.IsType<PropertyAccessNode>(rightNodeLeft.Parameters[0]);
                Assert.Equal("BirthDate", ((PropertyAccessNode)rightNodeLeft.Parameters[0]).PropertyPath.Property.Name);
                Assert.Equal(BinaryOperatorKind.Equal, nodeRight.OperatorKind);
                Assert.IsType<ConstantNode<int>>(nodeRight.Right);
                Assert.Equal(1971, ((ConstantNode<int>)nodeRight.Right).Value);
            }
        }
    }
}
