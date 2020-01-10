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

        [Fact]
        public void FilterFunctions_AreSet()
        {
            Assert.Equal(26, _entityDataModel.FilterFunctions.Count);

            Assert.Contains("cast", _entityDataModel.FilterFunctions);
            Assert.Contains("ceiling", _entityDataModel.FilterFunctions);
            Assert.Contains("concat", _entityDataModel.FilterFunctions);
            Assert.Contains("contains", _entityDataModel.FilterFunctions);
            Assert.Contains("day", _entityDataModel.FilterFunctions);
            Assert.Contains("endswith", _entityDataModel.FilterFunctions);
            Assert.Contains("floor", _entityDataModel.FilterFunctions);
            Assert.Contains("fractionalseconds", _entityDataModel.FilterFunctions);
            Assert.Contains("hour", _entityDataModel.FilterFunctions);
            Assert.Contains("indexof", _entityDataModel.FilterFunctions);
            Assert.Contains("isof", _entityDataModel.FilterFunctions);
            Assert.Contains("length", _entityDataModel.FilterFunctions);
            Assert.Contains("maxdatetime", _entityDataModel.FilterFunctions);
            Assert.Contains("mindatetime", _entityDataModel.FilterFunctions);
            Assert.Contains("minute", _entityDataModel.FilterFunctions);
            Assert.Contains("month", _entityDataModel.FilterFunctions);
            Assert.Contains("now", _entityDataModel.FilterFunctions);
            Assert.Contains("replace", _entityDataModel.FilterFunctions);
            Assert.Contains("round", _entityDataModel.FilterFunctions);
            Assert.Contains("second", _entityDataModel.FilterFunctions);
            Assert.Contains("startswith", _entityDataModel.FilterFunctions);
            Assert.Contains("substring", _entityDataModel.FilterFunctions);
            Assert.Contains("tolower", _entityDataModel.FilterFunctions);
            Assert.Contains("toupper", _entityDataModel.FilterFunctions);
            Assert.Contains("trim", _entityDataModel.FilterFunctions);
            Assert.Contains("year", _entityDataModel.FilterFunctions);
        }

        [Fact]
        public void IsEntitySet_ReturnsFalse_IfEdmTypeIsNotEntitySet()
        {
            Assert.True(_entityDataModel.IsEntitySet(EdmType.GetEdmType(typeof(Category))));
        }

        [Fact]
        public void IsEntitySet_ReturnsTrue_IfEdmTypeIsEntitySet()
        {
            Assert.True(_entityDataModel.IsEntitySet(EdmType.GetEdmType(typeof(Customer))));
        }

        [Fact]
        public void SupportedFormats_AreSet()
        {
            Assert.Equal(2, _entityDataModel.SupportedFormats.Count);

            Assert.Contains("application/json;odata.metadata=none", _entityDataModel.SupportedFormats);
            Assert.Contains("application/json;odata.metadata=minimal", _entityDataModel.SupportedFormats);
        }
    }
}
