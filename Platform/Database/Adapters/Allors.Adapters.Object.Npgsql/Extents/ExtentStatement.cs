// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtentStatement.cs" company="Allors bvba">
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
    using System.Collections;

    using Allors.Adapters.Database.Npgsql;
    using Allors.Meta;

    public abstract class ExtentStatement
    {
        private readonly ArrayList referenceAssociationInstances;
        private readonly ArrayList referenceAssociations;
        private readonly ArrayList referenceRoleInstances;
        private readonly ArrayList referenceRoles;

        private readonly SqlExtent extent;

        protected ExtentStatement(SqlExtent extent)
        {
            this.extent = extent;

            this.referenceRoles = new ArrayList();
            this.referenceAssociations = new ArrayList();

            this.referenceRoleInstances = new ArrayList();
            this.referenceAssociationInstances = new ArrayList();
        }

        public SqlExtent Extent
        {
            get { return this.extent; }
        }

        public abstract bool IsRoot { get; }

        public Schema Schema
        {
            get { return this.Session.Database.Schema; }
        }

        public ExtentSort Sorter
        {
            get { return this.extent.Sorter; }
        }

        protected DatabaseSession Session
        {
            get { return this.extent.Session; }
        }

        protected IComposite Type
        {
            get { return this.extent.ObjectType; }
        }

        public void AddJoins(IObjectType leafClass, string alias)
        {
            foreach (IRoleType role in this.referenceRoles)
            {
                var relationType = role.RelationType;
                var association = relationType.AssociationType;

                if (role.ObjectType is IComposite)
                {
                    if ((association.IsMany && role.IsMany) || !relationType.ExistExclusiveClasses)
                    {
                        this.Append(" LEFT OUTER JOIN " + this.Schema.Table(role) + " " + role.SingularPropertyName + "_R");
                        this.Append(" ON " + alias + "." + this.Schema.ObjectId + "=" + role.SingularPropertyName + "_R." + this.Schema.AssociationId);
                    }
                    else
                    {
                        if (role.IsMany)
                        {
                            var compositeType = (IComposite)role.ObjectType;
                            this.Append(" LEFT OUTER JOIN " + this.Schema.Table(compositeType.ExclusiveClass) + " " + role.SingularPropertyName + "_R");
                            this.Append(" ON " + alias + "." + this.Schema.ObjectId + "=" + role.SingularPropertyName + "_R." + this.Schema.Column(association));
                        }
                    }
                }
            }

            foreach (IRoleType role in this.referenceRoleInstances)
            {
                var relationType = role.RelationType;

                if (role.ObjectType is IComposite && role.IsOne)
                {
                    if (!relationType.ExistExclusiveClasses)
                    {
                        this.Append(" LEFT OUTER JOIN " + this.Schema.Objects + " " + this.GetJoinName(role));
                        this.Append(" ON " + this.GetJoinName(role) + "." + this.Schema.ObjectId + "=" + role.SingularPropertyName + "_R." + this.Schema.RoleId + " ");
                    }
                    else
                    {
                        this.Append(" LEFT OUTER JOIN " + this.Schema.Objects + " " + this.GetJoinName(role));
                        this.Append(" ON " + this.GetJoinName(role) + "." + this.Schema.ObjectId + "=" + alias + "." + this.Schema.Column(role) + " ");
                    }
                }
            }

            foreach (IAssociationType association in this.referenceAssociations)
            {
                var relationType = association.RelationType;
                var role = relationType.RoleType;

                if ((association.IsMany && role.IsMany) || !relationType.ExistExclusiveClasses)
                {
                    this.Append(" LEFT OUTER JOIN " + Schema.Table(association) + " " + association.SingularFullName + "_A");
                    this.Append(" ON " + alias + "." + this.Schema.ObjectId + "=" + association.SingularFullName + "_A." + this.Schema.RoleId);
                }
                else
                {
                    if (!role.IsMany)
                    {
                        this.Append(" LEFT OUTER JOIN " + Schema.Table(association.ObjectType.ExclusiveClass) + " " + association.SingularFullName + "_A");
                        this.Append(" ON " + alias + "." + this.Schema.ObjectId + "=" + association.SingularFullName + "_A." + Schema.Column(role));
                    }
                }
            }

            foreach (IAssociationType association in this.referenceAssociationInstances)
            {
                var relationType = association.RelationType;
                var role = relationType.RoleType;

                if (association.ObjectType is IComposite && association.IsOne)
                {
                    if (!relationType.ExistExclusiveClasses)
                    {
                        this.Append(" LEFT OUTER JOIN " + this.Schema.Objects + " " + this.GetJoinName(association));
                        this.Append(" ON " + this.GetJoinName(association) + "." + this.Schema.ObjectId + "=" + association.SingularFullName + "_A." + this.Schema.AssociationId + " ");
                    }
                    else
                    {
                        if (role.IsOne)
                        {
                            this.Append(" LEFT OUTER JOIN " + this.Schema.Objects + " " + this.GetJoinName(association));
                            this.Append(" ON " + this.GetJoinName(association) + "." + this.Schema.ObjectId + "=" + association.SingularFullName + "_A." + this.Schema.ObjectId + " ");
                        }
                        else
                        {
                            this.Append(" LEFT OUTER JOIN " + this.Schema.Objects + " " + this.GetJoinName(association));
                            this.Append(" ON " + this.GetJoinName(association) + "." + this.Schema.ObjectId + "=" + alias + "." + Schema.Column(association) + " ");
                        }
                    }
                }
            }
        }

        public abstract string AddParameter(object obj);

        public bool AddWhere(IObjectType leafClass, string alias)
        {
            var useWhere = !this.Extent.ObjectType.ExistExclusiveClass;
            
            if (useWhere)
            {
                this.Append(" WHERE ( ");
                this.Append(" " + alias + "." + this.Schema.TypeId + "=" + this.AddParameter(this.Type.Id));
                var @interface = this.Type as IInterface;
                if (@interface != null)
                {
                    foreach (var subClass in @interface.Subclasses)
                    {
                        this.Append(" OR " + alias + "." + this.Schema.TypeId + "=" + this.AddParameter(subClass.Id));
                    }
                }

                this.Append(" ) ");
            }

            return useWhere;
        }

        public abstract void Append(string part);

        public abstract string CreateAlias();

        public abstract ExtentStatement CreateChild(SqlExtent extent, IAssociationType association);

        public abstract ExtentStatement CreateChild(SqlExtent extent, IRoleType role);

        public string GetJoinName(IAssociationType association)
        {
            return association.SingularFullName + "_AC";
        }

        public string GetJoinName(IRoleType role)
        {
            return role.SingularFullName + "_RC";
        }

        public void UseAssociation(IAssociationType association)
        {
            if (association.ObjectType is IComposite && !this.referenceAssociations.Contains(association))
            {
                this.referenceAssociations.Add(association);
            }
        }

        public void UseAssociationInstance(IAssociationType association)
        {
            if (!this.referenceAssociationInstances.Contains(association))
            {
                this.referenceAssociationInstances.Add(association);
            }
        }

        public void UseRole(IRoleType role)
        {
            if (role.ObjectType is IComposite && !this.referenceRoles.Contains(role))
            {
                this.referenceRoles.Add(role);
            }
        }

        public void UseRoleInstance(IRoleType role)
        {
            if (!this.referenceRoleInstances.Contains(role))
            {
                this.referenceRoleInstances.Add(role);
            }
        }
    }
}