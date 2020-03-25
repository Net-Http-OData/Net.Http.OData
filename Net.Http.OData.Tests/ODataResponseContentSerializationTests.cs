using System.Dynamic;
using Xunit;

namespace Net.Http.OData.Tests
{
    public class ODataResponseContentSerializationTests
    {
        [Fact]
        public void Newtonsoft_JsonSerializationWith_ClassContent_Count()
        {
            var item = new Thing
            {
                Name = "Coffee",
                Total = 2.55M
            };

            var responseContent = new ODataResponseContent
            {
                Count = 12,
                Value = new[] { item },
            };

            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(responseContent, TestHelper.JsonSerializerSettings);

            Assert.Equal("{\"@odata.count\":12,\"value\":[{\"name\":\"Coffee\",\"total\":2.55}]}", jsonResult);
        }

        [Fact]
        public void Newtonsoft_JsonSerializationWith_Context_ClassContent_Count()
        {
            var item = new Thing
            {
                Name = "Coffee",
                Total = 2.55M
            };

            var responseContent = new ODataResponseContent
            {
                Context = "http://services.odata.org/OData/$metadata#Products",
                Count = 12,
                Value = new[] { item },
            };

            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(responseContent, TestHelper.JsonSerializerSettings);

            Assert.Equal("{\"@odata.context\":\"http://services.odata.org/OData/$metadata#Products\",\"@odata.count\":12,\"value\":[{\"name\":\"Coffee\",\"total\":2.55}]}", jsonResult);
        }

        [Fact]
        public void Newtonsoft_JsonSerializationWith_Context_ClassContent_Count_NextLink()
        {
            var item = new Thing
            {
                Name = "Coffee",
                Total = 2.55M
            };

            var responseContent = new ODataResponseContent
            {
                Context = "http://services.odata.org/OData/$metadata#Products",
                Count = 12,
                NextLink = "http://services.odata.org/OData/Products?$skip=5",
                Value = new[] { item },
            };

            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(responseContent, TestHelper.JsonSerializerSettings);

            Assert.Equal("{\"@odata.context\":\"http://services.odata.org/OData/$metadata#Products\",\"@odata.count\":12,\"@odata.nextLink\":\"http://services.odata.org/OData/Products?$skip=5\",\"value\":[{\"name\":\"Coffee\",\"total\":2.55}]}", jsonResult);
        }

        [Fact]
        public void Newtonsoft_JsonSerializationWith_Context_DynamicContent_Count()
        {
            dynamic item = new ExpandoObject();
            item.Id = 14225;
            item.Name = "Fred";

            var responseContent = new ODataResponseContent
            {
                Context = "http://services.odata.org/OData/$metadata#Products",
                Count = 12,
                Value = new[] { item },
            };

            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(responseContent, TestHelper.JsonSerializerSettings);

            Assert.Equal("{\"@odata.context\":\"http://services.odata.org/OData/$metadata#Products\",\"@odata.count\":12,\"value\":[{\"id\":14225,\"name\":\"Fred\"}]}", jsonResult);
        }

        [Fact]
        public void Newtonsoft_JsonSerializationWith_Context_DynamicContent_Count_NextLink()
        {
            dynamic item = new ExpandoObject();
            item.Id = 14225;
            item.Name = "Fred";

            var responseContent = new ODataResponseContent
            {
                Context = "http://services.odata.org/OData/$metadata#Products",
                Count = 12,
                NextLink = "http://services.odata.org/OData/Products?$skip=5",
                Value = new[] { item },
            };

            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(responseContent, TestHelper.JsonSerializerSettings);

            Assert.Equal("{\"@odata.context\":\"http://services.odata.org/OData/$metadata#Products\",\"@odata.count\":12,\"@odata.nextLink\":\"http://services.odata.org/OData/Products?$skip=5\",\"value\":[{\"id\":14225,\"name\":\"Fred\"}]}", jsonResult);
        }

        [Fact]
        public void Newtonsoft_JsonSerializationWith_DynamicContent_Count()
        {
            dynamic item = new ExpandoObject();
            item.Id = 14225;
            item.Name = "Fred";

            var responseContent = new ODataResponseContent
            {
                Count = 12,
                Value = new[] { item },
            };

            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(responseContent, TestHelper.JsonSerializerSettings);

            Assert.Equal("{\"@odata.count\":12,\"value\":[{\"id\":14225,\"name\":\"Fred\"}]}", jsonResult);
        }

        [Fact]
        public void Newtonsoft_JsonSerializationWith_SimpleContent_Count()
        {
            var responseContent = new ODataResponseContent
            {
                Count = 5,
                Value = new[] { 1, 2, 3 },
            };

            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(responseContent, TestHelper.JsonSerializerSettings);

            Assert.Equal("{\"@odata.count\":5,\"value\":[1,2,3]}", jsonResult);
        }

        [Fact]
        public void SystemText_JsonSerializationWith_ClassContent_Count()
        {
            var item = new Thing
            {
                Name = "Coffee",
                Total = 2.55M
            };

            var responseContent = new ODataResponseContent
            {
                Count = 12,
                Value = new[] { item },
            };

            string jsonResult = System.Text.Json.JsonSerializer.Serialize(responseContent, TestHelper.JsonSerializerOptions);

            Assert.Equal("{\"@odata.count\":12,\"value\":[{\"name\":\"Coffee\",\"total\":2.55}]}", jsonResult);
        }

        [Fact]
        public void SystemText_JsonSerializationWith_Context_ClassContent_Count()
        {
            var item = new Thing
            {
                Name = "Coffee",
                Total = 2.55M
            };

            var responseContent = new ODataResponseContent
            {
                Context = "http://services.odata.org/OData/$metadata#Products",
                Count = 12,
                Value = new[] { item },
            };

            string jsonResult = System.Text.Json.JsonSerializer.Serialize(responseContent, TestHelper.JsonSerializerOptions);

            Assert.Equal("{\"@odata.context\":\"http://services.odata.org/OData/$metadata#Products\",\"@odata.count\":12,\"value\":[{\"name\":\"Coffee\",\"total\":2.55}]}", jsonResult);
        }

        [Fact]
        public void SystemText_JsonSerializationWith_Context_ClassContent_Count_NextLink()
        {
            var item = new Thing
            {
                Name = "Coffee",
                Total = 2.55M
            };

            var responseContent = new ODataResponseContent
            {
                Context = "http://services.odata.org/OData/$metadata#Products",
                Count = 12,
                NextLink = "http://services.odata.org/OData/Products?$skip=5",
                Value = new[] { item },
            };

            string jsonResult = System.Text.Json.JsonSerializer.Serialize(responseContent, TestHelper.JsonSerializerOptions);

            Assert.Equal("{\"@odata.context\":\"http://services.odata.org/OData/$metadata#Products\",\"@odata.count\":12,\"@odata.nextLink\":\"http://services.odata.org/OData/Products?$skip=5\",\"value\":[{\"name\":\"Coffee\",\"total\":2.55}]}", jsonResult);
        }

        [Fact]
        public void SystemText_JsonSerializationWith_Context_DynamicContent_Count()
        {
            dynamic item = new ExpandoObject();
            item.Id = 14225;
            item.Name = "Fred";

            var responseContent = new ODataResponseContent
            {
                Context = "http://services.odata.org/OData/$metadata#Products",
                Count = 12,
                Value = new[] { item },
            };

            string jsonResult = System.Text.Json.JsonSerializer.Serialize(responseContent, TestHelper.JsonSerializerOptions);

            Assert.Equal("{\"@odata.context\":\"http://services.odata.org/OData/$metadata#Products\",\"@odata.count\":12,\"value\":[{\"Id\":14225,\"Name\":\"Fred\"}]}", jsonResult);
        }

        [Fact]
        public void SystemText_JsonSerializationWith_Context_DynamicContent_Count_NextLink()
        {
            dynamic item = new ExpandoObject();
            item.Id = 14225;
            item.Name = "Fred";

            var responseContent = new ODataResponseContent
            {
                Context = "http://services.odata.org/OData/$metadata#Products",
                Count = 12,
                NextLink = "http://services.odata.org/OData/Products?$skip=5",
                Value = new[] { item },
            };

            string jsonResult = System.Text.Json.JsonSerializer.Serialize(responseContent, TestHelper.JsonSerializerOptions);

            Assert.Equal("{\"@odata.context\":\"http://services.odata.org/OData/$metadata#Products\",\"@odata.count\":12,\"@odata.nextLink\":\"http://services.odata.org/OData/Products?$skip=5\",\"value\":[{\"Id\":14225,\"Name\":\"Fred\"}]}", jsonResult);
        }

        [Fact]
        public void SystemText_JsonSerializationWith_DynamicContent_Count()
        {
            dynamic item = new ExpandoObject();
            item.Id = 14225;
            item.Name = "Fred";

            var responseContent = new ODataResponseContent
            {
                Count = 12,
                Value = new[] { item },
            };

            string jsonResult = System.Text.Json.JsonSerializer.Serialize(responseContent, TestHelper.JsonSerializerOptions);

            Assert.Equal("{\"@odata.count\":12,\"value\":[{\"Id\":14225,\"Name\":\"Fred\"}]}", jsonResult);
        }

        [Fact]
        public void SystemText_JsonSerializationWith_SimpleContent_Count()
        {
            var responseContent = new ODataResponseContent
            {
                Count = 5,
                Value = new[] { 1, 2, 3 },
            };

            string jsonResult = System.Text.Json.JsonSerializer.Serialize(responseContent, TestHelper.JsonSerializerOptions);

            Assert.Equal("{\"@odata.count\":5,\"value\":[1,2,3]}", jsonResult);
        }

        public class Thing
        {
            public string Name { get; set; }

            public decimal Total { get; set; }
        }
    }
}
