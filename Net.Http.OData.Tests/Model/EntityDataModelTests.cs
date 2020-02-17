using System;
using Net.Http.OData.Model;
using NorthwindModel;
using Xunit;

namespace Net.Http.OData.Tests.Model
{
    public class EntityDataModelTests
    {
        private readonly EntityDataModel _entityDataModel;

        public EntityDataModelTests()
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

        [Theory]
        [InlineData("/odata/products")]
        [InlineData("/OData/Products")]
        [InlineData("/OData/Products/")]
        [InlineData("/OData/Products(1)")]
        [InlineData("/OData/Products(1)/Name/$value")]
        [InlineData("/OData/Products%281%29")]
        [InlineData("/OData/Products$select=Name")]
        public void EntitySetForPath_ReturnsEntitySet(string path)
        {
            TestHelper.EnsureEDM();

            EntitySet entitySet = EntityDataModel.Current.EntitySetForPath(path);

            Assert.NotNull(entitySet);
            Assert.Equal("Products", entitySet.Name);
        }

        [Fact]
        public void IsEntitySet_ReturnsFalse_IfEdmTypeIsNotEntitySet() => Assert.True(_entityDataModel.IsEntitySet(EdmType.GetEdmType(typeof(Category))));

        [Fact]
        public void IsEntitySet_ReturnsTrue_IfEdmTypeIsEntitySet() => Assert.True(_entityDataModel.IsEntitySet(EdmType.GetEdmType(typeof(Customer))));
    }
}
