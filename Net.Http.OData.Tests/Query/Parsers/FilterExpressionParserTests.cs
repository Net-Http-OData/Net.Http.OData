using System;
using System.Net;
using Net.Http.OData.Model;
using Net.Http.OData.Query.Parsers;
using Xunit;

namespace Net.Http.OData.Tests.Query.Parsers
{
    public partial class FilterExpressionParserTests
    {
        [Fact]
        public void Parse_Throws_ArgumentNullException_ForNullModel()
            => Assert.Throws<ArgumentNullException>(() => FilterExpressionParser.Parse("$filter=", null));

        public class InvalidSyntax
        {
            public InvalidSyntax()
            {
                TestHelper.EnsureEDM();
            }

            [Fact]
            public void ParseFunctionEqMissingExpression()
            {
                ODataException exception = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("ceiling(Freight) eq", EntityDataModel.Current.EntitySets["Orders"].EdmType));

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("Unable to parse the specified $filter system query option, the binary operator Equal has no right node", exception.Message);
            }

            [Fact]
            public void ParseFunctionExtraBeginParenthesisEqExpression()
            {
                ODataException exception = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("(ceiling(Freight) eq 32", EntityDataModel.Current.EntitySets["Orders"].EdmType)); ;

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("Unable to parse the specified $filter system query option, an extra opening or missing closing parenthesis may be present", exception.Message);
            }

            [Fact]
            public void ParseFunctionExtraEndParenthesisEqExpression()
            {
                ODataException exception = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("ceiling(Freight) eq 32)", EntityDataModel.Current.EntitySets["Orders"].EdmType)); ;

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("Unable to parse the specified $filter system query option, closing parenthesis not expected", exception.Message);
            }

            [Fact]
            public void ParseFunctionMissingParameterExpression()
            {
                ODataException exception = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("ceiling() eq 32", EntityDataModel.Current.EntitySets["Orders"].EdmType)); ;

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("Unable to parse the specified $filter system query option, the function ceiling has no parameters", exception.Message);
            }

            [Fact]
            public void ParseFunctionMissingParenthesisEqExpression()
            {
                ODataException exception = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("ceiling(Freight eq 32", EntityDataModel.Current.EntitySets["Orders"].EdmType)); ;

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("Unable to parse the specified $filter system query option, an extra opening or missing closing parenthesis may be present", exception.Message);
            }

            [Fact]
            public void ParseFunctionMissingSecondParameterExpression()
            {
                ODataException exception = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("cast(Colour,) eq 20", EntityDataModel.Current.EntitySets["Products"].EdmType)); ;

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("Unable to parse the specified $filter system query option, the function cast has a missing parameter or extra comma", exception.Message);
            }

            [Fact]
            public void ParseNotMissingExpression()
            {
                ODataException exception = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("not", EntityDataModel.Current.EntitySets["Products"].EdmType)); ;

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("Unable to parse the specified $filter system query option, an incomplete filter has been specified", exception.Message);
            }

            [Fact]
            public void ParsePropertyEqMissingExpression()
            {
                ODataException exception = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("Deleted eq", EntityDataModel.Current.EntitySets["Products"].EdmType)); ;

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("Unable to parse the specified $filter system query option, the binary operator Equal has no right node", exception.Message);
            }

            [Fact]
            public void ParsePropertyEqValueAndMissingExpression()
            {
                ODataException exception = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("Deleted eq true and", EntityDataModel.Current.EntitySets["Products"].EdmType)); ;

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("Unable to parse the specified $filter system query option, an incomplete filter has been specified", exception.Message);
            }

            [Fact]
            public void ParsePropertyEqValueOrMissingExpression()
            {
                ODataException exception = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("Deleted eq true or", EntityDataModel.Current.EntitySets["Products"].EdmType)); ;

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("Unable to parse the specified $filter system query option, an incomplete filter has been specified", exception.Message);
            }

            [Fact]
            public void ParseSingleOpeningParenthesis()
            {
                ODataException exception = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("(", EntityDataModel.Current.EntitySets["Orders"].EdmType)); ;

                Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
                Assert.Equal("Unable to parse the specified $filter system query option, an incomplete filter has been specified", exception.Message);
            }
        }
    }
}
