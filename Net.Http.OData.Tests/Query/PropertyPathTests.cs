using System;
using System.Net;
using Net.Http.OData.Model;
using Net.Http.OData.Query;
using NorthwindModel;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public class PropertyPathTests
    {
        public PropertyPathTests() => TestHelper.EnsureEDM();

        [Fact]
        public void For_PropertyPath()
        {
            var propertyPath = PropertyPath.For("Category/Name", EntityDataModel.Current.EntitySets["Products"].EdmType);

            Assert.NotNull(propertyPath.Next);
            Assert.NotNull(propertyPath.Property);
            Assert.Equal("Category", propertyPath.Property.Name);

            propertyPath = propertyPath.Next;

            Assert.Null(propertyPath.Next);
            Assert.NotNull(propertyPath.Property);
            Assert.Equal("Name", propertyPath.Property.Name);
        }

        [Fact]
        public void For_SingleProperty()
        {
            var propertyPath = PropertyPath.For("Name", EntityDataModel.Current.EntitySets["Products"].EdmType);

            Assert.Null(propertyPath.Next);
            Assert.NotNull(propertyPath.Property);
            Assert.Equal("Name", propertyPath.Property.Name);
        }

        [Fact]
        public void Throws_ODataException_For_InvalidNonNavigable()
        {
            ODataException odataException = Assert.Throws<ODataException>(
                () => PropertyPath.For("Colour/Name", EntityDataModel.Current.EntitySets["Products"].EdmType));

            Assert.Equal("The property 'Colour' in the path 'Colour/Name' is not a navigable property.", odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
        }

        [Fact]
        public void Throws_ODataException_For_InvalidPath()
        {
            ODataException odataException = Assert.Throws<ODataException>(
                () => PropertyPath.For("Category/Definition/Name", EntityDataModel.Current.EntitySets["Products"].EdmType));

            Assert.Equal("The type 'NorthwindModel.Category' does not contain a property named 'Definition'", odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
        }

        [Fact]
        public void WhenConstructed_WithEdmProperty()
        {
            Type type = typeof(Customer);
            var edmComplexType = new EdmComplexType(type, new EdmProperty[0]);

            var edmProperty = new EdmProperty(type.GetProperty("CompanyName"), EdmPrimitiveType.String, edmComplexType, new Lazy<bool>(() => true));

            var propertyPath = new PropertyPath(edmProperty);

            Assert.Null(propertyPath.Next);
            Assert.Equal(edmProperty, propertyPath.Property);
        }
    }
}
