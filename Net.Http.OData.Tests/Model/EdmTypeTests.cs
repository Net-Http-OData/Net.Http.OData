using System;
using Net.Http.OData.Model;
using Sample.Model;
using Xunit;

namespace Net.Http.OData.Tests.Model
{
    public class EdmTypeTests
    {
        [Fact]
        public void GetEdmType_String_ReturnsNullForUnknownType()
        {
            Assert.Null(EdmType.GetEdmType("System.Console"));
        }

        [Fact]
        public void GetEdmType_String_ReturnsPrimitives()
        {
            TestHelper.EnsureEDM();

            var edmType = EdmType.GetEdmType("Sample.Model.AccessLevel");

            Assert.NotNull(edmType);
            Assert.Equal(typeof(AccessLevel), edmType.ClrType);
        }

        [Fact]
        public void GetEdmType_Type_ReturnsNullForUnknownType()
        {
            Assert.Null(EdmType.GetEdmType(typeof(Console)));
        }

        [Fact]
        public void GetEdmType_Type_ReturnsPrimitives()
        {
            Assert.Equal(EdmType.GetEdmType(typeof(byte[])), EdmPrimitiveType.Binary);

            Assert.Equal(EdmType.GetEdmType(typeof(bool)), EdmPrimitiveType.Boolean);
            Assert.Equal(EdmType.GetEdmType(typeof(bool?)), EdmPrimitiveType.NullableBoolean);

            Assert.Equal(EdmType.GetEdmType(typeof(byte)), EdmPrimitiveType.Byte);
            Assert.Equal(EdmType.GetEdmType(typeof(byte?)), EdmPrimitiveType.NullableByte);

            Assert.Equal(EdmType.GetEdmType(typeof(DateTime)), EdmPrimitiveType.Date);
            Assert.Equal(EdmType.GetEdmType(typeof(DateTime?)), EdmPrimitiveType.NullableDate);

            Assert.Equal(EdmType.GetEdmType(typeof(DateTimeOffset)), EdmPrimitiveType.DateTimeOffset);
            Assert.Equal(EdmType.GetEdmType(typeof(DateTimeOffset?)), EdmPrimitiveType.NullableDateTimeOffset);

            Assert.Equal(EdmType.GetEdmType(typeof(decimal)), EdmPrimitiveType.Decimal);
            Assert.Equal(EdmType.GetEdmType(typeof(decimal?)), EdmPrimitiveType.NullableDecimal);

            Assert.Equal(EdmType.GetEdmType(typeof(double)), EdmPrimitiveType.Double);
            Assert.Equal(EdmType.GetEdmType(typeof(double?)), EdmPrimitiveType.NullableDouble);

            Assert.Equal(EdmType.GetEdmType(typeof(TimeSpan)), EdmPrimitiveType.Duration);
            Assert.Equal(EdmType.GetEdmType(typeof(TimeSpan?)), EdmPrimitiveType.NullableDuration);

            Assert.Equal(EdmType.GetEdmType(typeof(Guid)), EdmPrimitiveType.Guid);
            Assert.Equal(EdmType.GetEdmType(typeof(Guid?)), EdmPrimitiveType.NullableGuid);

            Assert.Equal(EdmType.GetEdmType(typeof(short)), EdmPrimitiveType.Int16);
            Assert.Equal(EdmType.GetEdmType(typeof(short?)), EdmPrimitiveType.NullableInt16);

            Assert.Equal(EdmType.GetEdmType(typeof(int)), EdmPrimitiveType.Int32);
            Assert.Equal(EdmType.GetEdmType(typeof(int?)), EdmPrimitiveType.NullableInt32);

            Assert.Equal(EdmType.GetEdmType(typeof(long)), EdmPrimitiveType.Int64);
            Assert.Equal(EdmType.GetEdmType(typeof(long?)), EdmPrimitiveType.NullableInt64);

            Assert.Equal(EdmType.GetEdmType(typeof(sbyte)), EdmPrimitiveType.SByte);
            Assert.Equal(EdmType.GetEdmType(typeof(sbyte?)), EdmPrimitiveType.NullableSByte);

            Assert.Equal(EdmType.GetEdmType(typeof(float)), EdmPrimitiveType.Single);
            Assert.Equal(EdmType.GetEdmType(typeof(float?)), EdmPrimitiveType.NullableSingle);

            ////Assert.Equal(EdmType.GetEdmType(typeof(Stream)), EdmPrimitiveType.Stream);

            Assert.Equal(EdmType.GetEdmType(typeof(char)), EdmPrimitiveType.String);
            Assert.Equal(EdmType.GetEdmType(typeof(char?)), EdmPrimitiveType.String);
            Assert.Equal(EdmType.GetEdmType(typeof(string)), EdmPrimitiveType.String);

            ////Assert.Equal(EdmType.GetEdmType(typeof(TimeSpan)), EdmPrimitiveType.TimeOfDay);
            ////Assert.Equal(EdmType.GetEdmType(typeof(TimeSpan?)), EdmPrimitiveType.NullableTimeOfDay);
        }
    }
}
