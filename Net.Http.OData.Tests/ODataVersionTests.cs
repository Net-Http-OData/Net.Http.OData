﻿using System;
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
        public void Parse_ReturnsPropertyRef_ForSameValue()
        {
            Assert.Same(ODataVersion.OData40, ODataVersion.Parse("4.0"));
        }

        [Fact]
        public void Parse_Throws_ArgumentNullException_IfVersionNull()
        {
            Assert.Throws<ArgumentNullException>(() => ODataVersion.Parse(null));
        }

        [Fact]
        public void Parse_Throws_ArgumentOutOfRangeException_IfVersionNotParsable()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ODataVersion.Parse(""));
            Assert.Throws<ArgumentOutOfRangeException>(() => ODataVersion.Parse("foo"));
            Assert.Throws<ArgumentOutOfRangeException>(() => ODataVersion.Parse("1.0.0"));
            Assert.Throws<ArgumentOutOfRangeException>(() => ODataVersion.Parse("10"));
        }

        [Fact]
        public void ToString_ReturnsString()
        {
            Assert.Equal("4.0", ODataVersion.OData40.ToString());
        }
    }
}