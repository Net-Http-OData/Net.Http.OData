using System;
using System.Collections.Generic;
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
            Assert.Contains(ODataFunctionNames.Concat, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.Contains, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.Endswith, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.IndexOf, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.Length, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.Startswith, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.Substring, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.ToLower, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.ToUpper, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.Trim, _odataServiceOptions.SupportedFilterFunctions);

            // Date functions
            Assert.Contains(ODataFunctionNames.Date, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.Day, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.FractionalSeconds, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.Hour, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.MaxDateTime, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.MinDateTime, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.Minute, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.Month, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.Now, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.Second, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.TotalOffsetMinutes, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.Year, _odataServiceOptions.SupportedFilterFunctions);

            // Math functions
            Assert.Contains(ODataFunctionNames.Ceiling, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.Floor, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.Round, _odataServiceOptions.SupportedFilterFunctions);

            // Type functions
            Assert.Contains(ODataFunctionNames.Cast, _odataServiceOptions.SupportedFilterFunctions);
            Assert.Contains(ODataFunctionNames.IsOf, _odataServiceOptions.SupportedFilterFunctions);
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
        public void Validate_ODataRequestOptions_DoesNotThrow_If_AllRequestOptions_Supported()
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
        public void Validate_ODataRequestOptions_Throws_ODataException_If_IsolationLevel_NotSupported()
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
            Assert.Equal(ODataRequestHeaderNames.ODataIsolation, odataException.Target);
        }

        [Fact]
        public void Validate_ODataRequestOptions_Throws_ODataException_If_MetadataLevel_NotSupported()
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
            Assert.Equal("odata.metadata", odataException.Target);
        }

        [Fact]
        public void Validate_ODataRequestOptions_Throws_ODataException_If_ODataMaxVersion_AboveMaxSupported()
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
            Assert.Equal(ODataRequestHeaderNames.ODataMaxVersion, odataException.Target);
        }

        [Fact]
        public void Validate_ODataRequestOptions_Throws_ODataException_If_ODataMaxVersion_BelowMaxSupported()
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
            Assert.Equal(ODataRequestHeaderNames.ODataMaxVersion, odataException.Target);
        }

        [Fact]
        public void Validate_ODataRequestOptions_Throws_ODataException_If_ODataVersion_AboveMinSupported()
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
            Assert.Equal(ODataRequestHeaderNames.ODataVersion, odataException.Target);
        }

        [Fact]
        public void Validate_ODataRequestOptions_Throws_ODataException_If_ODataVersion_BelowMinSupported()
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
            Assert.Equal(ODataRequestHeaderNames.ODataVersion, odataException.Target);
        }

        [Fact]
        public void Validate_RequestedMediaTypes_DoesNotThrow_IfOneOrMoreMediaTypes_Supported()
        {
            var odataServiceOptions = new ODataServiceOptions(
                ODataVersion.MinVersion,
                ODataVersion.MaxVersion,
                new[] { ODataIsolationLevel.None },
                new[] { "application/json", "text/plain" });

            odataServiceOptions.Validate(new[] { "application/json", "application/xml", "text/plain" });
        }

        [Fact]
        public void Validate_RequestedMediaTypes_Throws_ODataException_IfNoMediaTypes_Supported()
        {
            var odataServiceOptions = new ODataServiceOptions(
                ODataVersion.MinVersion,
                ODataVersion.MaxVersion,
                new[] { ODataIsolationLevel.None },
                new[] { "application/json", "text/plain" });

            ODataException odataException = Assert.Throws<ODataException>(() => odataServiceOptions.Validate(new[] { "application/xml" }));

            Assert.Equal(ExceptionMessage.MediaTypeNotAcceptable(odataServiceOptions.SupportedMediaTypes, odataServiceOptions.SupportedMetadataLevels, new[] { "application/xml" }), odataException.Message);
            Assert.Equal(HttpStatusCode.NotAcceptable, odataException.StatusCode);
            Assert.Equal("Accept", odataException.Target);
        }

        [Fact]
        public void Validate_Throws_ArgumentNullException_ForNull_ODataRequestOptions()
            => Assert.Throws<ArgumentNullException>(() => _odataServiceOptions.Validate(default(ODataRequestOptions)));

        [Fact]
        public void Validate_Throws_ArgumentNullException_ForNull_RequestedMediaTypes()
            => Assert.Throws<ArgumentNullException>(() => _odataServiceOptions.Validate(default(IEnumerable<string>)));
    }
}
