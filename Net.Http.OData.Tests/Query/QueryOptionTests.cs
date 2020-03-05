using System;
using System.Reflection;
using Moq;
using Net.Http.OData.Query;
using Xunit;

namespace Net.Http.OData.Tests.Query
{
    public class QueryOptionTests
    {
        [Fact]
        public void Constructor_ThrowsArgumentNullException_ForNullRawValue()
        {
            var mock = new Mock<QueryOption>(default(string)) { CallBase = true };

            TargetInvocationException exception = Assert.Throws<TargetInvocationException>(() => mock.Object);

            Assert.IsType<ArgumentNullException>(exception.InnerException);
        }
    }
}
