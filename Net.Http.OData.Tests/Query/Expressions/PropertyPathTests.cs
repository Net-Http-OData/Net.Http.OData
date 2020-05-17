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
        public void For_EdmComplexType_PropertyPath_MultipleProperties()
        {
            var propertyPath = PropertyPath.For(EntityDataModel.Current.EntitySets["Products"].EdmType, "Category/Name");

            Assert.NotNull(propertyPath.Next);
            Assert.NotNull(propertyPath.Property);
            Assert.Equal("Category", propertyPath.Property.Name);

            propertyPath = propertyPath.Next;

            Assert.Null(propertyPath.Next);
            Assert.NotNull(propertyPath.Property);
            Assert.Equal("Name", propertyPath.Property.Name);
        }

        [Fact]
        public void For_EdmComplexType_PropertyPath_MultipleProperties_Returns_Same_Instance()
            => Assert.Same(
                PropertyPath.For(EntityDataModel.Current.EntitySets["Products"].EdmType, "Category/Name"),
                PropertyPath.For(EntityDataModel.Current.EntitySets["Products"].EdmType, "Category/Name"));

        [Fact]
        public void For_EdmComplexType_PropertyPath_SingleProperty()
        {
            var propertyPath = PropertyPath.For(EntityDataModel.Current.EntitySets["Products"].EdmType, "Name");

            Assert.Null(propertyPath.Next);
            Assert.NotNull(propertyPath.Property);
            Assert.Equal("Name", propertyPath.Property.Name);
        }

        [Fact]
        public void For_EdmComplexType_PropertyPath_SingleProperty_Returns_Same_Instance()
            => Assert.Same(
                PropertyPath.For(EntityDataModel.Current.EntitySets["Products"].EdmType, "Name"),
                PropertyPath.For(EntityDataModel.Current.EntitySets["Products"].EdmType.GetProperty("Name")));

        [Fact]
        public void For_EdmComplexType_PropertyPath_SingleProperty_Returns_Same_Instance_As_For_EdmProperty()
            => Assert.Same(
                PropertyPath.For(EntityDataModel.Current.EntitySets["Products"].EdmType, "Name"),
                PropertyPath.For(EntityDataModel.Current.EntitySets["Products"].EdmType, "Name"));

        [Fact]
        public void For_EdmProperty()
        {
            EdmProperty edmProperty = EntityDataModel.Current.EntitySets["Customers"].EdmType.GetProperty("CompanyName");

            var propertyPath = PropertyPath.For(edmProperty);

            Assert.Null(propertyPath.Next);
            Assert.Equal(edmProperty, propertyPath.Property);
        }

        [Fact]
        public void For_EdmProperty_Returns_Same_Instance()
            => Assert.Same(
                PropertyPath.For(EntityDataModel.Current.EntitySets["Customers"].EdmType.GetProperty("CompanyName")),
                PropertyPath.For(EntityDataModel.Current.EntitySets["Customers"].EdmType.GetProperty("CompanyName")));

        [Fact]
        public void For_Throws_ArgumentNullException_For_Null_EdmProperty()
            => Assert.Throws<ArgumentNullException>(() => PropertyPath.For(null));

        [Fact]
        public void For_Throws_ODataException_For_InvalidNonNavigable()
        {
            ODataException odataException = Assert.Throws<ODataException>(
                () => PropertyPath.For(EntityDataModel.Current.EntitySets["Products"].EdmType, "Colour/Name"));

            Assert.Equal(ExceptionMessage.PropertyNotNavigable("Colour", "Colour/Name"), odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            Assert.Equal("Sample.Model.Product", odataException.Target);
        }

        [Fact]
        public void For_Throws_ODataException_For_InvalidPath()
        {
            ODataException odataException = Assert.Throws<ODataException>(
                () => PropertyPath.For(EntityDataModel.Current.EntitySets["Products"].EdmType, "Category/Definition/Name"));

            Assert.Equal(ExceptionMessage.EdmTypeDoesNotContainProperty("Sample.Model.Category", "Definition"), odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            Assert.Equal("Sample.Model.Category", odataException.Target);
        }
    }
}
