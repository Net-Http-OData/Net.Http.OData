using System;
using Net.Http.OData.Query;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public class ODataQueryOptionsValidatorTests
    {
        [Fact]
        public void GetValidator_ReturnsODataQueryOptionsValidator40_ForODataVersion40()
        {
            IODataQueryOptionsValidator validator = ODataQueryOptionsValidator.GetValidator(ODataVersion.OData40);

            Assert.NotNull(validator);
            Assert.IsType<ODataQueryOptionsValidator40>(validator);
        }

        [Fact]
        public void GetValidator_Throws_ArgumentNullException_For_Null_ODataVersion()
        {
            Assert.Throws<ArgumentNullException>(() => ODataQueryOptionsValidator.GetValidator(null));
        }

        [Fact]
        public void GetValidator_Throws_NotSupportedException_For_Unsupported_ODataVersion()
        {
            Assert.Throws<NotSupportedException>(() => ODataQueryOptionsValidator.GetValidator(ODataVersion.Parse("3.0")));
        }
    }
}
