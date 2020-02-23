using System;
using Net.Http.OData.Model;
using NorthwindModel;
using Xunit;

namespace Net.Http.OData.Tests.Model
{
    public class EdmPropertyTests
    {
        [Fact]
        public void Constructor_SetsProperties()
        {
            Type type = typeof(Customer);
            var edmComplexType = new EdmComplexType(type, new EdmProperty[0]);

            var edmProperty = new EdmProperty(type.GetProperty("CompanyName"), EdmPrimitiveType.String, edmComplexType, new Lazy<bool>(() => true));

            Assert.NotNull(edmProperty.ClrProperty);
            Assert.Same(edmComplexType, edmProperty.DeclaringType);
            Assert.True(edmProperty.IsNavigable);
            Assert.Equal("CompanyName", edmProperty.Name);
            Assert.Same(EdmPrimitiveType.String, edmProperty.PropertyType);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_ForNullDeclaringType()
        {
            Assert.Throws<ArgumentNullException>(() => new EdmProperty(typeof(Customer).GetProperty("CompanyName"), EdmPrimitiveType.String, null, new Lazy<bool>(() => false)));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_ForNullPropertyInfo()
        {
            Type type = typeof(Customer);
            var edmComplexType = new EdmComplexType(type, new EdmProperty[0]);

            Assert.Throws<ArgumentNullException>(() => new EdmProperty(null, EdmPrimitiveType.String, edmComplexType, new Lazy<bool>(() => false)));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_ForNullPropertyType()
        {
            Type type = typeof(Customer);
            var edmComplexType = new EdmComplexType(type, new EdmProperty[0]);

            Assert.Throws<ArgumentNullException>(() => new EdmProperty(type.GetProperty("CompanyName"), null, edmComplexType, new Lazy<bool>(() => false)));
        }

        [Fact]
        public void IsNullable_ReturnsFalse_ForClass_WithRequiredAttribute()
        {
            Type type = typeof(Employee);
            EdmTypeCache.Map.TryGetValue(typeof(string), out EdmType edmType);
            var edmProperty = new EdmProperty(type.GetProperty("Forename"), edmType, new EdmComplexType(type, new EdmProperty[0]), new Lazy<bool>(() => false));

            Assert.False(edmProperty.IsNullable);
        }

        [Fact]
        public void IsNullable_ReturnsFalse_ForStruct()
        {
            Type type = typeof(Customer);
            EdmTypeCache.Map.TryGetValue(typeof(int), out EdmType edmType);
            var edmProperty = new EdmProperty(type.GetProperty("LegacyId"), edmType, new EdmComplexType(type, new EdmProperty[0]), new Lazy<bool>(() => false));

            Assert.False(edmProperty.IsNullable);
        }

        [Fact]
        public void IsNullable_ReturnsTrue_ForClass()
        {
            Type type = typeof(Customer);
            EdmTypeCache.Map.TryGetValue(typeof(string), out EdmType edmType);
            var edmProperty = new EdmProperty(type.GetProperty("CompanyName"), edmType, new EdmComplexType(type, new EdmProperty[0]), new Lazy<bool>(() => false));

            Assert.True(edmProperty.IsNullable);
        }

        [Fact]
        public void IsNullable_ReturnsTrue_ForCollection()
        {
            Type type = typeof(Order);
            EdmTypeCache.Map.TryGetValue(typeof(OrderDetail), out EdmType edmType);
            var edmProperty = new EdmProperty(type.GetProperty("OrderDetails"), new EdmCollectionType(type, edmType), new EdmComplexType(typeof(Customer), new EdmProperty[0]), new Lazy<bool>(() => false));

            Assert.True(edmProperty.IsNullable);
        }

        [Fact]
        public void IsNullable_ReturnsTrue_ForNullableStruct()
        {
            Type type = typeof(Employee);
            EdmTypeCache.Map.TryGetValue(typeof(int?), out EdmType edmType);
            var edmProperty = new EdmProperty(type.GetProperty("LeavingDate"), edmType, new EdmComplexType(type, new EdmProperty[0]), new Lazy<bool>(() => false));

            Assert.True(edmProperty.IsNullable);
        }

        [Fact]
        public void ToString_ReturnsName()
        {
            Type type = typeof(Customer);
            var edmComplexType = new EdmComplexType(type, new EdmProperty[0]);

            var edmProperty = new EdmProperty(type.GetProperty("CompanyName"), EdmPrimitiveType.String, edmComplexType, new Lazy<bool>(() => false));

            Assert.Equal(edmProperty.ToString(), edmProperty.Name);
        }
    }
}
