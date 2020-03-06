using System;
using Xunit;

namespace Net.Http.OData.Tests
{
    public class ODataServiceOptionsTests
    {
        private readonly ODataServiceOptions _serviceOptions = new ODataServiceOptions(ODataVersion.MinVersion, ODataVersion.MaxVersion, new[] { ODataIsolationLevel.None }, new[] { "application/json" });

        [Fact]
        public void Constructor_Throws_ArgumentNullException_ForNull_MaxVersion()
            => Assert.Throws<ArgumentNullException>(() => new ODataServiceOptions(ODataVersion.MinVersion, null, new[] { ODataIsolationLevel.None }, new[] { "application/json" }));

        [Fact]
        public void Constructor_Throws_ArgumentNullException_ForNull_MinVersion()
            => Assert.Throws<ArgumentNullException>(() => new ODataServiceOptions(null, ODataVersion.MaxVersion, new[] { ODataIsolationLevel.None }, new[] { "application/json" }));

        [Fact]
        public void Constructor_Throws_ArgumentNullException_ForNull_SupportedIsolationLevels()
            => Assert.Throws<ArgumentNullException>(() => new ODataServiceOptions(ODataVersion.MinVersion, ODataVersion.MaxVersion, null, new[] { "application/json" }));

        [Fact]
        public void Constructor_Throws_ArgumentNullException_ForNull_SupportedMediaTypes()
            => Assert.Throws<ArgumentNullException>(() => new ODataServiceOptions(ODataVersion.MinVersion, ODataVersion.MaxVersion, new[] { ODataIsolationLevel.None }, null));

        [Fact]
        public void MaxVersion_IsODataVersionMaxVersion() => Assert.Equal(ODataVersion.MaxVersion, _serviceOptions.MaxVersion);

        [Fact]
        public void MinVersion_IsODataVersionMinVersion() => Assert.Equal(ODataVersion.MinVersion, _serviceOptions.MinVersion);

        [Fact]
        public void SupportedFilterFunctions_AreSet()
        {
            Assert.Equal(27, _serviceOptions.SupportedFilterFunctions.Count);

            // String functions
            Assert.Contains("concat", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("contains", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("endswith", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("indexof", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("length", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("startswith", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("substring", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("tolower", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("toupper", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("trim", _serviceOptions.SupportedFilterFunctions);

            // Date functions
            Assert.Contains("date", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("day", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("fractionalseconds", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("hour", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("maxdatetime", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("mindatetime", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("minute", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("month", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("now", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("second", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("totaloffsetminutes", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("year", _serviceOptions.SupportedFilterFunctions);

            // Math functions
            Assert.Contains("ceiling", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("floor", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("round", _serviceOptions.SupportedFilterFunctions);

            // Type functions
            Assert.Contains("cast", _serviceOptions.SupportedFilterFunctions);
            Assert.Contains("isof", _serviceOptions.SupportedFilterFunctions);
        }

        [Fact]
        public void SupportedIsolationLevels_AreSet()
        {
            Assert.Equal(1, _serviceOptions.SupportedIsolationLevels.Count);

            Assert.Contains(ODataIsolationLevel.None, _serviceOptions.SupportedIsolationLevels);

            Assert.DoesNotContain(ODataIsolationLevel.Snapshot, _serviceOptions.SupportedIsolationLevels);
        }

        [Fact]
        public void SupportedMetadataLevels_AreSet()
        {
            Assert.Equal(2, _serviceOptions.SupportedMetadataLevels.Count);

            Assert.Contains(ODataMetadataLevel.None, _serviceOptions.SupportedMetadataLevels);
            Assert.Contains(ODataMetadataLevel.Minimal, _serviceOptions.SupportedMetadataLevels);

            Assert.DoesNotContain(ODataMetadataLevel.Full, _serviceOptions.SupportedMetadataLevels);
        }
    }
}
