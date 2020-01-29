using Net.Http.OData.Model;
using Net.Http.OData.Query;
using Xunit;

namespace Net.Http.OData.Tests
{
    public class ODataUtilityTests
    {
        [Fact]
        public void ODataContext()
        {
            Assert.Equal(
                "https://services.odata.org/OData/$metadata",
                ODataUtility.ODataContext(ODataMetadataLevel.Full, "https", "services.odata.org", "/OData/Products"));

            Assert.Equal(
                "https://services.odata.org/OData/$metadata",
                ODataUtility.ODataContext(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products"));

            Assert.Equal(
                "https://services.odata.org/OData/$metadata",
                ODataUtility.ODataContext(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products/"));

            Assert.Equal(
                "https://services.odata.org/OData/$metadata",
                ODataUtility.ODataContext(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products(1)"));

            Assert.Equal(
                "https://services.odata.org/OData/$metadata",
                ODataUtility.ODataContext(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products(1)/Name/$value"));

            Assert.Equal(
                "https://services.odata.org/OData/$metadata",
                ODataUtility.ODataContext(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products%281%29"));

            Assert.Equal(
                "https://services.odata.org/OData/$metadata",
                ODataUtility.ODataContext(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products$select=Name"));

            Assert.Null(ODataUtility.ODataContext(ODataMetadataLevel.None, "https", "services.odata.org", "/OData/Products"));
        }

        [Fact]
        public void ODataContext_WithEntitySet()
        {
            TestHelper.EnsureEDM();

            EntitySet entitySet = EntityDataModel.Current.EntitySets["Products"];

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Products",
                ODataUtility.ODataContext(ODataMetadataLevel.Full, "https", "services.odata.org", "/OData/Products/", entitySet));

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Products",
                ODataUtility.ODataContext(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products/", entitySet));

            Assert.Null(ODataUtility.ODataContext(ODataMetadataLevel.None, "https", "services.odata.org", "/OData/Products", entitySet));
        }

        [Fact]
        public void ODataContext_WithEntitySet_AndSelectProperties()
        {
            TestHelper.EnsureEDM();

            EntitySet entitySet = EntityDataModel.Current.EntitySets["Products"];

            var queryOptions = new ODataQueryOptions("?$select=Name,Description,Price", entitySet);

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Products(Name,Description,Price)",
                ODataUtility.ODataContext(ODataMetadataLevel.Full, "https", "services.odata.org", "/OData/Products/", entitySet, queryOptions.Select));

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Products(Name,Description,Price)",
                ODataUtility.ODataContext(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products/", entitySet, queryOptions.Select));

            Assert.Null(ODataUtility.ODataContext(ODataMetadataLevel.None, "https", "services.odata.org", "/OData/Products/", entitySet, queryOptions.Select));
        }

        [Fact]
        public void ODataContext_WithEntitySet_AndSelectStar()
        {
            TestHelper.EnsureEDM();

            EntitySet entitySet = EntityDataModel.Current.EntitySets["Products"];

            var queryOptions = new ODataQueryOptions("?$select=*", entitySet);

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Products(*)",
                ODataUtility.ODataContext(ODataMetadataLevel.Full, "https", "services.odata.org", "/OData/Products/", entitySet, queryOptions.Select));

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Products(*)",
                ODataUtility.ODataContext(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products/", entitySet, queryOptions.Select));

            Assert.Null(ODataUtility.ODataContext(ODataMetadataLevel.None, "https", "services.odata.org", "/OData/Products/", entitySet, queryOptions.Select));
        }

        [Fact]
        public void ODataContext_WithEntitySet_EntityKey()
        {
            TestHelper.EnsureEDM();

            EntitySet entitySet = EntityDataModel.Current.EntitySets["Products"];

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Products/$entity",
                ODataUtility.ODataContext<int>(ODataMetadataLevel.Full, "https", "services.odata.org", "/OData/Products/", entitySet));

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Products/$entity",
                ODataUtility.ODataContext<int>(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products/", entitySet));

            Assert.Null(ODataUtility.ODataContext<int>(ODataMetadataLevel.None, "https", "services.odata.org", "/OData/Products/", entitySet));
        }

        [Fact]
        public void ODataContext_WithEntitySet_EntityKeyInt_Property()
        {
            TestHelper.EnsureEDM();

            EntitySet entitySet = EntityDataModel.Current.EntitySets["Products"];

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Products(10142276)/Name",
                ODataUtility.ODataContext(ODataMetadataLevel.Full, "https", "services.odata.org", "/OData/Products/", entitySet, 10142276, "Name"));

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Products(10142276)/Name",
                ODataUtility.ODataContext(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Products/", entitySet, 10142276, "Name"));

            Assert.Null(ODataUtility.ODataContext(ODataMetadataLevel.None, "https", "services.odata.org", "/OData/Products/", entitySet, 10142276, "Name"));
        }

        [Fact]
        public void ODataContext_WithEntitySet_EntityKeyString_Property()
        {
            TestHelper.EnsureEDM();

            EntitySet entitySet = EntityDataModel.Current.EntitySets["Customers"];

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Customers('Tesco')/CompanyName",
                ODataUtility.ODataContext(ODataMetadataLevel.Full, "https", "services.odata.org", "/OData/Customers/", entitySet, "Tesco", "CompanyName"));

            Assert.Equal(
                "https://services.odata.org/OData/$metadata#Customers('Tesco')/CompanyName",
                ODataUtility.ODataContext(ODataMetadataLevel.Minimal, "https", "services.odata.org", "/OData/Customers/", entitySet, "Tesco", "CompanyName"));

            Assert.Null(ODataUtility.ODataContext(ODataMetadataLevel.None, "https", "services.odata.org", "/OData/Customers/", entitySet, "Tesco", "CompanyName"));
        }

        [Fact]
        public void ODataId_WithEntityKeyInt_Property()
        {
            TestHelper.EnsureEDM();

            EntitySet entitySet = EntityDataModel.Current.EntitySets["Products"];

            Assert.Equal(
                "https://services.odata.org/OData/Products(10142276)",
                ODataUtility.ODataId("https", "services.odata.org", "/OData/Products/", entitySet, 10142276));
        }

        [Fact]
        public void ODataId_WithEntityKeyString_Property()
        {
            TestHelper.EnsureEDM();

            EntitySet entitySet = EntityDataModel.Current.EntitySets["Customers"];

            Assert.Equal(
                "https://services.odata.org/OData/Customers('Tesco')",
                ODataUtility.ODataId("https", "services.odata.org", "/OData/Customers/", entitySet, "Tesco"));
        }

        [Fact]
        public void ODataServiceRootUri()
        {
            Assert.Equal(
                "https://services.odata.org/OData/",
                ODataUtility.ODataServiceRootUri("https", "services.odata.org", "/OData").ToString());

            Assert.Equal(
                "https://services.odata.org/OData/",
                ODataUtility.ODataServiceRootUri("https", "services.odata.org", "/OData/Products").ToString());

            Assert.Equal(
                "https://services.odata.org/OData/",
                ODataUtility.ODataServiceRootUri("https", "services.odata.org", "/OData/Products/").ToString());

            Assert.Equal(
                "https://services.odata.org/OData/",
                ODataUtility.ODataServiceRootUri("https", "services.odata.org", "/OData/Products(1)").ToString());

            Assert.Equal(
                "https://services.odata.org/OData/",
                ODataUtility.ODataServiceRootUri("https", "services.odata.org", "/OData/Products(1)/Name/$value").ToString());

            Assert.Equal(
                "https://services.odata.org/OData/",
                ODataUtility.ODataServiceRootUri("https", "services.odata.org", "/OData/Products%281%29").ToString());

            Assert.Equal(
                "https://services.odata.org/OData/",
                ODataUtility.ODataServiceRootUri("https", "services.odata.org", "/OData/Products$select=Name").ToString());
        }

        [Fact]
        public void ResolveEntitySetName_ReturnsEntitySetName_IfODataPath()
        {
            Assert.Equal("Products", ODataUtility.ResolveEntitySetName("/odata/Products"));
            Assert.Equal("Products", ODataUtility.ResolveEntitySetName("/OData/Products"));
            Assert.Equal("Products", ODataUtility.ResolveEntitySetName("/OData/Products/"));
            Assert.Equal("Products", ODataUtility.ResolveEntitySetName("/OData/Products(1)"));
            Assert.Equal("Products", ODataUtility.ResolveEntitySetName("/OData/Products(1)/Name/$value"));
            Assert.Equal("Products", ODataUtility.ResolveEntitySetName("/OData/Products%281%29"));
            Assert.Equal("Products", ODataUtility.ResolveEntitySetName("/OData/Products$select=Name"));
        }

        [Fact]
        public void ResolveEntitySetName_ReturnsNull_IfNotODataPath()
        {
            Assert.Null(ODataUtility.ResolveEntitySetName("/odata"));
            Assert.Null(ODataUtility.ResolveEntitySetName("/api/Products"));
        }
    }
}
