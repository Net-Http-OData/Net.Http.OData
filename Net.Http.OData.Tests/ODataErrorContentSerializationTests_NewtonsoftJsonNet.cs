using System.Linq;
using Xunit;

namespace Net.Http.OData.Tests
{
    public class ODataErrorContentSerializationTests_NewtonsoftJsonNet
    {
        [Fact]
        public void JsonSerializationWith_Code_Message()
        {
            var odataErrorContent = ODataErrorContent.Create(404, "Not Found");

            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(odataErrorContent, TestHelper.JsonSerializerSettings);

            Assert.Equal("{\"error\":{\"code\":\"404\",\"message\":\"Not Found\"}}", jsonResult);

            ODataErrorContent deserializedODataErrorContent = Newtonsoft.Json.JsonConvert.DeserializeObject<ODataErrorContent>(jsonResult, TestHelper.JsonSerializerSettings);

            Assert.Equal(odataErrorContent.Error.Code, deserializedODataErrorContent.Error.Code);
            Assert.Equal(odataErrorContent.Error.Message, deserializedODataErrorContent.Error.Message);
            Assert.Equal(odataErrorContent.Error.Target, deserializedODataErrorContent.Error.Target);
        }

        [Fact]
        public void JsonSerializationWith_Code_Message_Target()
        {
            var odataErrorContent = ODataErrorContent.Create(404, "Not Found", "target");

            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(odataErrorContent, TestHelper.JsonSerializerSettings);

            Assert.Equal("{\"error\":{\"code\":\"404\",\"message\":\"Not Found\",\"target\":\"target\"}}", jsonResult);

            ODataErrorContent deserializedODataErrorContent = Newtonsoft.Json.JsonConvert.DeserializeObject<ODataErrorContent>(jsonResult, TestHelper.JsonSerializerSettings);

            Assert.Equal(odataErrorContent.Error.Code, deserializedODataErrorContent.Error.Code);
            Assert.Equal(odataErrorContent.Error.Message, deserializedODataErrorContent.Error.Message);
            Assert.Equal(odataErrorContent.Error.Target, deserializedODataErrorContent.Error.Target);
        }

        [Fact]
        public void JsonSerializationWith_Code_Message_Target_Details()
        {
            var odataErrorContent = ODataErrorContent.Create(501, "Unsupported functionality", "query", new[] { new ODataErrorDetail { Code = "301", Message = "$search query option not supported", Target = "$search" } });

            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(odataErrorContent, TestHelper.JsonSerializerSettings);

            Assert.Equal("{\"error\":{\"code\":\"501\",\"details\":[{\"code\":\"301\",\"message\":\"$search query option not supported\",\"target\":\"$search\"}],\"message\":\"Unsupported functionality\",\"target\":\"query\"}}", jsonResult);

            ODataErrorContent deserializedODataErrorContent = Newtonsoft.Json.JsonConvert.DeserializeObject<ODataErrorContent>(jsonResult, TestHelper.JsonSerializerSettings);

            Assert.Equal(odataErrorContent.Error.Code, deserializedODataErrorContent.Error.Code);
            Assert.Single(odataErrorContent.Error.Details);
            Assert.Equal(odataErrorContent.Error.Message, deserializedODataErrorContent.Error.Message);
            Assert.Equal(odataErrorContent.Error.Target, deserializedODataErrorContent.Error.Target);

            var deserializedODataErrorDetail = odataErrorContent.Error.Details.Single();
            Assert.Equal(odataErrorContent.Error.Details.Single().Code, deserializedODataErrorDetail.Code);
            Assert.Equal(odataErrorContent.Error.Details.Single().Message, deserializedODataErrorDetail.Message);
            Assert.Equal(odataErrorContent.Error.Details.Single().Target, deserializedODataErrorDetail.Target);
        }
    }
}
