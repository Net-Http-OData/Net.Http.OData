// -----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Project Contributors">
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
using System.Collections.Generic;

namespace Net.Http.OData
{
    /// <summary>
    /// Extension methods for the <see cref="string"/> class.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Reads the string in slices separated by a character from the start of the string.
        /// </summary>
        /// <param name="value">The string to read.</param>
        /// <param name="separator">The separator to slice by.</param>
        /// <returns>An IEnumerable containing the slices of the string.</returns>
        /// <remarks>Saves the extra string allocation that would occur by calling string.Split() by avoiding the string array if the array is not needed.</remarks>
        internal static IEnumerable<string> Slice(this string value, char separator)
            => Slice(value, separator, 0);

        /// <summary>
        /// Reads the string in slices separated by a character from the specified position within the string.
        /// </summary>
        /// <param name="value">The string to read.</param>
        /// <param name="separator">The separator to slice by.</param>
        /// <param name="startIndex">The index to start reading from.</param>
        /// <returns>An IEnumerable containing the slices of the string.</returns>
        /// <remarks>Saves the extra string allocation that would occur by calling string.Split() by avoiding the string array if the array is not needed.</remarks>
        internal static IEnumerable<string> Slice(this string value, char separator, int startIndex)
        {
            int startPos = startIndex;
            int endPos = value.IndexOf(separator, startPos);

            while (endPos > startPos && endPos < value.Length)
            {
                yield return value.Substring(startPos, endPos - startPos);

                startPos = endPos + 1;
                endPos = value.IndexOf(separator, startPos);
            }

            if (startPos < value.Length)
            {
                yield return value.Substring(startPos);
            }
        }
    }
}
