using System;
using System.IO;
using Net.Http.OData.Model;
using Xunit;

namespace Net.Http.OData.Tests.Model
{
    public class EdmPrimitiveTypeTests
    {
        [Fact]
        public void Binary()
        {
            Assert.Equal(typeof(byte[]), EdmPrimitiveType.Binary.ClrType);
            Assert.Equal("Edm.Binary", EdmPrimitiveType.Binary.FullName);
            Assert.Equal("Binary", EdmPrimitiveType.Binary.Name);
            Assert.Same(EdmPrimitiveType.Binary, EdmPrimitiveType.Binary);
            Assert.Equal(EdmPrimitiveType.Binary.ToString(), EdmPrimitiveType.Binary.FullName);
        }

        [Fact]
        public void Boolean()
        {
            Assert.Equal(typeof(bool), EdmPrimitiveType.Boolean.ClrType);
            Assert.Equal("Edm.Boolean", EdmPrimitiveType.Boolean.FullName);
            Assert.Equal("Boolean", EdmPrimitiveType.Boolean.Name);
            Assert.Same(EdmPrimitiveType.Boolean, EdmPrimitiveType.Boolean);
            Assert.Equal(EdmPrimitiveType.Boolean.ToString(), EdmPrimitiveType.Boolean.FullName);
        }

        [Fact]
        public void Byte()
        {
            Assert.Equal(typeof(byte), EdmPrimitiveType.Byte.ClrType);
            Assert.Equal("Edm.Byte", EdmPrimitiveType.Byte.FullName);
            Assert.Equal("Byte", EdmPrimitiveType.Byte.Name);
            Assert.Same(EdmPrimitiveType.Byte, EdmPrimitiveType.Byte);
            Assert.Equal(EdmPrimitiveType.Byte.ToString(), EdmPrimitiveType.Byte.FullName);
        }

        [Fact]
        public void Date()
        {
            Assert.Equal(typeof(DateTime), EdmPrimitiveType.Date.ClrType);
            Assert.Equal("Edm.Date", EdmPrimitiveType.Date.FullName);
            Assert.Equal("Date", EdmPrimitiveType.Date.Name);
            Assert.Same(EdmPrimitiveType.Date, EdmPrimitiveType.Date);
            Assert.Equal(EdmPrimitiveType.Date.ToString(), EdmPrimitiveType.Date.FullName);
        }

        [Fact]
        public void DateTimeOffset()
        {
            Assert.Equal(typeof(DateTimeOffset), EdmPrimitiveType.DateTimeOffset.ClrType);
            Assert.Equal("Edm.DateTimeOffset", EdmPrimitiveType.DateTimeOffset.FullName);
            Assert.Equal("DateTimeOffset", EdmPrimitiveType.DateTimeOffset.Name);
            Assert.Same(EdmPrimitiveType.DateTimeOffset, EdmPrimitiveType.DateTimeOffset);
            Assert.Equal(EdmPrimitiveType.DateTimeOffset.ToString(), EdmPrimitiveType.DateTimeOffset.FullName);
        }

        [Fact]
        public void Decimal()
        {
            Assert.Equal(typeof(decimal), EdmPrimitiveType.Decimal.ClrType);
            Assert.Equal("Edm.Decimal", EdmPrimitiveType.Decimal.FullName);
            Assert.Equal("Decimal", EdmPrimitiveType.Decimal.Name);
            Assert.Same(EdmPrimitiveType.Decimal, EdmPrimitiveType.Decimal);
            Assert.Equal(EdmPrimitiveType.Decimal.ToString(), EdmPrimitiveType.Decimal.FullName);
        }

        [Fact]
        public void Double()
        {
            Assert.Equal(typeof(double), EdmPrimitiveType.Double.ClrType);
            Assert.Equal("Edm.Double", EdmPrimitiveType.Double.FullName);
            Assert.Equal("Double", EdmPrimitiveType.Double.Name);
            Assert.Same(EdmPrimitiveType.Double, EdmPrimitiveType.Double);
            Assert.Equal(EdmPrimitiveType.Double.ToString(), EdmPrimitiveType.Double.FullName);
        }

        [Fact]
        public void Duration()
        {
            Assert.Equal(typeof(TimeSpan), EdmPrimitiveType.Duration.ClrType);
            Assert.Equal("Edm.Duration", EdmPrimitiveType.Duration.FullName);
            Assert.Equal("Duration", EdmPrimitiveType.Duration.Name);
            Assert.Same(EdmPrimitiveType.Duration, EdmPrimitiveType.Duration);
            Assert.Equal(EdmPrimitiveType.Duration.ToString(), EdmPrimitiveType.Duration.FullName);
        }

        [Fact]
        public void Guid()
        {
            Assert.Equal(typeof(Guid), EdmPrimitiveType.Guid.ClrType);
            Assert.Equal("Edm.Guid", EdmPrimitiveType.Guid.FullName);
            Assert.Equal("Guid", EdmPrimitiveType.Guid.Name);
            Assert.Same(EdmPrimitiveType.Guid, EdmPrimitiveType.Guid);
            Assert.Equal(EdmPrimitiveType.Guid.ToString(), EdmPrimitiveType.Guid.FullName);
        }

        [Fact]
        public void Int16()
        {
            Assert.Equal(typeof(short), EdmPrimitiveType.Int16.ClrType);
            Assert.Equal("Edm.Int16", EdmPrimitiveType.Int16.FullName);
            Assert.Equal("Int16", EdmPrimitiveType.Int16.Name);
            Assert.Same(EdmPrimitiveType.Int16, EdmPrimitiveType.Int16);
            Assert.Equal(EdmPrimitiveType.Int16.ToString(), EdmPrimitiveType.Int16.FullName);
        }

        [Fact]
        public void Int32()
        {
            Assert.Equal(typeof(int), EdmPrimitiveType.Int32.ClrType);
            Assert.Equal("Edm.Int32", EdmPrimitiveType.Int32.FullName);
            Assert.Equal("Int32", EdmPrimitiveType.Int32.Name);
            Assert.Same(EdmPrimitiveType.Int32, EdmPrimitiveType.Int32);
            Assert.Equal(EdmPrimitiveType.Int32.ToString(), EdmPrimitiveType.Int32.FullName);
        }

        [Fact]
        public void Int64()
        {
            Assert.Equal(typeof(long), EdmPrimitiveType.Int64.ClrType);
            Assert.Equal("Edm.Int64", EdmPrimitiveType.Int64.FullName);
            Assert.Equal("Int64", EdmPrimitiveType.Int64.Name);
            Assert.Same(EdmPrimitiveType.Int64, EdmPrimitiveType.Int64);
            Assert.Equal(EdmPrimitiveType.Int64.ToString(), EdmPrimitiveType.Int64.FullName);
        }

        [Fact]
        public void NullableBoolean()
        {
            Assert.Equal(typeof(bool?), EdmPrimitiveType.NullableBoolean.ClrType);
            Assert.Equal("Edm.Boolean", EdmPrimitiveType.NullableBoolean.FullName);
            Assert.Equal("Boolean", EdmPrimitiveType.NullableBoolean.Name);
            Assert.Same(EdmPrimitiveType.NullableBoolean, EdmPrimitiveType.NullableBoolean);
            Assert.Equal(EdmPrimitiveType.NullableBoolean.ToString(), EdmPrimitiveType.NullableBoolean.FullName);
        }

        [Fact]
        public void NullableByte()
        {
            Assert.Equal(typeof(byte?), EdmPrimitiveType.NullableByte.ClrType);
            Assert.Equal("Edm.Byte", EdmPrimitiveType.NullableByte.FullName);
            Assert.Equal("Byte", EdmPrimitiveType.NullableByte.Name);
            Assert.Same(EdmPrimitiveType.NullableByte, EdmPrimitiveType.NullableByte);
            Assert.Equal(EdmPrimitiveType.NullableByte.ToString(), EdmPrimitiveType.NullableByte.FullName);
        }

        [Fact]
        public void NullableDate()
        {
            Assert.Equal(typeof(DateTime?), EdmPrimitiveType.NullableDate.ClrType);
            Assert.Equal("Edm.Date", EdmPrimitiveType.NullableDate.FullName);
            Assert.Equal("Date", EdmPrimitiveType.NullableDate.Name);
            Assert.Same(EdmPrimitiveType.NullableDate, EdmPrimitiveType.NullableDate);
            Assert.Equal(EdmPrimitiveType.NullableDate.ToString(), EdmPrimitiveType.NullableDate.FullName);
        }

        [Fact]
        public void NullableDateTimeOffset()
        {
            Assert.Equal(typeof(DateTimeOffset?), EdmPrimitiveType.NullableDateTimeOffset.ClrType);
            Assert.Equal("Edm.DateTimeOffset", EdmPrimitiveType.NullableDateTimeOffset.FullName);
            Assert.Equal("DateTimeOffset", EdmPrimitiveType.NullableDateTimeOffset.Name);
            Assert.Same(EdmPrimitiveType.NullableDateTimeOffset, EdmPrimitiveType.NullableDateTimeOffset);
            Assert.Equal(EdmPrimitiveType.NullableDateTimeOffset.ToString(), EdmPrimitiveType.NullableDateTimeOffset.FullName);
        }

        [Fact]
        public void NullableDecimal()
        {
            Assert.Equal(typeof(decimal?), EdmPrimitiveType.NullableDecimal.ClrType);
            Assert.Equal("Edm.Decimal", EdmPrimitiveType.NullableDecimal.FullName);
            Assert.Equal("Decimal", EdmPrimitiveType.NullableDecimal.Name);
            Assert.Same(EdmPrimitiveType.NullableDecimal, EdmPrimitiveType.NullableDecimal);
            Assert.Equal(EdmPrimitiveType.NullableDecimal.ToString(), EdmPrimitiveType.NullableDecimal.FullName);
        }

        [Fact]
        public void NullableDouble()
        {
            Assert.Equal(typeof(double?), EdmPrimitiveType.NullableDouble.ClrType);
            Assert.Equal("Edm.Double", EdmPrimitiveType.NullableDouble.FullName);
            Assert.Equal("Double", EdmPrimitiveType.NullableDouble.Name);
            Assert.Same(EdmPrimitiveType.NullableDouble, EdmPrimitiveType.NullableDouble);
            Assert.Equal(EdmPrimitiveType.NullableDouble.ToString(), EdmPrimitiveType.NullableDouble.FullName);
        }

        [Fact]
        public void NullableDuration()
        {
            Assert.Equal(typeof(TimeSpan?), EdmPrimitiveType.NullableDuration.ClrType);
            Assert.Equal("Edm.Duration", EdmPrimitiveType.NullableDuration.FullName);
            Assert.Equal("Duration", EdmPrimitiveType.NullableDuration.Name);
            Assert.Same(EdmPrimitiveType.NullableDuration, EdmPrimitiveType.NullableDuration);
            Assert.Equal(EdmPrimitiveType.NullableDuration.ToString(), EdmPrimitiveType.NullableDuration.FullName);
        }

        [Fact]
        public void NullableGuid()
        {
            Assert.Equal(typeof(Guid?), EdmPrimitiveType.NullableGuid.ClrType);
            Assert.Equal("Edm.Guid", EdmPrimitiveType.NullableGuid.FullName);
            Assert.Equal("Guid", EdmPrimitiveType.NullableGuid.Name);
            Assert.Same(EdmPrimitiveType.NullableGuid, EdmPrimitiveType.NullableGuid);
            Assert.Equal(EdmPrimitiveType.NullableGuid.ToString(), EdmPrimitiveType.NullableGuid.FullName);
        }

        [Fact]
        public void NullableInt16()
        {
            Assert.Equal(typeof(short?), EdmPrimitiveType.NullableInt16.ClrType);
            Assert.Equal("Edm.Int16", EdmPrimitiveType.NullableInt16.FullName);
            Assert.Equal("Int16", EdmPrimitiveType.NullableInt16.Name);
            Assert.Same(EdmPrimitiveType.NullableInt16, EdmPrimitiveType.NullableInt16);
            Assert.Equal(EdmPrimitiveType.NullableInt16.ToString(), EdmPrimitiveType.NullableInt16.FullName);
        }

        [Fact]
        public void NullableInt32()
        {
            Assert.Equal(typeof(int?), EdmPrimitiveType.NullableInt32.ClrType);
            Assert.Equal("Edm.Int32", EdmPrimitiveType.NullableInt32.FullName);
            Assert.Equal("Int32", EdmPrimitiveType.NullableInt32.Name);
            Assert.Same(EdmPrimitiveType.NullableInt32, EdmPrimitiveType.NullableInt32);
            Assert.Equal(EdmPrimitiveType.NullableInt32.ToString(), EdmPrimitiveType.NullableInt32.FullName);
        }

        [Fact]
        public void NullableInt64()
        {
            Assert.Equal(typeof(long?), EdmPrimitiveType.NullableInt64.ClrType);
            Assert.Equal("Edm.Int64", EdmPrimitiveType.NullableInt64.FullName);
            Assert.Equal("Int64", EdmPrimitiveType.NullableInt64.Name);
            Assert.Same(EdmPrimitiveType.NullableInt64, EdmPrimitiveType.NullableInt64);
            Assert.Equal(EdmPrimitiveType.NullableInt64.ToString(), EdmPrimitiveType.NullableInt64.FullName);
        }

        [Fact]
        public void NullableSByte()
        {
            Assert.Equal(typeof(sbyte?), EdmPrimitiveType.NullableSByte.ClrType);
            Assert.Equal("Edm.SByte", EdmPrimitiveType.NullableSByte.FullName);
            Assert.Equal("SByte", EdmPrimitiveType.NullableSByte.Name);
            Assert.Same(EdmPrimitiveType.NullableSByte, EdmPrimitiveType.NullableSByte);
            Assert.Equal(EdmPrimitiveType.NullableSByte.ToString(), EdmPrimitiveType.NullableSByte.FullName);
        }

        [Fact]
        public void NullableSingle()
        {
            Assert.Equal(typeof(float?), EdmPrimitiveType.NullableSingle.ClrType);
            Assert.Equal("Edm.Single", EdmPrimitiveType.NullableSingle.FullName);
            Assert.Equal("Single", EdmPrimitiveType.NullableSingle.Name);
            Assert.Same(EdmPrimitiveType.NullableSingle, EdmPrimitiveType.NullableSingle);
            Assert.Equal(EdmPrimitiveType.NullableSingle.ToString(), EdmPrimitiveType.NullableSingle.FullName);
        }

        [Fact]
        public void NullableTimeOfDay()
        {
            Assert.Equal(typeof(TimeSpan?), EdmPrimitiveType.NullableTimeOfDay.ClrType);
            Assert.Equal("Edm.TimeOfDay", EdmPrimitiveType.NullableTimeOfDay.FullName);
            Assert.Equal("TimeOfDay", EdmPrimitiveType.NullableTimeOfDay.Name);
            Assert.Same(EdmPrimitiveType.NullableTimeOfDay, EdmPrimitiveType.NullableTimeOfDay);
            Assert.Equal(EdmPrimitiveType.NullableTimeOfDay.ToString(), EdmPrimitiveType.NullableTimeOfDay.FullName);
        }

        [Fact]
        public void SByte()
        {
            Assert.Equal(typeof(sbyte), EdmPrimitiveType.SByte.ClrType);
            Assert.Equal("Edm.SByte", EdmPrimitiveType.SByte.FullName);
            Assert.Equal("SByte", EdmPrimitiveType.SByte.Name);
            Assert.Same(EdmPrimitiveType.SByte, EdmPrimitiveType.SByte);
            Assert.Equal(EdmPrimitiveType.SByte.ToString(), EdmPrimitiveType.SByte.FullName);
        }

        [Fact]
        public void Single()
        {
            Assert.Equal(typeof(float), EdmPrimitiveType.Single.ClrType);
            Assert.Equal("Edm.Single", EdmPrimitiveType.Single.FullName);
            Assert.Equal("Single", EdmPrimitiveType.Single.Name);
            Assert.Same(EdmPrimitiveType.Single, EdmPrimitiveType.Single);
            Assert.Equal(EdmPrimitiveType.Single.ToString(), EdmPrimitiveType.Single.FullName);
        }

        [Fact]
        public void Stream()
        {
            Assert.Equal(typeof(Stream), EdmPrimitiveType.Stream.ClrType);
            Assert.Equal("Edm.Stream", EdmPrimitiveType.Stream.FullName);
            Assert.Equal("Stream", EdmPrimitiveType.Stream.Name);
            Assert.Same(EdmPrimitiveType.Stream, EdmPrimitiveType.Stream);
            Assert.Equal(EdmPrimitiveType.Stream.ToString(), EdmPrimitiveType.Stream.FullName);
        }

        [Fact]
        public void String()
        {
            Assert.Equal(typeof(string), EdmPrimitiveType.String.ClrType);
            Assert.Equal("Edm.String", EdmPrimitiveType.String.FullName);
            Assert.Equal("String", EdmPrimitiveType.String.Name);
            Assert.Same(EdmPrimitiveType.String, EdmPrimitiveType.String);
            Assert.Equal(EdmPrimitiveType.String.ToString(), EdmPrimitiveType.String.FullName);
        }

        [Fact]
        public void TimeOfDay()
        {
            Assert.Equal(typeof(TimeSpan), EdmPrimitiveType.TimeOfDay.ClrType);
            Assert.Equal("Edm.TimeOfDay", EdmPrimitiveType.TimeOfDay.FullName);
            Assert.Equal("TimeOfDay", EdmPrimitiveType.TimeOfDay.Name);
            Assert.Same(EdmPrimitiveType.TimeOfDay, EdmPrimitiveType.TimeOfDay);
            Assert.Equal(EdmPrimitiveType.TimeOfDay.ToString(), EdmPrimitiveType.TimeOfDay.FullName);
        }
    }
}
