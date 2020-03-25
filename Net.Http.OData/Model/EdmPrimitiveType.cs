// -----------------------------------------------------------------------
// <copyright file="EdmPrimitiveType.cs" company="Project Contributors">
// Copyright Project Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.IO;

namespace Net.Http.OData.Model
{
    /// <summary>
    /// A class which represents a primitive type in the Entity Data Model.
    /// </summary>
    /// <seealso cref="EdmType" />
    [System.Diagnostics.DebuggerDisplay("{FullName}: {ClrType}")]
    public sealed class EdmPrimitiveType : EdmType
    {
        private EdmPrimitiveType(Type clrType, string name)
            : base(clrType, name, "Edm." + name)
        {
        }

        /// <summary>
        /// Gets the EdmType which represent fixed- or variable- length binary data.
        /// </summary>
        public static EdmType Binary { get; } = new EdmPrimitiveType(typeof(byte[]), "Binary");

        /// <summary>
        /// Gets the EdmType which represents the mathematical concept of binary-valued logic.
        /// </summary>
        public static EdmType Boolean { get; } = new EdmPrimitiveType(typeof(bool), "Boolean");

        /// <summary>
        /// Gets the EdmType which represents an unsigned 8-bit integer value.
        /// </summary>
        public static EdmType Byte { get; } = new EdmPrimitiveType(typeof(byte), "Byte");

        /// <summary>
        /// Gets the EdmType which represents date with values ranging from January 1; 1753 A.D. through December 9999 A.D.
        /// </summary>
        public static EdmType Date { get; } = new EdmPrimitiveType(typeof(DateTime), "Date");

        /// <summary>
        /// Gets the EdmType which represents date and time as an Offset in minutes from GMT; with values ranging from 12:00:00 midnight; January 1; 1753 A.D. through 11:59:59 P.M; December 9999 A.D.
        /// </summary>
        public static EdmType DateTimeOffset { get; } = new EdmPrimitiveType(typeof(DateTimeOffset), "DateTimeOffset");

#pragma warning disable CA1720 // Identifier contains type name

        /// <summary>
        /// Gets the EdmType which represents numeric values with fixed precision and scale. This type can describe a numeric value ranging from negative 10^255 + 1 to positive 10^255 -1.
        /// </summary>
        public static EdmType Decimal { get; } = new EdmPrimitiveType(typeof(decimal), "Decimal");

        /// <summary>
        /// Gets the EdmType which represents a floating point number with 15 digits precision that can represent values with approximate range of ± 2.23e -308 through ± 1.79e +308.
        /// </summary>
        public static EdmType Double { get; } = new EdmPrimitiveType(typeof(double), "Double");

#pragma warning restore CA1720 // Identifier contains type name

        /// <summary>
        /// Gets the EdmType which represents a duration.
        /// </summary>
        public static EdmType Duration { get; } = new EdmPrimitiveType(typeof(TimeSpan), "Duration");

#pragma warning disable CA1720 // Identifier contains type name

        /// <summary>
        /// Gets the EdmType which represents a 16-byte (128-bit) unique identifier value.
        /// </summary>
        public static EdmType Guid { get; } = new EdmPrimitiveType(typeof(Guid), "Guid");

        /// <summary>
        /// Gets the EdmType which represents a signed 16-bit integer value.
        /// </summary>
        public static EdmType Int16 { get; } = new EdmPrimitiveType(typeof(short), "Int16");

        /// <summary>
        /// Gets the EdmType which represents a signed 32-bit integer value.
        /// </summary>
        public static EdmType Int32 { get; } = new EdmPrimitiveType(typeof(int), "Int32");

        /// <summary>
        /// Gets the EdmType which represents a signed 64-bit integer value.
        /// </summary>
        public static EdmType Int64 { get; } = new EdmPrimitiveType(typeof(long), "Int64");

#pragma warning restore CA1720 // Identifier contains type name

        /// <summary>
        /// Gets the EdmType which represents the mathematical concept of binary-valued logic.
        /// </summary>
        public static EdmType NullableBoolean { get; } = new EdmPrimitiveType(typeof(bool?), "Boolean");

        /// <summary>
        /// Gets the EdmType which represents an unsigned 8-bit integer value.
        /// </summary>
        public static EdmType NullableByte { get; } = new EdmPrimitiveType(typeof(byte?), "Byte");

        /// <summary>
        /// Gets the EdmType which represents date with values ranging from January 1; 1753 A.D. through December 9999 A.D.
        /// </summary>
        public static EdmType NullableDate { get; } = new EdmPrimitiveType(typeof(DateTime?), "Date");

        /// <summary>
        /// Gets the EdmType which represents date and time as an Offset in minutes from GMT; with values ranging from 12:00:00 midnight; January 1; 1753 A.D. through 11:59:59 P.M; December 9999 A.D.
        /// </summary>
        public static EdmType NullableDateTimeOffset { get; } = new EdmPrimitiveType(typeof(DateTimeOffset?), "DateTimeOffset");

        /// <summary>
        /// Gets the EdmType which represents numeric values with fixed precision and scale. This type can describe a numeric value ranging from negative 10^255 + 1 to positive 10^255 -1.
        /// </summary>
        public static EdmType NullableDecimal { get; } = new EdmPrimitiveType(typeof(decimal?), "Decimal");

        /// <summary>
        /// Gets the EdmType which represents a floating point number with 15 digits precision that can represent values with approximate range of ± 2.23e -308 through ± 1.79e +308.
        /// </summary>
        public static EdmType NullableDouble { get; } = new EdmPrimitiveType(typeof(double?), "Double");

        /// <summary>
        /// Gets the EdmType which represents a duration.
        /// </summary>
        public static EdmType NullableDuration { get; } = new EdmPrimitiveType(typeof(TimeSpan?), "Duration");

        /// <summary>
        /// Gets the EdmType which represents a 16-byte (128-bit) unique identifier value.
        /// </summary>
        public static EdmType NullableGuid { get; } = new EdmPrimitiveType(typeof(Guid?), "Guid");

        /// <summary>
        /// Gets the EdmType which represents a signed 16-bit integer value.
        /// </summary>
        public static EdmType NullableInt16 { get; } = new EdmPrimitiveType(typeof(short?), "Int16");

        /// <summary>
        /// Gets the EdmType which represents a signed 32-bit integer value.
        /// </summary>
        public static EdmType NullableInt32 { get; } = new EdmPrimitiveType(typeof(int?), "Int32");

        /// <summary>
        /// Gets the EdmType which represents a signed 64-bit integer value.
        /// </summary>
        public static EdmType NullableInt64 { get; } = new EdmPrimitiveType(typeof(long?), "Int64");

        /// <summary>
        /// Gets the EdmType which represents a signed 8-bit integer value.
        /// </summary>
        public static EdmType NullableSByte { get; } = new EdmPrimitiveType(typeof(sbyte?), "SByte");

        /// <summary>
        /// Gets the EdmType which represents a floating point number with 7 digits precision that can represent values with approximate range of ± 1.18e -38 through ± 3.40e +38.
        /// </summary>
        public static EdmType NullableSingle { get; } = new EdmPrimitiveType(typeof(float?), "Single");

        /// <summary>
        /// Gets the EdmType which represents the time of day with values ranging from 0:00:00.x to 23:59:59.y, where x and y depend upon the precision.
        /// </summary>
        public static EdmType NullableTimeOfDay { get; } = new EdmPrimitiveType(typeof(TimeSpan?), "TimeOfDay");

        /// <summary>
        /// Gets the EdmType which represents a signed 8-bit integer value.
        /// </summary>
        public static EdmType SByte { get; } = new EdmPrimitiveType(typeof(sbyte), "SByte");

#pragma warning disable CA1720 // Identifier contains type name

        /// <summary>
        /// Gets the EdmType which represents a floating point number with 7 digits precision that can represent values with approximate range of ± 1.18e -38 through ± 3.40e +38.
        /// </summary>
        public static EdmType Single { get; } = new EdmPrimitiveType(typeof(float), "Single");

#pragma warning restore CA1720 // Identifier contains type name

        /// <summary>
        /// Gets the EdmType which represents a binary data stream.
        /// </summary>
        public static EdmType Stream { get; } = new EdmPrimitiveType(typeof(Stream), "Stream");

#pragma warning disable CA1720 // Identifier contains type name

        /// <summary>
        /// Gets the EdmType which represents fixed- or variable-length character data.
        /// </summary>
        public static EdmType String { get; } = new EdmPrimitiveType(typeof(string), "String");

#pragma warning restore CA1720 // Identifier contains type name

        /// <summary>
        /// Gets the EdmType which represents the time of day with values ranging from 0:00:00.x to 23:59:59.y, where x and y depend upon the precision.
        /// </summary>
        public static EdmType TimeOfDay { get; } = new EdmPrimitiveType(typeof(TimeSpan), "TimeOfDay");
    }
}
