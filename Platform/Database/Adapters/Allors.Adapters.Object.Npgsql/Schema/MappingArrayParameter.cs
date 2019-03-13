// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SchemaTableParameter.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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

namespace Allors.Adapters.Object.Npgsql
{
    using NpgsqlTypes;

    public class MappingArrayParameter
    {
        public readonly NpgsqlDbType ElementType;
        public readonly string TypeName;
        public readonly string Name;
        public readonly string InvocationName;

        public MappingArrayParameter(Database database, string name, NpgsqlDbType elementType)
        {
            this.Name = string.Format(Mapping.ParamFormat, name);
            this.InvocationName = string.Format(Mapping.ParamInvocationFormat, name);
            this.ElementType = elementType;
            this.TypeName = database.GetSqlType(this.ElementType) + "[]";
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