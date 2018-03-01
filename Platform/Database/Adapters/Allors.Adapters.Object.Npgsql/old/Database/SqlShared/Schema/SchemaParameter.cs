// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SchemaParameter.cs" company="Allors bvba">
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
    using System.Data;

    public abstract class SchemaParameter
    {
        public readonly DbType DbType;
        public readonly string Name;
        public readonly string InvocationName;

        protected SchemaParameter(Schema schema, string name, DbType type)
        {
            this.Name = string.Format(schema.ParamFormat, name).ToLowerInvariant();
            this.InvocationName = string.Format(schema.ParamInvocationFormat, name);
            this.DbType = type;
        }
        
        /// <summary>
        /// Returns a String which represents the object instance.
        /// </summary>
        /// <returns>
        /// The string which represents the object instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}