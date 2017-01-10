//------------------------------------------------------------------------------------------------- 
// <copyright file="AllorsExtentStatementSql.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
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
// <summary>Defines the AllorsExtentStatementSql type.</summary>
//-------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Allors;
using Allors.Meta;

namespace Allors.Adapters.Relation.SqlClient
{
    using System.Data;
    using System.Data.SqlClient;

    internal abstract class AllorsExtentStatementSql
    {
        private readonly ArrayList associationInstances;
        private readonly ArrayList associations;
        private readonly AllorsExtentSql extent;
        private readonly Dictionary<object, string> paramNameByParamValue;
        private readonly ArrayList roleInstances;
        private readonly ArrayList roles;

        internal AllorsExtentStatementSql(AllorsExtentSql extent)
        {
            this.extent = extent;
            paramNameByParamValue = new Dictionary<object, string>();

            roles = new ArrayList();
            associations = new ArrayList();
            roleInstances = new ArrayList();
            associationInstances = new ArrayList();
        }

        internal AllorsExtentSql Extent
        {
            get { return extent; }
        }

        internal abstract bool IsRoot { get; }

        internal Mapping Mapping
        {
            get { return Session.Database.Mapping; }
        }

        protected Session Session
        {
            get { return extent.Session; }
        }

        internal AllorsExtentSortSql Sorter
        {
            get { return extent.Sorter; }
        }

        protected IComposite Type
        {
            get { return this.extent.ObjectType; }
        }

        internal void AddJoins(string alias)
        {
            foreach (IRoleType role in this.roles)
            {
                this.Append(" LEFT OUTER JOIN " + this.Session.Database.SchemaName + "." + this.Mapping.GetTableName(role) + " " + role.SingularFullName + "_R");
                this.Append(" ON " + alias + "." + Mapping.ColumnNameForObject + "=" + role.SingularFullName + "_R." + Mapping.ColumnNameForAssociation);
            }

            foreach (IRoleType role in this.roleInstances)
            {
                this.Append(" LEFT OUTER JOIN " + this.Session.Database.SchemaName + "." + Mapping.TableNameForObjects + " " + this.GetJoinName(role));
                this.Append(" ON " + this.GetJoinName(role) + "." + Mapping.ColumnNameForObject + "=" + role.SingularFullName + "_R." + Mapping.ColumnNameForRole + " ");
            }

            foreach (IAssociationType association in this.associations)
            {
                this.Append(" LEFT OUTER JOIN " + this.Session.Database.SchemaName + "." + this.Mapping.GetTableName(association) + " " + association.SingularFullName + "_A");
                this.Append(" ON " + alias + "." + Mapping.ColumnNameForObject + "=" + association.SingularFullName + "_A." + Mapping.ColumnNameForRole);
            }

            foreach (IAssociationType association in this.associationInstances)
            {
                this.Append(" LEFT OUTER JOIN " + this.Session.Database.SchemaName + "." + Mapping.TableNameForObjects + " " + this.GetJoinName(association));
                this.Append(" ON " + this.GetJoinName(association) + "." + Mapping.ColumnNameForObject + "=" + association.SingularFullName + "_A." + Mapping.ColumnNameForAssociation + " ");
            }
        }

        internal abstract string AddParameter(object obj);

        internal void AddWhere(string alias)
        {
            this.Append(" WHERE (");
            if (this.Type is IClass)
            {
                this.Append(" " + alias + "." + Mapping.ColumnNameForType + "=" + this.AddParameter(this.Type.Id));
            }
            else
            {
                bool first = true;
                foreach (var subClass in this.Type.Classes)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        this.Append(" OR ");
                    }

                    this.Append(" " + alias + "." + Mapping.ColumnNameForType + "=" + this.AddParameter(subClass.Id));
                }
            }

            this.Append(" ) ");
        }

        internal abstract void Append(string part);

        internal abstract string CreateAlias();

        internal abstract AllorsExtentStatementSql CreateChild(AllorsExtentSql extent, IAssociationType association);

        internal abstract AllorsExtentStatementSql CreateChild(AllorsExtentSql extent, IRoleType roleType);

        internal string GetJoinName(IAssociationType association)
        {
            return association.SingularFullName + "_AC";
        }

        internal string GetJoinName(IRoleType role)
        {
            return role.SingularFullName + "_RC";
        }

        internal void UseAssociation(IAssociationType association)
        {
            if (!this.associations.Contains(association))
            {
                this.associations.Add(association);
            }
        }

        internal void UseAssociationInstance(IAssociationType association)
        {
            if (!this.associationInstances.Contains(association))
            {
                this.associationInstances.Add(association);
            }
        }

        internal void UseRole(IRoleType role)
        {
            if (!this.roles.Contains(role))
            {
                this.roles.Add(role);
            }
        }

        internal void UseRoleInstance(IRoleType role)
        {
            if (!this.roleInstances.Contains(role))
            {
                this.roleInstances.Add(role);
            }
        }
    }

    internal class AllorsExtentStatementRootSql : AllorsExtentStatementSql
    {
        private readonly Dictionary<object, string> paramNameByParamValue;
        private readonly StringBuilder sql;
        private int aliasIndex;
        private int parameterIndex;

        internal AllorsExtentStatementRootSql(AllorsExtentSql extent)
            : base(extent)
        {
            this.parameterIndex = 0;
            this.aliasIndex = 0;
            this.sql = new StringBuilder();
            this.paramNameByParamValue = new Dictionary<object, string>();
        }

        internal override bool IsRoot
        {
            get { return true; }
        }

        public override string ToString()
        {
            return this.sql.ToString();
        }

        internal override string AddParameter(object obj)
        {
            if (!this.paramNameByParamValue.ContainsKey(obj))
            {
                var param = Mapping.ParamPrefix + "p" + (this.parameterIndex++);
                this.paramNameByParamValue[obj] = param;
                return param;
            }

            return this.paramNameByParamValue[obj];
        }

        internal override void Append(string part)
        {
            this.sql.Append(part);
        }

        internal override string CreateAlias()
        {
            return "alias" + (this.aliasIndex++);
        }

        internal override AllorsExtentStatementSql CreateChild(AllorsExtentSql extent, IAssociationType association)
        {
            return new AllorsExtentStatementChildSql(this, extent, association);
        }

        internal override AllorsExtentStatementSql CreateChild(AllorsExtentSql extent, IRoleType roleType)
        {
            return new AllorsExtentStatementChildSql(this, extent, roleType);
        }

        internal SqlCommand CreateSqlCommand()
        {
            var command = Session.CreateCommand(this.sql.ToString());

            foreach (KeyValuePair<object, string> paramNameByParamValuePair in this.paramNameByParamValue)
            {
                var paramName = paramNameByParamValuePair.Value;
                var paramValue = paramNameByParamValuePair.Key;

                if (paramValue == null || paramValue == DBNull.Value)
                {
                    command.Parameters.AddWithValue(paramName, DBNull.Value);
                }
                else if (paramValue is IObject)
                {
                    command.Parameters.AddWithValue(paramName, ((IObject)paramValue).Id);
                }
                else
                {
                    var param = command.Parameters.AddWithValue(paramName, paramValue);

                    if (paramValue is DateTime)
                    {
                        param.SqlDbType = SqlDbType.DateTime2;
                    }
                }
            }

            return command;
        }
    }

    internal class AllorsExtentStatementChildSql : AllorsExtentStatementSql
    {
        private readonly IAssociationType association;
        private readonly IRoleType role;
        private readonly AllorsExtentStatementRootSql root;

        internal AllorsExtentStatementChildSql(AllorsExtentStatementRootSql root, AllorsExtentSql extent, IRoleType role)
            : base(extent)
        {
            this.root = root;
            this.role = role;
        }

        internal AllorsExtentStatementChildSql(AllorsExtentStatementRootSql root, AllorsExtentSql extent, IAssociationType association)
            : base(extent)
        {
            this.root = root;
            this.association = association;
        }

        public IAssociationType Association
        {
            get { return this.association; }
        }

        internal override bool IsRoot
        {
            get { return false; }
        }

        public IRoleType Role
        {
            get { return role; }
        }

        public override string ToString()
        {
            return this.root.ToString();
        }

        internal override string AddParameter(object obj)
        {
            return this.root.AddParameter(obj);
        }

        internal override void Append(string part)
        {
            this.root.Append(part);
        }

        internal override string CreateAlias()
        {
            return this.root.CreateAlias();
        }

        internal override AllorsExtentStatementSql CreateChild(AllorsExtentSql extent, IAssociationType associationType)
        {
            return new AllorsExtentStatementChildSql(this.root, extent, associationType);
        }

        internal override AllorsExtentStatementSql CreateChild(AllorsExtentSql extent, IRoleType roleType)
        {
            return new AllorsExtentStatementChildSql(this.root, extent, roleType);
        }
    }
}