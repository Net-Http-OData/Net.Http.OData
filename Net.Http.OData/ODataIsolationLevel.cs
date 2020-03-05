// -----------------------------------------------------------------------
// <copyright file="ODataIsolationLevel.cs" company="Project Contributors">
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
    /// Specifies the isolation of the current request from external changes.
    /// </summary>
    public enum ODataIsolationLevel
    {
        /// <summary>
        /// No isolation level is specified in the request.
        /// </summary>
        None = 0,

        /// <summary>
        /// Snapshot isolation guarantees that all data returned for a request, including multiple requests within a batch or results retrieved across multiple pages, will be consistent as of a single point in time. Only data modifications made within the request (for example, by a data modification request within the same batch) are visible
        /// </summary>
        Snapshot = 1,
    }
}
