// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionCommands.cs" company="Allors bvba">
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

namespace Allors.Adapters.Database.Npgsql
{
    using System.Collections.Generic;

    using Allors.Adapters.Database.Npgsql.Commands;
    using Allors.Adapters.Database.Npgsql.Commands.Procedure;
    using Allors.Adapters.Database.Npgsql.Commands.Text;
    using Allors.Adapters.Database.Sql;
    using Allors.Meta;

    using global::Npgsql;

    public sealed class SessionCommands
    {
        private readonly Database database;
        private readonly DatabaseSession session;

        private Dictionary<IClass, NpgsqlCommand> deleteObjectByClass;





        private GetObjectTypeFactory getObjectTypeFactory;
        private InstantiateObjectsFactory instantiateObjectsFactory;
        private AddCompositeRoleFactory addCompositeRoleFactory;
        private RemoveCompositeRoleFactory removeCompositeRoleFactory;
        private CreateObjectFactory createObjectFactory;
        private CreateObjectsFactory createObjectsFactory;
        private InsertObjectFactory insertObjectFactory;
        private InstantiateObjectFactory instantiateObjectFactory;
        private GetCompositeAssociationFactory getCompositeAssociationFactory;
        private GetCompositeAssociationsFactory getCompositeAssociationsFactory;
        private GetCompositeRoleFactory getCompositeRoleFactory;
        private GetCompositeRolesFactory getCompositeRolesFactory;
        private GetUnitRolesFactory getUnitRolesFactory;
        private ClearCompositeAndCompositesRoleFactory clearCompositeAndCompositesRoleFactory;
        private SetCompositeRoleFactory setCompositeRoleFactory;
        private SetUnitRoleFactory setUnitRoleFactory;
        private SetUnitRolesFactory setUnitRolesFactory;
        private GetCacheIdsFactory getCacheIdsFactory;
        private UpdateCacheIdsFactory updateCacheIdsFactory;


        private GetObjectTypeFactory.GetObjectType getObjectType;
        private CreateObjectFactory.CreateObject createObjectCommand;
        private CreateObjectsFactory.CreateObjects createObjects;
        private InsertObjectFactory.InsertObject insertObject;
        private InstantiateObjectFactory.InstantiateObject instantiateObject;
        private InstantiateObjectsFactory.InstantiateObjects instantiateObjects;
        private GetCompositeRoleFactory.GetCompositeRole getCompositeRole;
        private SetCompositeRoleFactory.SetCompositeRole setCompositeRole;
        private ClearCompositeAndCompositesRoleFactory.ClearCompositeAndCompositesRole clearCompositeAndCompoisitesRole;
        private GetCompositeAssociationFactory.GetCompositeAssociation getCompositeAssociation;
        private GetCompositeRolesFactory.GetCompositeRoles getCompositeRoles;
        private AddCompositeRoleFactory.AddCompositeRole addCompositeRole;
        private RemoveCompositeRoleFactory.RemoveCompositeRole removeCompositeRole;
        private GetCompositeAssociationsFactory.GetCompositeAssociations getCompositeAssociations;
        private UpdateCacheIdsFactory.UpdateCacheIds updateCacheIds;
        private GetUnitRolesFactory.GetUnitRoles getUnitRoles;
        private SetUnitRoleFactory.SetUnitRole setUnitRole;
        private SetUnitRolesFactory.SetUnitRoles setUnitRoles;
        private GetCacheIdsFactory.GetCacheIds getCacheIds;

        internal SessionCommands(DatabaseSession session)
        {
            this.session = session;
            this.database = session.Database;
        }

        internal void DeleteObject(Strategy strategy)
        {
            this.deleteObjectByClass = this.deleteObjectByClass ?? new Dictionary<IClass, NpgsqlCommand>();

            var @class = strategy.Class;

            if (!this.deleteObjectByClass.TryGetValue(@class, out var command))
            {
                var schema = this.session.Schema;

                var sql = $@"
DELETE FROM {schema.Objects}
WHERE {schema.ObjectId}={schema.ObjectId.Param.InvocationName};
DELETE FROM {schema.Table(strategy.Class.ExclusiveClass)}
WHERE {schema.ObjectId}={schema.ObjectId.Param.InvocationName};
";

                command = this.session.CreateNpgsqlCommand();
                command.CommandText = sql;
                command.AddInObject(this.session.Schema.ObjectId.Param, strategy.ObjectId);

                this.deleteObjectByClass[@class] = command;
            }
            else
            {
                command.SetInObject(this.session.Schema.ObjectId.Param, strategy.ObjectId);
            }

            command.ExecuteNonQuery();
        }

        internal void GetUnitRoles(Roles roles)
        {
            this.getUnitRolesFactory = this.getUnitRolesFactory ?? new GetUnitRolesFactory(this.database);
            (this.getUnitRoles = this.getUnitRoles ?? this.getUnitRolesFactory.Create(this.session)).Execute(roles);
        }

        internal void SetUnitRole(List<UnitRelation> relations, IClass exclusiveRootClass, IRoleType roleType)
        {
            this.setUnitRoleFactory = this.setUnitRoleFactory ?? (this.setUnitRoleFactory = new SetUnitRoleFactory(this.database));
            (this.setUnitRole = this.setUnitRole ?? this.setUnitRoleFactory.Create(this.session)).Execute(relations, exclusiveRootClass, roleType);
        }

        internal void SetUnitRoles(Roles roles, List<IRoleType> sortedRoleTypes)
        {
            this.setUnitRolesFactory = this.setUnitRolesFactory ?? (this.setUnitRolesFactory = new SetUnitRolesFactory(this.database));
            (this.setUnitRoles = this.setUnitRoles ?? this.setUnitRolesFactory.Create(this.session)).Execute(roles, sortedRoleTypes);
        }

        internal void GetCompositeRole(Roles roles, IRoleType roleType)
        {
            this.getCompositeRoleFactory = this.getCompositeRoleFactory ?? (this.getCompositeRoleFactory = new GetCompositeRoleFactory(this.database));
            (this.getCompositeRole = this.getCompositeRole ?? this.getCompositeRoleFactory.Create(this.session)).Execute(roles, roleType);
        }

        internal void SetCompositeRole(List<CompositeRelation> relations, IRoleType roleType)
        {
            this.setCompositeRoleFactory = this.setCompositeRoleFactory ?? (this.setCompositeRoleFactory = new SetCompositeRoleFactory(this.database));
            (this.setCompositeRole = this.setCompositeRole ?? this.setCompositeRoleFactory.Create(this.session)).Execute(relations, roleType);
        }

        internal void GetCompositesRole(Roles roles, IRoleType roleType)
        {
            this.getCompositeRolesFactory = this.getCompositeRolesFactory ?? new GetCompositeRolesFactory(this.database);
            (this.getCompositeRoles = this.getCompositeRoles ?? this.getCompositeRolesFactory.Create(this.session)).Execute(roles, roleType);
        }

        internal void AddCompositeRole(List<CompositeRelation> relations, IRoleType roleType)
        {
            this.addCompositeRoleFactory = this.addCompositeRoleFactory ?? new AddCompositeRoleFactory(this.database);
            (this.addCompositeRole = (this.addCompositeRole ?? this.addCompositeRoleFactory.Create(this.session))).Execute(relations, roleType);
        }

        internal void RemoveCompositeRole(List<CompositeRelation> relations, IRoleType roleType)
        {
            this.removeCompositeRoleFactory = this.removeCompositeRoleFactory ?? new RemoveCompositeRoleFactory(this.database);
            (this.removeCompositeRole = this.removeCompositeRole ?? this.removeCompositeRoleFactory.Create(this.session)).Execute(relations, roleType);
        }

        internal void ClearCompositeAndCompositesRole(IList<long> associations, IRoleType roleType)
        {
            this.clearCompositeAndCompositesRoleFactory = this.clearCompositeAndCompositesRoleFactory ?? new ClearCompositeAndCompositesRoleFactory(this.database);
            (this.clearCompositeAndCompoisitesRole = this.clearCompositeAndCompoisitesRole ?? this.clearCompositeAndCompositesRoleFactory.Create(this.session)).Execute(associations, roleType);
        }

        internal Reference GetCompositeAssociation(Reference role, IAssociationType associationType)
        {
            this.getCompositeAssociationFactory = this.getCompositeAssociationFactory ?? new GetCompositeAssociationFactory(this.database);
            return (this.getCompositeAssociation = this.getCompositeAssociation ?? this.getCompositeAssociationFactory.Create(this.session)).Execute(role, associationType);
        }

        internal long[] GetCompositesAssociation(Strategy role, IAssociationType associationType)
        {
            this.getCompositeAssociationsFactory = this.getCompositeAssociationsFactory ?? new GetCompositeAssociationsFactory(this.database);
            return (this.getCompositeAssociations = this.getCompositeAssociations ?? this.getCompositeAssociationsFactory.Create(this.session)).Execute(role, associationType);
        }

        internal Reference CreateObject(IClass @class)
        {
            this.createObjectFactory = this.createObjectFactory ?? (this.createObjectFactory = new CreateObjectFactory());
            return (this.createObjectCommand = this.createObjectCommand ?? this.createObjectFactory.Create(this.session)).Execute(@class);
        }

        internal IList<Reference> CreateObjects(IClass @class, int count)
        {
            this.createObjectsFactory = this.createObjectsFactory = new CreateObjectsFactory(this.database);
            return (this.createObjects = this.createObjects ?? this.createObjectsFactory.Create(this.session)).Execute(@class, count);
        }

        internal Reference InsertObject(IClass @class, long objectId)
        {
            var objectFactory = this.insertObjectFactory ?? new InsertObjectFactory();
            return (this.insertObject = this.insertObject ?? objectFactory.Create(this.session)).Execute(@class, objectId);
        }

        internal Reference InstantiateObject(long objectId)
        {
            this.instantiateObjectFactory = this.instantiateObjectFactory ?? new InstantiateObjectFactory(this.database);
            return (this.instantiateObject = (this.instantiateObject ?? this.instantiateObjectFactory.Create(this.session))).Execute(objectId);
        }

        internal IEnumerable<Reference> InstantiateReferences(IEnumerable<long> objectIds)
        {
            this.instantiateObjectsFactory = this.instantiateObjectsFactory ?? new InstantiateObjectsFactory(this.database);
            return (this.instantiateObjects = this.instantiateObjects ?? this.instantiateObjectsFactory.Create(this.session)).Execute(objectIds);
        }

        internal Dictionary<long, int> GetVersions(ISet<Reference> references)
        {
            this.getCacheIdsFactory = this.getCacheIdsFactory ?? new GetCacheIdsFactory(this.database);
            return (this.getCacheIds = this.getCacheIds ?? this.getCacheIdsFactory.Create(this.session)).Execute(references);
        }

        internal void UpdateVersion(Dictionary<Reference, Roles> modifiedRolesByReference)
        {
            this.updateCacheIdsFactory = this.updateCacheIdsFactory ?? new UpdateCacheIdsFactory(this.database);
            (this.updateCacheIds = this.updateCacheIds ?? this.updateCacheIdsFactory.Create(this.session)).Execute(modifiedRolesByReference);
        }

        internal IObjectType GetObjectType(long objectId)
        {
            this.getObjectTypeFactory = this.getObjectTypeFactory ?? new GetObjectTypeFactory(this.database);
            return (this.getObjectType = this.getObjectType ?? this.getObjectTypeFactory.Create(this.session)).Execute(objectId);
        }
    }
}