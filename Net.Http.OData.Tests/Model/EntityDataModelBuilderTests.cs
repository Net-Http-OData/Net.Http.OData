using System;
using Net.Http.OData.Model;
using Sample.Model;
using Xunit;

namespace Net.Http.OData.Tests.Model
{
    public class EntityDataModelBuilderTests
    {
        [Fact]
        public void CollectionNamesCaseInsensitiveByDefault_RegisterCollectionThrowsArgumentExceptionForDuplicateKeyVaryingOnlyOnCasing()
        {
            var entityDataModelBuilder = new EntityDataModelBuilder(StringComparer.OrdinalIgnoreCase);
            entityDataModelBuilder.RegisterEntitySet<Category>("Categories", x => x.Name);

            ArgumentException exception = Assert.Throws<ArgumentException>(() => entityDataModelBuilder.RegisterEntitySet<Category>("categories", x => x.Name));
            Assert.Equal("An item with the same key has already been added. Key: categories", exception.Message);
        }

        [Fact]
        public void CollectionNamesCaseInsensitiveByDefault_ResolveCollectionVaryingCollectionNameCasing()
        {
            var entityDataModelBuilder = new EntityDataModelBuilder(StringComparer.OrdinalIgnoreCase);
            entityDataModelBuilder.RegisterEntitySet<Category>("Categories", x => x.Name);

            EntityDataModel entityDataModel = entityDataModelBuilder.BuildModel();

            Assert.Same(entityDataModel.EntitySets["categories"], entityDataModel.EntitySets["Categories"]);
        }

        [Fact]
        public void RegisterEntitySet_Throws_ArgumentNullException_ForNullEntityKeyExpression()
            => Assert.Throws<ArgumentNullException>(() => new EntityDataModelBuilder(StringComparer.OrdinalIgnoreCase).RegisterEntitySet<Category>("Categories", null));

        public class WhenCalling_BuildModelWith_Models_AndCustomEntitySetName
        {
            private readonly EntityDataModel _entityDataModel;

            public WhenCalling_BuildModelWith_Models_AndCustomEntitySetName()
            {
                var entityDataModelBuilder = new EntityDataModelBuilder(StringComparer.OrdinalIgnoreCase);
                entityDataModelBuilder.RegisterEntitySet<Category>("Categories", x => x.Name, Capabilities.Insertable | Capabilities.Updatable | Capabilities.Deletable);
                entityDataModelBuilder.RegisterEntitySet<Customer>("Customers", x => x.CompanyName, Capabilities.Updatable);
                entityDataModelBuilder.RegisterEntitySet<Employee>("Employees", x => x.Id);
                entityDataModelBuilder.RegisterEntitySet<Manager>("Managers", x => x.Id);
                entityDataModelBuilder.RegisterEntitySet<Order>("Orders", x => x.OrderId, Capabilities.Insertable | Capabilities.Updatable);
                entityDataModelBuilder.RegisterEntitySet<Product>("Products", x => x.ProductId, Capabilities.Insertable | Capabilities.Updatable);

                _entityDataModel = entityDataModelBuilder.BuildModel();
            }

            [Fact]
            public void EntityDataModel_Current_IsSetToTheReturnedModel() => Assert.Same(_entityDataModel, EntityDataModel.Current);

            [Fact]
            public void The_Categories_CollectionIsCorrect()
            {
                Assert.True(_entityDataModel.EntitySets.ContainsKey("Categories"));

                EntitySet entitySet = _entityDataModel.EntitySets["Categories"];

                Assert.Equal("Categories", entitySet.Name);
                Assert.Equal(Capabilities.Insertable | Capabilities.Updatable | Capabilities.Deletable, entitySet.Capabilities);

                EdmComplexType edmComplexType = entitySet.EdmType;

                Assert.Null(edmComplexType.BaseType);
                Assert.Equal(typeof(Category), edmComplexType.ClrType);
                Assert.Equal("Sample.Model.Category", edmComplexType.FullName);
                Assert.Equal("Category", edmComplexType.Name);
                Assert.Equal(2, edmComplexType.Properties.Count);

                Assert.Same(edmComplexType, edmComplexType.Properties[0].DeclaringType);
                Assert.False(edmComplexType.Properties[0].IsNavigable);
                Assert.False(edmComplexType.Properties[0].IsNullable);
                Assert.Equal("Description", edmComplexType.Properties[0].Name);
                Assert.Same(EdmPrimitiveType.String, edmComplexType.Properties[0].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[1].DeclaringType);
                Assert.False(edmComplexType.Properties[1].IsNavigable);
                Assert.False(edmComplexType.Properties[1].IsNullable);
                Assert.Equal("Name", edmComplexType.Properties[1].Name);
                Assert.Same(EdmPrimitiveType.String, edmComplexType.Properties[1].PropertyType);

                Assert.Same(edmComplexType.Properties[1], entitySet.EntityKey);
            }

            [Fact]
            public void The_Customers_CollectionIsCorrect()
            {
                Assert.True(_entityDataModel.EntitySets.ContainsKey("Customers"));

                EntitySet entitySet = _entityDataModel.EntitySets["Customers"];

                Assert.Equal("Customers", entitySet.Name);
                Assert.Equal(Capabilities.Updatable, entitySet.Capabilities);

                EdmComplexType edmComplexType = entitySet.EdmType;

                Assert.Null(edmComplexType.BaseType);
                Assert.Equal(typeof(Customer), edmComplexType.ClrType);
                Assert.Equal("Sample.Model.Customer", edmComplexType.FullName);
                Assert.Equal("Customer", edmComplexType.Name);
                Assert.Equal(10, edmComplexType.Properties.Count);

                Assert.Same(edmComplexType, edmComplexType.Properties[0].DeclaringType);
                Assert.True(edmComplexType.Properties[0].IsNavigable);
                Assert.False(edmComplexType.Properties[0].IsNullable);
                Assert.Equal("AccountManager", edmComplexType.Properties[0].Name);
                Assert.Same(EdmType.GetEdmType(typeof(Employee)), edmComplexType.Properties[0].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[1].DeclaringType);
                Assert.False(edmComplexType.Properties[1].IsNavigable);
                Assert.False(edmComplexType.Properties[1].IsNullable);
                Assert.Equal("Address", edmComplexType.Properties[1].Name);
                Assert.Same(EdmPrimitiveType.String, edmComplexType.Properties[1].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[2].DeclaringType);
                Assert.False(edmComplexType.Properties[2].IsNavigable);
                Assert.False(edmComplexType.Properties[2].IsNullable);
                Assert.Equal("City", edmComplexType.Properties[2].Name);
                Assert.Same(EdmPrimitiveType.String, edmComplexType.Properties[2].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[3].DeclaringType);
                Assert.False(edmComplexType.Properties[3].IsNavigable);
                Assert.False(edmComplexType.Properties[3].IsNullable);
                Assert.Equal("CompanyName", edmComplexType.Properties[3].Name);
                Assert.Same(EdmPrimitiveType.String, edmComplexType.Properties[3].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[4].DeclaringType);
                Assert.False(edmComplexType.Properties[4].IsNavigable);
                Assert.True(edmComplexType.Properties[4].IsNullable);
                Assert.Equal("ContactName", edmComplexType.Properties[4].Name);
                Assert.Same(EdmPrimitiveType.String, edmComplexType.Properties[4].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[5].DeclaringType);
                Assert.False(edmComplexType.Properties[5].IsNavigable);
                Assert.False(edmComplexType.Properties[5].IsNullable);
                Assert.Equal("Country", edmComplexType.Properties[5].Name);
                Assert.Same(EdmPrimitiveType.String, edmComplexType.Properties[5].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[6].DeclaringType);
                Assert.False(edmComplexType.Properties[6].IsNavigable);
                Assert.False(edmComplexType.Properties[6].IsNullable);
                Assert.Equal("LegacyId", edmComplexType.Properties[6].Name);
                Assert.Same(EdmPrimitiveType.Int32, edmComplexType.Properties[6].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[7].DeclaringType);
                Assert.True(edmComplexType.Properties[7].IsNavigable);
                Assert.True(edmComplexType.Properties[7].IsNullable);
                Assert.Equal("Orders", edmComplexType.Properties[7].Name);
                Assert.IsType<EdmCollectionType>(edmComplexType.Properties[7].PropertyType);
                Assert.Equal(EdmType.GetEdmType(typeof(Order)), ((EdmCollectionType)edmComplexType.Properties[7].PropertyType).ContainedType);

                Assert.Same(edmComplexType, edmComplexType.Properties[8].DeclaringType);
                Assert.False(edmComplexType.Properties[8].IsNavigable);
                Assert.True(edmComplexType.Properties[8].IsNullable);
                Assert.Equal("Phone", edmComplexType.Properties[8].Name);
                Assert.Same(EdmPrimitiveType.String, edmComplexType.Properties[8].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[9].DeclaringType);
                Assert.False(edmComplexType.Properties[9].IsNavigable);
                Assert.False(edmComplexType.Properties[9].IsNullable);
                Assert.Equal("PostalCode", edmComplexType.Properties[9].Name);
                Assert.Same(EdmPrimitiveType.String, edmComplexType.Properties[9].PropertyType);

                Assert.Same(edmComplexType.Properties[3], entitySet.EntityKey);
            }

            [Fact]
            public void The_Employees_CollectionIsCorrect()
            {
                Assert.True(_entityDataModel.EntitySets.ContainsKey("Employees"));

                EntitySet entitySet = _entityDataModel.EntitySets["Employees"];

                Assert.Equal("Employees", entitySet.Name);
                Assert.Equal(Capabilities.None, entitySet.Capabilities);

                EdmComplexType edmComplexType = entitySet.EdmType;

                Assert.Null(edmComplexType.BaseType);
                Assert.Equal(typeof(Employee), edmComplexType.ClrType);
                Assert.Equal("Sample.Model.Employee", edmComplexType.FullName);
                Assert.Equal("Employee", edmComplexType.Name);
                Assert.Equal(11, edmComplexType.Properties.Count);

                Assert.Same(edmComplexType, edmComplexType.Properties[0].DeclaringType);
                Assert.False(edmComplexType.Properties[0].IsNavigable);
                Assert.False(edmComplexType.Properties[0].IsNullable);
                Assert.Equal("AccessLevel", edmComplexType.Properties[0].Name);
                Assert.Same(EdmType.GetEdmType(typeof(AccessLevel)), edmComplexType.Properties[0].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[1].DeclaringType);
                Assert.False(edmComplexType.Properties[1].IsNavigable);
                Assert.False(edmComplexType.Properties[1].IsNullable);
                Assert.Equal("BirthDate", edmComplexType.Properties[1].Name);
                Assert.Same(EdmPrimitiveType.Date, edmComplexType.Properties[1].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[2].DeclaringType);
                Assert.False(edmComplexType.Properties[2].IsNavigable);
                Assert.False(edmComplexType.Properties[2].IsNullable);
                Assert.Equal("EmailAddress", edmComplexType.Properties[2].Name);
                Assert.Same(EdmPrimitiveType.String, edmComplexType.Properties[2].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[3].DeclaringType);
                Assert.False(edmComplexType.Properties[3].IsNavigable);
                Assert.False(edmComplexType.Properties[3].IsNullable);
                Assert.Equal("Forename", edmComplexType.Properties[3].Name);
                Assert.Same(EdmPrimitiveType.String, edmComplexType.Properties[3].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[4].DeclaringType);
                Assert.False(edmComplexType.Properties[4].IsNavigable);
                Assert.False(edmComplexType.Properties[4].IsNullable);
                Assert.Equal("Id", edmComplexType.Properties[4].Name);
                Assert.Same(EdmPrimitiveType.String, edmComplexType.Properties[4].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[5].DeclaringType);
                Assert.False(edmComplexType.Properties[5].IsNavigable);
                Assert.True(edmComplexType.Properties[5].IsNullable);
                Assert.Equal("ImageData", edmComplexType.Properties[5].Name);
                Assert.Same(EdmPrimitiveType.String, edmComplexType.Properties[5].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[6].DeclaringType);
                Assert.False(edmComplexType.Properties[6].IsNavigable);
                Assert.False(edmComplexType.Properties[6].IsNullable);
                Assert.Equal("JoiningDate", edmComplexType.Properties[6].Name);
                Assert.Same(EdmPrimitiveType.Date, edmComplexType.Properties[6].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[7].DeclaringType);
                Assert.False(edmComplexType.Properties[7].IsNavigable);
                Assert.True(edmComplexType.Properties[7].IsNullable);
                Assert.Equal("LeavingDate", edmComplexType.Properties[7].Name);
                Assert.Same(EdmPrimitiveType.Date, edmComplexType.Properties[1].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[8].DeclaringType);
                Assert.True(edmComplexType.Properties[8].IsNavigable);
                Assert.True(edmComplexType.Properties[8].IsNullable);
                Assert.Equal("Manager", edmComplexType.Properties[8].Name);
                Assert.Equal(EdmType.GetEdmType(typeof(Manager)), (EdmComplexType)edmComplexType.Properties[8].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[9].DeclaringType);
                Assert.False(edmComplexType.Properties[9].IsNavigable);
                Assert.False(edmComplexType.Properties[9].IsNullable);
                Assert.Equal("Surname", edmComplexType.Properties[9].Name);
                Assert.Same(EdmPrimitiveType.String, edmComplexType.Properties[9].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[10].DeclaringType);
                Assert.False(edmComplexType.Properties[10].IsNavigable);
                Assert.False(edmComplexType.Properties[10].IsNullable);
                Assert.Equal("Title", edmComplexType.Properties[10].Name);
                Assert.Same(EdmPrimitiveType.String, edmComplexType.Properties[10].PropertyType);

                Assert.Same(edmComplexType.Properties[4], entitySet.EntityKey);
            }

            [Fact]
            public void The_Managers_CollectionIsCorrect()
            {
                Assert.True(_entityDataModel.EntitySets.ContainsKey("Managers"));

                EntitySet entitySet = _entityDataModel.EntitySets["Managers"];

                Assert.Equal("Managers", entitySet.Name);
                Assert.Equal(Capabilities.None, entitySet.Capabilities);

                EdmComplexType edmComplexType = entitySet.EdmType;

                Assert.Equal(EdmType.GetEdmType(typeof(Employee)), edmComplexType.BaseType);
                Assert.Equal(typeof(Manager), edmComplexType.ClrType);
                Assert.Equal("Sample.Model.Manager", edmComplexType.FullName);
                Assert.Equal("Manager", edmComplexType.Name);
                Assert.Equal(2, edmComplexType.Properties.Count); // Does not include inherited properties

                Assert.Same(edmComplexType, edmComplexType.Properties[0].DeclaringType);
                Assert.False(edmComplexType.Properties[0].IsNavigable);
                Assert.False(edmComplexType.Properties[0].IsNullable);
                Assert.Equal("AnnualBudget", edmComplexType.Properties[0].Name);
                Assert.Same(EdmPrimitiveType.Decimal, edmComplexType.Properties[0].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[1].DeclaringType);
                Assert.True(edmComplexType.Properties[1].IsNavigable);
                Assert.True(edmComplexType.Properties[1].IsNullable);
                Assert.Equal("Employees", edmComplexType.Properties[1].Name);
                Assert.IsType<EdmCollectionType>(edmComplexType.Properties[1].PropertyType);
                Assert.Equal(EdmType.GetEdmType(typeof(Employee)), ((EdmCollectionType)edmComplexType.Properties[1].PropertyType).ContainedType);

                Assert.Null(entitySet.EntityKey);
            }

            [Fact]
            public void The_Orders_CollectionIsCorrect()
            {
                Assert.True(_entityDataModel.EntitySets.ContainsKey("Orders"));

                EntitySet entitySet = _entityDataModel.EntitySets["Orders"];

                Assert.Equal("Orders", entitySet.Name);
                Assert.Equal(Capabilities.Insertable | Capabilities.Updatable, entitySet.Capabilities);

                EdmComplexType edmComplexType = entitySet.EdmType;

                Assert.Null(edmComplexType.BaseType);
                Assert.Equal(typeof(Order), edmComplexType.ClrType);
                Assert.Equal("Sample.Model.Order", edmComplexType.FullName);
                Assert.Equal("Order", edmComplexType.Name);
                Assert.Equal(8, edmComplexType.Properties.Count);

                Assert.Same(edmComplexType, edmComplexType.Properties[0].DeclaringType);
                Assert.True(edmComplexType.Properties[0].IsNavigable);
                Assert.False(edmComplexType.Properties[0].IsNullable);
                Assert.Equal("Customer", edmComplexType.Properties[0].Name);
                Assert.Same(EdmType.GetEdmType(typeof(Customer)), edmComplexType.Properties[0].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[1].DeclaringType);
                Assert.False(edmComplexType.Properties[1].IsNavigable);
                Assert.False(edmComplexType.Properties[1].IsNullable);
                Assert.Equal("Date", edmComplexType.Properties[1].Name);
                Assert.Same(EdmPrimitiveType.DateTimeOffset, edmComplexType.Properties[1].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[2].DeclaringType);
                Assert.False(edmComplexType.Properties[2].IsNavigable);
                Assert.False(edmComplexType.Properties[2].IsNullable);
                Assert.Equal("Freight", edmComplexType.Properties[2].Name);
                Assert.Same(EdmPrimitiveType.Decimal, edmComplexType.Properties[2].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[3].DeclaringType);
                Assert.False(edmComplexType.Properties[3].IsNavigable);
                Assert.True(edmComplexType.Properties[3].IsNullable);
                Assert.Equal("OrderDetails", edmComplexType.Properties[3].Name);
                Assert.IsType<EdmCollectionType>(edmComplexType.Properties[3].PropertyType);
                Assert.Equal(EdmType.GetEdmType(typeof(OrderDetail)), ((EdmCollectionType)edmComplexType.Properties[3].PropertyType).ContainedType);

                Assert.Same(edmComplexType, edmComplexType.Properties[4].DeclaringType);
                Assert.False(edmComplexType.Properties[4].IsNavigable);
                Assert.False(edmComplexType.Properties[4].IsNullable);
                Assert.Equal("OrderId", edmComplexType.Properties[4].Name);
                Assert.Same(EdmPrimitiveType.Int64, edmComplexType.Properties[4].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[5].DeclaringType);
                Assert.False(edmComplexType.Properties[5].IsNavigable);
                Assert.False(edmComplexType.Properties[5].IsNullable);
                Assert.Equal("ShipCountry", edmComplexType.Properties[5].Name);
                Assert.Same(EdmPrimitiveType.String, edmComplexType.Properties[5].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[6].DeclaringType);
                Assert.False(edmComplexType.Properties[6].IsNavigable);
                Assert.False(edmComplexType.Properties[6].IsNullable);
                Assert.Equal("ShippingWeight", edmComplexType.Properties[6].Name);
                Assert.Same(EdmPrimitiveType.Double, edmComplexType.Properties[6].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[7].DeclaringType);
                Assert.False(edmComplexType.Properties[7].IsNavigable);
                Assert.False(edmComplexType.Properties[7].IsNullable);
                Assert.Equal("TransactionId", edmComplexType.Properties[7].Name);
                Assert.Same(EdmPrimitiveType.Guid, edmComplexType.Properties[7].PropertyType);

                Assert.Same(edmComplexType.Properties[4], entitySet.EntityKey);
            }

            [Fact]
            public void The_Products_CollectionIsCorrect()
            {
                Assert.True(_entityDataModel.EntitySets.ContainsKey("Products"));

                EntitySet entitySet = _entityDataModel.EntitySets["Products"];

                Assert.Equal("Products", entitySet.Name);
                Assert.Equal(Capabilities.Insertable | Capabilities.Updatable, entitySet.Capabilities);

                EdmComplexType edmComplexType = entitySet.EdmType;

                Assert.Null(edmComplexType.BaseType);
                Assert.Equal(typeof(Product), edmComplexType.ClrType);
                Assert.Equal("Sample.Model.Product", edmComplexType.FullName);
                Assert.Equal("Product", edmComplexType.Name);
                Assert.Equal(9, edmComplexType.Properties.Count);

                Assert.Same(edmComplexType, edmComplexType.Properties[0].DeclaringType);
                Assert.True(edmComplexType.Properties[0].IsNavigable);
                Assert.False(edmComplexType.Properties[0].IsNullable);
                Assert.Equal("Category", edmComplexType.Properties[0].Name);
                Assert.Same(EdmType.GetEdmType(typeof(Category)), edmComplexType.Properties[0].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[1].DeclaringType);
                Assert.False(edmComplexType.Properties[1].IsNavigable);
                Assert.False(edmComplexType.Properties[1].IsNullable);
                Assert.Equal("Colour", edmComplexType.Properties[1].Name);
                Assert.Same(EdmType.GetEdmType(typeof(Colour)), edmComplexType.Properties[1].PropertyType);

                var edmEnumType = (EdmEnumType)EdmType.GetEdmType(typeof(Colour));
                Assert.Equal(typeof(Colour), edmEnumType.ClrType);
                Assert.Equal("Sample.Model.Colour", edmEnumType.FullName);
                Assert.Equal("Colour", edmEnumType.Name);
                Assert.Equal(3, edmEnumType.Members.Count);
                Assert.Equal("Green", edmEnumType.Members[0].Name);
                Assert.Equal(1, edmEnumType.Members[0].Value);
                Assert.Equal("Blue", edmEnumType.Members[1].Name);
                Assert.Equal(2, edmEnumType.Members[1].Value);
                Assert.Equal("Red", edmEnumType.Members[2].Name);
                Assert.Equal(3, edmEnumType.Members[2].Value);

                Assert.Same(edmComplexType, edmComplexType.Properties[2].DeclaringType);
                Assert.False(edmComplexType.Properties[2].IsNavigable);
                Assert.False(edmComplexType.Properties[2].IsNullable);
                Assert.Equal("Description", edmComplexType.Properties[2].Name);
                Assert.Same(EdmPrimitiveType.String, edmComplexType.Properties[2].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[3].DeclaringType);
                Assert.False(edmComplexType.Properties[3].IsNavigable);
                Assert.False(edmComplexType.Properties[3].IsNullable);
                Assert.Equal("Discontinued", edmComplexType.Properties[3].Name);
                Assert.Same(EdmPrimitiveType.Boolean, edmComplexType.Properties[3].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[4].DeclaringType);
                Assert.False(edmComplexType.Properties[4].IsNavigable);
                Assert.False(edmComplexType.Properties[4].IsNullable);
                Assert.Equal("Name", edmComplexType.Properties[4].Name);
                Assert.Same(EdmPrimitiveType.String, edmComplexType.Properties[4].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[5].DeclaringType);
                Assert.False(edmComplexType.Properties[5].IsNavigable);
                Assert.False(edmComplexType.Properties[5].IsNullable);
                Assert.Equal("Price", edmComplexType.Properties[5].Name);
                Assert.Same(EdmPrimitiveType.Decimal, edmComplexType.Properties[5].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[6].DeclaringType);
                Assert.False(edmComplexType.Properties[6].IsNavigable);
                Assert.False(edmComplexType.Properties[6].IsNullable);
                Assert.Equal("ProductId", edmComplexType.Properties[6].Name);
                Assert.Same(EdmPrimitiveType.Int32, edmComplexType.Properties[6].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[7].DeclaringType);
                Assert.False(edmComplexType.Properties[7].IsNavigable);
                Assert.False(edmComplexType.Properties[7].IsNullable);
                Assert.Equal("Rating", edmComplexType.Properties[7].Name);
                Assert.Same(EdmPrimitiveType.Single, edmComplexType.Properties[7].PropertyType);

                Assert.Same(edmComplexType, edmComplexType.Properties[8].DeclaringType);
                Assert.False(edmComplexType.Properties[8].IsNavigable);
                Assert.False(edmComplexType.Properties[8].IsNullable);
                Assert.Equal("ReleaseDate", edmComplexType.Properties[8].Name);
                Assert.Same(EdmPrimitiveType.Date, edmComplexType.Properties[8].PropertyType);

                Assert.Same(edmComplexType.Properties[6], entitySet.EntityKey);
            }

            [Fact]
            public void ThereAre_6_RegisteredCollections() => Assert.Equal(6, _entityDataModel.EntitySets.Count);
        }
    }
}
