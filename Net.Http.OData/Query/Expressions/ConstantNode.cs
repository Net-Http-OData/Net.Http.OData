// -----------------------------------------------------------------------
// <copyright file="ConstantNode.cs" company="Project Contributors">
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
using Net.Http.OData.Model;

namespace Net.Http.OData.Query.Expressions
{
    /// <summary>
    /// A QueryNode which represents a constant value.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{LiteralText}")]
    public abstract class ConstantNode : QueryNode
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ConstantNode" /> class.
        /// </summary>
        /// <param name="edmType">The <see cref="EdmType"/> of the value.</param>
        /// <param name="literalText">The literal text.</param>
        protected ConstantNode(EdmType edmType, string literalText)
        {
            EdmType = edmType;
            LiteralText = literalText;
        }

        /// <summary>
        /// Gets the <see cref="EdmType"/> of the value.
        /// </summary>
        public EdmType EdmType { get; }

        /// <inheritdoc/>
        public override QueryNodeKind Kind { get; } = QueryNodeKind.Constant;

        /// <summary>
        /// Gets the literal text if the constant value.
        /// </summary>
        public string LiteralText { get; }

        /// <summary>
        /// Gets the constant value as an object.
        /// </summary>
        /// <remarks>
        /// If the value in this ConstantNode is a Value Type, it will be boxed on demand by calling this getter.
        /// To avoid boxing/unboxing then cast this ConstantNode to the appropriate <see cref="ConstantNode{T}"/> and acces the value via that instead.
        /// </remarks>
        public object Value => ValueAsObject();

        /// <summary>
        /// Gets the ConstantNode which represents a value of false.
        /// </summary>
        internal static ConstantNode<bool> False { get; } = new ConstantNode<bool>(EdmPrimitiveType.Boolean, "false", false);

        /// <summary>
        /// Gets the ConstantNode which represents a 32bit integer value of 0.
        /// </summary>
        internal static ConstantNode<int> Int32Zero { get; } = new ConstantNode<int>(EdmPrimitiveType.Int32, "0", 0);

        /// <summary>
        /// Gets the ConstantNode which represents a 64bit integer value of 0.
        /// </summary>
        internal static ConstantNode<long> Int64Zero { get; } = new ConstantNode<long>(EdmPrimitiveType.Int64, "0L", 0L);

        /// <summary>
        /// Gets the ConstantNode which represents not a number.
        /// </summary>
        internal static ConstantNode<double> NaN { get; } = new ConstantNode<double>(EdmPrimitiveType.Double, "NaN", double.NaN);

        /// <summary>
        /// Gets the ConstantNode which represents negative infinity.
        /// </summary>
        internal static ConstantNode<double> NegativeInfinity { get; } = new ConstantNode<double>(EdmPrimitiveType.Double, "-INF", double.NegativeInfinity);

        /// <summary>
        /// Gets the ConstantNode which represents a value of null.
        /// </summary>
        internal static ConstantNode<object> Null { get; } = new ConstantNode<object>(null, "null", null);

        /// <summary>
        /// Gets the ConstantNode which represents infinity.
        /// </summary>
        internal static ConstantNode<double> PositiveInfinity { get; } = new ConstantNode<double>(EdmPrimitiveType.Double, "INF", double.PositiveInfinity);

        /// <summary>
        /// Gets the ConstantNode which represents a value of true.
        /// </summary>
        internal static ConstantNode<bool> True { get; } = new ConstantNode<bool>(EdmPrimitiveType.Boolean, "true", true);

        /// <summary>
        /// Gets a ConstantNode which represents a binary value.
        /// </summary>
        /// <param name="literalText">The literal text.</param>
        /// <param name="value">The value.</param>
        /// <returns>A ConstantNode which represents a binary value.</returns>
        internal static ConstantNode<byte[]> Binary(string literalText, byte[] value) => new ConstantNode<byte[]>(EdmPrimitiveType.Binary, literalText, value);

        /// <summary>
        /// Gets a ConstantNode which represents an 8 bit signed integer value.
        /// </summary>
        /// <param name="literalText">The literal text.</param>
        /// <param name="value">The value.</param>
        /// <returns>A ConstantNode which represents an 8 bit signed integer value.</returns>
        internal static ConstantNode<byte> Byte(string literalText, byte value) => new ConstantNode<byte>(EdmPrimitiveType.Byte, literalText, value);

        /// <summary>
        /// Gets a ConstantNode which represents a Date value.
        /// </summary>
        /// <param name="literalText">The literal text.</param>
        /// <param name="value">The value.</param>
        /// <returns>A ConstantNode representing a Date value.</returns>
        internal static ConstantNode<DateTime> Date(string literalText, DateTime value) => new ConstantNode<DateTime>(EdmPrimitiveType.Date, literalText, value);

        /// <summary>
        /// Gets a ConstantNode which represents a DateTimeOffset value.
        /// </summary>
        /// <param name="literalText">The literal text.</param>
        /// <param name="value">The value.</param>
        /// <returns>A ConstantNode representing a DateTimeOffset value.</returns>
        internal static ConstantNode<DateTimeOffset> DateTimeOffset(string literalText, DateTimeOffset value) => new ConstantNode<DateTimeOffset>(EdmPrimitiveType.DateTimeOffset, literalText, value);

        /// <summary>
        /// Gets a ConstantNode which represents a decimal value.
        /// </summary>
        /// <param name="literalText">The literal text.</param>
        /// <param name="value">The value.</param>
        /// <returns>A ConstantNode representing a decimal value.</returns>
        internal static ConstantNode<decimal> Decimal(string literalText, decimal value) => new ConstantNode<decimal>(EdmPrimitiveType.Decimal, literalText, value);

        /// <summary>
        /// Gets a ConstantNode which represents a double value.
        /// </summary>
        /// <param name="literalText">The literal text.</param>
        /// <param name="value">The value.</param>
        /// <returns>A ConstantNode representing a double value.</returns>
        internal static ConstantNode<double> Double(string literalText, double value) => new ConstantNode<double>(EdmPrimitiveType.Double, literalText, value);

        /// <summary>
        /// Gets a ConstantNode which represents a duration value.
        /// </summary>
        /// <param name="literalText">The literal text.</param>
        /// <param name="value">The value.</param>
        /// <returns>A ConstantNode representing a duration value.</returns>
        internal static ConstantNode<TimeSpan> Duration(string literalText, TimeSpan value) => new ConstantNode<TimeSpan>(EdmPrimitiveType.Duration, literalText, value);

        /// <summary>
        /// Gets a ConstantNode which represents a type in the entity data model.
        /// </summary>
        /// <param name="literalText">The literal text.</param>
        /// <param name="edmType">The type in the entity data model.</param>
        /// <returns>A ConstantNode representing a duration value.</returns>
        internal static ConstantNode<EdmType> EdmTypeNode(string literalText, EdmType edmType) => new ConstantNode<EdmType>(edmType, literalText, edmType);

        /// <summary>
        /// Gets a ConstantNode which represents a Guid value.
        /// </summary>
        /// <param name="literalText">The literal text.</param>
        /// <param name="value">The value.</param>
        /// <returns>A ConstantNode representing a Guid value.</returns>
        internal static ConstantNode<Guid> Guid(string literalText, Guid value) => new ConstantNode<Guid>(EdmPrimitiveType.Guid, literalText, value);

        /// <summary>
        /// Gets a ConstantNode which represents a 16 bit signed integer value.
        /// </summary>
        /// <param name="literalText">The literal text.</param>
        /// <param name="value">The value.</param>
        /// <returns>A ConstantNode representing a 16 bit signed integer value.</returns>
        internal static ConstantNode<short> Int16(string literalText, short value) => new ConstantNode<short>(EdmPrimitiveType.Int16, literalText, value);

        /// <summary>
        /// Gets a ConstantNode which represents a 32 bit signed integer value.
        /// </summary>
        /// <param name="literalText">The literal text.</param>
        /// <param name="value">The value.</param>
        /// <returns>A ConstantNode representing a 32 bit signed integer value.</returns>
        internal static ConstantNode<int> Int32(string literalText, int value) => new ConstantNode<int>(EdmPrimitiveType.Int32, literalText, value);

        /// <summary>
        /// Gets a ConstantNode which represents a 64 bit signed integer value.
        /// </summary>
        /// <param name="literalText">The literal text.</param>
        /// <param name="value">The value.</param>
        /// <returns>A ConstantNode representing a 64 bit signed integer value.</returns>
        internal static ConstantNode<long> Int64(string literalText, long value) => new ConstantNode<long>(EdmPrimitiveType.Int64, literalText, value);

        /// <summary>
        /// Gets a ConstantNode which represents an 8 bit signed integer value.
        /// </summary>
        /// <param name="literalText">The literal text.</param>
        /// <param name="value">The value.</param>
        /// <returns>A ConstantNode which represents an 8 bit signed integer value.</returns>
        internal static ConstantNode<sbyte> SByte(string literalText, sbyte value) => new ConstantNode<sbyte>(EdmPrimitiveType.SByte, literalText, value);

        /// <summary>
        /// Gets a ConstantNode which represents a float value.
        /// </summary>
        /// <param name="literalText">The literal text.</param>
        /// <param name="value">The value.</param>
        /// <returns>A ConstantNode representing a float value.</returns>
        internal static ConstantNode<float> Single(string literalText, float value) => new ConstantNode<float>(EdmPrimitiveType.Single, literalText, value);

        /// <summary>
        /// Gets a ConstantNode which represents a string value.
        /// </summary>
        /// <param name="literalText">The literal text.</param>
        /// <param name="value">The value.</param>
        /// <returns>A ConstantNode representing a string value.</returns>
        internal static ConstantNode<string> String(string literalText, string value) => new ConstantNode<string>(EdmPrimitiveType.String, literalText, value);

        /// <summary>
        /// Gets a ConstantNode which represents a time value.
        /// </summary>
        /// <param name="literalText">The literal text.</param>
        /// <param name="value">The value.</param>
        /// <returns>A ConstantNode representing a time value.</returns>
        internal static ConstantNode<TimeSpan> Time(string literalText, TimeSpan value) => new ConstantNode<TimeSpan>(EdmPrimitiveType.TimeOfDay, literalText, value);

        /// <summary>
        /// Gets the constant node value as an object.
        /// </summary>
        /// <returns>The object representing the constant node value.</returns>
        protected abstract object ValueAsObject();
    }
}
