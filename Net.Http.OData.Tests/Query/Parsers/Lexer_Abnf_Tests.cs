using System.Net;
using Net.Http.OData.Query.Parsers;
using Xunit;

namespace Net.Http.OData.Tests.Query.Parsers
{
    /// <summary>
    /// Tests using <![CDATA[http://docs.oasis-open.org/odata/odata/v4.0/os/abnf/odata-abnf-testcases.xml]]>.
    /// </summary>
    public class Lexer_Abnf_Tests
    {
        [Theory]
        [InlineData("binary''")]
        [InlineData("binary'Zg=='")]
        [InlineData("binary'Zg'")]
        [InlineData("binary'Zm8='")]
        [InlineData("binary'Zm9v'")]
        [InlineData("binary'Zm9vYg=='")]
        [InlineData("binary'Zm9vYmE='")]
        [InlineData("binary'Zm9vYmFy'")]
        public void Binary(string value)
        {
            var lexer = new Lexer(value);

            Assert.True(lexer.MoveNext());
            Assert.Equal(TokenType.Base64Binary, lexer.Current.TokenType);
            Assert.Equal(value, lexer.Current.Value);
        }

        [Theory]
        [InlineData("2012-09-03")]
        [InlineData("2012-09-10")]
        [InlineData("2012-09-20")]
        [InlineData("0000-01-01")]
        public void Date(string value)
        {
            var lexer = new Lexer(value);

            Assert.True(lexer.MoveNext());
            Assert.Equal(TokenType.Date, lexer.Current.TokenType);
            Assert.Equal(value, lexer.Current.Value);
        }

        [Theory]
        [InlineData("2012-09-03T13:52Z")]
        [InlineData("2012-09-03T22:09:02Z")]
        [InlineData("2012-08-31T18:19:22.1Z")]
        [InlineData("0000-01-01T00:00Z")]
        [InlineData("2012-09-03T14:53+02:00")]
        [InlineData("2012-09-03T12:53Z")]
        [InlineData("2011-12-31T24:00Z")] // Lexer should parse but should fail in ConstantNodeParser
        [InlineData("2011-12-31T24:00:00Z")] // Lexer should parse but should fail in ConstantNodeParser
        [InlineData("2012-09-03T24:00-03:00")] // Lexer should parse but should fail in ConstantNodeParser
        public void DateTimeOffset(string value)
        {
            var lexer = new Lexer(value);

            Assert.True(lexer.MoveNext());
            Assert.Equal(TokenType.DateTimeOffset, lexer.Current.TokenType);
            Assert.Equal(value, lexer.Current.Value);
        }

        [Theory]
        [InlineData("3.14")]
        public void Decimal(string value)
        {
            var lexer = new Lexer(value);

            Assert.True(lexer.MoveNext());
            Assert.Equal(TokenType.Decimal, lexer.Current.TokenType);
            Assert.Equal(value, lexer.Current.Value);
        }

        [Theory]
        [InlineData("3.14d")]
        [InlineData("-0.314e1")]
        [InlineData("-INF")]
        [InlineData("INF")]
        [InlineData("NaN")]
        public void Double(string value)
        {
            var lexer = new Lexer(value);

            Assert.True(lexer.MoveNext());
            Assert.Equal(TokenType.Double, lexer.Current.TokenType);
            Assert.Equal(value, lexer.Current.Value);
        }

        [Theory]
        [InlineData("duration'P6DT23H59M59.9999S'")]
        public void Duration(string value)
        {
            var lexer = new Lexer(value);

            Assert.True(lexer.MoveNext());
            Assert.Equal(TokenType.Duration, lexer.Current.TokenType);
            Assert.Equal(value, lexer.Current.Value);
        }

        [Theory]
        [InlineData("01234567-89ab-cdef-0123-456789abcdef")]
        public void Guid(string value)
        {
            var lexer = new Lexer(value);

            Assert.True(lexer.MoveNext());
            Assert.Equal(TokenType.Guid, lexer.Current.TokenType);
            Assert.Equal(value, lexer.Current.Value);
        }

        [Theory]
        [InlineData("X'1a2B3c4D'")]
        public void NotBinary(string value)
        {
            var lexer = new Lexer(value);

            ODataException odataException = Assert.Throws<ODataException>(() => lexer.MoveNext());
            Assert.Equal(ExceptionMessage.GenericUnableToParseFilter, odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            Assert.Equal(ODataUriNames.FilterQueryOption, odataException.Target);
        }

        [Theory]
        [InlineData("+42.")]
        [InlineData(".1")]
        public void NotDecimal(string value)
        {
            var lexer = new Lexer(value);

            ODataException odataException = Assert.Throws<ODataException>(() => lexer.MoveNext());
            Assert.Equal(ExceptionMessage.GenericUnableToParseFilter, odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            Assert.Equal(ODataUriNames.FilterQueryOption, odataException.Target);
        }

        [Theory]
        [InlineData("P1Y6DT23H59M59.9999S")]
        [InlineData("P1M6DT23H59M59.9999S")]
        public void NotDuration(string value)
        {
            var lexer = new Lexer(value);

            ODataException odataException = Assert.Throws<ODataException>(() => lexer.MoveNext());
            Assert.Equal(ExceptionMessage.GenericUnableToParseFilter, odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            Assert.Equal(ODataUriNames.FilterQueryOption, odataException.Target);
        }

        [Theory]
        [InlineData("01234g67-89ab-cdef-0123-456789abcdef")]
        [InlineData("01234567-89ab-cdef-456789abcdef")]
        public void NotGuid(string value)
        {
            var lexer = new Lexer(value);

            ODataException odataException = Assert.Throws<ODataException>(() => lexer.MoveNext());
            Assert.Equal(ExceptionMessage.GenericUnableToParseFilter, odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            Assert.Equal(ODataUriNames.FilterQueryOption, odataException.Target);
        }

        [Theory]
        [InlineData("0time")]
        [InlineData("No.Dot")]
        public void NotPropertyName(string value)
        {
            var lexer = new Lexer(value);

            ODataException odataException = Assert.Throws<ODataException>(() => lexer.MoveNext());
            Assert.Equal(ExceptionMessage.GenericUnableToParseFilter, odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            Assert.Equal(ODataUriNames.FilterQueryOption, odataException.Target);
        }

        [Fact]
        public void Null()
        {
            string value = "null";
            var lexer = new Lexer(value);

            Assert.True(lexer.MoveNext());
            Assert.Equal(TokenType.Null, lexer.Current.TokenType);
            Assert.Equal(value, lexer.Current.Value);
        }

        [Theory]
        [InlineData("TheQuickBrownFoxSays42")]
        [InlineData("__ID")]
        public void PropertyName(string value)
        {
            var lexer = new Lexer(value);

            Assert.True(lexer.MoveNext());
            Assert.Equal(TokenType.PropertyName, lexer.Current.TokenType);
            Assert.Equal(value, lexer.Current.Value);
        }

        [Theory]
        [InlineData("'ABCDEFGHIHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~!$&('')*+,;=@'")]
        [InlineData("'O''Neil'")]
        public void String(string value)
        {
            var lexer = new Lexer(value);

            Assert.True(lexer.MoveNext());
            Assert.Equal(TokenType.String, lexer.Current.TokenType);
            Assert.Equal(value, lexer.Current.Value);
        }

        [Theory]
        [InlineData("11:22:33")]
        [InlineData("11:22")]
        [InlineData("11:22:33.4444444")]
        [InlineData("24:00:00")] // Lexer should parse but should fail in ConstantNodeParser
        public void TimeOfDay(string value)
        {
            var lexer = new Lexer(value);

            Assert.True(lexer.MoveNext());
            Assert.Equal(TokenType.TimeOfDay, lexer.Current.TokenType);
            Assert.Equal(value, lexer.Current.Value);
        }
    }
}
