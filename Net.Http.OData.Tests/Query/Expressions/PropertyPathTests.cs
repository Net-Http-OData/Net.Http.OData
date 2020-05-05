using System;
using System.Net;
using Net.Http.OData.Model;
using Net.Http.OData.Query.Expressions;
using Xunit;

namespace Net.Http.OData.Tests.Query.Expressions
{
    public class PropertyPathTests
    {
        public PropertyPathTests() => TestHelper.EnsureEDM();

        [Fact]
        public void For_EdmProperty()
        {
            EdmProperty edmProperty = EntityDataModel.Current.EntitySets["Customers"].EdmType.GetProperty("CompanyName");

            var propertyPath = PropertyPath.For(edmProperty);

            Assert.Null(propertyPath.Next);
            Assert.Equal(edmProperty, propertyPath.Property);
        }

        [Fact]
        public void For_EdmProperty_ReturnsSameInstance()
        {
            EdmProperty edmProperty = EntityDataModel.Current.EntitySets["Customers"].EdmType.GetProperty("CompanyName");

            Assert.Same(PropertyPath.For(edmProperty), PropertyPath.For(edmProperty));
        }

        [Fact]
        public void For_PropertyPath_MultipleProperties()
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
        public void For_PropertyPath_SingleProperty()
        {
            var propertyPath = PropertyPath.For("Name", EntityDataModel.Current.EntitySets["Products"].EdmType);

            Assert.Null(propertyPath.Next);
            Assert.NotNull(propertyPath.Property);
            Assert.Equal("Name", propertyPath.Property.Name);
        }

        [Fact]
        public void For_PropertyPath_SingleProperty_ReturnsSameInstance()
            => Assert.Same(PropertyPath.For("Name", EntityDataModel.Current.EntitySets["Products"].EdmType), PropertyPath.For("Name", EntityDataModel.Current.EntitySets["Products"].EdmType));

        [Fact]
        public void For_Throws_ArgumentNullException_For_Null_EdmProperty()
            => Assert.Throws<ArgumentNullException>(() => PropertyPath.For(null));

        [Fact]
        public void For_Throws_ODataException_For_InvalidNonNavigable()
        {
            ODataException odataException = Assert.Throws<ODataException>(
                () => PropertyPath.For("Colour/Name", EntityDataModel.Current.EntitySets["Products"].EdmType));

            Assert.Equal(ExceptionMessage.PropertyNotNavigable("Colour", "Colour/Name"), odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            Assert.Equal("Sample.Model.Product", odataException.Target);
        }

        [Fact]
        public void For_Throws_ODataException_For_InvalidPath()
        {
            ODataException odataException = Assert.Throws<ODataException>(
                () => PropertyPath.For("Category/Definition/Name", EntityDataModel.Current.EntitySets["Products"].EdmType));

            Assert.Equal(ExceptionMessage.EdmTypeDoesNotContainProperty("Sample.Model.Category", "Definition"), odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            Assert.Equal("Sample.Model.Category", odataException.Target);
        }
    }
}
