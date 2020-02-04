using System.Net;
using Xunit;

namespace Net.Http.OData.Tests
{
    public class ODataErrorContentTests
    {
        [Fact]
        public void Create_Code_Message()
        {
            var odataErrorContent = ODataErrorContent.Create(404, "Not Found");

            Assert.NotNull(odataErrorContent);
            Assert.NotNull(odataErrorContent.Error);
            Assert.Equal("404", odataErrorContent.Error.Code);
            Assert.Equal("Not Found", odataErrorContent.Error.Message);
            Assert.Null(odataErrorContent.Error.Target);
        }

        [Fact]
        public void Create_Code_Message_Target()
        {
            var odataErrorContent = ODataErrorContent.Create(404, "Not Found", "target");

            Assert.NotNull(odataErrorContent);
            Assert.NotNull(odataErrorContent.Error);
            Assert.Equal("404", odataErrorContent.Error.Code);
            Assert.Equal("Not Found", odataErrorContent.Error.Message);
            Assert.Equal("target", odataErrorContent.Error.Target);
        }

        [Fact]
        public void Create_ODataException_HttpStatusCode_Message()
        {
            var odataErrorContent = ODataErrorContent.Create(new ODataException(HttpStatusCode.NotFound, "Not Found"));

            Assert.NotNull(odataErrorContent);
            Assert.NotNull(odataErrorContent.Error);
            Assert.Equal("404", odataErrorContent.Error.Code);
            Assert.Equal("Not Found", odataErrorContent.Error.Message);
            Assert.Null(odataErrorContent.Error.Target);
        }

        [Fact]
        public void Create_ODataException_HttpStatusCode_Message_Target()
        {
            var odataErrorContent = ODataErrorContent.Create(new ODataException(HttpStatusCode.NotFound, "Not Found", "target"));

            Assert.NotNull(odataErrorContent);
            Assert.NotNull(odataErrorContent.Error);
            Assert.Equal("404", odataErrorContent.Error.Code);
            Assert.Equal("Not Found", odataErrorContent.Error.Message);
            Assert.Equal("target", odataErrorContent.Error.Target);
        }
    }
}
