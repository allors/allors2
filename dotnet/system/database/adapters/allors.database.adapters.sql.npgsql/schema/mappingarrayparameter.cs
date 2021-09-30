// <copyright file="MappingArrayParameter.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql.Npgsql
{
    using NpgsqlTypes;

    public class MappingArrayParameter
    {
        public readonly NpgsqlDbType ElementType;
        public readonly string TypeName;
        public readonly string Name;
        public readonly string InvocationName;

        public MappingArrayParameter(Database database, Mapping mapping, string name, NpgsqlDbType elementType)
        {
            this.Name = string.Format(Mapping.ParameterFormat, name);
            this.InvocationName = string.Format(mapping.ParamInvocationFormat, name);
            this.ElementType = elementType;
            this.TypeName = database.GetSqlType(this.ElementType) + "[]";
        }

        /// <summary>
        /// Returns a String which represents the object state.
        /// </summary>
        /// <returns>
        /// The string which represents the object state.
        /// </returns>
        public override string ToString() => this.Name;
    }
}
