// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SchemaIndexType.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Database.Sql
{
    public enum SchemaIndexType
    {
        /// <summary>
        /// Don't use an index
        /// </summary>
        None,

        /// <summary>
        /// Only index this column
        /// </summary>
        Single,

        /// <summary>
        /// Index this column together with the (first column of the) primary key
        /// </summary>
        Combined
    }
}