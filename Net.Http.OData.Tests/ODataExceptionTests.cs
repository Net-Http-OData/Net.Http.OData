using System.Net;
using Xunit;

namespace Net.Http.OData.Tests
{
    public class ODataExceptionTests
    {
        [Fact]
        public void Create_ODataException_HttpStatusCode_Message()
        {
            var odataException = new ODataException("Not Found", HttpStatusCode.NotFound);
            var odataErrorContent = odataException.ToODataErrorContent();

            Assert.NotNull(odataErrorContent);
            Assert.NotNull(odataErrorContent.Error);
            Assert.Equal("404", odataErrorContent.Error.Code);
            Assert.Equal("Not Found", odataErrorContent.Error.Message);
            Assert.Null(odataErrorContent.Error.Target);
        }

        [Fact]
        public void Create_ODataException_HttpStatusCode_Message_Target()
        {
            var odataException = new ODataException("Not Found", HttpStatusCode.NotFound, "target");
            var odataErrorContent = odataException.ToODataErrorContent();

            Assert.NotNull(odataErrorContent);
            Assert.NotNull(odataErrorContent.Error);
            Assert.Equal("404", odataErrorContent.Error.Code);
            Assert.Equal("Not Found", odataErrorContent.Error.Message);
            Assert.Equal("target", odataErrorContent.Error.Target);
        }
    }
}
