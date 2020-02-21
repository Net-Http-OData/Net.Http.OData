using System.Net;
using Net.Http.OData.Query;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public class FormatQueryOptionTests
    {
        [Fact]
        public void WhenConstructedWithRawValueAtom_AnODataExceptionIsThrown()
        {
            ODataException exception = Assert.Throws<ODataException>(() => new FormatQueryOption("$format=atom"));

            Assert.Equal("The $format 'atom' is not supported by this service, acceptable values are 'json'.", exception.Message);
            Assert.Equal(HttpStatusCode.UnsupportedMediaType, exception.StatusCode);
        }

        [Fact]
        public void WhenConstructedWithRawValueVCard_AnODataExceptionIsThrown()
        {
            ODataException exception = Assert.Throws<ODataException>(() => new FormatQueryOption("$format=text/vcard"));

            Assert.Equal("The $format 'text/vcard' is not supported by this service, acceptable values are 'json'.", exception.Message);
            Assert.Equal(HttpStatusCode.UnsupportedMediaType, exception.StatusCode);
        }

        [Fact]
        public void WhenConstructedWithRawValueXml_AnODataExceptionIsThrown()
        {
            ODataException exception = Assert.Throws<ODataException>(() => new FormatQueryOption("$format=xml"));

            Assert.Equal("The $format 'xml' is not supported by this service, acceptable values are 'json'.", exception.Message);
            Assert.Equal(HttpStatusCode.UnsupportedMediaType, exception.StatusCode);
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
