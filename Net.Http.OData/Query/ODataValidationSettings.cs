// -----------------------------------------------------------------------
// <copyright file="ODataValidationSettings.cs" company="Project Contributors">
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

namespace Net.Http.OData.Query
{
    /// <summary>
    /// A class which defines the validation settings to use when validating values in <see cref="ODataQueryOptions"/>.
    /// </summary>
    public sealed class ODataValidationSettings : IEquatable<ODataValidationSettings>
    {
        /// <summary>
        /// Gets the validation settings for when all OData queries are allowed.
        /// </summary>
        public static ODataValidationSettings All
            => new ODataValidationSettings
            {
                AllowedArithmeticOperators = AllowedArithmeticOperators.All,
                AllowedFunctions = AllowedFunctions.AllFunctions,
                AllowedLogicalOperators = AllowedLogicalOperators.All,
                AllowedQueryOptions = AllowedQueryOptions.All,
                MaxTop = 100,
            };

        /// <summary>
        /// Gets the validation settings for when no OData queries are allowed.
        /// </summary>
        public static ODataValidationSettings None
            => new ODataValidationSettings
            {
                AllowedArithmeticOperators = AllowedArithmeticOperators.None,
                AllowedFunctions = AllowedFunctions.None,
                AllowedLogicalOperators = AllowedLogicalOperators.None,
                AllowedQueryOptions = AllowedQueryOptions.None,
                MaxTop = 0,
            };

        /// <summary>
        /// Gets or sets the allowed arithmetic operators.
        /// </summary>
        public AllowedArithmeticOperators AllowedArithmeticOperators { get; set; }

        /// <summary>
        /// Gets or sets the allowed functions.
        /// </summary>
        public AllowedFunctions AllowedFunctions { get; set; }

        /// <summary>
        /// Gets or sets the allowed logical operators.
        /// </summary>
        public AllowedLogicalOperators AllowedLogicalOperators { get; set; }

        /// <summary>
        /// Gets or sets the allowed query options.
        /// </summary>
        public AllowedQueryOptions AllowedQueryOptions { get; set; }

        /// <summary>
        /// Gets or sets the max value allowed in the $top query option.
        /// </summary>
        /// <remarks>
        /// This is used to ensure that 'paged' queries do not return excessive results on each call.
        /// </remarks>
        public int MaxTop { get; set; }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public static bool operator !=(ODataValidationSettings left, ODataValidationSettings right) => !(left == right);

        public static bool operator ==(ODataValidationSettings left, ODataValidationSettings right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <inheritdoc/>
        public override bool Equals(object obj) => Equals(obj as ODataValidationSettings);

        /// <inheritdoc/>
        public bool Equals(ODataValidationSettings other)
        {
            if (other is null)
            {
                return false;
            }

            return other.AllowedArithmeticOperators == AllowedArithmeticOperators
                && other.AllowedFunctions == AllowedFunctions
                && other.AllowedLogicalOperators == AllowedLogicalOperators
                && other.AllowedQueryOptions == AllowedQueryOptions
                && other.MaxTop == MaxTop;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
            => AllowedArithmeticOperators.GetHashCode()
            ^ AllowedFunctions.GetHashCode()
            ^ AllowedLogicalOperators.GetHashCode()
            ^ AllowedQueryOptions.GetHashCode()
            ^ MaxTop.GetHashCode();
    }
}
