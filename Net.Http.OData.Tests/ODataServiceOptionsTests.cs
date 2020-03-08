using System;
using System.Net;
using Xunit;

namespace Net.Http.OData.Tests
{
    public class ODataServiceOptionsTests
    {
        private readonly ODataServiceOptions _odataServiceOptions = new ODataServiceOptions(
            ODataVersion.MinVersion,
            ODataVersion.MaxVersion,
            new[] { ODataIsolationLevel.None },
            new[] { "application/json" });

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
        public void MaxVersion_IsODataVersion_IsSet()
            => Assert.Equal(ODataVersion.MaxVersion, _odataServiceOptions.MaxVersion);

        [Fact]
        public void MinVersion_IsODataVersion_IsSet()
            => Assert.Equal(ODataVersion.MinVersion, _odataServiceOptions.MinVersion);

        [Fact]
        public void SupportedFilterFunctions_AreSet()
        {
            Assert.Equal(27, _odataServiceOptions.SupportedFilterFunctions.Count);

            // String functions
            Assert.Contains("concat", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("contains", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("endswith", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("indexof", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("length", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("startswith", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("substring", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("tolower", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("toupper", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("trim", _odataServiceOptions.SupportedFilterFunctions);

            // Date functions
            Assert.Contains("date", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("day", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("fractionalseconds", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("hour", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("maxdatetime", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("mindatetime", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("minute", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("month", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("now", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("second", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("totaloffsetminutes", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("year", _odataServiceOptions.SupportedFilterFunctions);

            // Math functions
            Assert.Contains("ceiling", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("floor", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("round", _odataServiceOptions.SupportedFilterFunctions);

            // Type functions
            Assert.Contains("cast", _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains("isof", _odataServiceOptions.SupportedFilterFunctions);
        }

        [Fact]
        public void SupportedIsolationLevels_AreSet()
        {
            Assert.Equal(1, _odataServiceOptions.SupportedIsolationLevels.Count);

            Assert.Contains(ODataIsolationLevel.None, _odataServiceOptions.SupportedIsolationLevels);

            Assert.DoesNotContain(ODataIsolationLevel.Snapshot, _odataServiceOptions.SupportedIsolationLevels);
        }

        [Fact]
        public void SupportedMediaTypes_AreSet()
        {
            Assert.Equal(1, _odataServiceOptions.SupportedMediaTypes.Count);

            Assert.Contains("application/json", _odataServiceOptions.SupportedMediaTypes);
        }

        [Fact]
        public void SupportedMetadataLevels_AreSet()
        {
            Assert.Equal(2, _odataServiceOptions.SupportedMetadataLevels.Count);

            Assert.Contains(ODataMetadataLevel.None, _odataServiceOptions.SupportedMetadataLevels);
            Assert.Contains(ODataMetadataLevel.Minimal, _odataServiceOptions.SupportedMetadataLevels);

            Assert.DoesNotContain(ODataMetadataLevel.Full, _odataServiceOptions.SupportedMetadataLevels);
        }

        [Fact]
        public void Validate_DoesNotThrow_If_AllRequestOptions_Supported()
        {
            var odataRequestOptions = new ODataRequestOptions(
                new Uri("https://services.odata.org/OData"),
                ODataIsolationLevel.None,
                ODataMetadataLevel.Minimal,
                ODataVersion.OData40,
                ODataVersion.OData40);

            var odataServiceOptions = new ODataServiceOptions(
                ODataVersion.MinVersion,
                ODataVersion.MaxVersion,
                new[] { ODataIsolationLevel.None },
                new[] { "application/json" });

            odataServiceOptions.Validate(odataRequestOptions);
        }

        [Fact]
        public void Validate_Throws_ArgumentNullException_ForNullODataRequestOptions()
            => Assert.Throws<ArgumentNullException>(() => _odataServiceOptions.Validate(null));

        [Fact]
        public void Validate_Throws_ODataException_If_IsolationLevel_NotSupported()
        {
            var odataRequestOptions = new ODataRequestOptions(
                new Uri("https://services.odata.org/OData"),
                ODataIsolationLevel.Snapshot,
                ODataMetadataLevel.Minimal,
                ODataVersion.OData40,
                ODataVersion.OData40);

            var odataServiceOptions = new ODataServiceOptions(
                ODataVersion.MinVersion,
                ODataVersion.MaxVersion,
                new[] { ODataIsolationLevel.None },
                new[] { "application/json" });

            ODataException odataException = Assert.Throws<ODataException>(() => odataServiceOptions.Validate(odataRequestOptions));

            Assert.Equal(ExceptionMessage.ODataIsolationLevelNotSupported("Snapshot"), odataException.Message);
            Assert.Equal(HttpStatusCode.PreconditionFailed, odataException.StatusCode);
        }

        [Fact]
        public void Validate_Throws_ODataException_If_MetadataLevel_NotSupported()
        {
            var odataRequestOptions = new ODataRequestOptions(
                new Uri("https://services.odata.org/OData"),
                ODataIsolationLevel.None,
                ODataMetadataLevel.Full,
                ODataVersion.OData40,
                ODataVersion.OData40);

            var odataServiceOptions = new ODataServiceOptions(
                ODataVersion.MinVersion,
                ODataVersion.MaxVersion,
                new[] { ODataIsolationLevel.None },
                new[] { "application/json" });

            ODataException odataException = Assert.Throws<ODataException>(() => odataServiceOptions.Validate(odataRequestOptions));

            Assert.Equal(ExceptionMessage.ODataMetadataLevelNotSupported("full", new[] { "none", "minimal" }), odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
        }

        [Fact]
        public void Validate_Throws_ODataException_If_ODataMaxVersion_AboveMaxSupported()
        {
            var odataRequestOptions = new ODataRequestOptions(
                new Uri("https://services.odata.org/OData"),
                ODataIsolationLevel.None,
                ODataMetadataLevel.Minimal,
                ODataVersion.OData40,
                ODataVersion.Parse("5.0"));

            var odataServiceOptions = new ODataServiceOptions(
                ODataVersion.MinVersion,
                ODataVersion.MaxVersion,
                new[] { ODataIsolationLevel.None },
                new[] { "application/json" });

            ODataException odataException = Assert.Throws<ODataException>(() => odataServiceOptions.Validate(odataRequestOptions));

            Assert.Equal(ExceptionMessage.ODataMaxVersionNotSupported(odataRequestOptions.ODataMaxVersion, odataServiceOptions.MinVersion, odataServiceOptions.MaxVersion), odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
        }

        [Fact]
        public void Validate_Throws_ODataException_If_ODataMaxVersion_BelowMaxSupported()
        {
            var odataRequestOptions = new ODataRequestOptions(
                new Uri("https://services.odata.org/OData"),
                ODataIsolationLevel.None,
                ODataMetadataLevel.Minimal,
                ODataVersion.OData40, // symantically this makes no sense but the scenario is needed for the test case.
                ODataVersion.Parse("3.0"));

            var odataServiceOptions = new ODataServiceOptions(
                ODataVersion.MinVersion,
                ODataVersion.MaxVersion,
                new[] { ODataIsolationLevel.None },
                new[] { "application/json" });

            ODataException odataException = Assert.Throws<ODataException>(() => odataServiceOptions.Validate(odataRequestOptions));

            Assert.Equal(ExceptionMessage.ODataMaxVersionNotSupported(odataRequestOptions.ODataMaxVersion, odataServiceOptions.MinVersion, odataServiceOptions.MaxVersion), odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
        }

        [Fact]
        public void Validate_Throws_ODataException_If_ODataVersion_AboveMinSupported()
        {
            var odataRequestOptions = new ODataRequestOptions(
                new Uri("https://services.odata.org/OData"),
                ODataIsolationLevel.None,
                ODataMetadataLevel.Minimal,
                ODataVersion.Parse("5.0"), // symantically this makes no sense but the scenario is needed for the test case.
                ODataVersion.OData40);

            var odataServiceOptions = new ODataServiceOptions(
                ODataVersion.MinVersion,
                ODataVersion.MaxVersion,
                new[] { ODataIsolationLevel.None },
                new[] { "application/json" });

            ODataException odataException = Assert.Throws<ODataException>(() => odataServiceOptions.Validate(odataRequestOptions));

            Assert.Equal(ExceptionMessage.ODataVersionNotSupported(odataRequestOptions.ODataVersion, odataServiceOptions.MinVersion, odataServiceOptions.MaxVersion), odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
        }

        [Fact]
        public void Validate_Throws_ODataException_If_ODataVersion_BelowMinSupported()
        {
            var odataRequestOptions = new ODataRequestOptions(
                new Uri("https://services.odata.org/OData"),
                ODataIsolationLevel.None,
                ODataMetadataLevel.Minimal,
                ODataVersion.Parse("3.0"), // symantically this makes no sense but the scenario is needed for the test case.
                ODataVersion.OData40);

            var odataServiceOptions = new ODataServiceOptions(
                ODataVersion.MinVersion,
                ODataVersion.MaxVersion,
                new[] { ODataIsolationLevel.None },
                new[] { "application/json" });

            ODataException odataException = Assert.Throws<ODataException>(() => odataServiceOptions.Validate(odataRequestOptions));

            Assert.Equal(ExceptionMessage.ODataVersionNotSupported(odataRequestOptions.ODataVersion, odataServiceOptions.MinVersion, odataServiceOptions.MaxVersion), odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
        }
    }
}
