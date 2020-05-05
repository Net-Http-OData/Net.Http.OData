using System;
using System.Net;
using Net.Http.OData.Model;
using Sample.Model;
using Xunit;

namespace Net.Http.OData.Tests.Model
{
    public class EntityDataModelTests
    {
        private readonly EntityDataModel _entityDataModel;

        public EntityDataModelTests()
        {
            EntityDataModelBuilder entityDataModelBuilder = new EntityDataModelBuilder(StringComparer.OrdinalIgnoreCase)
                .RegisterEntitySet<Category>("Categories", x => x.Name, Capabilities.Insertable | Capabilities.Updatable | Capabilities.Deletable)
                .RegisterEntitySet<Customer>("Customers", x => x.CompanyName, Capabilities.Updatable)
                .RegisterEntitySet<Employee>("Employees", x => x.Id)
                .RegisterEntitySet<Manager>("Managers", x => x.Id)
                .RegisterEntitySet<Order>("Orders", x => x.OrderId, Capabilities.Insertable | Capabilities.Updatable)
                .RegisterEntitySet<Product>("Products", x => x.ProductId, Capabilities.Insertable | Capabilities.Updatable);

            _entityDataModel = entityDataModelBuilder.BuildModel();
        }

        [Fact]
        public void Constructor_Throws_ArgumentNullException_For_Null_EntitySets()
            => Assert.Throws<ArgumentNullException>(() => new EntityDataModel(null));

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
        public void EntitySetForPath_Throws_ODataException_IfEntitySetNotRegistered()
        {
            ODataException odataException = Assert.Throws<ODataException>(() => EntityDataModel.Current.EntitySetForPath("/OData/Colour"));

            Assert.Equal(ExceptionMessage.EntityDataModelDoesNotContainEntitySet("Colour"), odataException.Message);
            Assert.Equal(HttpStatusCode.BadRequest, odataException.StatusCode);
            Assert.Equal("Colour", odataException.Target);
        }

        [Fact]
        public void IsEntitySet_ReturnsFalse_IfEdmTypeIsNotEntitySet()
            => Assert.True(_entityDataModel.IsEntitySet(EdmType.GetEdmType(typeof(Category))));

        [Fact]
        public void IsEntitySet_ReturnsTrue_IfEdmTypeIsEntitySet()
            => Assert.True(_entityDataModel.IsEntitySet(EdmType.GetEdmType(typeof(Customer))));
    }
}
