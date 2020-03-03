using System;
using System.Collections.Generic;
using System.Text;
using Net.Http.OData.Query;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public class SearchQueryOptionTests
    {
        public class WhenConstructedWithASearchExpression
        {
            private readonly SearchQueryOption _option;
            private readonly string _rawValue;

            public WhenConstructedWithASearchExpression()
            {
                _rawValue = "$search=blue OR green";
                _option = new SearchQueryOption(_rawValue);
            }

            [Fact]
            public void TheExpressionShouldBeSet()
            {
                Assert.Equal("blue OR green", _option.Expression);
            }

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
            {
                Assert.Equal(_rawValue, _option.RawValue);
            }
        }
    }
}
