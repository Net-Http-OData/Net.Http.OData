using System;
using Xunit;

namespace Net.Http.OData.Tests
{
    public class ODataVersionTests
    {
        [Fact]
        public void Equals_ReturnsFalse_ForDifferentValue()
        {
            Assert.False(ODataVersion.OData40.Equals(ODataVersion.Parse("4.01")));
            Assert.False(ODataVersion.OData40 == ODataVersion.Parse("4.01"));
        }

        [Fact]
        public void Equals_ReturnsTrue_ForSameInstance()
        {
            Assert.True(ODataVersion.OData40.Equals(ODataVersion.OData40));
            Assert.True(ODataVersion.OData40 == ODataVersion.OData40);
        }

        [Fact]
        public void Equals_ReturnsTrue_ForSameValue()
        {
            Assert.True(ODataVersion.OData40.Equals(ODataVersion.Parse("4.0")));

            Assert.True(ODataVersion.OData40 == ODataVersion.Parse("4.0"));
        }

        [Fact]
        public void GreaterThan_ReturnsTrue()
        {
            Assert.True(ODataVersion.Parse("1.01") > ODataVersion.Parse("1.0"));
            Assert.True(ODataVersion.Parse("1.1") > ODataVersion.Parse("1.0"));
            Assert.True(ODataVersion.Parse("2.0") > ODataVersion.Parse("1.1"));
        }

        [Fact]
        public void GreaterThanEquals_ReturnsTrue()
        {
            Assert.True(ODataVersion.Parse("1.0") >= ODataVersion.Parse("1.0"));
            Assert.True(ODataVersion.Parse("1.01") >= ODataVersion.Parse("1.0"));
            Assert.True(ODataVersion.Parse("1.1") >= ODataVersion.Parse("1.0"));
            Assert.True(ODataVersion.Parse("1.1") >= ODataVersion.Parse("1.1"));
            Assert.True(ODataVersion.Parse("2.0") >= ODataVersion.Parse("1.1"));
        }

        [Fact]
        public void LessThan_ReturnsTrue()
        {
            Assert.True(ODataVersion.Parse("1.0") < ODataVersion.Parse("1.01"));
            Assert.True(ODataVersion.Parse("1.0") < ODataVersion.Parse("1.1"));
            Assert.True(ODataVersion.Parse("1.1") < ODataVersion.Parse("2.0"));
        }

        [Fact]
        public void LessThanEquals_ReturnsTrue()
        {
            Assert.True(ODataVersion.Parse("1.0") <= ODataVersion.Parse("1.0"));
            Assert.True(ODataVersion.Parse("1.0") <= ODataVersion.Parse("1.01"));
            Assert.True(ODataVersion.Parse("1.0") <= ODataVersion.Parse("1.1"));
            Assert.True(ODataVersion.Parse("1.1") <= ODataVersion.Parse("1.1"));
            Assert.True(ODataVersion.Parse("1.1") <= ODataVersion.Parse("2.0"));
        }

        [Fact]
        public void MaxVersion_Returns_OData40()
        {
            Assert.Same(ODataVersion.OData40, ODataVersion.MaxVersion);
        }

        [Fact]
        public void MinVersion_Returns_OData40()
        {
            Assert.Same(ODataVersion.OData40, ODataVersion.MinVersion);
        }

        [Fact]
        public void NotEquals_ReturnsTrue_ForDifferentValue()
        {
            Assert.True(ODataVersion.OData40 != ODataVersion.Parse("4.01"));
        }

        [Fact]
        public void Parse_Returns_ODataVersion()
        {
            Assert.NotNull(ODataVersion.Parse("3.0"));
        }

        [Fact]
        public void Parse_ReturnsPropertyRef_ForSameValue()
        {
            Assert.Same(ODataVersion.OData40, ODataVersion.Parse("4.0"));
        }

        [Fact]
        public void Parse_Throws_ArgumentNullException_IfVersionNull()
        {
            Assert.Throws<ArgumentNullException>(() => ODataVersion.Parse(null));
        }

        [Theory]
        [InlineData("")]
        [InlineData("foo")]
        [InlineData("1.0.0")]
        [InlineData("10")]
        public void Parse_Throws_ArgumentOutOfRangeException_IfVersionNotParsable(string version)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ODataVersion.Parse(version));
        }

        [Fact]
        public void ToString_ReturnsString()
        {
            Assert.Equal("4.0", ODataVersion.OData40.ToString());
        }

        [Theory]
        [InlineData("")]
        [InlineData("foo")]
        [InlineData("1.0.0")]
        [InlineData("10")]
        public void TryParse_Returns_False_NullODataVersion(string version)
        {
            Assert.False(ODataVersion.TryParse(version, out ODataVersion odataVersion));
            Assert.Null(odataVersion);
        }

        [Fact]
        public void TryParse_Returns_True_ODataVersion()
        {
            Assert.True(ODataVersion.TryParse("3.0", out ODataVersion odataVersion));
            Assert.NotNull(odataVersion);
        }
    }
}
