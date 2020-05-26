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

            Assert.Equal(ExceptionMessage.QueryOptionValueNotSupported(ODataUriNames.FormatQueryOption, "atom", "'json, application/json'"), odataException.Message);
            Assert.Equal(HttpStatusCode.NotAcceptable, odataException.StatusCode);
            Assert.Equal(ODataUriNames.FormatQueryOption, odataException.Target);
        }

        [Fact]
        public void Constructor_Throws_ODataException_For_FormatTextVCard()
        {
            ODataException odataException = Assert.Throws<ODataException>(() => new FormatQueryOption("$format=text/vcard"));

            Assert.Equal(ExceptionMessage.QueryOptionValueNotSupported(ODataUriNames.FormatQueryOption, "text/vcard", "'json, application/json'"), odataException.Message);
            Assert.Equal(HttpStatusCode.NotAcceptable, odataException.StatusCode);
            Assert.Equal(ODataUriNames.FormatQueryOption, odataException.Target);
        }

        [Fact]
        public void Constructor_Throws_ODataException_For_FormatXml()
        {
            ODataException odataException = Assert.Throws<ODataException>(() => new FormatQueryOption("$format=xml"));

            Assert.Equal(ExceptionMessage.QueryOptionValueNotSupported(ODataUriNames.FormatQueryOption, "xml", "'json, application/json'"), odataException.Message);
            Assert.Equal(HttpStatusCode.NotAcceptable, odataException.StatusCode);
            Assert.Equal(ODataUriNames.FormatQueryOption, odataException.Target);
        }

        [Fact]
        public void Constructor_Throws_ODataException_For_FormatXml_AndMetadataLevel()
        {
            ODataException odataException = Assert.Throws<ODataException>(() => new FormatQueryOption("$format=xml;odata.metadata=none"));

            Assert.Equal(ExceptionMessage.QueryOptionValueNotSupported(ODataUriNames.FormatQueryOption, "xml", "'json, application/json'"), odataException.Message);
            Assert.Equal(HttpStatusCode.NotAcceptable, odataException.StatusCode);
            Assert.Equal(ODataUriNames.FormatQueryOption, odataException.Target);
        }

        public class WhenConstructedWithRawValueApplicationJson
        {
            private readonly FormatQueryOption _option;
            private readonly string _rawValue;

            public WhenConstructedWithRawValueApplicationJson()
            {
                _rawValue = "$format=application/json";
                _option = new FormatQueryOption(_rawValue);
            }

            [Fact]
            public void TheMediaTypeHeaderValueShouldBeApplicationJson()
                => Assert.Equal("application/json", _option.MediaTypeHeaderValue.MediaType);

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
                => Assert.Equal(_rawValue, _option.RawValue);
        }

        public class WhenConstructedWithRawValueApplicationJsonAndMetadataLevel
        {
            private readonly FormatQueryOption _option;
            private readonly string _rawValue;

            public WhenConstructedWithRawValueApplicationJsonAndMetadataLevel()
            {
                _rawValue = "$format=application/json;odata.metadata=none";
                _option = new FormatQueryOption(_rawValue);
            }

            [Fact]
            public void TheMediaTypeHeaderValueShouldBeApplicationJson()
                => Assert.Equal("application/json", _option.MediaTypeHeaderValue.MediaType);

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
                => Assert.Equal(_rawValue, _option.RawValue);
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
                => Assert.Equal("application/json", _option.MediaTypeHeaderValue.MediaType);

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
                => Assert.Equal(_rawValue, _option.RawValue);
        }
    }
}
