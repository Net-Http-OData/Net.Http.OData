// -----------------------------------------------------------------------
// <copyright file="AbstractSelectExpandBinder.cs" company="Project Contributors">
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
namespace Net.Http.OData.Query.Binders
{
    /// <summary>
    /// A base class for binding the $select and $expand query options.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public abstract class AbstractSelectExpandBinder
    {
        /// <summary>
        /// Binds the $select and $expand properties from the OData Query.
        /// </summary>
        /// <param name="selectExpandQueryOption">The select/expand query option.</param>
        public void Bind(SelectExpandQueryOption selectExpandQueryOption)
        {
            if (selectExpandQueryOption is null)
            {
                return;
            }

            for (int i = 0; i < selectExpandQueryOption.PropertyPaths.Count; i++)
            {
                Bind(selectExpandQueryOption.PropertyPaths[i]);
            }
        }

        /// <summary>
        /// Binds the specified <see cref="PropertyPath"/>.
        /// </summary>
        /// <param name="propertyPath">The <see cref="PropertyPath"/> to bind.</param>
        protected abstract void Bind(PropertyPath propertyPath);
    }
}
