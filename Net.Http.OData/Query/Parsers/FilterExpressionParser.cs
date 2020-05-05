// -----------------------------------------------------------------------
// <copyright file="FilterExpressionParser.cs" company="Project Contributors">
// Copyright Project Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Net.Http.OData.Model;
using Net.Http.OData.Query.Expressions;

namespace Net.Http.OData.Query.Parsers
{
    internal static class FilterExpressionParser
    {
        internal static QueryNode Parse(string filterValue, EdmComplexType model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var parserImpl = new FilterExpressionParserImpl(model);
            QueryNode queryNode = parserImpl.ParseQueryNode(new Lexer(filterValue));

            return queryNode;
        }

        private sealed class FilterExpressionParserImpl
        {
            private readonly EdmComplexType _model;
            private readonly Stack<QueryNode> _nodeStack = new Stack<QueryNode>();
            private readonly Queue<Token> _tokens = new Queue<Token>();
            private int _groupingDepth;
            private BinaryOperatorKind _nextBinaryOperatorKind = BinaryOperatorKind.None;

            internal FilterExpressionParserImpl(EdmComplexType model) => _model = model;

            internal QueryNode ParseQueryNode(Lexer lexer)
            {
                while (lexer.MoveNext())
                {
                    Token token = lexer.Current;

                    switch (token.TokenType)
                    {
                        case TokenType.And:
                        case TokenType.Or:
                            _nextBinaryOperatorKind = token.Value.ToBinaryOperatorKind();
                            UpdateExpressionTree();
                            break;

                        default:
                            _tokens.Enqueue(token);
                            break;
                    }
                }

                _nextBinaryOperatorKind = BinaryOperatorKind.None;
                UpdateExpressionTree();

                if (_groupingDepth != 0 || _nodeStack.Count != 1)
                {
                    throw ODataException.BadRequest(ExceptionMessage.UnableToParseFilter("an extra opening or missing closing parenthesis may be present"), "$filter");
                }

                QueryNode node = _nodeStack.Pop();

                if (node is BinaryOperatorNode binaryNode && binaryNode.Right is null)
                {
                    throw ODataException.BadRequest(ExceptionMessage.UnableToParseFilter($"the binary operator {binaryNode.OperatorKind.ToString()} has no right node"), "$filter");
                }

                return node;
            }

            private QueryNode ParseFunctionCallNode()
            {
                BinaryOperatorNode binaryNode = null;
                FunctionCallNode node = null;

                var stack = new Stack<FunctionCallNode>();

                while (_tokens.Count > 0)
                {
                    Token token = _tokens.Dequeue();

                    switch (token.TokenType)
                    {
                        case TokenType.Base64Binary:
                        case TokenType.Date:
                        case TokenType.DateTimeOffset:
                        case TokenType.Decimal:
                        case TokenType.Double:
                        case TokenType.Duration:
                        case TokenType.EdmType:
                        case TokenType.Enum:
                        case TokenType.False:
                        case TokenType.Guid:
                        case TokenType.Integer:
                        case TokenType.Null:
                        case TokenType.Single:
                        case TokenType.String:
                        case TokenType.TimeOfDay:
                        case TokenType.True:
                            ConstantNode constantNode = ConstantNodeParser.ParseConstantNode(token);

                            if (stack.Count > 0)
                            {
                                stack.Peek().AddParameter(constantNode);
                            }
                            else
                            {
                                if (binaryNode == null)
                                {
                                    throw ODataException.BadRequest(ExceptionMessage.GenericUnableToParseFilter, "$filter");
                                }

                                binaryNode.Right = constantNode;
                            }

                            break;

                        case TokenType.BinaryOperator:
                            binaryNode = new BinaryOperatorNode(node, token.Value.ToBinaryOperatorKind(), null);
                            break;

                        case TokenType.CloseParentheses:
                            if (_groupingDepth == 0)
                            {
                                throw ODataException.BadRequest(ExceptionMessage.UnableToParseFilter($"the closing parenthesis not expected", token.Position), "$filter");
                            }

                            _groupingDepth--;

                            if (stack.Count > 0)
                            {
                                FunctionCallNode lastNode = stack.Pop();

                                if (stack.Count > 0)
                                {
                                    stack.Peek().AddParameter(lastNode);
                                }
                                else
                                {
                                    if (binaryNode != null)
                                    {
                                        binaryNode.Right = lastNode;
                                    }
                                    else
                                    {
                                        node = lastNode;
                                    }
                                }
                            }

                            break;

                        case TokenType.Comma:
                            if (_tokens.Count > 0 && _tokens.Peek().TokenType == TokenType.CloseParentheses)
                            {
                                // If there is a comma in a function call, there should be another parameter followed by a closing comma
                                throw ODataException.BadRequest(ExceptionMessage.UnableToParseFilter($"the function {node?.Name} has a missing parameter or extra comma", token.Position), "$filter");
                            }

                            break;

                        case TokenType.FunctionName:
                            node = new FunctionCallNode(token.Value);
                            break;

                        case TokenType.OpenParentheses:
                            if (_tokens.Count > 0 && _tokens.Peek().TokenType == TokenType.CloseParentheses)
                            {
                                // All OData functions have at least 1 or 2 parameters
                                throw ODataException.BadRequest(ExceptionMessage.UnableToParseFilter($"the function {node?.Name} has no parameters specified", token.Position), "$filter");
                            }

                            _groupingDepth++;
                            stack.Push(node);
                            break;

                        case TokenType.PropertyName:
                            var propertyAccessNode = new PropertyAccessNode(PropertyPath.For(token.Value, _model));

                            if (stack.Count > 0)
                            {
                                stack.Peek().AddParameter(propertyAccessNode);
                            }
                            else
                            {
                                if (binaryNode == null)
                                {
                                    throw ODataException.BadRequest(ExceptionMessage.GenericUnableToParseFilter, "$filter");
                                }

                                binaryNode.Right = propertyAccessNode;
                            }

                            break;

                        default:
                            throw ODataException.BadRequest(ExceptionMessage.UnableToParseFilter($"unexpected {token.Value}", token.Position), "$filter");
                    }
                }

                if (binaryNode != null)
                {
                    return binaryNode;
                }

                return node;
            }

            private QueryNode ParsePropertyAccessNode()
            {
                QueryNode result = null;

                QueryNode leftNode = null;
                BinaryOperatorKind operatorKind = BinaryOperatorKind.None;
                QueryNode rightNode = null;

                while (_tokens.Count > 0)
                {
                    Token token = _tokens.Dequeue();

                    switch (token.TokenType)
                    {
                        case TokenType.Base64Binary:
                        case TokenType.Date:
                        case TokenType.DateTimeOffset:
                        case TokenType.Decimal:
                        case TokenType.Double:
                        case TokenType.Duration:
                        case TokenType.Enum:
                        case TokenType.False:
                        case TokenType.Guid:
                        case TokenType.Integer:
                        case TokenType.Null:
                        case TokenType.Single:
                        case TokenType.String:
                        case TokenType.TimeOfDay:
                        case TokenType.True:
                            rightNode = ConstantNodeParser.ParseConstantNode(token);
                            break;

                        case TokenType.BinaryOperator:
                            if (operatorKind != BinaryOperatorKind.None)
                            {
                                result = new BinaryOperatorNode(leftNode, operatorKind, rightNode);
                                leftNode = null;
                                rightNode = null;
                            }

                            operatorKind = token.Value.ToBinaryOperatorKind();
                            break;

                        case TokenType.CloseParentheses:
                            _groupingDepth--;
                            break;

                        case TokenType.FunctionName:
                            rightNode = new FunctionCallNode(token.Value);
                            break;

                        case TokenType.OpenParentheses:
                            _groupingDepth++;
                            break;

                        case TokenType.PropertyName:
                            var propertyAccessNode = new PropertyAccessNode(PropertyPath.For(token.Value, _model));

                            if (leftNode is null)
                            {
                                leftNode = propertyAccessNode;
                            }
                            else if (rightNode is null)
                            {
                                rightNode = propertyAccessNode;
                            }

                            break;

                        default:
                            throw ODataException.BadRequest(ExceptionMessage.UnableToParseFilter($"unexpected {token.Value}", token.Position), "$filter");
                    }
                }

                result = result is null
                    ? new BinaryOperatorNode(leftNode, operatorKind, rightNode)
                    : new BinaryOperatorNode(result, operatorKind, leftNode ?? rightNode);

                return result;
            }

            private QueryNode ParseQueryNode()
            {
                if (_tokens.Count == 0)
                {
                    throw ODataException.BadRequest(ExceptionMessage.UnableToParseFilter("an incomplete filter has been specified"), "$filter");
                }

                QueryNode node;

                switch (_tokens.Peek().TokenType)
                {
                    case TokenType.FunctionName:
                        node = ParseFunctionCallNode();
                        break;

                    case TokenType.OpenParentheses:
                        _groupingDepth++;
                        _tokens.Dequeue();
                        node = ParseQueryNode();
                        break;

                    case TokenType.PropertyName:
                        node = ParsePropertyAccessNode();
                        break;

                    case TokenType.UnaryOperator:
                        Token token = _tokens.Dequeue();
                        node = ParseQueryNode();
                        node = new UnaryOperatorNode(node, token.Value.ToUnaryOperatorKind());
                        break;

                    default:
                        throw ODataException.BadRequest(ExceptionMessage.UnableToParseFilter($"unexpected {_tokens.Peek().Value}", _tokens.Peek().Position), "$filter");
                }

                return node;
            }

            private void UpdateExpressionTree()
            {
                int initialGroupingDepth = _groupingDepth;

                QueryNode node = ParseQueryNode();

                if (_groupingDepth == initialGroupingDepth)
                {
                    if (_nodeStack.Count == 0)
                    {
                        if (_nextBinaryOperatorKind == BinaryOperatorKind.None)
                        {
                            _nodeStack.Push(node);
                        }
                        else
                        {
                            _nodeStack.Push(new BinaryOperatorNode(node, _nextBinaryOperatorKind, null));
                        }
                    }
                    else
                    {
                        QueryNode leftNode = _nodeStack.Pop();

                        if (leftNode is BinaryOperatorNode binaryNode && binaryNode.Right is null)
                        {
                            binaryNode.Right = node;

                            if (_nextBinaryOperatorKind != BinaryOperatorKind.None)
                            {
                                binaryNode = new BinaryOperatorNode(binaryNode, _nextBinaryOperatorKind, null);
                            }
                        }
                        else
                        {
                            binaryNode = new BinaryOperatorNode(leftNode, _nextBinaryOperatorKind, node);
                        }

                        _nodeStack.Push(binaryNode);
                    }
                }
                else if (_groupingDepth > initialGroupingDepth)
                {
                    _nodeStack.Push(new BinaryOperatorNode(node, _nextBinaryOperatorKind, null));
                }
                else
                {
                    var binaryNode = (BinaryOperatorNode)_nodeStack.Pop();
                    binaryNode.Right = node;

                    if (_nextBinaryOperatorKind == BinaryOperatorKind.None)
                    {
                        _nodeStack.Push(binaryNode);

                        while (_nodeStack.Count > 1)
                        {
                            QueryNode rightNode = _nodeStack.Pop();

                            var binaryParent = (BinaryOperatorNode)_nodeStack.Pop();
                            binaryParent.Right = rightNode;

                            _nodeStack.Push(binaryParent);
                        }
                    }
                    else
                    {
                        if (_groupingDepth == 0 && _nodeStack.Count > 0)
                        {
                            var binaryParent = (BinaryOperatorNode)_nodeStack.Pop();
                            binaryParent.Right = binaryNode;

                            binaryNode = binaryParent;
                        }

                        _nodeStack.Push(new BinaryOperatorNode(binaryNode, _nextBinaryOperatorKind, null));
                    }
                }
            }
        }
    }
}
