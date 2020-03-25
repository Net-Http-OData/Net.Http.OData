// -----------------------------------------------------------------------
// <copyright file="ODataQueryOptionsValidator.cs" company="Project Contributors">
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
using System.Collections.Generic;

namespace Net.Http.OData.Query
{
    /// <summary>
    /// The entry point for resolving an <see cref="IODataQueryOptionsValidator"/>.
    /// </summary>
    public static class ODataQueryOptionsValidator
    {
        private static readonly Dictionary<ODataVersion, IODataQueryOptionsValidator> s_validators = new Dictionary<ODataVersion, IODataQueryOptionsValidator>
        {
            [ODataVersion.OData40] = new ODataQueryOptionsValidator40(),
        };

        /// <summary>
        /// Gets the <see cref="IODataQueryOptionsValidator"/> for the specified <see cref="ODataVersion"/>.
        /// </summary>
        /// <param name="odataVersion">The <see cref="ODataVersion"/> to validate.</param>
        /// <returns>The <see cref="IODataQueryOptionsValidator"/> for the specified <see cref="ODataVersion"/>.</returns>
        public static IODataQueryOptionsValidator GetValidator(ODataVersion odataVersion)
        {
            if (odataVersion is null)
            {
                throw new ArgumentNullException(nameof(odataVersion));
            }

            if (s_validators.TryGetValue(odataVersion, out IODataQueryOptionsValidator validator))
            {
                return validator;
            }

            throw new NotSupportedException(odataVersion.ToString());
        }
    }
}
