using System.Linq;
using System.Net;
using Xunit;

namespace Net.Http.OData.Tests
{
    public class ODataExceptionTests
    {
        [Fact]
        public void ToODataErrorContent_HttpStatusCode_Message()
        {
            var odataException = new ODataException("Not Found", HttpStatusCode.NotFound);
            var odataErrorContent = odataException.ToODataErrorContent();

            Assert.NotNull(odataErrorContent);
            Assert.NotNull(odataErrorContent.Error);
            Assert.Equal(((int)odataException.StatusCode).ToString(), odataErrorContent.Error.Code);
            Assert.Equal(odataException.Message, odataErrorContent.Error.Message);
            Assert.Null(odataErrorContent.Error.Target);
        }

        [Fact]
        public void ToODataErrorContent_HttpStatusCode_Message_Target()
        {
            var odataException = new ODataException("Not Found", HttpStatusCode.NotFound, "target");
            var odataErrorContent = odataException.ToODataErrorContent();

            Assert.NotNull(odataErrorContent);
            Assert.NotNull(odataErrorContent.Error);
            Assert.Equal(((int)odataException.StatusCode).ToString(), odataErrorContent.Error.Code);
            Assert.Equal(odataException.Message, odataErrorContent.Error.Message);
            Assert.Equal(odataException.Target, odataErrorContent.Error.Target);
        }

        [Fact]
        public void ToODataErrorContent_HttpStatusCode_Message_Target_InnerODataException()
        {
            var odataException = new ODataException("Not Found", HttpStatusCode.NotFound, "target", new ODataException("Not Implemented", HttpStatusCode.NotImplemented, "inner target"));
            var odataErrorContent = odataException.ToODataErrorContent();

            Assert.NotNull(odataErrorContent);
            Assert.NotNull(odataErrorContent.Error);
            Assert.Equal(((int)odataException.StatusCode).ToString(), odataErrorContent.Error.Code);
            Assert.Equal(odataException.Message, odataErrorContent.Error.Message);
            Assert.Equal(odataException.Target, odataErrorContent.Error.Target);

            Assert.Single(odataErrorContent.Error.Details);
            var innerODataException = (ODataException)odataException.InnerException;
            Assert.Equal(((int)innerODataException.StatusCode).ToString(), odataErrorContent.Error.Details.Single().Code);
            Assert.Equal(innerODataException.Message, odataErrorContent.Error.Details.Single().Message);
            Assert.Equal(innerODataException.Target, odataErrorContent.Error.Details.Single().Target);
        }

        [Fact]
        public void ToODataErrorDetail_HttpStatusCode_Message()
        {
            var odataException = new ODataException("Not Found", HttpStatusCode.NotFound);
            var odataErrorDetail = odataException.ToODataErrorDetail();

            Assert.NotNull(odataErrorDetail);
            Assert.Equal(((int)odataException.StatusCode).ToString(), odataErrorDetail.Code);
            Assert.Equal(odataException.Message, odataErrorDetail.Message);
            Assert.Null(odataErrorDetail.Target);
        }

        [Fact]
        public void ToODataErrorDetail_HttpStatusCode_Message_Target()
        {
            var odataException = new ODataException("Not Found", HttpStatusCode.NotFound, "target");
            var odataErrorDetail = odataException.ToODataErrorDetail();

            Assert.NotNull(odataErrorDetail);
            Assert.Equal(((int)odataException.StatusCode).ToString(), odataErrorDetail.Code);
            Assert.Equal(odataException.Message, odataErrorDetail.Message);
            Assert.Equal(odataException.Target, odataErrorDetail.Target);
        }

        [Fact]
        public void When_Constructed_Without_Specifying_HttpStatusCode()
            => Assert.Equal(HttpStatusCode.InternalServerError, new ODataException().StatusCode);
    }
}
