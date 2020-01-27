using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Xunit;

namespace Net.Http.OData.Tests
{
    public class UriUtilityTests
    {
        [Fact]
        public void ODataEntityUri_WithEntityKeyInt_Property()
        {
            TestHelper.EnsureEDM();

            EntitySet entitySet = EntityDataModel.Current.EntitySets["Products"];

            Assert.Equal(
                "https://services.odata.org/OData/Products(10142276)",
                UriUtility.ODataEntityUri("https", "services.odata.org", "/OData/Products/", entitySet, 10142276).ToString());
        }

        [Fact]
        public void ODataEntityUri_WithEntityKeyString_Property()
        {
            TestHelper.EnsureEDM();

            EntitySet entitySet = EntityDataModel.Current.EntitySets["Customers"];

            Assert.Equal(
                "https://services.odata.org/OData/Customers('Tesco')",
                UriUtility.ODataEntityUri("https", "services.odata.org", "/OData/Customers/", entitySet, "Tesco").ToString());
        }

        [Fact]
        public void ODataServiceContextUri()
        {
            Assert.Equal(
                "https://services.odata.org/OData/$metadata",
                UriUtility.ODataServiceContextUri(ODataMetadataLevel.Full, "https", "services.odata.org", "/OData/Products").ToString());

            Assert.Equal(
                "https://services.odata.org/OData/$metadata",
                UriUtility.ODataServiceContextUri(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products").ToString());

            Assert.Equal(
                "https://services.odata.org/OData/$metadata",
                UriUtility.ODataServiceContextUri(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products/").ToString());

            Assert.Equal(
                "https://services.odata.org/OData/$metadata",
                UriUtility.ODataServiceContextUri(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products(1)").ToString());

            Assert.Equal(
                "https://services.odata.org/OData/$metadata",
                UriUtility.ODataServiceContextUri(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products(1)/Name/$value").ToString());

            Assert.Equal(
                "https://services.odata.org/OData/$metadata",
                UriUtility.ODataServiceContextUri(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products%281%29").ToString());

            Assert.Equal(
                "https://services.odata.org/OData/$metadata",
                UriUtility.ODataServiceContextUri(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products$select=Name").ToString());

            Assert.Null(UriUtility.ODataServiceContextUri(ODataMetadataLevel.None, "https", "services.odata.org", "/OData/Products"));
        }

        [Fact]
        public void ODataServiceContextUri_WithEntitySet()
        {
            TestHelper.EnsureEDM();

            EntitySet entitySet = EntityDataModel.Current.EntitySets["Products"];

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Products",
                UriUtility.ODataServiceContextUri(ODataMetadataLevel.Full, "https", "services.odata.org", "/OData/Products/", entitySet).ToString());

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Products",
                UriUtility.ODataServiceContextUri(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products/", entitySet).ToString());

            Assert.Null(UriUtility.ODataServiceContextUri(ODataMetadataLevel.None, "https", "services.odata.org", "/OData/Products", entitySet));
        }

        [Fact]
        public void ODataServiceContextUri_WithEntitySet_AndSelectProperties()
        {
            TestHelper.EnsureEDM();

            EntitySet entitySet = EntityDataModel.Current.EntitySets["Products"];

            var queryOptions = new ODataQueryOptions("?$select=Name,Description,Price", entitySet);

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Products(Name,Description,Price)",
                UriUtility.ODataServiceContextUri(ODataMetadataLevel.Full, "https", "services.odata.org", "/OData/Products/", entitySet, queryOptions.Select).ToString());

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Products(Name,Description,Price)",
                UriUtility.ODataServiceContextUri(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products/", entitySet, queryOptions.Select).ToString());

            Assert.Null(UriUtility.ODataServiceContextUri(ODataMetadataLevel.None, "https", "services.odata.org", "/OData/Products/", entitySet, queryOptions.Select));
        }

        [Fact]
        public void ODataServiceContextUri_WithEntitySet_AndSelectStar()
        {
            TestHelper.EnsureEDM();

            EntitySet entitySet = EntityDataModel.Current.EntitySets["Products"];

            var queryOptions = new ODataQueryOptions("?$select=*", entitySet);

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Products(*)",
                UriUtility.ODataServiceContextUri(ODataMetadataLevel.Full, "https", "services.odata.org", "/OData/Products/", entitySet, queryOptions.Select).ToString());

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Products(*)",
                UriUtility.ODataServiceContextUri(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products/", entitySet, queryOptions.Select).ToString());

            Assert.Null(UriUtility.ODataServiceContextUri(ODataMetadataLevel.None, "https", "services.odata.org", "/OData/Products/", entitySet, queryOptions.Select));
        }

        [Fact]
        public void ODataServiceContextUri_WithEntitySet_EntityKey()
        {
            TestHelper.EnsureEDM();

            EntitySet entitySet = EntityDataModel.Current.EntitySets["Products"];

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Products/$entity",
                UriUtility.ODataServiceContextUri(ODataMetadataLevel.Full, "https", "services.odata.org", "/OData/Products/", entitySet, 10142276).ToString());

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Products/$entity",
                UriUtility.ODataServiceContextUri(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products/", entitySet, 10142276).ToString());

            Assert.Null(UriUtility.ODataServiceContextUri(ODataMetadataLevel.None, "https", "services.odata.org", "/OData/Products/", entitySet, 10142276));
        }

        [Fact]
        public void ODataServiceContextUri_WithEntitySet_EntityKeyInt_Property()
        {
            TestHelper.EnsureEDM();

            EntitySet entitySet = EntityDataModel.Current.EntitySets["Products"];

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Products(10142276)/Name",
                UriUtility.ODataServiceContextUri(ODataMetadataLevel.Full, "https", "services.odata.org", "/OData/Products/", entitySet, 10142276, "Name").ToString());

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Products(10142276)/Name",
                UriUtility.ODataServiceContextUri(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products/", entitySet, 10142276, "Name").ToString());

            Assert.Null(UriUtility.ODataServiceContextUri(ODataMetadataLevel.None, "https", "services.odata.org", "/OData/Products/", entitySet, 10142276, "Name"));
        }

        [Fact]
        public void ODataServiceContextUri_WithEntitySet_EntityKeyString_Property()
        {
            TestHelper.EnsureEDM();

            EntitySet entitySet = EntityDataModel.Current.EntitySets["Customers"];

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Customers('Tesco')/CompanyName",
                UriUtility.ODataServiceContextUri(ODataMetadataLevel.Full, "https", "services.odata.org", "/OData/Customers/", entitySet, "Tesco", "CompanyName").ToString());

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Customers('Tesco')/CompanyName",
                UriUtility.ODataServiceContextUri(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Customers/", entitySet, "Tesco", "CompanyName").ToString());

            Assert.Null(UriUtility.ODataServiceContextUri(ODataMetadataLevel.None, "https", "services.odata.org", "/OData/Customers/", entitySet, "Tesco", "CompanyName"));
        }

        [Fact]
        public void ODataServiceRootUri()
        {
            Assert.Equal(
                "https://services.odata.org/OData/",
                UriUtility.ODataServiceRootUri("https", "services.odata.org", "/OData").ToString());

            Assert.Equal(
                "https://services.odata.org/OData/",
                UriUtility.ODataServiceRootUri("https", "services.odata.org", "/OData/Products").ToString());

            Assert.Equal(
                "https://services.odata.org/OData/",
                UriUtility.ODataServiceRootUri("https", "services.odata.org", "/OData/Products/").ToString());

            Assert.Equal(
                "https://services.odata.org/OData/",
                UriUtility.ODataServiceRootUri("https", "services.odata.org", "/OData/Products(1)").ToString());

            Assert.Equal(
                "https://services.odata.org/OData/",
                UriUtility.ODataServiceRootUri("https", "services.odata.org", "/OData/Products(1)/Name/$value").ToString());

            Assert.Equal(
                "https://services.odata.org/OData/",
                UriUtility.ODataServiceRootUri("https", "services.odata.org", "/OData/Products%281%29").ToString());

            Assert.Equal(
                "https://services.odata.org/OData/",
                UriUtility.ODataServiceRootUri("https", "services.odata.org", "/OData/Products$select=Name").ToString());
        }

        [Fact]
        public void ResolveEntitySetName_ReturnsEntitySetName_IfODataPath()
        {
            Assert.Equal("Products", UriUtility.ResolveEntitySetName("/odata/Products"));
            Assert.Equal("Products", UriUtility.ResolveEntitySetName("/OData/Products"));
            Assert.Equal("Products", UriUtility.ResolveEntitySetName("/OData/Products/"));
            Assert.Equal("Products", UriUtility.ResolveEntitySetName("/OData/Products(1)"));
            Assert.Equal("Products", UriUtility.ResolveEntitySetName("/OData/Products(1)/Name/$value"));
            Assert.Equal("Products", UriUtility.ResolveEntitySetName("/OData/Products%281%29"));
            Assert.Equal("Products", UriUtility.ResolveEntitySetName("/OData/Products$select=Name"));
        }

        [Fact]
        public void ResolveEntitySetName_ReturnsNull_IfNotODataPath()
        {
            Assert.Null(UriUtility.ResolveEntitySetName("/odata"));
            Assert.Null(UriUtility.ResolveEntitySetName("/api/Products"));
        }
    }
}
