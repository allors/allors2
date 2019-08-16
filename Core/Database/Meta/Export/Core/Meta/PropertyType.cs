//-------------------------------------------------------------------------------------------------
// <copyright file="PropertyType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta
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
        /// Get the value of the property on this object.
        /// </summary>
        /// <param name="strategy">
        /// The strategy.
        /// </param>
        /// <returns>
        /// The operand value.
        /// </returns>
        public abstract object Get(IStrategy strategy);

        /// <summary>
        /// Get the object type.
        /// </summary>
        /// <returns>
        /// The <see cref="IObjectType"/>.
        /// </returns>
        public abstract ObjectType GetObjectType();
    }
}
