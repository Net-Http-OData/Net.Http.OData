using System;
using Net.Http.OData.Model;
using NorthwindModel;

namespace Net.Http.OData.Tests
{
    internal static class TestHelper
    {
        internal static void EnsureEDM()
        {
            var entityDataModelBuilder = new EntityDataModelBuilder(StringComparer.OrdinalIgnoreCase);

            entityDataModelBuilder.RegisterEntitySet<Category>("Categories", x => x.Name, Capabilities.Insertable | Capabilities.Updatable | Capabilities.Deletable)
                .RegisterEntitySet<Customer>("Customers", x => x.CompanyName, Capabilities.Updatable)
                .RegisterEntitySet<Employee>("Employees", x => x.Id)
                .RegisterEntitySet<Manager>("Managers", x => x.Id)
                .RegisterEntitySet<Order>("Orders", x => x.OrderId, Capabilities.Insertable | Capabilities.Updatable)
                .RegisterEntitySet<Product>("Products", x => x.ProductId, Capabilities.Insertable | Capabilities.Updatable);

            entityDataModelBuilder.BuildModel();
        }
    }
}
