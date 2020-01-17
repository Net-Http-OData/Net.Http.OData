using System;
using Net.Http.OData.Model;
using Net.Http.OData.Query;
using NorthwindModel;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public class PropertyPathTests
    {
        public PropertyPathTests()
        {
            TestHelper.EnsureEDM();
        }

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
        public void WhenConstructed_WithEdmProperty()
        {
            Type type = typeof(Customer);
            var edmComplexType = new EdmComplexType(type, new EdmProperty[0]);

            var edmProperty = new EdmProperty(type.GetProperty("CompanyName"), EdmPrimitiveType.String, edmComplexType, (_) => true);

            var propertyPath = new PropertyPath(edmProperty);

            Assert.Null(propertyPath.Next);
            Assert.Equal(edmProperty, propertyPath.Property);
        }
    }
}
