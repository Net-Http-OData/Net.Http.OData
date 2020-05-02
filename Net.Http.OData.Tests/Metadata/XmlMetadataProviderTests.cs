using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Schema;
using Net.Http.OData.Metadata;
using Net.Http.OData.Model;
using Xunit;

namespace Net.Http.OData.Tests.Metadata
{
    public class MetadataProviderTests
    {
        [Fact]
        public void Create_Returns_CSDL()
        {
            TestHelper.EnsureEDM();

            ODataServiceOptions serviceOptions = TestHelper.ODataServiceOptions;

            var expected = XDocument.Parse($@"<edmx:Edmx xmlns:edmx=""http://docs.oasis-open.org/odata/ns/edmx"" Version=""{serviceOptions.MaxVersion}"">
  <edmx:DataServices>
    <Schema xmlns=""http://docs.oasis-open.org/odata/ns/edm"" Namespace=""Sample.Model"">
      <EnumType Name=""AccessLevel"" UnderlyingType=""Edm.Int32"" IsFlags=""true"">
        <Member Name=""None"" Value=""0"" />
        <Member Name=""Read"" Value=""1"" />
        <Member Name=""Write"" Value=""2"" />
        <Member Name=""Delete"" Value=""4"" />
      </EnumType>
      <EnumType Name=""Colour"" UnderlyingType=""Edm.Int32"" IsFlags=""false"">
        <Member Name=""Green"" Value=""1"" />
        <Member Name=""Blue"" Value=""2"" />
        <Member Name=""Red"" Value=""3"" />
      </EnumType>
      <ComplexType Name=""OrderDetail"">
        <Property Name=""OrderId"" Type=""Edm.Int64"" Nullable=""false"" />
        <Property Name=""ProductId"" Type=""Edm.Int32"" Nullable=""false"" />
        <Property Name=""Quantity"" Type=""Edm.Int16"" Nullable=""false"" />
        <Property Name=""UnitPrice"" Type=""Edm.Decimal"" Nullable=""false"" />
        <NavigationProperty Name=""Order"" Type=""Sample.Model.Order"" />
      </ComplexType>
      <EntityType Name=""Categories"">
        <Key>
          <PropertyRef Name=""Name"" />
        </Key>
        <Property Name=""Name"" Type=""Edm.String"" />
      </EntityType>
      <EntityType Name=""Customers"">
        <Key>
          <PropertyRef Name=""CompanyName"" />
        </Key>
        <Property Name=""City"" Type=""Edm.String"" />
        <Property Name=""CompanyName"" Type=""Edm.String"" />
        <Property Name=""Country"" Type=""Edm.String"" />
        <Property Name=""LegacyId"" Type=""Edm.Int32"" Nullable=""false"" />
      </EntityType>
      <EntityType Name=""Employees"">
        <Key>
          <PropertyRef Name=""Id"" />
        </Key>
        <Property Name=""AccessLevel"" Type=""Sample.Model.AccessLevel"" Nullable=""false"" />
        <Property Name=""BirthDate"" Type=""Edm.Date"" Nullable=""false"" />
        <Property Name=""EmailAddress"" Type=""Edm.String"" Nullable=""false"" />
        <Property Name=""Forename"" Type=""Edm.String"" Nullable=""false"" />
        <Property Name=""Id"" Type=""Edm.String"" Nullable=""false"" />
        <Property Name=""ImageData"" Type=""Edm.String"" />
        <Property Name=""JoiningDate"" Type=""Edm.Date"" Nullable=""false"" />
        <Property Name=""LeavingDate"" Type=""Edm.Date"" />
        <Property Name=""Surname"" Type=""Edm.String"" Nullable=""false"" />
        <Property Name=""Title"" Type=""Edm.String"" Nullable=""false"" />
        <NavigationProperty Name=""Manager"" Type=""Sample.Model.Manager"" />
      </EntityType>
      <EntityType Name=""Managers"" BaseType=""Sample.Model.Employee"">
        <Property Name=""AnnualBudget"" Type=""Edm.Decimal"" Nullable=""false"" />
        <NavigationProperty Name=""Employees"" Type=""Collection(Sample.Model.Employee)"" />
      </EntityType>
      <EntityType Name=""Orders"">
        <Key>
          <PropertyRef Name=""OrderId"" />
        </Key>
        <Property Name=""Date"" Type=""Edm.DateTimeOffset"" Nullable=""false"" />
        <Property Name=""Freight"" Type=""Edm.Decimal"" Nullable=""false"" />
        <Property Name=""OrderDetails"" Type=""Collection(Sample.Model.OrderDetail)"" />
        <Property Name=""OrderId"" Type=""Edm.Int64"" Nullable=""false"" />
        <Property Name=""ShipCountry"" Type=""Edm.String"" />
        <Property Name=""TransactionId"" Type=""Edm.Guid"" Nullable=""false"" />
      </EntityType>
      <EntityType Name=""Products"">
        <Key>
          <PropertyRef Name=""ProductId"" />
        </Key>
        <Property Name=""Colour"" Type=""Sample.Model.Colour"" Nullable=""false"" />
        <Property Name=""Deleted"" Type=""Edm.Boolean"" Nullable=""false"" />
        <Property Name=""Description"" Type=""Edm.String"" />
        <Property Name=""Name"" Type=""Edm.String"" />
        <Property Name=""Price"" Type=""Edm.Decimal"" Nullable=""false"" />
        <Property Name=""ProductId"" Type=""Edm.Int32"" Nullable=""false"" />
        <Property Name=""Rating"" Type=""Edm.Int32"" Nullable=""false"" />
        <Property Name=""ReleaseDate"" Type=""Edm.Date"" Nullable=""false"" />
        <NavigationProperty Name=""Category"" Type=""Sample.Model.Category"" />
      </EntityType>
      <EntityContainer Name=""DefaultContainer"">
        <EntitySet Name=""Categories"" EntityType=""Sample.Model.Category"">
          <Annotation Term=""Org.OData.Core.V1.ResourcePath"" String=""Categories"" />
          <Annotation Term=""Org.OData.Capabilities.V1.InsertRestrictions"">
            <Record>
              <PropertyValue Property=""Insertable"" Bool=""true"" />
            </Record>
          </Annotation>
          <Annotation Term=""Org.OData.Capabilities.V1.UpdateRestrictions"">
            <Record>
              <PropertyValue Property=""Updatable"" Bool=""true"" />
            </Record>
          </Annotation>
          <Annotation Term=""Org.OData.Capabilities.V1.DeleteRestrictions"">
            <Record>
              <PropertyValue Property=""Deletable"" Bool=""true"" />
            </Record>
          </Annotation>
        </EntitySet>
        <EntitySet Name=""Customers"" EntityType=""Sample.Model.Customer"">
          <Annotation Term=""Org.OData.Core.V1.ResourcePath"" String=""Customers"" />
          <Annotation Term=""Org.OData.Capabilities.V1.InsertRestrictions"">
            <Record>
              <PropertyValue Property=""Insertable"" Bool=""false"" />
            </Record>
          </Annotation>
          <Annotation Term=""Org.OData.Capabilities.V1.UpdateRestrictions"">
            <Record>
              <PropertyValue Property=""Updatable"" Bool=""true"" />
            </Record>
          </Annotation>
          <Annotation Term=""Org.OData.Capabilities.V1.DeleteRestrictions"">
            <Record>
              <PropertyValue Property=""Deletable"" Bool=""false"" />
            </Record>
          </Annotation>
        </EntitySet>
        <EntitySet Name=""Employees"" EntityType=""Sample.Model.Employee"">
          <Annotation Term=""Org.OData.Core.V1.ResourcePath"" String=""Employees"" />
          <Annotation Term=""Org.OData.Capabilities.V1.InsertRestrictions"">
            <Record>
              <PropertyValue Property=""Insertable"" Bool=""false"" />
            </Record>
          </Annotation>
          <Annotation Term=""Org.OData.Capabilities.V1.UpdateRestrictions"">
            <Record>
              <PropertyValue Property=""Updatable"" Bool=""false"" />
            </Record>
          </Annotation>
          <Annotation Term=""Org.OData.Capabilities.V1.DeleteRestrictions"">
            <Record>
              <PropertyValue Property=""Deletable"" Bool=""false"" />
            </Record>
          </Annotation>
        </EntitySet>
        <EntitySet Name=""Managers"" EntityType=""Sample.Model.Manager"">
          <Annotation Term=""Org.OData.Core.V1.ResourcePath"" String=""Managers"" />
          <Annotation Term=""Org.OData.Capabilities.V1.InsertRestrictions"">
            <Record>
              <PropertyValue Property=""Insertable"" Bool=""false"" />
            </Record>
          </Annotation>
          <Annotation Term=""Org.OData.Capabilities.V1.UpdateRestrictions"">
            <Record>
              <PropertyValue Property=""Updatable"" Bool=""false"" />
            </Record>
          </Annotation>
          <Annotation Term=""Org.OData.Capabilities.V1.DeleteRestrictions"">
            <Record>
              <PropertyValue Property=""Deletable"" Bool=""false"" />
            </Record>
          </Annotation>
        </EntitySet>
        <EntitySet Name=""Orders"" EntityType=""Sample.Model.Order"">
          <Annotation Term=""Org.OData.Core.V1.ResourcePath"" String=""Orders"" />
          <Annotation Term=""Org.OData.Capabilities.V1.InsertRestrictions"">
            <Record>
              <PropertyValue Property=""Insertable"" Bool=""true"" />
            </Record>
          </Annotation>
          <Annotation Term=""Org.OData.Capabilities.V1.UpdateRestrictions"">
            <Record>
              <PropertyValue Property=""Updatable"" Bool=""true"" />
            </Record>
          </Annotation>
          <Annotation Term=""Org.OData.Capabilities.V1.DeleteRestrictions"">
            <Record>
              <PropertyValue Property=""Deletable"" Bool=""true"" />
            </Record>
          </Annotation>
        </EntitySet>
        <EntitySet Name=""Products"" EntityType=""Sample.Model.Product"">
          <Annotation Term=""Org.OData.Core.V1.ResourcePath"" String=""Products"" />
          <Annotation Term=""Org.OData.Capabilities.V1.InsertRestrictions"">
            <Record>
              <PropertyValue Property=""Insertable"" Bool=""true"" />
            </Record>
          </Annotation>
          <Annotation Term=""Org.OData.Capabilities.V1.UpdateRestrictions"">
            <Record>
              <PropertyValue Property=""Updatable"" Bool=""true"" />
            </Record>
          </Annotation>
          <Annotation Term=""Org.OData.Capabilities.V1.DeleteRestrictions"">
            <Record>
              <PropertyValue Property=""Deletable"" Bool=""true"" />
            </Record>
          </Annotation>
        </EntitySet>
      </EntityContainer>
      <Annotations Target=""Sample.Model.DefaultContainer"">
        <Annotation Term=""Org.OData.Capabilities.V1.ConformanceLevel"">
          <EnumMember>Org.OData.Capabilities.V1.ConformanceLevelType/Minimal</EnumMember>
        </Annotation>
        <Annotation Term=""Org.OData.Capabilities.V1.SupportedFormats"">
          <Collection>
            <String>application/json;odata.metadata=none</String>
            <String>application/json;odata.metadata=minimal</String>
          </Collection>
        </Annotation>
        <Annotation Term=""Org.OData.Capabilities.V1.SupportedMetadataFormats"">
          <Collection>
            <String>application/json</String>
            <String>text/plain</String>
          </Collection>
        </Annotation>
        <Annotation Term=""Org.OData.Capabilities.V1.AsynchronousRequestsSupported"" Bool=""false"" />
        <Annotation Term=""Org.OData.Capabilities.V1.BatchContinueOnErrorSupported"" Bool=""false"" />
        <Annotation Term=""Org.OData.Capabilities.V1.CrossJoinSupported"" Bool=""false"" />
        <Annotation Term=""Org.OData.Capabilities.V1.IndexableByKey"" Bool=""true"" />
        <Annotation Term=""Org.OData.Capabilities.V1.TopSupported"" Bool=""true"" />
        <Annotation Term=""Org.OData.Capabilities.V1.SkipSupported"" Bool=""true"" />
        <Annotation Term=""Org.OData.Capabilities.V1.ComputeSupported"" Bool=""false"" />
        <Annotation Term=""Org.OData.Capabilities.V1.BatchSupported"" Bool=""false"" />
        <Annotation Term=""Org.OData.Capabilities.V1.FilterFunctions"">
          <Collection>
            <String>concat</String>
            <String>contains</String>
            <String>endswith</String>
            <String>indexof</String>
            <String>length</String>
            <String>startswith</String>
            <String>substring</String>
            <String>tolower</String>
            <String>toupper</String>
            <String>trim</String>
            <String>date</String>
            <String>day</String>
            <String>fractionalseconds</String>
            <String>hour</String>
            <String>maxdatetime</String>
            <String>mindatetime</String>
            <String>minute</String>
            <String>month</String>
            <String>now</String>
            <String>second</String>
            <String>totaloffsetminutes</String>
            <String>year</String>
            <String>ceiling</String>
            <String>floor</String>
            <String>round</String>
            <String>cast</String>
            <String>isof</String>
          </Collection>
        </Annotation>
        <Annotation Term=""Org.OData.Capabilities.V1.KeyAsSegmentSupported"" Bool=""false"" />
        <Annotation Term=""Org.OData.Capabilities.V1.QuerySegmentSupported"" Bool=""false"" />
        <Annotation Term=""Org.OData.Capabilities.V1.AnnotationValuesInQuerySupported"" Bool=""false"" />
        <Annotation Term=""Org.OData.Capabilities.V1.MediaLocationUpdateSupported"" Bool=""false"" />
      </Annotations>
    </Schema>
  </edmx:DataServices>
</edmx:Edmx>");

            XDocument csdlDocument = XmlMetadataProvider.Create(EntityDataModel.Current, serviceOptions);

            Assert.Equal(expected.ToString(), csdlDocument.ToString());
        }

        /// <summary>
        /// https://github.com/Net-Http-OData/Net.Http.OData/issues/3 - Boolean values in the CSDL which are calculated are cased incorrectly
        /// </summary>
        /// <remarks>
        /// Validates against the published schemas which have been downloaded and stored in the unit tests project.
        /// <![CDATA[http://docs.oasis-open.org/odata/odata/v4.0/errata03/os/complete/schemas/edmx.xsd]]>
        /// <![CDATA[http://docs.oasis-open.org/odata/odata/v4.0/errata03/os/complete/schemas/edm.xsd]]>
        /// </remarks>
        [Fact]
        public void Create_Returns_CSDL_WhichValidatesAgainstXSD()
        {
            TestHelper.EnsureEDM();

            var schemas = new XmlSchemaSet();
            schemas.Add(null, Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Metadata", "Schemas", "v4.0", "edmx.xsd"));
            schemas.Add(null, Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Metadata", "Schemas", "v4.0", "edm.xsd"));

            XDocument csdlDocument = XmlMetadataProvider.Create(EntityDataModel.Current, TestHelper.ODataServiceOptions);
            csdlDocument.Validate(schemas, (s, a) => Assert.Null(a.Message));
        }

        [Fact]
        public void Create_Throws_ArgumentNullException_For_Null_EntityDataModel()
            => Assert.Throws<ArgumentNullException>(() => XmlMetadataProvider.Create(null, TestHelper.ODataServiceOptions));

        [Fact]
        public void Create_Throws_ArgumentNullException_For_Null_ODataServiceOptions()
            => Assert.Throws<ArgumentNullException>(() => XmlMetadataProvider.Create(EntityDataModel.Current, null));
    }
}
