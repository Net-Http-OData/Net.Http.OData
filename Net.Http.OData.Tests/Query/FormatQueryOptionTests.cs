using System.Net;
using Net.Http.OData.Query;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public class FormatQueryOptionTests
    {
        [Fact]
        public void Constructor_Throws_ODataException_For_FormatAtom()
        {
            ODataException odataException = Assert.Throws<ODataException>(() => new FormatQueryOption("$format=atom"));

            Assert.Equal(ExceptionMessage.QueryOptionValueNotSupported("$format", "atom", "'json'"), odataException.Message);
            Assert.Equal(HttpStatusCode.UnsupportedMediaType, odataException.StatusCode);
        }

        [Fact]
        public void Constructor_Throws_ODataException_For_FormatTextVCard()
        {
            ODataException odataException = Assert.Throws<ODataException>(() => new FormatQueryOption("$format=text/vcard"));

            Assert.Equal(ExceptionMessage.QueryOptionValueNotSupported("$format", "text/vcard", "'json'"), odataException.Message);
            Assert.Equal(HttpStatusCode.UnsupportedMediaType, odataException.StatusCode);
        }

        [Fact]
        public void Constructor_Throws_ODataException_For_FormatXml()
        {
            ODataException odataException = Assert.Throws<ODataException>(() => new FormatQueryOption("$format=xml"));

            Assert.Equal(ExceptionMessage.QueryOptionValueNotSupported("$format", "xml", "'json'"), odataException.Message);
            Assert.Equal(HttpStatusCode.UnsupportedMediaType, odataException.StatusCode);
        }

        public class WhenConstructedWithRawValueJson
        {
            private readonly FormatQueryOption _option;
            private readonly string _rawValue;

            public WhenConstructedWithRawValueJson()
            {
                _rawValue = "$format=json";
                _option = new FormatQueryOption(_rawValue);
            }

            [Fact]
            public void TheMediaTypeHeaderValueShouldBeApplicationJson()
            {
                Assert.Equal("application/json", _option.MediaTypeHeaderValue.MediaType);
            }

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
            {
                Assert.Equal(_rawValue, _option.RawValue);
            }
        }
    }
}
