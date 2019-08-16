
//------------------------------------------------------------------------------------------------- 
// <copyright file="IObject.cs" company="Allors bvba">
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
// <summary>Defines the IObject type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Allors
{
    /// <summary>
    /// <para>
    /// A strategy based object delegates its framework related
    /// behavior to its own strategy object.
    /// </para>
    /// <para>
    /// Examples of framework related behavior are: persistence, relation management,
    /// life cycle management, transaction management, etc.
    /// </para>
    /// </summary>
    public partial interface IObject
    {
        /// <summary>
        /// Gets the Strategy.
        /// </summary>
        /// <value>The strategy.</value>
        IStrategy Strategy { get; }

        /// <summary>
        /// Gets the Object Id.
        /// </summary>
        /// <value>The object id.</value>
        long Id { get; }
    }
}
