using Net.Http.OData.Query.Parsers;
using Xunit;

namespace Net.Http.OData.Tests.Query.Parsers
{
    public class ParserSettingsTests
    {
        [Fact]
        public void CultureInfo()
            => Assert.Equal(System.Globalization.CultureInfo.InvariantCulture, ParserSettings.CultureInfo);

        [Fact]
        public void DateTimeStyles()
            => Assert.Equal(System.Globalization.DateTimeStyles.AssumeUniversal, ParserSettings.DateTimeStyles);

        [Fact]
        public void ODataDateFormat()
            => Assert.Equal("yyyy-MM-dd", ParserSettings.ODataDateFormat);
    }
}
