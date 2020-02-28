using System;
using Xunit;

namespace Net.Http.OData.Tests
{
    public class ODataRequestOptionsTests
    {
        [Fact]
        public void Constructor_Sets_Properties()
        {
            var odataRequestOptions = new ODataRequestOptions(
                new Uri("https://services.odata.org/OData"),
                ODataIsolationLevel.Snapshot,
                ODataMetadataLevel.Minimal,
                ODataVersion.OData40);

            Assert.Equal(ODataIsolationLevel.Snapshot, odataRequestOptions.IsolationLevel);
            Assert.Equal(ODataMetadataLevel.Minimal, odataRequestOptions.MetadataLevel);
            Assert.Equal(new Uri("https://services.odata.org/OData"), odataRequestOptions.ServiceRootUri);
            Assert.Equal(ODataVersion.OData40, odataRequestOptions.Version);
        }

        [Fact]
        public void Constructor_Throws_ArgumentNullException_For_Null_ServiceRootUri()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new ODataRequestOptions(null, ODataIsolationLevel.None, ODataMetadataLevel.None, ODataVersion.OData40));

            Assert.Equal("serviceRootUri", exception.ParamName);
        }

        [Fact]
        public void Constructor_Throws_ArgumentNullException_For_Null_Version()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new ODataRequestOptions(new Uri("https://services.odata.org/OData"), ODataIsolationLevel.None, ODataMetadataLevel.None, null));

            Assert.Equal("version", exception.ParamName);
        }
    }
}
