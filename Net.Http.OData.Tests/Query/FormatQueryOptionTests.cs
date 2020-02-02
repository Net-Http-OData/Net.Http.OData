using Net.Http.OData.Query;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public class FormatQueryOptionTests
    {
        public class WhenConstructedWithRawValueAtom
        {
            private readonly FormatQueryOption _option;
            private readonly string _rawValue;

            public WhenConstructedWithRawValueAtom()
            {
                _rawValue = "$format=atom";
                _option = new FormatQueryOption(_rawValue);
            }

            [Fact]
            public void TheMediaTypeHeaderValueShouldBeApplicationAtomXml()
            {
                Assert.Equal("application/atom+xml", _option.MediaTypeHeaderValue.MediaType);
            }

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
            {
                Assert.Equal(_rawValue, _option.RawValue);
            }
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

        public class WhenConstructedWithRawValueVCard
        {
            private readonly FormatQueryOption _option;
            private readonly string _rawValue;

            public WhenConstructedWithRawValueVCard()
            {
                _rawValue = "$format=text/vcard";
                _option = new FormatQueryOption(_rawValue);
            }

            [Fact]
            public void TheMediaTypeHeaderValueShouldBeTextVCard()
            {
                Assert.Equal("text/vcard", _option.MediaTypeHeaderValue.MediaType);
            }

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
            {
                Assert.Equal(_rawValue, _option.RawValue);
            }
        }

        public class WhenConstructedWithRawValueXml
        {
            private readonly FormatQueryOption _option;
            private readonly string _rawValue;

            public WhenConstructedWithRawValueXml()
            {
                _rawValue = "$format=xml";
                _option = new FormatQueryOption(_rawValue);
            }

            [Fact]
            public void TheMediaTypeHeaderValueShouldBeApplicationXml()
            {
                Assert.Equal("application/xml", _option.MediaTypeHeaderValue.MediaType);
            }

            [Fact]
            public void TheRawValueShouldEqualTheValuePassedToTheConstructor()
            {
                Assert.Equal(_rawValue, _option.RawValue);
            }
        }
    }
}
