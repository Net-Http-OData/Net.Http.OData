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
        public void Parse_Throws_ArgumentNullException_For_NullModel()
            => Assert.Throws<ArgumentNullException>(() => FilterExpressionParser.Parse("$filter=", null));

        public class InvalidSyntax
        {
            public InvalidSyntax() => TestHelper.EnsureEDM();

            [Fact]
            public void ParseFunctionEqMissingExpression()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("ceiling(Freight) eq", EntityDataModel.Current.EntitySets["Orders"].EdmType));

                Assert.Equal(ExceptionMessage.UnableToParseFilter("the binary operator Equal has no right node"), odataException.Message);
                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            }

            [Fact]
            public void ParseFunctionExtraBeginParenthesisEqExpression()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("(ceiling(Freight) eq 32", EntityDataModel.Current.EntitySets["Orders"].EdmType)); ;

                Assert.Equal(ExceptionMessage.UnableToParseFilter("an extra opening or missing closing parenthesis may be present"), odataException.Message);
                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            }

            [Fact]
            public void ParseFunctionExtraEndParenthesisEqExpression()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("ceiling(Freight) eq 32)", EntityDataModel.Current.EntitySets["Orders"].EdmType)); ;

                Assert.Equal(ExceptionMessage.UnableToParseFilter("the closing parenthesis not expected at position 23"), odataException.Message);
                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            }

            [Fact]
            public void ParseFunctionInvalidSyntax()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("cast(Colour, not", EntityDataModel.Current.EntitySets["Products"].EdmType)); ;

                Assert.Equal(ExceptionMessage.UnableToParseFilter("unexpected not at position 14"), odataException.Message);
                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            }

            [Fact]
            public void ParseFunctionMissingParameterExpression()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("ceiling() eq 32", EntityDataModel.Current.EntitySets["Orders"].EdmType)); ;

                Assert.Equal(ExceptionMessage.UnableToParseFilter("the function ceiling has no parameters specified at position 8"), odataException.Message);
                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            }

            [Fact]
            public void ParseFunctionMissingParenthesisEqExpression()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("ceiling(Freight eq 32", EntityDataModel.Current.EntitySets["Orders"].EdmType)); ;

                Assert.Equal(ExceptionMessage.UnableToParseFilter("an extra opening or missing closing parenthesis may be present"), odataException.Message);
                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            }

            [Fact]
            public void ParseFunctionMissingSecondParameterExpression()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("cast(Colour,) eq 20", EntityDataModel.Current.EntitySets["Products"].EdmType)); ;

                Assert.Equal(ExceptionMessage.UnableToParseFilter("the function cast has a missing parameter or extra comma at position 12"), odataException.Message);
                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            }

            [Fact]
            public void ParseInvalidPropertyEqualsSyntax()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("Name eq %", EntityDataModel.Current.EntitySets["Products"].EdmType)); ;

                Assert.Equal(ExceptionMessage.GenericUnableToParseFilter, odataException.Message);
                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            }

            [Fact]
            public void ParseInvalidSyntax()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("true", EntityDataModel.Current.EntitySets["Products"].EdmType)); ;

                Assert.Equal(ExceptionMessage.UnableToParseFilter("unexpected true at position 1"), odataException.Message);
                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            }

            [Fact]
            public void ParseLeftBinaryNodeMissingExpression()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("and Deleted eq true", EntityDataModel.Current.EntitySets["Products"].EdmType)); ;

                Assert.Equal(ExceptionMessage.UnableToParseFilter("an incomplete filter has been specified"), odataException.Message);
                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            }

            [Fact]
            public void ParseNotMissingExpression()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("not", EntityDataModel.Current.EntitySets["Products"].EdmType)); ;

                Assert.Equal(ExceptionMessage.UnableToParseFilter("an incomplete filter has been specified"), odataException.Message);
                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            }

            [Fact]
            public void ParsePropertyEqMissingExpression()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("Deleted eq", EntityDataModel.Current.EntitySets["Products"].EdmType)); ;

                Assert.Equal(ExceptionMessage.UnableToParseFilter("the binary operator Equal has no right node"), odataException.Message);
                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            }

            [Fact]
            public void ParsePropertyEqValueAndMissingExpression()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("Deleted eq true and", EntityDataModel.Current.EntitySets["Products"].EdmType)); ;

                Assert.Equal(ExceptionMessage.UnableToParseFilter("an incomplete filter has been specified"), odataException.Message);
                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            }

            [Fact]
            public void ParsePropertyEqValueOrMissingExpression()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("Deleted eq true or", EntityDataModel.Current.EntitySets["Products"].EdmType)); ;

                Assert.Equal(ExceptionMessage.UnableToParseFilter("an incomplete filter has been specified"), odataException.Message);
                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            }

            [Fact]
            public void ParsePropertyInvalidSyntax()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("Colour not", EntityDataModel.Current.EntitySets["Products"].EdmType)); ;

                Assert.Equal(ExceptionMessage.UnableToParseFilter("unexpected not at position 8"), odataException.Message);
                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            }

            [Fact]
            public void ParseSingleOpeningParenthesis()
            {
                ODataException odataException = Assert.Throws<ODataException>(
                    () => FilterExpressionParser.Parse("(", EntityDataModel.Current.EntitySets["Orders"].EdmType)); ;

                Assert.Equal(ExceptionMessage.UnableToParseFilter("an incomplete filter has been specified"), odataException.Message);
                Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            }
        }
    }
}
