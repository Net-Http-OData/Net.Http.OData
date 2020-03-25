﻿using System;
using System.Net;
using Net.Http.OData.Model;
using NorthwindModel;
using Xunit;

namespace Net.Http.OData.Tests.Model
{
    public class EdmComplexTypeTests
    {
        [Fact]
        public void Constructor_Throws_ArgumentNullException_For_Null_Properties()
        {
            Type type = typeof(Customer);

            Assert.Throws<ArgumentNullException>(() => new EdmComplexType(type, null));
        }

        [Fact]
        public void Constructor_Type_Properties_SetsProperties()
        {
            Type type = typeof(Customer);
            var properties = new EdmProperty[0];

            var edmComplexType = new EdmComplexType(type, properties);

            Assert.Null(edmComplexType.BaseType);
            Assert.Same(type, edmComplexType.ClrType);
            Assert.Equal(type.FullName, edmComplexType.FullName);
            Assert.Equal(type.Name, edmComplexType.Name);
            Assert.Same(properties, edmComplexType.Properties);
        }

        [Fact]
        public void Constructor_Type_Type_Properties_SetsProperties()
        {
            Type type = typeof(Manager);
            var baseType = new EdmComplexType(typeof(Employee), new EdmProperty[0]);
            var properties = new EdmProperty[0];

            var edmComplexType = new EdmComplexType(type, properties, baseType);

            Assert.Same(baseType, edmComplexType.BaseType);
            Assert.Same(type, edmComplexType.ClrType);
            Assert.Equal(type.FullName, edmComplexType.FullName);
            Assert.Equal(type.Name, edmComplexType.Name);
            Assert.Same(properties, edmComplexType.Properties);
        }

        [Fact]
        public void Equality_False_IfOtherNotEdmType()
        {
            Type type = typeof(Customer);
            var properties = new EdmProperty[0];

            var edmComplexType1 = new EdmComplexType(type, properties);

            Assert.False(edmComplexType1.Equals("Customer"));
        }

        [Fact]
        public void Equality_False_IfOtherNull()
        {
            Type type = typeof(Customer);
            var properties = new EdmProperty[0];

            var edmComplexType1 = new EdmComplexType(type, properties);

            Assert.False(edmComplexType1.Equals(null));
        }

        [Fact]
        public void Equality_True_IfReferenceIsSame()
        {
            Type type = typeof(Customer);
            var properties = new EdmProperty[0];

            var edmComplexType1 = new EdmComplexType(type, properties);
            EdmComplexType edmComplexType2 = edmComplexType1;

            Assert.True(edmComplexType1.Equals(edmComplexType2));
        }

        [Fact]
        public void Equality_True_IfTypeAreSame()
        {
            Type type = typeof(Customer);
            var properties = new EdmProperty[0];

            var edmComplexType1 = new EdmComplexType(type, properties);
            var edmComplexType2 = new EdmComplexType(type, properties);

            Assert.True(edmComplexType1.Equals(edmComplexType2));
        }

        [Fact]
        public void GetProperty_ReturnsProperty()
        {
            TestHelper.EnsureEDM();

            EdmComplexType edmComplexType = EntityDataModel.Current.EntitySets["Customers"].EdmType;

            EdmProperty edmProperty = edmComplexType.GetProperty("CompanyName");

            Assert.NotNull(edmProperty);
            Assert.Equal("CompanyName", edmProperty.Name);
        }

        [Fact]
        public void GetProperty_Throws_ODataException_If_PropertyNameNotFound()
        {
            TestHelper.EnsureEDM();

            EdmComplexType edmComplexType = EntityDataModel.Current.EntitySets["Customers"].EdmType;

            ODataException odataException = Assert.Throws<ODataException>(() => edmComplexType.GetProperty("Name"));

            Assert.Equal(ExceptionMessage.EdmTypeDoesNotContainProperty("NorthwindModel.Customer", "Name"), odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
        }

        [Fact]
        public void ToString_ReturnsFullName()
        {
            Type type = typeof(Customer);
            var properties = new EdmProperty[0];

            var edmComplexType = new EdmComplexType(type, properties);

            Assert.Equal(edmComplexType.ToString(), edmComplexType.FullName);
        }
    }
}
