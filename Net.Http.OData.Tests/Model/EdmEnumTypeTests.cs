﻿using System;
using Net.Http.OData.Model;
using Sample.Model;
using Xunit;

namespace Net.Http.OData.Tests.Model
{
    public class EdmEnumTypeTests
    {
        [Fact]
        public void Constructor_SetsProperties()
        {
            Type type = typeof(AccessLevel);
            var members = new EdmEnumMember[0];

            var edmEnumType = new EdmEnumType(type, members);

            Assert.Same(type, edmEnumType.ClrType);
            Assert.Equal(type.FullName, edmEnumType.FullName);
            Assert.Equal(type.Name, edmEnumType.Name);
            Assert.Same(members, edmEnumType.Members);
        }

        [Fact]
        public void Constructor_Throws_ArgumentNullException_For_Null_Members()
        {
            Type type = typeof(AccessLevel);

            Assert.Throws<ArgumentNullException>(() => new EdmEnumType(type, null));
        }

        [Fact]
        public void GetClrValue()
        {
            Type type = typeof(AccessLevel);
            var members = new EdmEnumMember[] { new EdmEnumMember("Read", 1) };

            var edmEnumType = new EdmEnumType(type, members);

            Assert.Equal(AccessLevel.Read, edmEnumType.GetClrValue("Read"));
        }
    }
}
