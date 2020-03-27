// -----------------------------------------------------------------------
// <copyright file="ODataError.cs" company="Project Contributors">
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
namespace Net.Http.OData
{
    /// <summary>
    /// A class representing the details of an OData error.
    /// </summary>
    public sealed class ODataError
    {
        /// <summary>
        /// Gets or sets a service-defined error code.
        /// </summary>
        /// <remarks>This code serves as a sub-status for the HTTP error code specified in the response.</remarks>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets a human-readable, language-dependent representation of the error.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the target of the particular error (for example, the name of the property in error).
        /// </summary>
        public string Target { get; set; }
    }
}
