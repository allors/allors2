// <copyright file="State.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Sql
{
    using System.Collections.Generic;
    using System.Linq;

    using Meta;

    public sealed class State
    {
        public State(Transaction transaction)
        {
            this.Transaction = transaction;
            this.ReferenceByObjectId = new Dictionary<long, Reference>();

            this.ExistingObjectIdsWithoutReference = new HashSet<long>();
            this.ReferencesWithoutVersions = new HashSet<Reference>();

            this.AssociationByRoleByAssociationType = new Dictionary<IAssociationType, Dictionary<Reference, Reference>>();
            this.AssociationsByRoleByAssociationType = new Dictionary<IAssociationType, Dictionary<Reference, long[]>>();

            this.ChangeLog = new ChangeLog();
        }

        internal ChangeLog ChangeLog { get; }

        internal Dictionary<long, Reference> ReferenceByObjectId { get; set; }

        internal Dictionary<Reference, Strategy> ModifiedRolesByReference { get; set; }

        internal Dictionary<IAssociationType, Dictionary<Reference, Reference>> AssociationByRoleByAssociationType { get; set; }

        internal Dictionary<IAssociationType, Dictionary<Reference, long[]>> AssociationsByRoleByAssociationType { get; set; }

        internal Dictionary<Reference, Strategy> UnflushedRolesByReference { get; set; }

        internal Dictionary<IAssociationType, HashSet<long>> TriggersFlushRolesByAssociationType { get; set; }

        internal HashSet<long> ExistingObjectIdsWithoutReference { get; set; }

        internal HashSet<Reference> ReferencesWithoutVersions { get; set; }

        private Transaction Transaction { get; }

        public Reference GetOrCreateReferenceForExistingObject(long objectId, Transaction transaction)
        {
            if (!this.ReferenceByObjectId.TryGetValue(objectId, out var reference))
            {
                var objectType = transaction.Database.Cache.GetObjectType(objectId);
                if (objectType == null)
                {
                    this.ExistingObjectIdsWithoutReference.Add(objectId);

                    transaction.InstantiateReferences(this.ExistingObjectIdsWithoutReference);

                    this.ExistingObjectIdsWithoutReference = new HashSet<long>();
                    this.ReferenceByObjectId.TryGetValue(objectId, out reference);
                }
                else
                {
                    reference = new Reference(transaction, objectType, objectId, false);
                    this.ReferenceByObjectId[objectId] = reference;
                    this.ReferencesWithoutVersions.Add(reference);
                }
            }
            else
            {
                reference.Exists = true;
            }

            return reference;
        }

        public Reference[] GetOrCreateReferencesForExistingObjects(IEnumerable<long> objectIds, Transaction transaction)
        {
            var objectIdArray = objectIds.ToArray();

            var instantiate = false;
            foreach (var objectId in objectIdArray)
            {
                if (!this.ReferenceByObjectId.TryGetValue(objectId, out var reference))
                {
                    var objectType = transaction.Database.Cache.GetObjectType(objectId);
                    if (objectType == null)
                    {
                        instantiate = true;
                        this.ExistingObjectIdsWithoutReference.Add(objectId);
                    }
                }
            }

            if (instantiate)
            {
                transaction.InstantiateReferences(this.ExistingObjectIdsWithoutReference);
                this.ExistingObjectIdsWithoutReference = new HashSet<long>();
            }

            var references = new List<Reference>();
            foreach (var objectId in objectIdArray)
            {
                if (!this.ReferenceByObjectId.TryGetValue(objectId, out var reference))
                {
                    var objectType = transaction.Database.Cache.GetObjectType(objectId);
                    if (objectType == null)
                    {
                        this.ExistingObjectIdsWithoutReference.Add(objectId);
                    }
                    else
                    {
                        reference = new Reference(transaction, objectType, objectId, false);
                        this.ReferenceByObjectId[objectId] = reference;
                        this.ReferencesWithoutVersions.Add(reference);
                    }
                }
                else
                {
                    reference.Exists = true;
                }

                references.Add(reference);
            }

            return references.ToArray();
        }

        public Reference GetOrCreateReferenceForExistingObject(IClass objectType, long objectId, Transaction transaction)
        {
            if (!this.ReferenceByObjectId.TryGetValue(objectId, out var reference))
            {
                reference = new Reference(transaction, objectType, objectId, false);
                this.ReferenceByObjectId[objectId] = reference;
                this.ReferencesWithoutVersions.Add(reference);
            }
            else
            {
                reference.Exists = true;
            }

            return reference;
        }

        public Reference GetOrCreateReferenceForExistingObject(IClass objectType, long objectId, long version, Transaction transaction)
        {
            if (!this.ReferenceByObjectId.TryGetValue(objectId, out var reference))
            {
                reference = new Reference(transaction, objectType, objectId, version);
                this.ReferenceByObjectId[objectId] = reference;
            }
            else
            {
                reference.Version = version;
                reference.Exists = true;
            }

            return reference;
        }

        public Reference CreateReferenceForNewObject(IClass objectType, long objectId, Transaction transaction)
        {
            var strategyReference = new Reference(transaction, objectType, objectId, true);
            this.ReferenceByObjectId[objectId] = strategyReference;
            return strategyReference;
        }

        public Strategy GetOrCreateRoles(Reference reference)
        {
            if (this.ModifiedRolesByReference == null)
            {
                return new Strategy(reference);
            }

            return this.ModifiedRolesByReference.TryGetValue(reference, out var roles) ? roles : new Strategy(reference);
        }

        public Dictionary<Reference, Reference> GetAssociationByRole(IAssociationType associationType)
        {
            if (this.AssociationByRoleByAssociationType.TryGetValue(associationType, out var associationByRole))
            {
                return associationByRole;
            }

            associationByRole = new Dictionary<Reference, Reference>();
            this.AssociationByRoleByAssociationType[associationType] = associationByRole;

            return associationByRole;
        }

        public Dictionary<Reference, long[]> GetAssociationsByRole(IAssociationType associationType)
        {
            if (this.AssociationsByRoleByAssociationType.TryGetValue(associationType, out var associationsByRole))
            {
                return associationsByRole;
            }

            associationsByRole = new Dictionary<Reference, long[]>();
            this.AssociationsByRoleByAssociationType[associationType] = associationsByRole;

            return associationsByRole;
        }

        public long GetObjectIdForExistingObject(string objectStringId)
        {
            var objectId = long.Parse(objectStringId);
            if (!this.ReferenceByObjectId.ContainsKey(objectId))
            {
                this.ExistingObjectIdsWithoutReference.Add(objectId);
            }

            return objectId;
        }

        #region Flushing
        public void RequireFlush(Strategy roles)
        {
            this.UnflushedRolesByReference ??= new Dictionary<Reference, Strategy>();
            this.ModifiedRolesByReference ??= new Dictionary<Reference, Strategy>();

            this.UnflushedRolesByReference[roles.Reference] = roles;
            this.ModifiedRolesByReference[roles.Reference] = roles;
        }

        public void TriggerFlush(long role, IAssociationType associationType)
        {
            this.TriggersFlushRolesByAssociationType ??= new Dictionary<IAssociationType, HashSet<long>>();

            if (!this.TriggersFlushRolesByAssociationType.TryGetValue(associationType, out var associations))
            {
                associations = new HashSet<long>();
                this.TriggersFlushRolesByAssociationType[associationType] = associations;
            }

            associations.Add(role);
        }

        public void Flush()
        {
            if (this.UnflushedRolesByReference == null)
            {
                return;
            }

            var flush = new Flush(this.Transaction, this.UnflushedRolesByReference);
            flush.Execute();

            this.UnflushedRolesByReference = null;
            this.TriggersFlushRolesByAssociationType = null;
        }

        internal void FlushConditionally(long roleId, IAssociationType associationType)
        {
            if (this.TriggersFlushRolesByAssociationType == null)
            {
                return;
            }

            if (!this.TriggersFlushRolesByAssociationType.TryGetValue(associationType, out var roles))
            {
                return;
            }

            if (roles.Contains(roleId))
            {
                this.Flush();
            }
        }
        #endregion
    }
}
