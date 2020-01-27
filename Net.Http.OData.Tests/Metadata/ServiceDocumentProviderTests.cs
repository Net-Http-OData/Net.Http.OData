using System;
using System.Collections.Generic;
using System.Linq;
using Net.Http.OData.Metadata;
using Net.Http.OData.Model;
using Xunit;

namespace Net.Http.OData.Tests.Metadata
{
    public class ServiceDocumentProviderTests
    {
        [Fact]
        public void Create_MetadataMinimum()
        {
            TestHelper.EnsureEDM();

            List<ServiceDocumentItem> serviceDocumentItems = ServiceDocumentProvider.Create(
                EntityDataModel.Current,
                new ODataRequestOptions(new Uri("https://services.odata.org/OData"), ODataIsolationLevel.None, ODataMetadataLevel.Minimal))
                .ToList();

            Assert.Equal(6, serviceDocumentItems.Count);

            Assert.Equal("EntitySet", serviceDocumentItems[0].Kind);
            Assert.Equal("Categories", serviceDocumentItems[0].Name);
            Assert.Equal("Categories", serviceDocumentItems[0].Url.ToString());

            Assert.Equal("EntitySet", serviceDocumentItems[1].Kind);
            Assert.Equal("Customers", serviceDocumentItems[1].Name);
            Assert.Equal("Customers", serviceDocumentItems[1].Url.ToString());

            Assert.Equal("EntitySet", serviceDocumentItems[2].Kind);
            Assert.Equal("Employees", serviceDocumentItems[2].Name);
            Assert.Equal("Employees", serviceDocumentItems[2].Url.ToString());

            Assert.Equal("EntitySet", serviceDocumentItems[3].Kind);
            Assert.Equal("Managers", serviceDocumentItems[3].Name);
            Assert.Equal("Managers", serviceDocumentItems[3].Url.ToString());

            Assert.Equal("EntitySet", serviceDocumentItems[4].Kind);
            Assert.Equal("Orders", serviceDocumentItems[4].Name);
            Assert.Equal("Orders", serviceDocumentItems[4].Url.ToString());

            Assert.Equal("EntitySet", serviceDocumentItems[5].Kind);
            Assert.Equal("Products", serviceDocumentItems[5].Name);
            Assert.Equal("Products", serviceDocumentItems[5].Url.ToString());
        }

        [Fact]
        public void Create_MetadataNone()
        {
            TestHelper.EnsureEDM();

            List<ServiceDocumentItem> serviceDocumentItems = ServiceDocumentProvider.Create(
                EntityDataModel.Current,
                new ODataRequestOptions(new Uri("https://services.odata.org/OData/"), ODataIsolationLevel.None, ODataMetadataLevel.None))
                .ToList();

            Assert.Equal(6, serviceDocumentItems.Count);

            Assert.Equal("EntitySet", serviceDocumentItems[0].Kind);
            Assert.Equal("Categories", serviceDocumentItems[0].Name);
            Assert.Equal("https://services.odata.org/OData/Categories", serviceDocumentItems[0].Url.ToString());

            Assert.Equal("EntitySet", serviceDocumentItems[1].Kind);
            Assert.Equal("Customers", serviceDocumentItems[1].Name);
            Assert.Equal("https://services.odata.org/OData/Customers", serviceDocumentItems[1].Url.ToString());

            Assert.Equal("EntitySet", serviceDocumentItems[2].Kind);
            Assert.Equal("Employees", serviceDocumentItems[2].Name);
            Assert.Equal("https://services.odata.org/OData/Employees", serviceDocumentItems[2].Url.ToString());

            Assert.Equal("EntitySet", serviceDocumentItems[3].Kind);
            Assert.Equal("Managers", serviceDocumentItems[3].Name);
            Assert.Equal("https://services.odata.org/OData/Managers", serviceDocumentItems[3].Url.ToString());

            Assert.Equal("EntitySet", serviceDocumentItems[4].Kind);
            Assert.Equal("Orders", serviceDocumentItems[4].Name);
            Assert.Equal("https://services.odata.org/OData/Orders", serviceDocumentItems[4].Url.ToString());

            Assert.Equal("EntitySet", serviceDocumentItems[5].Kind);
            Assert.Equal("Products", serviceDocumentItems[5].Name);
            Assert.Equal("https://services.odata.org/OData/Products", serviceDocumentItems[5].Url.ToString());
        }
    }
}
