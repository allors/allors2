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
using Allors;
using Allors.Meta;

namespace Allors.Adapters.Relation.SqlClient
{
    internal abstract class AllorsExtentStatementSql
    {
        private readonly List<Object> associationInstances;
        private readonly List<Object> associations;
        private readonly AllorsExtentSql extent;
        private readonly Dictionary<object, string> paramNameByParamValue;
        private readonly List<Object> roleInstances;
        private readonly List<Object> roles;

        internal AllorsExtentStatementSql(AllorsExtentSql extent)
        {
            this.extent = extent;
            paramNameByParamValue = new Dictionary<object, string>();

            associations = new List<Object>();
            roleInstances = new List<Object>();
            associationInstances = new List<Object>();
            roles = new List<object>();
        }

        internal AllorsExtentSql Extent => extent;

        internal abstract bool IsRoot { get; }

        internal Mapping Mapping => Session.Database.Mapping;

        protected Session Session => extent.Session;

        internal AllorsExtentSortSql Sorter => extent.Sorter;

        protected IComposite Type => this.extent.ObjectType;

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
}