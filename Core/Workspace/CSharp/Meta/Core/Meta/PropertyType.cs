// <copyright file="PropertyType.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Meta
{
    public abstract partial class PropertyType : OperandType
    {
        protected PropertyType(MetaPopulation metaPopulation)
            : base(metaPopulation)
        {
        }

        /// <summary>
        /// Gets the operand name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Get the object type.
        /// </summary>
        /// <returns>
        /// The <see cref="IObjectType"/>.
        /// </returns>
        public abstract ObjectType GetObjectType();
    }
}
