// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Session.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Allors;

namespace Allors.Adapters.Relation.SqlClient
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Allors.Meta;
    using Adapters;

    public class Session : ISession
    {
        internal static readonly long[] EmptyObjectIds = { };
        private static readonly IObject[] EmptyObjects = { };

        private readonly Database database;
        private readonly IRoleCache roleCache;
        private readonly IClassCache classCache;

        private SqlConnection connection;
        private SqlTransaction transaction;

        private Dictionary<string, object> properties;

        private Dictionary<long, long> cacheIdByObjectId;
        private Dictionary<long, IClass> classByObjectId;

        private HashSet<long> newObjects;
        private HashSet<long> deletedObjects;
        private HashSet<long> flushDeletedObjects;
        private HashSet<long> changedObjects;

        private ChangeSet changeSet;

        private Dictionary<IRoleType, Dictionary<long, object>> unitRoleByAssociationByRoleType;
        private Dictionary<IRoleType, HashSet<long>> unitFlushAssociationsByRoleType;

        private Dictionary<IRoleType, Dictionary<long, long?>> oneToOneRoleByAssociationByRoleType;
        private Dictionary<IRoleType, HashSet<long>> oneToOneFlushAssociationsByRoleType;
        private Dictionary<IAssociationType, Dictionary<long, long?>> oneToOneAssociationByRoleByAssociationType;

        private Dictionary<IRoleType, Dictionary<long, long?>> manyToOneRoleByAssociationByRoleType;
        private Dictionary<IRoleType, HashSet<long>> manyToOneFlushAssociationsByRoleType;
        private Dictionary<IAssociationType, Dictionary<long, long[]>> manyToOneAssociationByRoleByAssociationType;
        private Dictionary<IAssociationType, HashSet<long>> manyToOneTriggerFlushRolesByAssociationType;

        private Dictionary<IRoleType, Dictionary<long, long[]>> oneToManyCurrentRoleByAssociationByRoleType;
        private Dictionary<IRoleType, Dictionary<long, long[]>> oneToManyOriginalRoleByAssociationByRoleType;
        private Dictionary<IAssociationType, Dictionary<long, long?>> oneToManyAssociationByRoleByAssociationType;

        private Dictionary<IRoleType, Dictionary<long, long[]>> manyToManyCurrentRoleByAssociationByRoleType;
        private Dictionary<IRoleType, Dictionary<long, long[]>> manyToManyOriginalRoleByAssociationByRoleType;
        private Dictionary<IAssociationType, Dictionary<long, long[]>> manyToManyAssociationByRoleByAssociationType;
        private Dictionary<IAssociationType, HashSet<long>> manyToManyTriggerFlushRolesByAssociationType;

        private HashSet<long> objectsToFetch;

        public Session(Database database)
        {
            this.database = database;
            this.roleCache = database.RoleCache;
            this.classCache = database.ClassCache;
        }

        IDatabase ISession.Database => this.database;

        public Database Database => this.database;

        internal ChangeSet ChangeSet => this.changeSet ?? (this.changeSet = new ChangeSet());

        public object this[string name]
        {
            get
            {
                if (this.properties == null)
                {
                    return null;
                }

                object value;
                this.properties.TryGetValue(name, out value);
                return value;
            }

            set
            {
                if (this.properties == null)
                {
                    this.properties = new Dictionary<string, object>();
                }

                if (value == null)
                {
                    this.properties.Remove(name);
                    if (this.properties.Count == 0)
                    {
                        this.properties = null;
                    }
                }
                else
                {
                    this.properties[name] = value;
                }
            }
        }

        public IChangeSet Checkpoint()
        {
            try
            {
                return this.ChangeSet;
            }
            finally
            {
                this.changeSet = null;
            }
        }

        public Extent<T> Extent<T>() where T : IObject
        {
            return this.Extent((IComposite)this.Database.ObjectFactory.GetObjectTypeForType(typeof(T)));
        }

        public Extent Extent(IComposite objectType)
        {
            return new AllorsExtentFilteredSql(this, objectType);
        }

        public Extent Except(Extent firstOperand, Extent secondOperand)
        {
            return new AllorsExtentOperationSql((AllorsExtentSql)firstOperand, (AllorsExtentSql)secondOperand, AllorsExtentOperationTypeSqlBundled.EXCEPT);
        }

        public Extent Intersect(Extent firstOperand, Extent secondOperand)
        {
            return new AllorsExtentOperationSql((AllorsExtentSql)firstOperand, (AllorsExtentSql)secondOperand, AllorsExtentOperationTypeSqlBundled.INTERSECT);
        }

        public Extent Union(Extent firstOperand, Extent secondOperand)
        {
            return new AllorsExtentOperationSql((AllorsExtentSql)firstOperand, (AllorsExtentSql)secondOperand, AllorsExtentOperationTypeSqlBundled.UNION);
        }

        public void Commit()
        {
            try
            {
                this.Flush();
                this.UpdateCacheIds();

                // update class cache
                if (this.deletedObjects != null && this.deletedObjects.Count > 0)
                {
                    var deletedObjectsArray = new List<long>(this.deletedObjects).ToArray();
                    this.roleCache.Invalidate(deletedObjectsArray);
                    this.classCache.Invalidate(deletedObjectsArray);
                }

                // update role cache
                if (this.changedObjects != null && this.changedObjects.Count > 0)
                {
                    var changedObjectsArray = new List<long>(this.changedObjects).ToArray();
                    this.roleCache.Invalidate(changedObjectsArray);
                }

                this.Reset();

                if (this.transaction != null)
                {
                    this.transaction.Commit();
                    this.transaction = null;
                }
            }
            finally
            {
                this.LazyDisconnect();
            }
        }

        public void Rollback()
        {
            try
            {
                this.Reset();
            }
            finally
            {
                this.LazyDisconnect();
            }
        }

        public T Create<T>() where T : IObject
        {
            var objectType = this.Database.ObjectFactory.GetObjectTypeForType(typeof(T));
            var @class = objectType as IClass;
            if (@class == null)
            {
                throw new Exception("IObjectType is not a Class");
            }

            return (T)this.Create(@class);
        }

        public IObject Create(IClass objectType)
        {
            using (var command = this.CreateCommand(this.Database.SchemaName + "." + Mapping.ProcedureNameForCreateObject))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(Mapping.ParameterNameForType, Mapping.SqlDbTypeForType).Value = objectType.Id;
                command.Parameters.Add(Mapping.ParameterNameForCache, Mapping.SqlDbTypeForCache).Value = Database.CacheDefaultValue;

                var result = command.ExecuteScalar().ToString();
                
                var objectId = long.Parse(result);

                if (this.cacheIdByObjectId == null)
                {
                    this.cacheIdByObjectId = new Dictionary<long, long>();
                }

                if (this.classByObjectId == null)
                {
                    this.classByObjectId = new Dictionary<long, IClass>();
                }

                this.cacheIdByObjectId[objectId] = Database.CacheDefaultValue;
                this.classByObjectId[objectId] = objectType;

                this.ChangeSet.OnCreated(objectId);
                var strategy = new Strategy(this, objectId);
                var domainObject = strategy.GetObject();

                return domainObject;
            }
        }
        
        public IObject[] Create(IClass objectType, int count)
        {
            var arrayType = this.Database.ObjectFactory.GetTypeForObjectType(objectType);
            var domainObjects = (IObject[])Array.CreateInstance(arrayType, count);

            using (var command = this.CreateCommand(this.Database.SchemaName + "." + Mapping.ProcedureNameForCreateObjects))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(Mapping.ParameterNameForCount, Mapping.SqlDbTypeForCount).Value = count;
                command.Parameters.Add(Mapping.ParameterNameForType, Mapping.SqlDbTypeForType).Value = objectType.Id;
                command.Parameters.Add(Mapping.ParameterNameForCache, Mapping.SqlDbTypeForCache).Value = Database.CacheDefaultValue;

                if (this.cacheIdByObjectId == null)
                {
                    this.cacheIdByObjectId = new Dictionary<long, long>();
                }

                if (this.classByObjectId == null)
                {
                    this.classByObjectId = new Dictionary<long, IClass>();
                }

                using (var reader = command.ExecuteReader())
                {
                    var i = 0;
                    while (reader.Read())
                    {
                        var objectId = long.Parse(reader[0].ToString());

                        this.classByObjectId[objectId] = objectType;
                        this.cacheIdByObjectId[objectId] = Database.CacheDefaultValue;

                        this.ChangeSet.OnCreated(objectId);
                        var strategy = new Strategy(this, objectId);
                        var domainObject = strategy.GetObject();
                        domainObjects[i++] = domainObject;
                    }
                }
            }

            return domainObjects;
        }

        public IObject Instantiate(IObject obj)
        {
            return this.Instantiate(obj.Id);
        }

        public IObject Instantiate(string objectId)
        {
            return this.Instantiate(long.Parse(objectId));
        }

        public IObject Instantiate(long objectId)
        {
            var strategy = this.InstantiateStrategy(objectId);
            return strategy?.GetObject();
        }

        public IObject[] Instantiate(IObject[] objects)
        {
            if (objects == null || objects.Length == 0)
            {
                return EmptyObjects;
            }
            
            var objectIds = new List<long>(objects.Length);
            foreach (var obj in objects)
            {
                if (obj != null)
                {
                    objectIds.Add(obj.Id);
                }
            }

            return this.Instantiate(objectIds.ToArray());
        }

        public IObject[] Instantiate(string[] objectIdStrings)
        {
            if (objectIdStrings == null || objectIdStrings.Length == 0)
            {
                return EmptyObjects;
            }

            var objectIds = new List<long>(objectIdStrings.Length);
            foreach (var objectIdString in objectIdStrings)
            {
                if (objectIdString != null)
                {
                    objectIds.Add(long.Parse(objectIdString));
                }
            }

            return this.Instantiate(objectIds.ToArray());
        }

        public IObject[] Instantiate(long[] objectIds)
        {
            if (objectIds == null || objectIds.Length == 0)
            {
                return EmptyObjects;
            }
            
            var strategies = this.InstantiateStrategies(objectIds);
            if (strategies == null || strategies.Length == 0)
            {
                return EmptyObjects;
            }

            var objects = new IObject[strategies.Length];
            for (var i = 0; i < objects.Length; i++)
            {
                objects[i] = strategies[i].GetObject();
            }

            return objects;
        }

        public void Prefetch(PrefetchPolicy prefetchPolicy, params string[] objectIds)
        {
            // TODO:
        }

        public void Prefetch(PrefetchPolicy prefetchPolicy, long[] objectIds)
        {
            // TODO:
        }

        public void Prefetch(PrefetchPolicy prefetchPolicy, params IStrategy[] strategies)
        {
            // TODO:
        }

        public void Prefetch(PrefetchPolicy prefetchPolicy, params IObject[] objects)
        {
            // TODO:
        }

        public IObject Insert(IClass @class, string objectIdString)
        {
             var objectId = long.Parse(objectIdString);
             var insertedObject = this.Insert(@class, objectId);

            return insertedObject;
        }

        public IObject Insert(IClass @class, long objectId)
        {
            if (this.flushDeletedObjects != null)
            {
                this.Flush();
            }

            var mapping = this.Database.Mapping;
            var cmdText = @"
BEGIN
SET IDENTITY_INSERT " + this.Database.SchemaName + "." + Mapping.TableNameForObjects + @" ON;
INSERT INTO " + this.Database.SchemaName + "." + Mapping.TableNameForObjects + " (" + Mapping.ColumnNameForObject + ", " + Mapping.ColumnNameForType + ", " + Mapping.ColumnNameForCache + @")
VALUES (" + Mapping.ParameterNameForObject + ", " + Mapping.ParameterNameForType + @", " + Mapping.ParameterNameForCache + @");
SET IDENTITY_INSERT " + this.Database.SchemaName + "." + Mapping.TableNameForObjects + @" OFF;
END
";
            using (var command = this.CreateCommand(this.Database.SchemaName + "." + Mapping.ProcedureNameForInsertObject))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(Mapping.ParameterNameForObject, mapping.SqlDbTypeForObject).Value = objectId;
                command.Parameters.Add(Mapping.ParameterNameForType, Mapping.SqlDbTypeForType).Value = @class.Id;
                command.Parameters.Add(Mapping.ParameterNameForCache, Mapping.SqlDbTypeForCache).Value = Database.CacheDefaultValue;
                command.ExecuteNonQuery();

                if (this.cacheIdByObjectId == null)
                {
                    this.cacheIdByObjectId = new Dictionary<long, long>();
                }

                if (this.classByObjectId == null)
                {
                    this.classByObjectId = new Dictionary<long, IClass>();
                }

                this.cacheIdByObjectId[objectId] = Database.CacheDefaultValue;
                this.classByObjectId[objectId] = @class;

                this.ChangeSet.OnCreated(objectId);
                var strategy = new Strategy(this, objectId);
                var domainObject = strategy.GetObject();

                return domainObject;
            }
        }

        IStrategy ISession.InstantiateStrategy(long objectId)
        {
            return this.InstantiateStrategy(objectId);
        }

        public void Dispose()
        {
            this.Rollback();
        }

        internal IClass GetObjectType(long objectId)
        {
            return this.classByObjectId[objectId];
        }

        internal bool IsNew(long objectId)
        {
            return this.newObjects != null && this.newObjects.Contains(objectId);
        }

        internal bool IsDeleted(long objectId)
        {
            if (this.deletedObjects != null && this.deletedObjects.Contains(objectId))
            {
                return true;
            }

            if (this.cacheIdByObjectId != null && this.cacheIdByObjectId.ContainsKey(objectId))
            {
                return false;
            }

            this.AddObjectToFetch(objectId);            
            this.FetchObjects();

            if (this.deletedObjects != null)
            {
                if (this.deletedObjects.Contains(objectId))
                {
                    return true;
                }
            }

            return false;
        }

        internal void Delete(long objectId)
        {
            if (this.deletedObjects == null)
            {
                this.deletedObjects = new HashSet<long>();
            }

            if (this.flushDeletedObjects == null)
            {
                this.flushDeletedObjects = new HashSet<long>();
            }

            this.deletedObjects.Add(objectId);
            this.flushDeletedObjects.Add(objectId);

            this.ChangeSet.OnDeleted(objectId);
        }
        
        #region Unit
        internal bool ExistUnitRole(long association, IRoleType roleType)
        {
            return this.GetUnitRole(association, roleType) != null;
        }

        internal virtual object GetUnitRole(long association, IRoleType roleType)
        {
            var roleByAssociation = this.GetUnitRoleByAssociation(roleType);

            object role;
            if (!roleByAssociation.TryGetValue(association, out role))
            {
                var cacheId = this.GetCacheId(association);
                if (!this.roleCache.TryGetUnit(association, cacheId, roleType, out role))
                {
                    role = this.FetchUnitRole(association, roleType);
                }
                
                roleByAssociation[association] = role;
            }

            return role;
        }

        internal virtual void SetUnitRole(long association, IRoleType roleType, object role)
        {
            roleType.Normalize(role);

            var existingRole = this.GetUnitRole(association, roleType);
            if (!Equals(role, existingRole))
            {
                var roleByAssociation = this.GetUnitRoleByAssociation(roleType);
                var flushAssociations = this.GetUnitFlushAssociations(roleType);

                roleByAssociation[association] = role;
                flushAssociations.Add(association);

                this.ChangeSet.OnChangingUnitRole(association, roleType);
                this.OnObjectChanged(association);
            }
        }

        internal void RemoveUnitRole(long association, IRoleType roleType)
        {
            var existingRole = this.GetUnitRole(association, roleType);
            if (existingRole != null)
            {
                var roleByAssociation = this.GetUnitRoleByAssociation(roleType);
                var flushAssociations = this.GetUnitFlushAssociations(roleType);

                roleByAssociation[association] = null;
                flushAssociations.Add(association);

                this.ChangeSet.OnChangingUnitRole(association, roleType);
                this.OnObjectChanged(association);
            }
        }

        #endregion

        #region One <-> One
        internal bool ExistCompositeRoleOneToOne(long association, IRoleType roleType)
        {
            return this.GetCompositeRoleOneToOne(association, roleType) != null;
        }

        internal long? GetCompositeRoleOneToOne(long association, IRoleType roleType)
        {
            var roleByAssociation = this.GetOneToOneRoleByAssociation(roleType);

            long? role;
            if (!roleByAssociation.TryGetValue(association, out role))
            {
                var cacheId = this.GetCacheId(association);
                if (!this.roleCache.TryGetComposite(association, cacheId, roleType, out role))
                {
                    role = this.FetchCompositeRole(association, roleType);
                }
                
                roleByAssociation[association] = role;
            }

            return role;
        }

        internal void SetCompositeRoleOneToOne(long association, IRoleType roleType, long role)
        {
            var existingRole = this.GetCompositeRoleOneToOne(association, roleType);
            if (!Equals(role, existingRole))
            {
                var roleByAssociation = this.GetOneToOneRoleByAssociation(roleType);
                var flushAssociations = this.GetOneToOneFlushAssociations(roleType);

                var associationByRole = this.GetOneToOneAssociationByRole(roleType.AssociationType);

                // Existing Association -> Role
                long? existingAssociation;
                if (!associationByRole.TryGetValue(role, out existingAssociation))
                {
                    var existingAssociationFound = false;
                    foreach (var kvp in roleByAssociation)
                    {
                        if (role.Equals(kvp.Value))
                        {
                            existingAssociation = kvp.Key;
                            existingAssociationFound = true;
                            break;
                        }
                    }

                    if (!existingAssociationFound)
                    {
                        existingAssociation = this.GetCompositeAssociationOneToOne(role, roleType.AssociationType);
                    }
                }

                if (existingAssociation != null)
                {
                    this.ChangeSet.OnChangingCompositeRole(existingAssociation.Value, roleType, existingRole, null);

                    roleByAssociation[existingAssociation.Value] = null;
                    this.OnObjectChanged(existingAssociation.Value);
                }
                
                // Association <- Existing Role
                if (existingRole != null)
                {
                    associationByRole[existingRole.Value] = null;
                }
                
                // Association <- Role
                associationByRole[role] = association;
                
                // Association -> Role
                roleByAssociation[association] = role;
                flushAssociations.Add(association);

                this.ChangeSet.OnChangingCompositeRole(association, roleType, existingRole, role);
                this.OnObjectChanged(association);
            }
        }

        internal void RemoveCompositeRoleOneToOne(long association, IRoleType roleType)
        {
            var existingRole = this.GetCompositeRoleOneToOne(association, roleType);
            if (existingRole != null)
            {
                var roleByAssociation = this.GetOneToOneRoleByAssociation(roleType);
                var associationByRole = this.GetOneToOneAssociationByRole(roleType.AssociationType);

                // Association <- Role
                associationByRole[existingRole.Value] = null;

                // Association -> Role
                roleByAssociation[association] = null;

                var flushAssociations = this.GetOneToOneFlushAssociations(roleType);
                flushAssociations.Add(association);

                this.ChangeSet.OnChangingCompositeRole(association, roleType, existingRole, null);
                this.OnObjectChanged(association);
            }
        }

        internal bool ExistCompositeAssociationOneToOne(long role, IAssociationType associationType)
        {
            return this.GetCompositeAssociationOneToOne(role, associationType) != null;
        }

        internal long? GetCompositeAssociationOneToOne(long role, IAssociationType associationType)
        {
            var associationByRole = this.GetOneToOneAssociationByRole(associationType);
            long? association;
            if (!associationByRole.TryGetValue(role, out association))
            {
                association = this.FetchCompositeAssociation(role, associationType);
                associationByRole[role] = association;
            }

            return association;
        }
        #endregion

        #region Many <-> One
        internal bool ExistCompositeRoleManyToOne(long association, IRoleType roleType)
        {
            return this.GetCompositeRoleManyToOne(association, roleType) != null;
        }

        internal long? GetCompositeRoleManyToOne(long association, IRoleType roleType)
        {
            var roleByAssociation = this.GetManyToOneRoleByAssociation(roleType);

            long? role;
            if (!roleByAssociation.TryGetValue(association, out role))
            {
                var cacheId = this.GetCacheId(association);
                if (!this.roleCache.TryGetComposite(association, cacheId, roleType, out role))
                {
                    role = this.FetchCompositeRole(association, roleType);
                }

                roleByAssociation[association] = role;
            }

            return role;
        }

        internal void SetCompositeRoleManyToOne(long association, IRoleType roleType, long role)
        {
            var existingRole = this.GetCompositeRoleManyToOne(association, roleType);
            if (!Equals(role, existingRole))
            {
                var roleByAssociation = this.GetManyToOneRoleByAssociation(roleType);
                var flushAssociations = this.GetManyToOneFlushAssociations(roleType);

                var associationByRole = this.GetManyToOneAssociationByRole(roleType.AssociationType);
                var triggerFlushRoles = this.GetManyToOneTriggerFlushRoles(roleType.AssociationType);

                // Association <- Existing Role
                if (existingRole != null)
                {
                    associationByRole.Remove(existingRole.Value);
                    triggerFlushRoles.Add(existingRole.Value);
                }

                // Association <- Role
                associationByRole.Remove(role);
                triggerFlushRoles.Add(role);

                // Association -> Role
                roleByAssociation[association] = role;
                flushAssociations.Add(association);

                this.ChangeSet.OnChangingCompositeRole(association, roleType, existingRole, role);
                this.OnObjectChanged(association);
            }
        }

        internal void RemoveCompositeRoleManyToOne(long association, IRoleType roleType)
        {
            var existingRole = this.GetCompositeRoleManyToOne(association, roleType);
            if (existingRole != null)
            {
                var roleByAssociation = this.GetManyToOneRoleByAssociation(roleType);
                var flushAssociations = this.GetManyToOneFlushAssociations(roleType);

                var associationByRole = this.GetManyToOneAssociationByRole(roleType.AssociationType);
                var triggerFlushRoles = this.GetManyToOneTriggerFlushRoles(roleType.AssociationType);

                // Association <- Existing Role
                long[] existingAssociations;
                if (associationByRole.TryGetValue(existingRole.Value, out existingAssociations))
                {
                    var associations = new List<long>(existingAssociations);
                    associations.Remove(association);
                    associationByRole[existingRole.Value] = associations.Count != 0 ? associations.ToArray() : null;
                }
                else
                {
                    triggerFlushRoles.Add(existingRole.Value);
                }

                // Association -> Role
                roleByAssociation[association] = null;
                flushAssociations.Add(association);

                this.ChangeSet.OnChangingCompositeRole(association, roleType, existingRole, null);
                this.OnObjectChanged(association);
            }
        }

        internal bool ExistCompositeAssociationsManyToOne(long role, IAssociationType associationType)
        {
            return this.GetCompositeAssociationsManyToOne(role, associationType).Length > 0;
        }

        internal long[] GetCompositeAssociationsManyToOne(long role, IAssociationType associationType)
        {
            if (this.manyToOneTriggerFlushRolesByAssociationType != null)
            {
                HashSet<long> triggerFlushRoles;
                if (this.manyToOneTriggerFlushRolesByAssociationType.TryGetValue(associationType, out triggerFlushRoles))
                {
                    if (triggerFlushRoles.Contains(role))
                    {
                        this.FlushManyToOne(associationType.RoleType);
                        this.manyToOneTriggerFlushRolesByAssociationType.Remove(associationType);
                    }
                }
            }

            var associationByRole = this.GetManyToOneAssociationByRole(associationType);
            long[] associations;
            if (!associationByRole.TryGetValue(role, out associations))
            {
                associations = this.FetchCompositeAssociations(role, associationType);
                associationByRole[role] = associations;
            }

            return associations ?? EmptyObjectIds;
        }
        #endregion

        #region One <-> Many
        internal bool ExistCompositeRoleOneToMany(long association, IRoleType roleType)
        {
            return this.GetCompositeRolesOneToMany(association, roleType).Length > 0;
        }

        internal long[] GetCompositeRolesOneToMany(long association, IRoleType roleType)
        {
            var roleByAssociation = this.GetOneToManyCurrentRoleByAssociation(roleType);

            long[] role;
            if (!roleByAssociation.TryGetValue(association, out role))
            {
                var cacheId = this.GetCacheId(association);
                if (!this.roleCache.TryGetComposites(association, cacheId, roleType, out role))
                {
                    role = this.FetchCompositeRoles(association, roleType);
                }

                roleByAssociation[association] = role;
            }

            return role;
        }

        internal void AddCompositeRoleOneToMany(long association, IRoleType roleType, long role)
        {
            var existingRoleArray = this.GetCompositeRolesOneToMany(association, roleType);
            var existingRoles = new List<long>(existingRoleArray);
            if (!existingRoles.Contains(role))
            {
                var currentRoleByAssociation = this.GetOneToManyCurrentRoleByAssociation(roleType);
                var originalRoleByAssociation = this.GetOneToManyOriginalRoleByAssociation(roleType);

                var associationByRole = this.GetOneToManyAssociationByRole(roleType.AssociationType);

                // Existing Association -> Role
                long? existingAssociation;
                if (!associationByRole.TryGetValue(role, out existingAssociation))
                {
                    var existingAssociationFound = false;
                    foreach (var kvp in currentRoleByAssociation)
                    {
                        if (role.Equals(kvp.Value))
                        {
                            existingAssociation = kvp.Key;
                            existingAssociationFound = true;
                            break;
                        }
                    }

                    if (!existingAssociationFound)
                    {
                        existingAssociation = this.GetCompositeAssociationOneToOne(role, roleType.AssociationType);
                    }
                }

                if (existingAssociation != null)
                {
                    this.ChangeSet.OnChangingCompositesRole(existingAssociation.Value, roleType, null);

                    long[] rolesOfExistingAssociation;
                    if (currentRoleByAssociation.TryGetValue(existingAssociation.Value, out rolesOfExistingAssociation))
                    {
                        var newRolesOfExistingAssociation = new List<long>(rolesOfExistingAssociation);
                        newRolesOfExistingAssociation.Remove(role);
                        currentRoleByAssociation[existingAssociation.Value] = newRolesOfExistingAssociation.ToArray();
                    }

                    this.OnObjectChanged(existingAssociation.Value);
                }

                // Association <- Role
                associationByRole[role] = association;

                // Association -> Roles
                if (!originalRoleByAssociation.ContainsKey(association))
                {
                    originalRoleByAssociation[association] = existingRoleArray;
                }
                
                existingRoles.Add(role);
                currentRoleByAssociation[association] = existingRoles.ToArray();

                this.ChangeSet.OnChangingCompositesRole(association, roleType, role);
                this.OnObjectChanged(association);
            }
        }

        internal void RemoveCompositeRoleOneToMany(long association, IRoleType roleType, long role)
        {
            var existingRoleArray = this.GetCompositeRolesOneToMany(association, roleType);
            var existingRoles = new List<long>(existingRoleArray);
            if (existingRoles.Contains(role))
            {
                var currentRoleByAssociation = this.GetOneToManyCurrentRoleByAssociation(roleType);
                var originalRoleByAssociation = this.GetOneToManyOriginalRoleByAssociation(roleType);

                var associationByRole = this.GetOneToManyAssociationByRole(roleType.AssociationType);

                // Association <- Role
                associationByRole[role] = null;

                // Association -> Roles
                if (!originalRoleByAssociation.ContainsKey(association))
                {
                    originalRoleByAssociation[association] = existingRoleArray;
                }

                existingRoles.Remove(role);
                currentRoleByAssociation[association] = existingRoles.ToArray();

                this.ChangeSet.OnChangingCompositesRole(association, roleType, role);
                this.OnObjectChanged(association);
            }
        }

        internal void SetCompositeRoleOneToMany(long association, IRoleType roleType, long[] roles)
        {
            // TODO: optimize
            var existingRoles = new HashSet<long>(this.GetCompositeRolesOneToMany(association, roleType));

            var addRoles = new HashSet<long>(roles);
            addRoles.ExceptWith(existingRoles);
            
            existingRoles.ExceptWith(roles);
            var removeRoles = existingRoles;

            foreach (var role in addRoles)
            {
                this.AddCompositeRoleOneToMany(association, roleType, role);
            }

            foreach (var role in removeRoles)
            {
                this.RemoveCompositeRoleOneToMany(association, roleType, role);
            }
        }

        internal void RemoveCompositeRolesOneToMany(long association, IRoleType roleType)
        {
            // TODO: Optimize
            foreach (var role in this.GetCompositeRolesOneToMany(association, roleType))
            {
                this.RemoveCompositeRoleOneToMany(association, roleType, role);
            }
        }

        internal bool ExistCompositeAssociationOneToMany(long role, IAssociationType associationType)
        {
            return this.GetCompositeAssociationOneToMany(role, associationType) != null;
        }

        // TODO: Merge with GetCompositeAssociationOneToOne
        internal long? GetCompositeAssociationOneToMany(long role, IAssociationType associationType)
        {
            var associationByRole = this.GetOneToManyAssociationByRole(associationType);
            long? association;
            if (!associationByRole.TryGetValue(role, out association))
            {
                association = this.FetchCompositeAssociation(role, associationType);
                associationByRole[role] = association;
            }

            return association;
        }
        #endregion

        #region Many <-> Many
        internal bool ExistCompositeRoleManyToMany(long association, IRoleType roleType)
        {
            return this.GetCompositeRolesManyToMany(association, roleType).Length > 0;
        }

        internal long[] GetCompositeRolesManyToMany(long association, IRoleType roleType)
        {
            var roleByAssociation = this.GetManyToManyCurrentRoleByAssociation(roleType);

            long[] role;
            if (!roleByAssociation.TryGetValue(association, out role))
            {
                role = this.FetchCompositeRoles(association, roleType);
                roleByAssociation[association] = role;
            }

            return role;
        }

        internal void AddCompositeRoleManyToMany(long association, IRoleType roleType, long role)
        {
            var existingRoleArray = this.GetCompositeRolesManyToMany(association, roleType);
            var existingRoles = new List<long>(existingRoleArray);
            if (!existingRoles.Contains(role))
            {
                var originalRoleByAssociation = this.GetManyToManyOriginalRoleByAssociation(roleType);
                var currentRoleByAssociation = this.GetManyToManyCurrentRoleByAssociation(roleType);

                var triggerFlushRoles = this.GetManyToManyTriggerFlushRoles(roleType.AssociationType);
                var associationByRole = this.GetManyToManyAssociationByRole(roleType.AssociationType);

                // Association <- Role
                triggerFlushRoles.Add(role);
                associationByRole.Remove(role);

                // Association -> Roles
                if (!originalRoleByAssociation.ContainsKey(association))
                {
                    originalRoleByAssociation[association] = existingRoleArray;
                }

                existingRoles.Add(role);
                currentRoleByAssociation[association] = existingRoles.ToArray();

                this.ChangeSet.OnChangingCompositesRole(association, roleType, role);
                this.OnObjectChanged(association);
            }
        }

        internal void RemoveCompositeRoleManyToMany(long association, IRoleType roleType, long role)
        {
            var existingRoleArray = this.GetCompositeRolesManyToMany(association, roleType);
            var existingRoles = new List<long>(existingRoleArray);
            if (existingRoles.Contains(role))
            {
                var originalRoleByAssociation = this.GetManyToManyOriginalRoleByAssociation(roleType);
                var currentRoleByAssociation = this.GetManyToManyCurrentRoleByAssociation(roleType);

                var triggerFlushRoles = this.GetManyToManyTriggerFlushRoles(roleType.AssociationType);
                var associationByRole = this.GetManyToManyAssociationByRole(roleType.AssociationType);

                // Association <- Role
                triggerFlushRoles.Add(role);
                associationByRole.Remove(role);

                // Association -> Roles
                if (!originalRoleByAssociation.ContainsKey(association))
                {
                    originalRoleByAssociation[association] = existingRoleArray;
                }

                existingRoles.Remove(role);
                currentRoleByAssociation[association] = existingRoles.ToArray();

                this.ChangeSet.OnChangingCompositesRole(association, roleType, role);
                this.OnObjectChanged(association);
            }
        }

        internal void SetCompositeRoleManyToMany(long association, IRoleType roleType, long[] roles)
        {
            // TODO: optimize
            var existingRoles = new HashSet<long>(this.GetCompositeRolesManyToMany(association, roleType));

            var addRoles = new HashSet<long>(roles);
            addRoles.ExceptWith(existingRoles);

            existingRoles.ExceptWith(roles);
            var removeRoles = existingRoles;

            foreach (var role in addRoles)
            {
                this.AddCompositeRoleManyToMany(association, roleType, role);
            }

            foreach (var role in removeRoles)
            {
                this.RemoveCompositeRoleManyToMany(association, roleType, role);
            }
        }

        internal void RemoveCompositeRolesManyToMany(long association, IRoleType roleType)
        {
            // TODO: Optimize
            foreach (var role in this.GetCompositeRolesManyToMany(association, roleType))
            {
                this.RemoveCompositeRoleManyToMany(association, roleType, role);
            }
        }

        internal bool ExistCompositeAssociationsManyToMany(long role, IAssociationType associationType)
        {
            return this.GetCompositeAssociationsManyToMany(role, associationType).Length > 0;
        }

        internal long[] GetCompositeAssociationsManyToMany(long role, IAssociationType associationType)
        {
            if (this.manyToManyTriggerFlushRolesByAssociationType != null)
            {
                HashSet<long> triggerFlushRoles;
                if (this.manyToManyTriggerFlushRolesByAssociationType.TryGetValue(associationType, out triggerFlushRoles))
                {
                    if (triggerFlushRoles.Contains(role))
                    {
                        this.FlushManyToMany(associationType.RoleType);
                        this.manyToManyTriggerFlushRolesByAssociationType.Remove(associationType);
                    }
                }
            }

            var associationByRole = this.GetManyToManyAssociationByRole(associationType);
            long[] associations;
            if (!associationByRole.TryGetValue(role, out associations))
            {
                associations = this.FetchCompositeAssociations(role, associationType);
                associationByRole[role] = associations;
            }

            return associations ?? EmptyObjectIds;
        }
        #endregion

        internal SqlCommand CreateCommand(string cmdText)
        {
            if (this.connection == null)
            {
                this.connection = new SqlConnection(this.Database.ConnectionString);
                this.connection.Open();
                this.transaction = this.connection.BeginTransaction(this.Database.IsolationLevel);
            }

            var command = new SqlCommand(cmdText, this.connection, this.transaction)
            {
                CommandTimeout = this.Database.CommandTimeout
            };
            return command;
        }

        internal void Flush()
        {
            if (this.unitFlushAssociationsByRoleType != null)
            {
                foreach (var roleType in new List<IRoleType>(this.unitFlushAssociationsByRoleType.Keys))
                {
                    this.FlushUnit(roleType);
                }
            }

            if (this.oneToOneFlushAssociationsByRoleType != null)
            {
                foreach (var roleType in new List<IRoleType>(this.oneToOneFlushAssociationsByRoleType.Keys))
                {
                    this.FlushOneToOne(roleType);
                }
            }

            if (this.manyToOneFlushAssociationsByRoleType != null)
            {
                foreach (var roleType in new List<IRoleType>(this.manyToOneFlushAssociationsByRoleType.Keys))
                {
                    this.FlushManyToOne(roleType);
                }
            }

            if (this.oneToManyOriginalRoleByAssociationByRoleType != null)
            {
                foreach (var roleType in new List<IRoleType>(this.oneToManyOriginalRoleByAssociationByRoleType.Keys))
                {
                    this.FlushOneToMany(roleType);
                }
            }

            if (this.manyToManyOriginalRoleByAssociationByRoleType != null)
            {
                foreach (var roleType in new List<IRoleType>(this.manyToManyOriginalRoleByAssociationByRoleType.Keys))
                {
                    this.FlushManyToMany(roleType);
                }
            }

            this.FlushDeletedObjects();
        }

        private static void SplitFlushAssociations(IEnumerable<long> flushAssociations, IReadOnlyDictionary<long, object> roleByAssociation, out IList<long> flushDeleted, out IList<long> flushChanged)
        {
            IList<long> deleted = null;
            IList<long> changed = null;
            foreach (var flushAssociation in flushAssociations)
            {
                var unit = roleByAssociation[flushAssociation];

                if (unit == null)
                {
                    if (deleted == null)
                    {
                        deleted = new List<long>();
                    }

                    deleted.Add(flushAssociation);
                }
                else
                {
                    if (changed == null)
                    {
                        changed = new List<long>();
                    }

                    changed.Add(flushAssociation);
                }
            }

            flushDeleted = deleted;
            flushChanged = changed;
        }

        private static void SplitFlushAssociations(IEnumerable<long> flushAssociations, IReadOnlyDictionary<long, long?> roleByAssociation, out IList<long> flushDeleted, out IList<long> flushChanged)
        {
            IList<long> deleted = null;
            IList<long> changed = null;
            foreach (var flushAssociation in flushAssociations)
            {
                var unit = roleByAssociation[flushAssociation];

                if (unit == null)
                {
                    if (deleted == null)
                    {
                        deleted = new List<long>();
                    }

                    deleted.Add(flushAssociation);
                }
                else
                {
                    if (changed == null)
                    {
                        changed = new List<long>();
                    }

                    changed.Add(flushAssociation);
                }
            }

            flushDeleted = deleted;
            flushChanged = changed;
        }

        internal long GetCacheId(long objectId)
        {
            long cacheId;
            if (this.cacheIdByObjectId == null || !this.cacheIdByObjectId.TryGetValue(objectId, out cacheId))
            {
                this.AddObjectToFetch(objectId);
                this.FetchObjects();
                cacheId = this.cacheIdByObjectId[objectId];
            }

            return cacheId;
        }

        private IStrategy InstantiateStrategy(long objectId)
        {
            if (this.deletedObjects != null && this.deletedObjects.Contains(objectId))
            {
                return null;
            }

            if (this.cacheIdByObjectId == null || !this.cacheIdByObjectId.ContainsKey(objectId))
            {
                this.AddObjectToFetch(objectId);
                this.FetchObjects();
            }

            if (this.deletedObjects != null && this.deletedObjects.Contains(objectId))
            {
                return null;
            }

            return new Strategy(this, objectId);
        }
        
        private IStrategy[] InstantiateStrategies(long[] objectIds)
        {
            var fetchRequired = false;
            foreach (var objectId in objectIds)
            {
                if (this.deletedObjects != null && this.deletedObjects.Contains(objectId))
                {
                    continue;
                }

                if (this.cacheIdByObjectId == null || !this.cacheIdByObjectId.ContainsKey(objectId))
                {
                    this.AddObjectToFetch(objectId);
                    fetchRequired = true;
                }
            }

            if (fetchRequired)
            {
                this.FetchObjects();
            }

            var strategies = new List<IStrategy>();
            foreach (var objectId in objectIds)
            {
                if (this.deletedObjects == null || !this.deletedObjects.Contains(objectId))
                {
                    var strategy = new Strategy(this, objectId);
                    strategies.Add(strategy);
                }
            }

            return strategies.ToArray();
        }
        
        private void FetchObjects()
        {
            var mapping = this.database.Mapping;
            using (var command = this.CreateCommand(this.Database.SchemaName + "." + Mapping.ProcedureNameForFetchObjects))
            {
                command.CommandType = CommandType.StoredProcedure;
                var objectDataRecords = new ObjectDataRecords(mapping, this.objectsToFetch);
                var parameter = command.Parameters.Add(Mapping.ParameterNameForObjectTable, SqlDbType.Structured);
                parameter.TypeName = this.Database.SchemaName + "." + Mapping.TableTypeNameForObjects;
                parameter.Value = objectDataRecords;
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var objectId = long.Parse(reader.GetValue(0).ToString());
                        var typeId = reader.GetGuid(1);
                        var cacheId = reader.GetInt64(2);
                        var type = (IClass)this.database.ObjectFactory.MetaPopulation.Find(typeId);

                        if (this.classByObjectId == null)
                        {
                            this.classByObjectId = new Dictionary<long, IClass>();
                        }

                        if (this.cacheIdByObjectId == null)
                        {
                            this.cacheIdByObjectId = new Dictionary<long, long>();
                        }
                        
                        this.classByObjectId[objectId] = type;
                        this.cacheIdByObjectId[objectId] = cacheId;

                        this.classCache.Set(objectId, type);

                        this.objectsToFetch.Remove(objectId);
                    }

                    foreach (var objectId in this.objectsToFetch)
                    {
                        if (this.deletedObjects == null)
                        {
                            this.deletedObjects = new HashSet<long>();
                        }

                        this.deletedObjects.Add(objectId);
                    }
                }

                this.objectsToFetch = null;
            }
        }

        private void UpdateCacheIds()
        {
            if (this.changedObjects != null)
            {
                var mapping = this.database.Mapping;
                using (var command = this.CreateCommand(this.Database.SchemaName + "." + Mapping.ProcedureNameForUpdateCacheIds))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var objectDataRecords = new ObjectDataRecords(mapping, this.changedObjects);
                    var parameter = command.Parameters.Add(Mapping.ParameterNameForObjectTable, SqlDbType.Structured);
                    parameter.TypeName = this.Database.SchemaName + "." + Mapping.TableTypeNameForObjects;
                    parameter.Value = objectDataRecords;

                    command.ExecuteNonQuery();
                }
            }
        }

        private object FetchUnitRole(long association, IRoleType roleType)
        {
            var mapping = this.database.Mapping;
            using (var command = this.CreateCommand(this.Database.SchemaName + "." + mapping.GetProcedureNameForGetRole(roleType.RelationType)))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(Mapping.ParameterNameForAssociation, mapping.SqlDbTypeForObject).Value = association;
                var role = command.ExecuteScalar();

                if (role is DateTime)
                {
                    var dateTime = (DateTime)role;
                    if (dateTime != DateTime.MaxValue && dateTime != DateTime.MinValue)
                    {
                        role = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, DateTimeKind.Utc);
                    }
                }

                var cacheId = this.GetCacheId(association);
                this.roleCache.SetUnit(association, cacheId, roleType, role);
                
                return role;
            }
        }

        private long? FetchCompositeRole(long association, IRoleType roleType)
        {
            var mapping = this.database.Mapping;

            using (var command = this.CreateCommand(this.Database.SchemaName + "." + mapping.GetProcedureNameForGetRole(roleType.RelationType)))
            {
                long? role = null;
                
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(Mapping.ParameterNameForAssociation, mapping.SqlDbTypeForObject).Value = association;
                var result = command.ExecuteScalar();
                if (result != null)
                {
                    role = long.Parse(result.ToString());
                    this.AddObjectToFetchIfNotCached(role.Value);
                }

                var cacheId = this.GetCacheId(association);
                this.roleCache.SetComposite(association, cacheId, roleType, role);

                return role;
            }
        }

        private long[] FetchCompositeRoles(long association, IRoleType roleType)
        {
            var mapping = this.database.Mapping;

            List<long> roles = null;
            using (var command = this.CreateCommand(this.Database.SchemaName + "." + mapping.GetProcedureNameForGetRole(roleType.RelationType)))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(Mapping.ParameterNameForAssociation, mapping.SqlDbTypeForObject).Value = association;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (roles == null)
                        {
                            roles = new List<long>();
                        }

                        var value = reader.GetValue(0).ToString();
                        var role = long.Parse(value);
                        roles.Add(role);

                        this.AddObjectToFetchIfNotCached(role);
                    }
                }
            }

            var roleArray = roles != null ? roles.ToArray() : EmptyObjectIds;

            var cacheId = this.GetCacheId(association);
            this.roleCache.SetComposites(association, cacheId, roleType, roleArray);

            return roleArray;
        }

        private long? FetchCompositeAssociation(long role, IAssociationType associationType)
        {
            var mapping = this.database.Mapping;

            using (var command = this.CreateCommand(this.Database.SchemaName + "." + mapping.GetProcedureNameForGetAssociation(associationType.RelationType)))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(Mapping.ParameterNameForRole, mapping.SqlDbTypeForObject).Value = role;
                var result = command.ExecuteScalar();
                if (result != null)
                {
                    var association = long.Parse(result.ToString());

                    this.AddObjectToFetchIfNotCached(association);

                    return association;
                }
            }

            return null;
        }

        private long[] FetchCompositeAssociations(long role, IAssociationType associationType)
        {
            var mapping = this.database.Mapping;

            List<long> associations = null;
            using (var command = this.CreateCommand(this.Database.SchemaName + "." + mapping.GetProcedureNameForGetAssociation(associationType.RelationType)))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(Mapping.ParameterNameForRole, mapping.SqlDbTypeForObject).Value = role;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (associations == null)
                        {
                            associations = new List<long>();
                        }

                        var value = reader.GetValue(0).ToString();
                        var association = long.Parse(value);
                        associations.Add(association);

                        this.AddObjectToFetchIfNotCached(association);
                    }
                }
            }

            return associations != null ? associations.ToArray() : EmptyObjectIds;
        }

        private void FlushDeletedObjects()
        {
            if (this.flushDeletedObjects != null)
            {
                var mapping = this.database.Mapping;

                using (var command = this.CreateCommand(this.Database.SchemaName + "." + Mapping.ProcedureNameForDeleteObjects))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var objectDataRecords = new ObjectDataRecords(mapping, this.flushDeletedObjects);
                    var parameter = command.Parameters.Add(Mapping.ParameterNameForObjectTable, SqlDbType.Structured);
                    parameter.TypeName = this.Database.SchemaName + "." + Mapping.TableTypeNameForObjects;
                    parameter.Value = objectDataRecords;

                    command.ExecuteNonQuery();
                }
            }
        }

        private void FlushUnit(IRoleType roleType)
        {
            if (this.unitFlushAssociationsByRoleType != null)
            {
                HashSet<long> flushAssociations;
                if(this.unitFlushAssociationsByRoleType.TryGetValue(roleType, out flushAssociations))
                {
                    var roleByAssociation = this.unitRoleByAssociationByRoleType[roleType];

                    IList<long> flushDeleted;
                    IList<long> flushChanged;
                    SplitFlushAssociations(flushAssociations, roleByAssociation, out flushDeleted, out flushChanged);

                    this.FlushRelationDelete(roleType, flushDeleted);

                    if (flushChanged != null)
                    {
                        var relationType = roleType.RelationType;
                        var mapping = this.database.Mapping;
                        using (var command = this.CreateCommand(this.Database.SchemaName + "." + mapping.GetProcedureNameForSetRole(relationType)))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            var unitRoleDataRecords = new UnitRoleDataRecords(mapping, roleType, flushChanged, roleByAssociation);
                            var parameter = command.Parameters.Add(Mapping.ParameterNameForRelationTable, SqlDbType.Structured);
                            parameter.TypeName = this.Database.SchemaName + "." + mapping.GetTableTypeName(relationType);
                            parameter.Value = unitRoleDataRecords;
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private void FlushOneToOne(IRoleType roleType)
        {
            if (this.oneToOneFlushAssociationsByRoleType != null)
            {
                HashSet<long> flushAssociations;
                if(this.oneToOneFlushAssociationsByRoleType.TryGetValue(roleType, out flushAssociations))
                {
                    var roleByAssociation = this.oneToOneRoleByAssociationByRoleType[roleType];

                    IList<long> flushDeleted;
                    IList<long> flushChanged;
                    SplitFlushAssociations(flushAssociations, roleByAssociation, out flushDeleted, out flushChanged);

                    this.FlushRelationDelete(roleType, flushDeleted);

                    if (flushChanged != null)
                    {
                        var relationType = roleType.RelationType;
                        var mapping = this.database.Mapping;
                        using (var command = this.CreateCommand(this.Database.SchemaName + "." + mapping.GetProcedureNameForSetRole(relationType)))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            var compositeRoleDataRecords = new CompositeRoleDataRecords(mapping, roleType, flushChanged, roleByAssociation);
                            var parameter = command.Parameters.Add(Mapping.ParameterNameForRelationTable, SqlDbType.Structured);
                            parameter.TypeName = this.Database.SchemaName + "." + mapping.GetTableTypeName(relationType);
                            parameter.Value = compositeRoleDataRecords;
                            command.ExecuteNonQuery();
                        }
                    }
                }

                this.oneToOneFlushAssociationsByRoleType.Remove(roleType);
            }
        }

        private void FlushManyToOne(IRoleType roleType)
        {
            if (this.manyToOneFlushAssociationsByRoleType != null)
            {
                HashSet<long> flushAssociations;
                if (this.manyToOneFlushAssociationsByRoleType.TryGetValue(roleType, out flushAssociations))
                {
                    var roleByAssociation = this.manyToOneRoleByAssociationByRoleType[roleType];

                    IList<long> flushDeleted;
                    IList<long> flushChanged;
                    SplitFlushAssociations(flushAssociations, roleByAssociation, out flushDeleted, out flushChanged);

                    this.FlushRelationDelete(roleType, flushDeleted);

                    if (flushChanged != null)
                    {
                        var relationType = roleType.RelationType;
                        var mapping = this.database.Mapping;
                        using (var command = this.CreateCommand(this.Database.SchemaName + "." + mapping.GetProcedureNameForSetRole(relationType)))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            var compositeRoleDataRecords = new CompositeRoleDataRecords(mapping, roleType, flushChanged, roleByAssociation);
                            var parameter = command.Parameters.Add(Mapping.ParameterNameForRelationTable, SqlDbType.Structured);
                            parameter.TypeName = this.Database.SchemaName + "." + mapping.GetTableTypeName(relationType);
                            parameter.Value = compositeRoleDataRecords;
                            command.ExecuteNonQuery();
                        }
                    }
                }

                this.manyToOneFlushAssociationsByRoleType.Remove(roleType);
            }
        }

        private void FlushOneToMany(IRoleType roleType)
        {
            if (this.oneToManyOriginalRoleByAssociationByRoleType != null)
            {
                Dictionary<long, long[]> originalRoleByAssociation;
                if (this.oneToManyOriginalRoleByAssociationByRoleType.TryGetValue(roleType, out originalRoleByAssociation))
                {
                    var roleByAssociation = this.oneToManyCurrentRoleByAssociationByRoleType[roleType];

                    List<long> flushDeleted = null;
                    Dictionary<long, long[]> flushRemovedRoleByAssociation = null;
                    Dictionary<long, long[]> flushAddedRoleByAssociation = null;

                    foreach (var kvp in originalRoleByAssociation)
                    {
                        var association = kvp.Key;
                        var originalRole = kvp.Value;
                        var currentRole = roleByAssociation[association];

                        if (currentRole.Length == 0)
                        {
                            if (flushDeleted == null)
                            {
                                flushDeleted = new List<long>();
                            }

                            flushDeleted.Add(association);
                        }
                        else
                        {
                            var remove = new HashSet<long>(originalRole);
                            remove.ExceptWith(currentRole);
                            if (remove.Count > 0)
                            {
                                if (flushRemovedRoleByAssociation == null)
                                {
                                    flushRemovedRoleByAssociation = new Dictionary<long, long[]>();
                                }

                                flushRemovedRoleByAssociation[association] = new List<long>(remove).ToArray();
                            }

                            var add = new HashSet<long>(currentRole);
                            add.ExceptWith(originalRole);
                            if (add.Count > 0)
                            {
                                if (flushAddedRoleByAssociation == null)
                                {
                                    flushAddedRoleByAssociation = new Dictionary<long, long[]>();
                                }

                                flushAddedRoleByAssociation[association] = new List<long>(add).ToArray();
                            }
                        }
                    }

                    this.FlushRelationDelete(roleType, flushDeleted);

                    if (flushAddedRoleByAssociation != null)
                    {
                        var relationType = roleType.RelationType;
                        var mapping = this.database.Mapping;
                        using (var command = this.CreateCommand(this.Database.SchemaName + "." + mapping.GetProcedureNameForAddRole(relationType)))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            var compositesRoleDataRecords = new CompositesRoleDataRecords(mapping, roleType, flushAddedRoleByAssociation);
                            var parameter = command.Parameters.Add(Mapping.ParameterNameForRelationTable, SqlDbType.Structured);
                            parameter.TypeName = this.Database.SchemaName + "." + mapping.GetTableTypeName(relationType);
                            parameter.Value = compositesRoleDataRecords;
                            command.ExecuteNonQuery();
                        }
                    }

                    if (flushRemovedRoleByAssociation != null)
                    {
                        var relationType = roleType.RelationType;
                        var mapping = this.database.Mapping;
                        using (var command = this.CreateCommand(this.Database.SchemaName + "." + mapping.GetProcedureNameForRemoveRole(relationType)))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            var compositesRoleDataRecords = new CompositesRoleDataRecords(mapping, roleType, flushRemovedRoleByAssociation);
                            var parameter = command.Parameters.Add(Mapping.ParameterNameForRelationTable, SqlDbType.Structured);
                            parameter.TypeName = this.Database.SchemaName + "." + mapping.GetTableTypeName(relationType);
                            parameter.Value = compositesRoleDataRecords;
                            command.ExecuteNonQuery();
                        }
                    }
                }

                this.oneToManyOriginalRoleByAssociationByRoleType.Remove(roleType);
            }
        }

        private void FlushManyToMany(IRoleType roleType)
        {
            if (this.manyToManyOriginalRoleByAssociationByRoleType != null)
            {
                Dictionary<long, long[]> originalRoleByAssociation;
                if (this.manyToManyOriginalRoleByAssociationByRoleType.TryGetValue(roleType, out originalRoleByAssociation))
                {
                    var roleByAssociation = this.manyToManyCurrentRoleByAssociationByRoleType[roleType];

                    List<long> flushDeleted = null;
                    Dictionary<long, long[]> flushRemovedRoleByAssociation = null;
                    Dictionary<long, long[]> flushAddedRoleByAssociation = null;

                    foreach (var kvp in originalRoleByAssociation)
                    {
                        var association = kvp.Key;
                        var originalRole = kvp.Value;
                        var currentRole = roleByAssociation[association];
                        if (currentRole.Length == 0)
                        {
                            if (flushDeleted == null)
                            {
                                flushDeleted = new List<long>();
                            }

                            flushDeleted.Add(association);
                        }
                        else
                        {
                            var remove = new HashSet<long>(originalRole);
                            remove.ExceptWith(currentRole);
                            if (remove.Count > 0)
                            {
                                if (flushRemovedRoleByAssociation == null)
                                {
                                    flushRemovedRoleByAssociation = new Dictionary<long, long[]>();
                                }

                                flushRemovedRoleByAssociation[association] = new List<long>(remove).ToArray();
                            }

                            var add = new HashSet<long>(currentRole);
                            add.ExceptWith(originalRole);
                            if (add.Count > 0)
                            {
                                if (flushAddedRoleByAssociation == null)
                                {
                                    flushAddedRoleByAssociation = new Dictionary<long, long[]>();
                                }

                                flushAddedRoleByAssociation[association] = new List<long>(add).ToArray();
                            }
                        }
                    }

                    this.FlushRelationDelete(roleType, flushDeleted);

                    if (flushAddedRoleByAssociation != null)
                    {
                        var relationType = roleType.RelationType;
                        var mapping = this.database.Mapping;
                        using (var command = this.CreateCommand(this.Database.SchemaName + "." + mapping.GetProcedureNameForAddRole(relationType)))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            var compositesRoleDataRecords = new CompositesRoleDataRecords(mapping, roleType, flushAddedRoleByAssociation);
                            var parameter = command.Parameters.Add(Mapping.ParameterNameForRelationTable, SqlDbType.Structured);
                            parameter.TypeName = this.Database.SchemaName + "." + mapping.GetTableTypeName(relationType);
                            parameter.Value = compositesRoleDataRecords;
                            command.ExecuteNonQuery();
                        }
                    }

                    if (flushRemovedRoleByAssociation != null)
                    {
                        var relationType = roleType.RelationType;
                        var mapping = this.database.Mapping;
                        using (var command = this.CreateCommand(this.Database.SchemaName + "." + mapping.GetProcedureNameForRemoveRole(relationType)))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            var compositesRoleDataRecords = new CompositesRoleDataRecords(mapping, roleType, flushRemovedRoleByAssociation);
                            var parameter = command.Parameters.Add(Mapping.ParameterNameForRelationTable, SqlDbType.Structured);
                            parameter.TypeName = this.Database.SchemaName + "." + mapping.GetTableTypeName(relationType);
                            parameter.Value = compositesRoleDataRecords;
                            command.ExecuteNonQuery();
                        }
                    }
                }

                this.manyToManyOriginalRoleByAssociationByRoleType.Remove(roleType);
            }
        }

        private void FlushRelationDelete(IRoleType roleType, IList<long> flushDeleted)
        {
            if (flushDeleted != null)
            {
                var mapping = this.database.Mapping;
                using (var command = this.CreateCommand(this.Database.SchemaName + "." + mapping.GetProcedureNameForDeleteRole(roleType.RelationType)))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    var objectDataRecords = new ObjectDataRecords(mapping, flushDeleted);
                    var parameter = command.Parameters.Add(Mapping.ParameterNameForObjectTable, SqlDbType.Structured);
                    parameter.TypeName = this.Database.SchemaName + "." + Mapping.TableTypeNameForObjects;
                    parameter.Value = objectDataRecords;
                    command.ExecuteNonQuery();
                }
            }
        }

        private void OnObjectChanged(long association)
        {
            if (this.changedObjects == null)
            {
                this.changedObjects = new HashSet<long>();
            }

            this.changedObjects.Add(association);
        }

        private void Reset()
        {
            this.classByObjectId = null;
            this.cacheIdByObjectId = null;

            this.objectsToFetch = null;

            this.newObjects = null;
            this.deletedObjects = null;
            this.flushDeletedObjects = null;
            this.changedObjects = null;

            this.unitRoleByAssociationByRoleType = null;
            this.unitFlushAssociationsByRoleType = null;

            this.oneToOneRoleByAssociationByRoleType = null;
            this.oneToOneAssociationByRoleByAssociationType = null;
            this.oneToOneFlushAssociationsByRoleType = null;

            this.manyToOneRoleByAssociationByRoleType = null;
            this.manyToOneAssociationByRoleByAssociationType = null;
            this.manyToOneFlushAssociationsByRoleType = null;
            this.manyToOneTriggerFlushRolesByAssociationType = null;

            this.oneToManyCurrentRoleByAssociationByRoleType = null;
            this.oneToManyOriginalRoleByAssociationByRoleType = null;
            this.oneToManyAssociationByRoleByAssociationType = null;

            this.manyToManyCurrentRoleByAssociationByRoleType = null;
            this.manyToManyOriginalRoleByAssociationByRoleType = null;
            this.manyToManyAssociationByRoleByAssociationType = null;
            this.manyToManyTriggerFlushRolesByAssociationType = null;

            this.changeSet = null;
        }

        private void LazyDisconnect()
        {
            try
            {
                if (this.transaction != null)
                {
                    this.transaction.Rollback();
                }
            }
            finally
            {
                this.transaction = null;

                try
                {
                    if (this.connection != null)
                    {
                        this.connection.Close();
                    }
                }
                finally
                {
                    this.connection = null;
                }
            }
        }

        private void AddObjectToFetchIfNotCached(long objectId)
        {
            if (this.cacheIdByObjectId == null || !this.cacheIdByObjectId.ContainsKey(objectId))
            {
                this.AddObjectToFetch(objectId);
            }
        }

        private void AddObjectToFetch(long objectId)
        {
            if (this.objectsToFetch == null)
            {
                this.objectsToFetch = new HashSet<long>();
            }

            this.objectsToFetch.Add(objectId);
        }
        
        #region Lazy Dictionaries
        private Dictionary<long, object> GetUnitRoleByAssociation(IRoleType roleType)
        {
            if (this.unitRoleByAssociationByRoleType == null)
            {
                this.unitRoleByAssociationByRoleType = new Dictionary<IRoleType, Dictionary<long, object>>();
            }

            Dictionary<long, object> roleByAssociation;
            if (!this.unitRoleByAssociationByRoleType.TryGetValue(roleType, out roleByAssociation))
            {
                roleByAssociation = new Dictionary<long, object>();
                this.unitRoleByAssociationByRoleType[roleType] = roleByAssociation;
            }

            return roleByAssociation;
        }

        private HashSet<long> GetUnitFlushAssociations(IRoleType roleType)
        {
            if (this.unitFlushAssociationsByRoleType == null)
            {
                this.unitFlushAssociationsByRoleType = new Dictionary<IRoleType, HashSet<long>>();
            }

            HashSet<long> flushAssociations;
            if (!this.unitFlushAssociationsByRoleType.TryGetValue(roleType, out flushAssociations))
            {
                flushAssociations = new HashSet<long>();
                this.unitFlushAssociationsByRoleType[roleType] = flushAssociations;
            }

            return flushAssociations;
        }

        private Dictionary<long, long?> GetOneToOneRoleByAssociation(IRoleType roleType)
        {
            if (this.oneToOneRoleByAssociationByRoleType == null)
            {
                this.oneToOneRoleByAssociationByRoleType = new Dictionary<IRoleType, Dictionary<long, long?>>();
            }

            Dictionary<long, long?> roleByAssociation;
            if (!this.oneToOneRoleByAssociationByRoleType.TryGetValue(roleType, out roleByAssociation))
            {
                roleByAssociation = new Dictionary<long, long?>();
                this.oneToOneRoleByAssociationByRoleType[roleType] = roleByAssociation;
            }

            return roleByAssociation;
        }

        private HashSet<long> GetOneToOneFlushAssociations(IRoleType roleType)
        {
            if (this.oneToOneFlushAssociationsByRoleType == null)
            {
                this.oneToOneFlushAssociationsByRoleType = new Dictionary<IRoleType, HashSet<long>>();
            }

            HashSet<long> flushAssociations;
            if (!this.oneToOneFlushAssociationsByRoleType.TryGetValue(roleType, out flushAssociations))
            {
                flushAssociations = new HashSet<long>();
                this.oneToOneFlushAssociationsByRoleType[roleType] = flushAssociations;
            }

            return flushAssociations;
        }

        private Dictionary<long, long?> GetOneToOneAssociationByRole(IAssociationType associationType)
        {
            if (this.oneToOneAssociationByRoleByAssociationType == null)
            {
                this.oneToOneAssociationByRoleByAssociationType = new Dictionary<IAssociationType, Dictionary<long, long?>>();
            }

            Dictionary<long, long?> associationByRole;
            if (!this.oneToOneAssociationByRoleByAssociationType.TryGetValue(associationType, out associationByRole))
            {
                associationByRole = new Dictionary<long, long?>();
                this.oneToOneAssociationByRoleByAssociationType[associationType] = associationByRole;
            }

            return associationByRole;
        }

        private Dictionary<long, long?> GetManyToOneRoleByAssociation(IRoleType roleType)
        {
            if (this.manyToOneRoleByAssociationByRoleType == null)
            {
                this.manyToOneRoleByAssociationByRoleType = new Dictionary<IRoleType, Dictionary<long, long?>>();
            }

            Dictionary<long, long?> roleByAssociation;
            if (!this.manyToOneRoleByAssociationByRoleType.TryGetValue(roleType, out roleByAssociation))
            {
                roleByAssociation = new Dictionary<long, long?>();
                this.manyToOneRoleByAssociationByRoleType[roleType] = roleByAssociation;
            }

            return roleByAssociation;
        }

        private HashSet<long> GetManyToOneFlushAssociations(IRoleType roleType)
        {
            if (this.manyToOneFlushAssociationsByRoleType == null)
            {
                this.manyToOneFlushAssociationsByRoleType = new Dictionary<IRoleType, HashSet<long>>();
            }

            HashSet<long> flushAssociations;
            if (!this.manyToOneFlushAssociationsByRoleType.TryGetValue(roleType, out flushAssociations))
            {
                flushAssociations = new HashSet<long>();
                this.manyToOneFlushAssociationsByRoleType[roleType] = flushAssociations;
            }

            return flushAssociations;
        }

        private Dictionary<long, long[]> GetManyToOneAssociationByRole(IAssociationType associationType)
        {
            if (this.manyToOneAssociationByRoleByAssociationType == null)
            {
                this.manyToOneAssociationByRoleByAssociationType = new Dictionary<IAssociationType, Dictionary<long, long[]>>();
            }

            Dictionary<long, long[]> associationByRole;
            if (!this.manyToOneAssociationByRoleByAssociationType.TryGetValue(associationType, out associationByRole))
            {
                associationByRole = new Dictionary<long, long[]>();
                this.manyToOneAssociationByRoleByAssociationType[associationType] = associationByRole;
            }

            return associationByRole;
        }

        private HashSet<long> GetManyToOneTriggerFlushRoles(IAssociationType associationType)
        {
            if (this.manyToOneTriggerFlushRolesByAssociationType == null)
            {
                this.manyToOneTriggerFlushRolesByAssociationType = new Dictionary<IAssociationType, HashSet<long>>();
            }

            HashSet<long> triggerFlushRoles;
            if (!this.manyToOneTriggerFlushRolesByAssociationType.TryGetValue(associationType, out triggerFlushRoles))
            {
                triggerFlushRoles = new HashSet<long>();
                this.manyToOneTriggerFlushRolesByAssociationType[associationType] = triggerFlushRoles;
            }

            return triggerFlushRoles;
        }

        private Dictionary<long, long[]> GetOneToManyCurrentRoleByAssociation(IRoleType roleType)
        {
            if (this.oneToManyCurrentRoleByAssociationByRoleType == null)
            {
                this.oneToManyCurrentRoleByAssociationByRoleType = new Dictionary<IRoleType, Dictionary<long, long[]>>();
            }

            Dictionary<long, long[]> roleByAssociation;
            if (!this.oneToManyCurrentRoleByAssociationByRoleType.TryGetValue(roleType, out roleByAssociation))
            {
                roleByAssociation = new Dictionary<long, long[]>();
                this.oneToManyCurrentRoleByAssociationByRoleType[roleType] = roleByAssociation;
            }

            return roleByAssociation;
        }

        private Dictionary<long, long[]> GetOneToManyOriginalRoleByAssociation(IRoleType roleType)
        {
            if (this.oneToManyOriginalRoleByAssociationByRoleType == null)
            {
                this.oneToManyOriginalRoleByAssociationByRoleType = new Dictionary<IRoleType, Dictionary<long, long[]>>();
            }

            Dictionary<long, long[]> roleByAssociation;
            if (!this.oneToManyOriginalRoleByAssociationByRoleType.TryGetValue(roleType, out roleByAssociation))
            {
                roleByAssociation = new Dictionary<long, long[]>();
                this.oneToManyOriginalRoleByAssociationByRoleType[roleType] = roleByAssociation;
            }

            return roleByAssociation;
        }

        private Dictionary<long, long?> GetOneToManyAssociationByRole(IAssociationType associationType)
        {
            if (this.oneToManyAssociationByRoleByAssociationType == null)
            {
                this.oneToManyAssociationByRoleByAssociationType = new Dictionary<IAssociationType, Dictionary<long, long?>>();
            }

            Dictionary<long, long?> associationByRole;
            if (!this.oneToManyAssociationByRoleByAssociationType.TryGetValue(associationType, out associationByRole))
            {
                associationByRole = new Dictionary<long, long?>();
                this.oneToManyAssociationByRoleByAssociationType[associationType] = associationByRole;
            }

            return associationByRole;
        }

        private Dictionary<long, long[]> GetManyToManyCurrentRoleByAssociation(IRoleType roleType)
        {
            if (this.manyToManyCurrentRoleByAssociationByRoleType == null)
            {
                this.manyToManyCurrentRoleByAssociationByRoleType = new Dictionary<IRoleType, Dictionary<long, long[]>>();
            }

            Dictionary<long, long[]> roleByAssociation;
            if (!this.manyToManyCurrentRoleByAssociationByRoleType.TryGetValue(roleType, out roleByAssociation))
            {
                roleByAssociation = new Dictionary<long, long[]>();
                this.manyToManyCurrentRoleByAssociationByRoleType[roleType] = roleByAssociation;
            }

            return roleByAssociation;
        }

        private Dictionary<long, long[]> GetManyToManyOriginalRoleByAssociation(IRoleType roleType)
        {
            if (this.manyToManyOriginalRoleByAssociationByRoleType == null)
            {
                this.manyToManyOriginalRoleByAssociationByRoleType = new Dictionary<IRoleType, Dictionary<long, long[]>>();
            }

            Dictionary<long, long[]> roleByAssociation;
            if (!this.manyToManyOriginalRoleByAssociationByRoleType.TryGetValue(roleType, out roleByAssociation))
            {
                roleByAssociation = new Dictionary<long, long[]>();
                this.manyToManyOriginalRoleByAssociationByRoleType[roleType] = roleByAssociation;
            }

            return roleByAssociation;
        }
        
        private Dictionary<long, long[]> GetManyToManyAssociationByRole(IAssociationType associationType)
        {
            if (this.manyToManyAssociationByRoleByAssociationType == null)
            {
                this.manyToManyAssociationByRoleByAssociationType = new Dictionary<IAssociationType, Dictionary<long, long[]>>();
            }

            Dictionary<long, long[]> associationByRole;
            if (!this.manyToManyAssociationByRoleByAssociationType.TryGetValue(associationType, out associationByRole))
            {
                associationByRole = new Dictionary<long, long[]>();
                this.manyToManyAssociationByRoleByAssociationType[associationType] = associationByRole;
            }

            return associationByRole;
        }

        private HashSet<long> GetManyToManyTriggerFlushRoles(IAssociationType associationType)
        {
            if (this.manyToManyTriggerFlushRolesByAssociationType == null)
            {
                this.manyToManyTriggerFlushRolesByAssociationType = new Dictionary<IAssociationType, HashSet<long>>();
            }

            HashSet<long> triggerFlushRoles;
            if (!this.manyToManyTriggerFlushRolesByAssociationType.TryGetValue(associationType, out triggerFlushRoles))
            {
                triggerFlushRoles = new HashSet<long>();
                this.manyToManyTriggerFlushRolesByAssociationType[associationType] = triggerFlushRoles;
            }

            return triggerFlushRoles;
        }
        #endregion
    }
}