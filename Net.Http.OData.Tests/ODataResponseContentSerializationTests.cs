using System.Dynamic;
using Xunit;

namespace Net.Http.OData.Tests
{
    public class ODataResponseContentSerializationTests
    {
        private static readonly System.Text.Json.JsonSerializerOptions s_jsonSerializerOptions = new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase };
        private static readonly Newtonsoft.Json.JsonSerializerSettings s_jsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() };

        [Fact]
        public void Newtonsoft_JsonSerializationWith_ClassContent_Count()
        {
            var item = new Thing
            {
                Name = "Coffee",
                Total = 2.55M
            };

            var responseContent = new ODataResponseContent(
                new[] { item },
                null,
                count: 12);

            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(responseContent, s_jsonSerializerSettings);

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

            var responseContent = new ODataResponseContent(
                new[] { item },
                "http://services.odata.org/OData/$metadata#Products",
                count: 12);

            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(responseContent, s_jsonSerializerSettings);

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

            var responseContent = new ODataResponseContent(
                new[] { item },
                "http://services.odata.org/OData/$metadata#Products",
                count: 12,
                nextLink: "http://services.odata.org/OData/Products?$skip=5");

            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(responseContent, s_jsonSerializerSettings);

            Assert.Equal("{\"@odata.context\":\"http://services.odata.org/OData/$metadata#Products\",\"@odata.count\":12,\"@odata.nextLink\":\"http://services.odata.org/OData/Products?$skip=5\",\"value\":[{\"name\":\"Coffee\",\"total\":2.55}]}", jsonResult);
        }

        [Fact]
        public void Newtonsoft_JsonSerializationWith_Context_DynamicContent_Count()
        {
            dynamic item = new ExpandoObject();
            item.Id = 14225;
            item.Name = "Fred";

            var responseContent = new ODataResponseContent(
                new[] { item },
                "http://services.odata.org/OData/$metadata#Products",
                count: 12);

            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(responseContent, s_jsonSerializerSettings);

            Assert.Equal("{\"@odata.context\":\"http://services.odata.org/OData/$metadata#Products\",\"@odata.count\":12,\"value\":[{\"id\":14225,\"name\":\"Fred\"}]}", jsonResult);
        }

        [Fact]
        public void Newtonsoft_JsonSerializationWith_Context_DynamicContent_Count_NextLink()
        {
            dynamic item = new ExpandoObject();
            item.Id = 14225;
            item.Name = "Fred";

            var responseContent = new ODataResponseContent(
                new[] { item },
                "http://services.odata.org/OData/$metadata#Products",
                count: 12,
                nextLink: "http://services.odata.org/OData/Products?$skip=5");

            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(responseContent, s_jsonSerializerSettings);

            Assert.Equal("{\"@odata.context\":\"http://services.odata.org/OData/$metadata#Products\",\"@odata.count\":12,\"@odata.nextLink\":\"http://services.odata.org/OData/Products?$skip=5\",\"value\":[{\"id\":14225,\"name\":\"Fred\"}]}", jsonResult);
        }

        [Fact]
        public void Newtonsoft_JsonSerializationWith_DynamicContent_Count()
        {
            dynamic item = new ExpandoObject();
            item.Id = 14225;
            item.Name = "Fred";

            var responseContent = new ODataResponseContent(
                new[] { item },
                null,
                count: 12);

            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(responseContent, s_jsonSerializerSettings);

            Assert.Equal("{\"@odata.count\":12,\"value\":[{\"id\":14225,\"name\":\"Fred\"}]}", jsonResult);
        }

        [Fact]
        public void Newtonsoft_JsonSerializationWith_SimpleContent_Count()
        {
            var responseContent = new ODataResponseContent(
                new[] { 1, 2, 3 },
                null,
                count: 5);

            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(responseContent, s_jsonSerializerSettings);

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

            var responseContent = new ODataResponseContent(
                new[] { item },
                null,
                count: 12);

            string jsonResult = System.Text.Json.JsonSerializer.Serialize(responseContent, s_jsonSerializerOptions);

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

            var responseContent = new ODataResponseContent(
                new[] { item },
                "http://services.odata.org/OData/$metadata#Products",
                count: 12);

            string jsonResult = System.Text.Json.JsonSerializer.Serialize(responseContent, s_jsonSerializerOptions);

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

            var responseContent = new ODataResponseContent(
                new[] { item },
                "http://services.odata.org/OData/$metadata#Products",
                count: 12,
                nextLink: "http://services.odata.org/OData/Products?$skip=5");

            string jsonResult = System.Text.Json.JsonSerializer.Serialize(responseContent, s_jsonSerializerOptions);

            Assert.Equal("{\"@odata.context\":\"http://services.odata.org/OData/$metadata#Products\",\"@odata.count\":12,\"@odata.nextLink\":\"http://services.odata.org/OData/Products?$skip=5\",\"value\":[{\"name\":\"Coffee\",\"total\":2.55}]}", jsonResult);
        }

        [Fact]
        public void SystemText_JsonSerializationWith_Context_DynamicContent_Count()
        {
            dynamic item = new ExpandoObject();
            item.Id = 14225;
            item.Name = "Fred";

            var responseContent = new ODataResponseContent(
                new[] { item },
                "http://services.odata.org/OData/$metadata#Products",
                count: 12);

            string jsonResult = System.Text.Json.JsonSerializer.Serialize(responseContent, s_jsonSerializerOptions);

            Assert.Equal("{\"@odata.context\":\"http://services.odata.org/OData/$metadata#Products\",\"@odata.count\":12,\"value\":[{\"Id\":14225,\"Name\":\"Fred\"}]}", jsonResult);
        }

        [Fact]
        public void SystemText_JsonSerializationWith_Context_DynamicContent_Count_NextLink()
        {
            dynamic item = new ExpandoObject();
            item.Id = 14225;
            item.Name = "Fred";

            var responseContent = new ODataResponseContent(
                new[] { item },
                "http://services.odata.org/OData/$metadata#Products",
                count: 12,
                nextLink: "http://services.odata.org/OData/Products?$skip=5");

            string jsonResult = System.Text.Json.JsonSerializer.Serialize(responseContent, s_jsonSerializerOptions);

            Assert.Equal("{\"@odata.context\":\"http://services.odata.org/OData/$metadata#Products\",\"@odata.count\":12,\"@odata.nextLink\":\"http://services.odata.org/OData/Products?$skip=5\",\"value\":[{\"Id\":14225,\"Name\":\"Fred\"}]}", jsonResult);
        }

        [Fact]
        public void SystemText_JsonSerializationWith_DynamicContent_Count()
        {
            dynamic item = new ExpandoObject();
            item.Id = 14225;
            item.Name = "Fred";

            var responseContent = new ODataResponseContent(
                new[] { item },
                null,
                count: 12);

            string jsonResult = System.Text.Json.JsonSerializer.Serialize(responseContent, s_jsonSerializerOptions);

            Assert.Equal("{\"@odata.count\":12,\"value\":[{\"Id\":14225,\"Name\":\"Fred\"}]}", jsonResult);
        }

        [Fact]
        public void SystemText_JsonSerializationWith_SimpleContent_Count()
        {
            var responseContent = new ODataResponseContent(
                new[] { 1, 2, 3 },
                null,
                count: 5);

            string jsonResult = System.Text.Json.JsonSerializer.Serialize(responseContent, s_jsonSerializerOptions);

            Assert.Equal("{\"@odata.count\":5,\"value\":[1,2,3]}", jsonResult);
        }

        public class Thing
        {
            public string Name { get; set; }

            public decimal Total { get; set; }
        }
    }
}
