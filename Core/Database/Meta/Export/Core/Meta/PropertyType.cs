//------------------------------------------------------------------------------------------------- 
// <copyright file="PropertyType.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
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
