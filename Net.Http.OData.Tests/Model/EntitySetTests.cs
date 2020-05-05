using System;
using Net.Http.OData.Model;
using Sample.Model;
using Xunit;

namespace Net.Http.OData.Tests.Model
{
    public class EntitySetTests
    {
        [Fact]
        public void Constructor_Throws_ArgumentException_For_Empty_Name()
            => Assert.Throws<ArgumentException>(() => new EntitySet("", new EdmComplexType(typeof(Customer), new EdmProperty[0]), null, Capabilities.None));

        [Fact]
        public void Constructor_Throws_ArgumentException_For_Null_Name()
            => Assert.Throws<ArgumentException>(() => new EntitySet(null, new EdmComplexType(typeof(Customer), new EdmProperty[0]), null, Capabilities.None));

        [Fact]
        public void Constructor_Throws_ArgumentNullException_For_Null_EdmComplexType()
            => Assert.Throws<ArgumentNullException>(() => new EntitySet("Customers", null, null, Capabilities.None));
    }
}
