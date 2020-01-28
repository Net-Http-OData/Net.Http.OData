// -----------------------------------------------------------------------
// <copyright file="ODataVersion.cs" company="Project Contributors">
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
using System.Globalization;
using System.Text.RegularExpressions;

namespace Net.Http.OData
{
    /// <summary>
    /// A class which represents an OData Version.
    /// </summary>
    public sealed class ODataVersion : IComparable, IComparable<ODataVersion>, IEquatable<ODataVersion>
    {
        private static readonly Regex s_regex = new Regex(@"^\d+\.\d+$");

        private readonly decimal _decimalVersion;
        private readonly string _version;

        /// <summary>
        /// Initialises a new instance of the <see cref="ODataVersion"/> class.
        /// </summary>
        /// <param name="version">The string representation of the OData version (e.g. '4.0').</param>
        private ODataVersion(string version)
        {
            _decimalVersion = decimal.Parse(version, CultureInfo.InvariantCulture);
            _version = version;
        }

        /// <summary>
        /// Gets the max <see cref="ODataVersion"/> supported by the library.
        /// </summary>
        public static ODataVersion MaxVersion => OData40;

        /// <summary>
        /// Gets the min <see cref="ODataVersion"/> supported by the library.
        /// </summary>
        public static ODataVersion MinVersion => OData40;

        /// <summary>
        /// Gets the <see cref="ODataVersion"/> for 4.0.
        /// </summary>
        public static ODataVersion OData40 { get; } = new ODataVersion("4.0");

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public static bool operator !=(ODataVersion left, ODataVersion right) => !(left == right);

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public static bool operator <(ODataVersion a, ODataVersion b)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (a == null || b == null)
            {
                return false;
            }

            return a._decimalVersion < b._decimalVersion;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public static bool operator <=(ODataVersion a, ODataVersion b)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (a == null || b == null)
            {
                return false;
            }

            return a._decimalVersion <= b._decimalVersion;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public static bool operator ==(ODataVersion left, ODataVersion right)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public static bool operator >(ODataVersion a, ODataVersion b)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (a == null || b == null)
            {
                return false;
            }

            return a._decimalVersion > b._decimalVersion;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public static bool operator >=(ODataVersion a, ODataVersion b)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (a == null || b == null)
            {
                return false;
            }

            return a._decimalVersion >= b._decimalVersion;
        }

        /// <summary>
        /// Returns an <see cref="ODataVersion"/> which represents the specified version string.
        /// </summary>
        /// <param name="version">The string representation of the OData version (e.g. '4.0').</param>
        /// <returns>An <see cref="ODataVersion"/> which represents the specified version string.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the specified version is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified version is not a string representation of an OData version.</exception>
        public static ODataVersion Parse(string version)
        {
            if (version is null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            return TryParse(version, out ODataVersion odataVersion) ? odataVersion : throw new ArgumentOutOfRangeException(nameof(version));
        }

        /// <summary>
        /// Converts the string representation of an OData version to its <see cref="ODataVersion"/> equivalent.
        /// A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="version">The string representation of the OData version (e.g. '4.0').</param>
        /// <param name="odataVersion">The <see cref="ODataVersion"/> representation of the specified string if converted successfully, otherwise null.</param>
        /// <returns> true if s was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string version, out ODataVersion odataVersion)
        {
            if (version is null || !s_regex.IsMatch(version))
            {
                odataVersion = default;
                return false;
            }

            switch (version)
            {
                case "4.0":
                    odataVersion = OData40;
                    return true;

                default:
                    odataVersion = new ODataVersion(version);
                    return true;
            }
        }

        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return 0;
            }

            return CompareTo(obj as ODataVersion);
        }

        /// <inheritdoc/>
        public int CompareTo(ODataVersion other)
        {
            if (other == null)
            {
                return 1;
            }

            return _decimalVersion.CompareTo(other._decimalVersion);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return Equals(obj as ODataVersion);
        }

        /// <inheritdoc/>
        public bool Equals(ODataVersion other)
        {
            if (other == null)
            {
                return false;
            }

            return _version == other._version;
        }

        /// <inheritdoc/>
        public override int GetHashCode() => _version.GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => _version;
    }
}
