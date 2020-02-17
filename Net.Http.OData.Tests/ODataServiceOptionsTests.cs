using Xunit;

namespace Net.Http.OData.Tests
{
    public class ODataServiceOptionsTests
    {
        [Fact]
        public void Default_MaxVersion_IsODataVersionMaxVersion() => Assert.Equal(ODataVersion.MaxVersion, ODataServiceOptions.Default.MaxVersion);

        [Fact]
        public void Default_MinVersion_IsODataVersionMinVersion() => Assert.Equal(ODataVersion.MinVersion, ODataServiceOptions.Default.MinVersion);

        [Fact]
        public void Default_SupportedFilterFunctions_AreSet()
        {
            ODataServiceOptions serviceOptions = ODataServiceOptions.Default;

            Assert.Equal(26, serviceOptions.SupportedFilterFunctions.Count);

            Assert.Contains("cast", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("ceiling", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("concat", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("contains", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("day", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("endswith", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("floor", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("fractionalseconds", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("hour", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("indexof", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("isof", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("length", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("maxdatetime", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("mindatetime", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("minute", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("month", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("now", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("replace", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("round", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("second", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("startswith", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("substring", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("tolower", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("toupper", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("trim", serviceOptions.SupportedFilterFunctions);
            Assert.Contains("year", serviceOptions.SupportedFilterFunctions);
        }

        [Fact]
        public void Default_SupportedMetadataLevels_AreSet()
        {
            ODataServiceOptions serviceOptions = ODataServiceOptions.Default;

            Assert.Equal(2, serviceOptions.SupportedMetadataLevels.Count);

            Assert.Contains(ODataMetadataLevel.None, serviceOptions.SupportedMetadataLevels);
            Assert.Contains(ODataMetadataLevel.Minimal, serviceOptions.SupportedMetadataLevels);

            Assert.DoesNotContain(ODataMetadataLevel.Full, serviceOptions.SupportedMetadataLevels);
        }
    }
}
